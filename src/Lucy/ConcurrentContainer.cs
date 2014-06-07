using System;
using System.Collections.Generic;
using System.Threading;

namespace Lucy
{
    public class ConcurrentContainer<TData>
    {
        #region Methods

        // Public Methods 

        /// <summary>
        /// Adds item if it doesn't already exist
        /// </summary>
        /// <param name="item"></param>
        public void AddIfNotExists(TData item)
        {
            _lockSlim.EnterUpgradeableReadLock();
            try
            {
                if (_list.Contains(item))
                    return;
                _lockSlim.EnterWriteLock();
                try
                {
                    if (!_list.Contains(item)) // I think: must check again
                        _list.Add(item);
                }
                finally
                {
                    _lockSlim.ExitWriteLock();
                }
            }
            finally
            {
                _lockSlim.ExitUpgradeableReadLock();
            }
        }

        public void Iterate(Action<TData> action)
        {
            TData[] lockedItems;
            _lockSlim.EnterReadLock();
            try
            {
                lockedItems = _list.ToArray();
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
            if (lockedItems.Length == 0)
                return;
            foreach (var data in lockedItems)
                action(data);
        }

        public List<TData> ToList()
        {
            _lockSlim.EnterReadLock();
            try
            {
                var result = new List<TData>(_list.Count);
                result.AddRange(_list);
                return result;
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
        }

        #endregion Methods

        #region Fields

        readonly List<TData> _list = new List<TData>();
        private readonly ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();

        #endregion Fields
    }

}
