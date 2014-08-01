using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace GridMvc.Sorting
{
    /// <summary>
    ///     Object applies ThenBy and ThenByDescending order for items collection
    /// </summary>
    internal class ThenByColumnOrderer<T, TKey> : IColumnOrderer<T>
    {
        private readonly Expression<Func<T, TKey>> _expression;
        private readonly GridSortDirection _initialDirection;

        public ThenByColumnOrderer(Expression<Func<T, TKey>> expression, GridSortDirection initialDirection)
        {
            _expression = expression;
            _initialDirection = initialDirection;
        }

        #region IColumnOrderer<T> Members

        public IQueryable<T> ApplyOrder(IQueryable<T> items)
        {
            var comparer = new KeyComparer();
            var ordered = items as IOrderedQueryable<T>;
            if (ordered == null) return items; //not ordered collection

            switch (_initialDirection)
            {
                case GridSortDirection.Ascending:
                    return ordered.ThenBy(_expression, comparer);
                case GridSortDirection.Descending:
                    return ordered.ThenByDescending(_expression, comparer);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IQueryable<T> ApplyOrder(IQueryable<T> items, GridSortDirection direction)
        {
            return ApplyOrder(items);
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