using SharpAdbClient;
using System.Linq;
using System.Threading;

namespace ADBTest
{
    public class CommandLineHandler : BaseHandler
    {
        private const int time = 3000;

        public override BaseHandler Handle()
        {
            Thread.Sleep(time);
            ADBClientHandler.client.ExecuteRemoteCommand("pm grant com.finchtechnologies.trackingtestingandroid android.permission.WRITE_EXTERNAL_STORAGE", ADBClientHandler.client.GetDevices().First(), PackageManagerHandler.Receiver);
            Thread.Sleep(time);
            ADBClientHandler.client.ExecuteRemoteCommand("monkey -p com.finchtechnologies.trackingtestingandroid -c android.intent.category.LAUNCHER 1", ADBClientHandler.client.GetDevices().First(), PackageManagerHandler.Receiver);
            Thread.Sleep(time);
            ADBClientHandler.client.ExecuteRemoteCommand("input tap 360 640", ADBClientHandler.client.GetDevices().First(), PackageManagerHandler.Receiver);
            Thread.Sleep(time);

            return base.Handle();
        }
    }
}
