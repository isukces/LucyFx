using System.Reflection;

namespace Lucy.TextProviders
{
    public interface ILucyTextProvider
    {
        string GetLabelForObjectMember(MemberInfo member, string locale);

        string GetTranslation(string text, string locale);
    }
}
