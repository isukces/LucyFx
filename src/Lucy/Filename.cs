using System;
using Nancy.Extensions;

namespace Lucy
{
    public struct Filename : IEquatable<Filename>, IComparable<Filename>
    {
        #region Fields

        readonly string _name;

        #endregion Fields

        #region Properties

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
            _name = name;
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
