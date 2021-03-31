using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace MetricsIntegrator.GUI.Skia.Tizen
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new MetricsIntegrator.GUI.App(), args);
            host.Run();
        }
    }
}
