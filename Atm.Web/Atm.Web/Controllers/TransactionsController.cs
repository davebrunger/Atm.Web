using Azure.Core;

namespace Atm.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TransactionsController : SessionControllerBase
{
    public TransactionsController(IAtmDbContext atmDbContext) : base(atmDbContext)
    {
    }

    [HttpPost("withdrawal")]
    [ProducesResponseType(typeof(TransactionResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResultType), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<TransactionResponse>> MakeWithdrawal(
        [FromBody] MakeWithdrawalRequest request,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var utcNow = DateTime.UtcNow;

        var session = await GetSession(
            utcNow,
            s => s
            .Include(s => s.Account)
            .Include(s => s.Account.Transactions
                .Where(t =>
                    EF.Functions.DateDiffDay(t.DateTime, utcNow) == 0 &&
                    t.Amount < 0)),
            cancellationToken);

        if (session == null)
        {
            return Unauthorized();
        }

        if (session.Account.Transactions.Sum(t => t.Amount) - request.Amount <= -1000)
        {
            ModelState.AddModelError(string.Empty, "Daily withdrawal limit reached");
            return BadRequest(ModelState);
        }

        var transaction = new Transaction
        {
            Account = session.Account,
            Amount = -request.Amount,
            DateTime = utcNow,
        };

        atmDbContext.Add(transaction);

        session.LastActivityAtUtc = utcNow;
        session.Account.Balance = session.Account.Balance - request.Amount;

        await atmDbContext.SaveChangesAsync(cancellationToken);

        return new TransactionResponse
        {
            AccountNumber = session.AccountNumber.Obfuscate(),
            Amount = request.Amount,
            DateTime = utcNow,
            Id = transaction.Id,
        };
    }

    [HttpGet("lastFive")]
    [ProducesResponseType(typeof(TransactionResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BadRequestResultType), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.Unauthorized)]
    public async Task<ActionResult<TransactionResponse[]>> GetLastFiveTransactions(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var utcNow = DateTime.UtcNow;

        var session = await GetSession(
            utcNow,
            s => s
                .Include(s => s.Account)
                .Include(s => s.Account.Transactions
                    .OrderByDescending(t => t.DateTime)
                    .Take(5)),
            cancellationToken);

        if (session == null)
        {
            return Unauthorized();
        }

        session.LastActivityAtUtc = utcNow;

        await atmDbContext.SaveChangesAsync(cancellationToken);

        return session.Account.Transactions
            .Select( t => new TransactionResponse
            {
                AccountNumber = session.AccountNumber.Obfuscate(),
                Amount = t.Amount,
                DateTime = t.DateTime,
                Id = t.Id,
            })
            .ToArray();
    }
}
