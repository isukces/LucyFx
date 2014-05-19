using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy.Bundle
{
    public struct AliasBundleType
    {
        #region Properties

        public Alias Alias { get; set; }

        public BundleTypes BundleType { get; set; }

        #endregion Properties

        #region Methods

        public AliasBundleType(Alias alias, BundleTypes bundleType)
            : this()
        {
            Alias = alias;
            BundleType = bundleType;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Alias, BundleType);
        }

        #endregion Methods
    }
}
