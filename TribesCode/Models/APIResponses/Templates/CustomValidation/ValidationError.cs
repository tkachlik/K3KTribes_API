using Newtonsoft.Json;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Templates.CustomValidation
{
    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; init; }

        public string ErrorMessage { get; init; }


        public ValidationError() { }

        public ValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            ErrorMessage = message;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as ValidationError;

            if (other == null) return false;

            if (Field != other.Field || ErrorMessage != other.ErrorMessage) return false;

            return true;
        }
    }
}