using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GridMvc;
using GridMvc.Pagination;

namespace Grid.Mvc.Ajax.GridExtensions
{
    public class AjaxGridPager : IGridPager, IAjaxGridPager
    {
        private readonly IGrid _grid;
        public int PagePartitionSize { get; set; }

        public AjaxGridPager(IGrid grid, int itemCount = 0)
        {
            _grid = grid;
            //manual override item count
            ItemCount = itemCount;
        }

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }
        public int ItemCount { get; set; }

        public string TemplateName
        {
            get
            {
                //Custom view name to render this pager
                return "_AjaxGridPager";
            }
        }

        /// <summary>
        ///     Returns true if the pager has pages
        /// </summary>
        public bool HasPages
        {
            get
            {
                var itemCount = ItemCount;
                if (itemCount == 0)
                {
                    itemCount = _grid.ItemsToDisplay.Count();
                }
                else
                {
                    itemCount = ItemCount - (CurrentPage*(PageSize-1));
                }
                return itemCount > PageSize;
            }
        }

        public int Pages { get; set; }

        public void Initialize<T>(IQueryable<T> items)
        {
            int count = items.Count();
            Pages = count/PageSize;
            if (count%PageSize > 0)
                Pages++;
        }

        public void Initialize<T>(IEnumerable<T> items, int count)
        {
            Pages = count / PageSize;
            if (count % PageSize > 0)
                Pages++;
        }
    }
}