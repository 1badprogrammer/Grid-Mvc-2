using System.Collections.Generic;
using System.Linq;

namespace Grid.Mvc.Ajax.GridExtensions
{
    public interface IAjaxGridFactory
    {
        IAjaxGrid CreateAjaxGrid<T>(IQueryable<T> gridItems, int page, bool renderOnlyRows, int pagePartitionSize = 0)
            where T : class;

        IAjaxGrid CreateAjaxGrid<T>(IEnumerable<T> gridItems, int page,int count,  int pageSize,bool renderOnlyRows, int pagePartitionSize = 0)
                 where T : class;
    }
}