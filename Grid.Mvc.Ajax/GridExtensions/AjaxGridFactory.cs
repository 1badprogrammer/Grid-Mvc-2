using System.Linq;

namespace Grid.Mvc.Ajax.GridExtensions
{
    public class AjaxGridFactory : IAjaxGridFactory
    {
        public IAjaxGrid CreateAjaxGrid<T>(IQueryable<T> gridItems, int page, bool renderOnlyRows)
            where T : class
        {
            var grid = new AjaxGrid<T>(gridItems, page, renderOnlyRows);
            return grid;
        }
    }
}