using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(studyProject.Startup))]
namespace studyProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
