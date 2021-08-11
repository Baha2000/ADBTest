using SharpAdbClient;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace ADBTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = new ADBServerHandler();

            start.SetNext(new ADBClientHandler()).SetNext(new PackageManagerHandler()).SetNext(new FileUploadHandler()).SetNext(new CommandLineHandler()).
                SetNext(new FileDownloadHandler());

            PackageManagerHandler.appdirectory = @"C:\Info\Android\com.finchtechnologies.trackingtestingandroid.apk";
            FileUploadHandler.inputfile = @"C:\config";
            FileDownloadHandler.outputfile = @"C:\Users\aliev\TrackingTestingAndroid.log";
            start.Handle();

            //var server = new ADBServerHandler();
            //var client = new ADBClientHandler();
            //var package = new PackageManagerHandler();
            //
            //server.SetNext(client).SetNext(package);
            //
            //server.Handle();
            Console.ReadKey();


            //SharpAdbClient.AdbServer server = new();
            //var result = server.StartServer(@"C:\Info\Android\platform-tools\adb.exe", restartServerIfNewer: false);
            //
            //AdbClient client = new();
            //var devices = client.GetDevices();
            //
            //foreach (var device in devices)
            //{
            //    Console.WriteLine(device);
            //}
            //Console.ReadKey();
            //SharpAdbClient.DeviceCommands.PackageManager manager = new SharpAdbClient.DeviceCommands.PackageManager(client, devices[0]);
            //
            //manager.InstallPackage(@"C:\Info\Android\com.finchtechnologies.trackingtestingandroid.apk", reinstall: true);
            //
            ////команда для передачи на пуш файл adb shell pm grant com.finchtechnologies.trackingtestingandroid android.permission.WRITE_EXTERNAL_STORAGE
            //var receiver = new ConsoleOutputReceiver();
            ////client.ExecuteRemoteCommand("adb shell", devices[0], receiver);
            ////Console.WriteLine(receiver.ToString());
            ////client.ExecuteRemoteCommand("su", devices[0], receiver);
            ////Console.WriteLine(receiver.ToString());
            //client.ExecuteRemoteCommand("mount -o remount,rw /system",  devices[0], receiver);
            //Console.WriteLine(receiver.ToString());
            //Console.ReadKey();
            //
            //using (SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), devices[0]))
            //using (Stream stream = File.OpenRead(@"C:\Info\config"))
            //{
            //    service.Push(stream, "/Databases/Standard/", 444, DateTime.Now, null, CancellationToken.None);
            //}
            //
            //Console.ReadKey();

        }


    }
}
