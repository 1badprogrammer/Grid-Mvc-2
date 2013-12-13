using System.Linq;
using System.Web.Mvc;
using Grid.Mvc.Ajax.Helpers;
using GridMvc;

namespace Grid.Mvc.Ajax.GridExtensions
{
    public class AjaxGrid<T> : Grid<T>, IAjaxGrid where T: class
    {
        public bool HasItems { get { return Pager.CurrentPage < (Pager as AjaxGridPager).Pages; } }

        public AjaxGrid(IQueryable<T> items, int page, bool renderOnlyRows)
            : base(items)
        {
            Pager = new AjaxGridPager(this) { CurrentPage = page }; 
            RenderOptions.RenderRowsOnly = renderOnlyRows;
        }

        public string ToJson(string gridPartialViewName, Controller controller)
        {
            var htmlHelper = new KlaHtmlHelpers();
            return htmlHelper.RenderPartialViewToString(gridPartialViewName, this, controller);
        }
    }
}