using System.Linq;

namespace Grid.Mvc.Ajax.GridExtensions
{
    public interface IAjaxGridFactory
    {
        IAjaxGrid CreateAjaxGrid<T>(IQueryable<T> gridItems, int page, bool renderOnlyRows)
            where T : class;
    }
}