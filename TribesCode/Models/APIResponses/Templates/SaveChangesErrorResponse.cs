namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Templates
{
    public class SaveChangesErrorResponse : ErrorResponse
    {
        public SaveChangesErrorResponse()
        {
            StatusCode = 400;
            Errors = new List<Error>()
            {
                new Error("", "Saving to database failed. Unknown error.")
            };
        }
    }
}