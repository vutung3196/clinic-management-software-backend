using Autofac;
using ClinicManagementSoftware.Core.Interfaces;
using ClinicManagementSoftware.Core.Services;

namespace ClinicManagementSoftware.Core
{
    public class DefaultCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ToDoItemSearchService>()
                .As<IToDoItemSearchService>().InstancePerLifetimeScope();
        }
    }
}
