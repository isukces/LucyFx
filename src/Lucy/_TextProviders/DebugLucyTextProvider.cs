using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public class DebugLucyTextProvider : ILucyTextProvider
    {
        public string GetLabelForObjectMember(MemberInfo member, string locale)
        {
            return string.Format("@Label-{1}:{0}@", member.Name, locale);
        }


        public string GetTranslation(string text, string locale)
        {
            return string.Format("@Translate-{1}:{0}@", text, locale);
        }
    }
}
