namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Templates
{
    public class Error
    {
        public string Field { get; init; }
        public string ErrorMessage { get; init; }

        public Error() { }

        public Error(string field, string errorMessage)
        {
            Field = field;
            ErrorMessage = errorMessage;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as Error;

            if (other == null) return false;

            if (Field != other.Field || ErrorMessage != other.ErrorMessage) return false;

            return true;
        }
    }
}