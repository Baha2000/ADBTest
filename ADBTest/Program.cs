using System;
using System.IO;
using System.Net;
using System.Threading;
using SharpAdbClient;
using SharpAdbClient.DeviceCommands;

namespace ADBTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new ADBServerHandler();
            var client = new ADBClientHandler();
            var package = new PackageManagerHandler();
            
            server.SetNext(client).SetNext(package);
            
            server.Handle("Start");
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
