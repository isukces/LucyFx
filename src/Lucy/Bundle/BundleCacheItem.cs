using System;
using System.IO;
using System.Linq;

namespace Lucy.Bundle
{
    internal class BundleCacheItem
    {
        #region Constructors

        public BundleCacheItem(FileInfo[] files)
        {
            _files = files;
            TimeBetweenFileChecking = TimeSpan.FromSeconds(10);
        }

        #endregion Constructors

        #region Static Methods

        // Private Methods 

        private static DateTime GetLastModificationDate(FileInfo[] files)
        {
            if (files == null || files.Length == 0) return DateTime.MinValue;
            return files.Max(fileInfo => GetLastModificationDate(fileInfo));
        }

        private static DateTime GetLastModificationDate(FileInfo fileInfo)
        {
            fileInfo.Refresh();
            var lastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
            var creationTimeUtc = fileInfo.CreationTimeUtc;
            return lastWriteTimeUtc > creationTimeUtc ? lastWriteTimeUtc : creationTimeUtc;
        }

        #endregion Static Methods

        #region Methods

        // Private Methods 

        private DateTime UpdateModificationTime(bool force)
        {
            var utcNow = DateTime.UtcNow;
            if (!force && utcNow < _dontUpdateModificationTimeUntilUtc)
                return _lastFileModified;
            _lastFileModified = GetLastModificationDate(_files);
            _dontUpdateModificationTimeUntilUtc = utcNow + TimeBetweenFileChecking;
            return _lastFileModified;
        }

        #endregion Methods

        #region Fields

        private string _content;
        private DateTime _dontUpdateModificationTimeUntilUtc;
        private readonly FileInfo[] _files;
        /// <summary>
        /// Ostatnio odczytany z pliku/plików  i odświeżany co jakiś czas
        /// </summary>
        private DateTime _lastFileModified;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Keszowana treść
        /// </summary>
        public string Content
        {
            get { return _content; }
            set
            {
                ContentTimestampUtc = UpdateModificationTime(true);
                _content = value;
            }
        }

        /// <summary>
        /// creation or last modification of newest file which was source of current content
        /// </summary>
        public DateTime ContentTimestampUtc { get; private set; }

        public string ETag
        {
            get { return ContentTimestampUtc.Ticks.ToString("x"); }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool NeedRebuild
        {
            get
            {
                UpdateModificationTime(false);
                return ContentTimestampUtc != _lastFileModified;
            }
        }

        /// <summary>
        /// Min differece between update <see cref="ContentTimestampUtc">ContentTimestampUtc</see>
        /// Default 10 sec.
        /// </summary>
        public TimeSpan TimeBetweenFileChecking { get; set; }

        #endregion Properties
    }
}