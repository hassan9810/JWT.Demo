using static JWT.Demo.Helpers.Enums.Enums;

namespace JWT.Demo.Helpers.GenericSearchFilter
{
    /// <summary>
    /// Filter parameters Model Class
    /// </summary>
    public class FilterParams
    {
        public string ColumnName { get; set; } = string.Empty;
        public string FilterValue { get; set; } = string.Empty;
        public FilterOptions FilterOption { get; set; } = FilterOptions.Contains;
    }
}
