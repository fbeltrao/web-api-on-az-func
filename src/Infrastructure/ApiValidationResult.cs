using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure
{
    /// <summary>
    /// Represents the result of an object validation
    /// </summary>
    public class ApiValidationResult
    {
        public List<ValidationResult> ValidationResults { get; }

        public bool IsValid => ValidationResults == null;

        public static ApiValidationResult Valid => new ApiValidationResult();

        public ApiValidationResult(List<ValidationResult> validationResults)
        {
            ValidationResults = validationResults;
        }

        private ApiValidationResult()
        {
        }
    }
}