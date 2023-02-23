using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace dusicyon_midnight_tribes_backend.Models.APIResponses.Templates.CustomValidation
{
    public class ValidationResultModel : IResponse
    {
        public string Status { get; } = "Ok";
        public int StatusCode { get; } = 400;
        public string Title { get; } = "One or more validation errors have occurred.";
        public List<ValidationError> Errors { get; init; }

        public ValidationResultModel() { }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Status = "Error";
            StatusCode = 400;
            Title = "One or more validation errors have occurred.";
            Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors
                    .Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
        }

        public ValidationResultModel(string field, string errorMessage)
        {
            Errors = new List<ValidationError>()
            {
                new ValidationError(field, errorMessage)
            };
        }

        public override bool Equals(object? obj)
        {
            var other = obj as ValidationResultModel;

            if (other == null) return false;

            if (Status != other.Status ||
                StatusCode != other.StatusCode ||
                Title != other.Title ||
                Errors.Count != other.Errors.Count)
                return false;

            var errorsSorted = Errors.OrderBy(e => e.Field).OrderBy(e => e.ErrorMessage).ToList();
            var otherErrorsSorted = other.Errors.OrderBy(eo => eo.Field).OrderBy(eo => eo.ErrorMessage).ToList();

            for (int i = 0; i < errorsSorted.Count; i++)
            {
                if (!errorsSorted[i].Equals(otherErrorsSorted[i]))
                    return false;
            }

            return true;
        }
    }
}