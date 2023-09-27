namespace Atm.Web.Controllers.Models;

/// <summary>
/// Return type so that Swagger documentation in generated correctly
/// </summary>
[Serializable]
public class ActionResultType
{
    public string Type { get; init; } = null!;
    public string Title { get; init; } = null!;
    public int Status { get; init; }
    public string TraceId { get; init; } = null!;
}


/// <summary>
/// Return type so that Swagger documentation in generated correctly
/// </summary>
[Serializable]
public class BadRequestResultType : ActionResultType
{
    public Dictionary<string, string[]> Errors { get; init; } = null!;
}