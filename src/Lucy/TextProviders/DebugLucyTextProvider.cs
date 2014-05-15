using System.Reflection;

namespace Lucy.TextProviders
{
    public class DebugLucyTextProvider : ILucyTextProvider
    {
		#region Methods 

		// Public Methods 

        public string GetLabelForObjectMember(MemberInfo member, string locale)
        {
            return string.Format("@Label-{1}:{0}@", member.Name, locale);
        }

        public string GetTranslation(string text, string locale)
        {
            return string.Format("@Translate-{1}:{0}@", text, locale);
        }

		#endregion Methods 
    }
}
