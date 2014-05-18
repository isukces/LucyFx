using System;
using Nancy.Extensions;

namespace Lucy
{
    public struct Filename : IEquatable<Filename>, IComparable<Filename>
    {
        #region Fields

        readonly string _name;
        private static readonly char[] SlashOrBackslash = { '/', '\\' };

        #endregion Fields

        #region Properties

        public string Extension
        {
            get
            {
                var shortName = ShortName;
                if (string.IsNullOrEmpty(shortName))
                    return "";
                var lastIndexOfDot = _name.LastIndexOf('.');
                return lastIndexOfDot < 0 ? "" : _name.Substring(lastIndexOfDot);
            }
        }

        public string ShortName
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                    return "";
                var lastIndexOfSlashOrBackslash = _name.LastIndexOfAny(SlashOrBackslash);
                return lastIndexOfSlashOrBackslash < 0
                    ? _name :
                    _name.Substring(lastIndexOfSlashOrBackslash + 1);
            }
        }

        public string Name
        {
            get
            {
                return _name ?? "";
            }
        }

        #endregion Properties

        #region Methods

        public void Check()
        {
            Name.CheckTildaPrefixedPath();
        }

        public int CompareTo(Filename other)
        {
            return String.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public bool Equals(Filename other)
        {
            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public Filename(string name)
            : this()
        {
            _name = (name ?? string.Empty).Trim();
        }

        public static implicit operator Filename(string src)
        {
            return new Filename(src);
        }

        internal string GetFullUriPath(Nancy.NancyContext nancyContext)
        {
            Check();
            return nancyContext.ToFullPath(Name);
        }

        public override int GetHashCode()
        {
            return Name.ToLowerInvariant().GetHashCode();
        }

        public static implicit operator string(Filename src)
        {
            return src.Name;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods
    }
}
