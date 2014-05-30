using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.TinyIoc;

namespace Lucy.SampleServer.Code
{
 
    public class Bootstrapper:DefaultNancyBootstrapper
    {
        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines,
            NancyContext context)
        {
            base.RequestStartup(requestContainer, pipelines, context);
            LucyEngine.RequestStartup(requestContainer, context); // spięcie kontenera 
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            Conventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("assets", "assets"));
        }
    }
}