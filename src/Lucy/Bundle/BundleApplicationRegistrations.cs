using Nancy.Bootstrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy.Bundle
{
    public class BundleApplicationRegistrations : IRegistrations
    {
        public IEnumerable<CollectionTypeRegistration> CollectionTypeRegistrations
        {
            get
            {
                return null;
            }
        }

        public IEnumerable<InstanceRegistration> InstanceRegistrations
        {
            get
            {

                IEnumerable<LucyBundleRegistration> tmp = AppDomainAssemblyTypeScanner
                 .TypesOf<LucyBundleRegistration>()
                 .Select(type => (LucyBundleRegistration)Activator.CreateInstance(type))
                 .ToList();

                yield return new InstanceRegistration(typeof(IEnumerable<LucyBundleRegistration>), tmp);

            }
        }

        public IEnumerable<TypeRegistration> TypeRegistrations
        {
            get
            {
                return null;
                //IEnumerable<StyleBundle> styleBundles = AppDomainAssemblyTypeScanner
                //    .TypesOf<StyleBundle>()
                //    .Select(type => (StyleBundle)Activator.CreateInstance(type))
                //    .ToList();

                //yield return new InstanceRegistration(typeof(IEnumerable<StyleBundle>),
                //    styleBundles);
            }
        }
    }
}
