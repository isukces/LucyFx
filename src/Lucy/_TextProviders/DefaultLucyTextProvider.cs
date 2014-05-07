using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public class DefaultLucyTextProvider : ILucyTextProvider
    {
        #region Methods

        // Public Methods 

        public string GetLabelForObjectMember(MemberInfo member, string locale)
        {
            return Uncamel(member.Name);
        }
        // Private Methods 

        private string Uncamel(string camelString)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var singleChar in camelString)
            {
                if (char.IsUpper(singleChar) && stringBuilder.Length > 0)
                    stringBuilder.Append(" " + singleChar.ToString().ToLower());
                else
                    stringBuilder.Append(singleChar);
            }
            return stringBuilder.ToString().Trim();
        }

        #endregion Methods

        public static void RegisterMe(Nancy.TinyIoc.TinyIoCContainer container)
        {
            container.Register<ILucyTextProvider>(new DefaultLucyTextProvider());
        }


        public string GetTranslation(string text, string locale)
        {
            return text + "!!!";
        }
    }
}
