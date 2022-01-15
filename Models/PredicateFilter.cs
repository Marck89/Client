using System.Linq;

namespace ModelClient
{
    public class PredicateFilter : ModelBase
    {
        public string? FieldName { get; set; }
        public object? FieldValue { get; set; }
        public object? FieldType { get; set; }

        private string? _parameterName = "customerPar";
        public string? ParameterName
        {
            get => _parameterName;
            set => _parameterName = string.IsNullOrWhiteSpace(value) ? ParameterName : value;
        }

        public PredicateFilter(string? parameterName = null) => ParameterName = parameterName;

        public string ToCustomerParameter() => string.Join("&", new[]
            {
                !string.IsNullOrWhiteSpace(FieldName) ? $"{ParameterName}={FieldName}" : string.Empty,
                !string.IsNullOrWhiteSpace(FieldValue?.ToString()) ? $"{ParameterName}={FieldValue}" : string.Empty,
                !string.IsNullOrWhiteSpace(FieldType?.ToString()) ? $"{ParameterName}={FieldType}": string.Empty
            }.Where(p => !string.IsNullOrWhiteSpace(p)));
    }
}
