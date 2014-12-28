using System;
using System.Globalization;

namespace Lucy
{
    static class HtmlDataSerialize
    {
        #region Static Methods

        // Internal Methods 

        internal static string ToString(object obj)
        {
            if (obj == null)
                return "";
            var stringValue = obj as string;
            if (stringValue != null)
                return stringValue;
            if (obj is int)
                return ((int)obj).ToString(CultureInfo.InvariantCulture);
            if (obj is Guid)
                return ((Guid) obj).ToString("B");
            throw new NotImplementedException();
        }

        #endregion Static Methods
    }
}
