using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace GridMvc.Sorting
{
    /// <summary>
    ///     Object applies order (OrderBy, OrderByDescending) for items collection
    /// </summary>
    internal class OrderByGridOrderer<T, TKey> : IColumnOrderer<T>
    {
        private readonly Expression<Func<T, TKey>> _expression;

        public OrderByGridOrderer(Expression<Func<T, TKey>> expression)
        {
            _expression = expression;
        }

        #region IColumnOrderer<T> Members

        public IQueryable<T> ApplyOrder(IQueryable<T> items)
        {
            return ApplyOrder(items, GridSortDirection.Ascending);
        }

        public IQueryable<T> ApplyOrder(IQueryable<T> items, GridSortDirection direction)
        {
            var comparer = new KeyComparer();

            switch (direction)
            {
                case GridSortDirection.Ascending:
                    return items.OrderBy(_expression, comparer);
                case GridSortDirection.Descending:
                    return items.OrderByDescending(_expression, comparer);
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }
        }

        #endregion

        internal class KeyComparer : IComparer<TKey>
        {
            public int Compare(TKey x, TKey y)
            {
                return StringComparer.Create(CultureInfo.CurrentCulture, false).Compare(x, y);
            }
        }
    }
}