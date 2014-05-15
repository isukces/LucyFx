using System;
using System.Diagnostics;

namespace Lucy
{
    public class DisposableWithAction : IDisposable
    {
        #region Constructors

        public DisposableWithAction(Action action)
        {
            Debug.Assert(action != null, "action != null");
            _action = action;
        }

        #endregion Constructors

        #region Methods

        // Public Methods 

        public void Dispose()
        {
            _action();
        }

        #endregion Methods

        #region Fields

        private readonly Action _action;

        #endregion Fields
    }
}
