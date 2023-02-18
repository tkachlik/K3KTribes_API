namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Templates
{
    public class ErrorResponse : IResponse
    {
        public string Status { get; } = "Error";
        public int StatusCode { get; init; }
        public string Title { get; } = "One or more errors have occurred.";
        public List<Error> Errors { get; init; }

        public ErrorResponse() { }

        public ErrorResponse(int statusCode, string field, string errorMessage)
        {
            StatusCode = statusCode;
            Errors = new List<Error>()
            {
                new Error(field, errorMessage)
            };
        }

        public ErrorResponse(int statusCode, string field, string errorMessage, string field2, string errorMessage2)
        {
            StatusCode = statusCode;
            Errors = new List<Error>()
            {
                new Error(field, errorMessage),
                new Error(field2, errorMessage2)
            };
        }

        public override bool Equals(object? obj)
        {
            var other = obj as ErrorResponse;

            if (other == null) return false;

            if (Status != other.Status ||
                StatusCode != other.StatusCode ||
                Title != other.Title ||
                Errors.Count != other.Errors.Count) return false;

            for (int i = 0; i < Errors.Count; i++)
            {
                if (!Errors[i].Equals(other.Errors[i])) return false;
            }
            
            return true;
        }
    }
}