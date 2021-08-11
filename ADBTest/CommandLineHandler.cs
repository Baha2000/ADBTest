using SharpAdbClient;
using System.Linq;
using System.Threading;

namespace ADBTest
{
    class CommandLineHandler : BaseHandler
    {
        public override BaseHandler Handle()
        {
            Thread.Sleep(3000);
            ADBClientHandler.client.ExecuteRemoteCommand("pm grant com.finchtechnologies.trackingtestingandroid android.permission.WRITE_EXTERNAL_STORAGE", ADBClientHandler.client.GetDevices().First(), PackageManagerHandler.receiver);
            Thread.Sleep(3000);
            ADBClientHandler.client.ExecuteRemoteCommand("monkey -p com.finchtechnologies.trackingtestingandroid -c android.intent.category.LAUNCHER 1", ADBClientHandler.client.GetDevices().First(), PackageManagerHandler.receiver);
            Thread.Sleep(6000);
            ADBClientHandler.client.ExecuteRemoteCommand("input tap 360 640", ADBClientHandler.client.GetDevices().First(), PackageManagerHandler.receiver);
            Thread.Sleep(3000);

            SetNext(new FileDownloadHandler());
            return base.Handle();
        }
    }
}
