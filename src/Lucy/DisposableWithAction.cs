using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public class DisposableWithAction : IDisposable
    {
        #region Constructors

        public DisposableWithAction(Action action)
        {
            this.action = action;
        }

        #endregion Constructors

        #region Methods

        // Public Methods 

        public void Dispose()
        {
            action();
        }

        #endregion Methods

        #region Fields

        private Action action;

        #endregion Fields
    }
}
