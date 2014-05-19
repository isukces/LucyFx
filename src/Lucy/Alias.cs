using System;

namespace Lucy
{
    public struct Alias : IEquatable<Alias>, IComparable<Alias>
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

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(_name);
            }
        }

        #endregion Properties

        #region Methods

        public Alias(string name)
            : this()
        {
            _name = name == null ? string.Empty : name.Trim();
        }

        public static implicit operator Alias(string src)
        {
            return new Alias(src);
        }

        public void Check()
        {
            Name.CheckAlias();
        }

        public int CompareTo(Alias other)
        {
            return String.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public bool Equals(Alias other)
        {
            return Name.Equals(other.Name, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return Name.ToLowerInvariant().GetHashCode();
        }

        public static implicit operator string(Alias src)
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
