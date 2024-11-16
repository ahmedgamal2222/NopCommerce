using Nop.Core;
using Nop.Services.Plugins;

namespace Nop.Plugin.Customers.DynamicsIntegration
{
    public class DynamicsIntegrationPlugin : BasePlugin, IPlugin
    {
        public override async Task InstallAsync()
        {
            // كود التثبيت
            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            // كود إلغاء التثبيت
            await base.UninstallAsync();
        }
    }
}
