namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Templates
{
    public class OkResponse : IResponse
    {
        public string Status { get; } = "OK";
    }
}