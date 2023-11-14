using static JWT.Demo.Helpers.Enums.Enums;

namespace JWT.Demo.Helpers.GenericSort
{
    public class SortingParams
    {
        public SortOrders SortOrder { get; set; } = SortOrders.Asc;
        public string ColumnName { get; set; }
    }
}
