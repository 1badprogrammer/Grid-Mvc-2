namespace Grid.Mvc.Ajax.GridExtensions
{
    public interface IAjaxGridPager
    {
        int Pages { get; set; }
        int ItemCount { get; set; }
        int PagePartitionSize { get; set; }
    }
}