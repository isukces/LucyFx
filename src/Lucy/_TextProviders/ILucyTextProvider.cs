using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public interface ILucyTextProvider
    {
        string GetLabelForObjectMember(MemberInfo member, string locale);

        string GetTranslation(string text, string locale);
    }
}
