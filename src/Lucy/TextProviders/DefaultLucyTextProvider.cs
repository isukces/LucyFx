using System.Reflection;
using System.Text;

namespace Lucy.TextProviders
{
    public class DefaultLucyTextProvider : ILucyTextProvider
    {
        #region Static Methods

        // Private Methods 

        private static string Uncamel(string camelString)
        {
            var stringBuilder = new StringBuilder();
            foreach (var singleChar in camelString)
            {
                if (char.IsUpper(singleChar) && stringBuilder.Length > 0)
                    stringBuilder.Append(" " + singleChar.ToString().ToLower());
                else
                    stringBuilder.Append(singleChar);
            }
            return stringBuilder.ToString().Trim();
        }

        #endregion Static Methods

        #region Methods

        // Public Methods 

        public string GetLabelForObjectMember(MemberInfo member, string locale)
        {
            return Uncamel(member.Name);
        }

        public string GetTranslation(string text, string locale)
        {
            return text + "!!!";
        }

        #endregion Methods
    }
}
