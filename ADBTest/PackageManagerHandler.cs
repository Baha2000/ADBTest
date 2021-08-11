using SharpAdbClient;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace ADBTest
{
    class PackageManagerHandler : BaseHandler
    {
        public override BaseHandler Handle()
        {
            SharpAdbClient.DeviceCommands.PackageManager manager = new(ADBClientHandler.client, ADBClientHandler.client.GetDevices().First());
            manager.InstallPackage(@"C:\Info\Android\com.finchtechnologies.trackingtestingandroid.apk", reinstall: true);
            var receiver = new ConsoleOutputReceiver();
            ADBClientHandler.client.ExecuteRemoteCommand("pm grant com.finchtechnologies.trackingtestingandroid android.permission.WRITE_EXTERNAL_STORAGE", ADBClientHandler.client.GetDevices().First(), receiver);
            //System.Console.WriteLine("Success");
            UploadFile();
            ADBClientHandler.client.ExecuteRemoteCommand("monkey -p com.finchtechnologies.trackingtestingandroid -c android.intent.category.LAUNCHER 1", ADBClientHandler.client.GetDevices().First(), receiver);
            Thread.Sleep(6000);
            ADBClientHandler.client.ExecuteRemoteCommand("input tap 360 640", ADBClientHandler.client.GetDevices().First(), receiver);
            Thread.Sleep(6000);
            System.Console.WriteLine(receiver.ToString());
            DownloadFile();
            
            SetNext(null);
            return base.Handle();
        }
        public void DownloadFile()
        {
            var device = ADBClientHandler.client.GetDevices().First();

            using (SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device))
            using (Stream stream = File.OpenWrite(@"C:\Users\aliev\TrackingTestingAndroid.log"))
            {
                service.Pull("/sdcard/TrackingTestingAndroid.log", stream, null, CancellationToken.None);
            }
        }

        public void UploadFile()
        {
            var device = ADBClientHandler.client.GetDevices().First();

            using (SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device))
            using (Stream stream = File.OpenRead(@"C:\config"))
            {
                service.Push(stream, "/sdcard/Databases/Standard/config", 444, DateTime.Now, null, CancellationToken.None);
            }
        }

    }
}
