using System;
using System.Reflection;
using JetBrains.Annotations;

namespace Lucy
{
    public class ExtendedMemberInfo
    {
        public ExtendedMemberInfo(MemberInfo member, string path)
        {
            Member = member;
            Path = path;
        }

        public override string ToString()
        {
            return Path;
        }
        public MemberInfo Member { get; private set; }
        public string Path { get; private set; }

        public string HtmlFormName
        {
            get
            {
                return Path; // .Replace(".", "__");
            }
        }

        public string HtmlId
        {
            get
            {
                return Path; // .Replace(".", "__");
            }
        }

        [NotNull]
        public static ExtendedMemberInfo FromMember([NotNull] MemberInfo member)
        {
            if (member == null) throw new ArgumentNullException("member");
            return new ExtendedMemberInfo(member, member.Name);
        }

        [NotNull]
        public Type GetDataType()
        {
            if (Member is PropertyInfo)
                return (Member as PropertyInfo).PropertyType;
            if (Member is FieldInfo)
                return (Member as FieldInfo).FieldType;
            throw new NotSupportedException();
        }
    }
}
