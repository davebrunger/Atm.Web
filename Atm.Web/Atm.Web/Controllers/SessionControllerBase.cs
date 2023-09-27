namespace Atm.Web.Controllers;

public abstract class SessionControllerBase : ControllerBase
{
    protected readonly IAtmDbContext atmDbContext;

    protected SessionControllerBase(IAtmDbContext atmDbContext)
    {
        this.atmDbContext = atmDbContext;
    }

    protected async Task<Session?> GetSession(DateTime utcNow, Func<IQueryable<Session>, IQueryable<Session>>? addIncludes = null, CancellationToken cancellationToken = default)
    {
        var sessionIdClaim = User.Claims.SingleOrDefault(c => c.Type == "SessionId")?.Value ?? string.Empty;

        if (!long.TryParse(sessionIdClaim, out var sessionId))
        {
            return null;
        }

        var session = atmDbContext.Sessions
            .Where(s => s.Id == sessionId && s.FinishedAtUtc == null && s.LastActivityAtUtc.AddMinutes(10) >= utcNow);

        if (addIncludes != null)
        {
            session = addIncludes(session);
        }

        return await session.SingleOrDefaultAsync(cancellationToken);
    }
}