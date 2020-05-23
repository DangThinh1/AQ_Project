namespace Identity.Core.Services.Interfaces
{
    public interface IRequestServiceBase
    {
        string BaseUrl { get; set; }
        string BaseToken { get; set; }
    }
}
