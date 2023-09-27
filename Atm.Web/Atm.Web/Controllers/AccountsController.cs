namespace Atm.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountsController : SessionControllerBase
{
    private readonly IConfiguration configuration;

    public AccountsController(IAtmDbContext atmDbContext, IConfiguration configuration) : base(atmDbContext)
    {
        this.configuration = configuration;
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ActionResultType), (int)HttpStatusCode.NotFound)]
    [ProducesResponseType(typeof(BadRequestResultType), (int)HttpStatusCode.BadRequest)]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var utcNow = DateTime.UtcNow;

        var account = await atmDbContext.Accounts
            .Where(a => a.AccountNumber == request.AccountNumber && a.Pin == request.Pin)
            .Include(a => a.Sessions
                .Where(s => s.FinishedAtUtc == null && s.StartedAtUtc.AddMinutes(10) >= utcNow)
                .OrderByDescending(s => s.StartedAtUtc)
                .Take(1))
            .SingleOrDefaultAsync(cancellationToken);

        if (account == null)
        {
            return NotFound();
        }

        if (account.Sessions.Count > 0)
        {
            ModelState.AddModelError(string.Empty, "Session already in progress");
            return BadRequest(ModelState);
        }

        var sesssion = new Session
        {
            Account = account,
            StartedAtUtc = utcNow,
            LastActivityAtUtc = utcNow,
        };

        atmDbContext.Add(sesssion);

        await atmDbContext.SaveChangesAsync(cancellationToken);

        var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("SessionId", sesssion.Id.ToString()),
                new Claim("AccountNumber", account.AccountNumber),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(60), // An hour should be enough. We are going to check the session Id anyway.
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new LoginResponse
        {
            AccountNumber = request.AccountNumber.Obfuscate(),
            Token = tokenHandler.WriteToken(token)
        };
    }

    [HttpGet("balance")]
    [ProducesResponseType(typeof(GetBalanceResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<GetBalanceResponse>> GetBalance(CancellationToken cancellationToken)
    {
        var utcNow = DateTime.UtcNow;

        var session = await GetSession(utcNow, s => s.Include(s => s.Account), cancellationToken);

        if (session == null)
        {
            return Unauthorized();
        }

        session.LastActivityAtUtc = utcNow;

        await atmDbContext.SaveChangesAsync(cancellationToken);

        return new GetBalanceResponse
        {
            AccountNumber = session.Account.AccountNumber.Obfuscate(),
            Balance = session.Account.Balance
        };
    }

    [HttpPost("logout")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult> Logout(CancellationToken cancellationToken)
    {
        var utcNow = DateTime.UtcNow;

        var session = await GetSession(utcNow, cancellationToken: cancellationToken);

        if (session == null)
        {
            return Unauthorized();
        }

        session.LastActivityAtUtc = utcNow;
        session.FinishedAtUtc = utcNow;

        await atmDbContext.SaveChangesAsync(cancellationToken);

        return Ok();
    }
}
