using System;

namespace ADBTest
{
    internal class Program
    {
        private static void Main()
        {
            var start = new AdbServerHandler(@"D:\Program Files\platform-tools\adb.exe");
            var inputFiles = new string[]
            {
                @"D:\Program Files\Files\Testing1.txt",
                @"D:\Program Files\Files\Testing2.txt",
                @"D:\Program Files\Files\Testing3.txt",
                @"D:\Program Files\Files\Testing4.txt",
                @"D:\Program Files\Files\Testing5.txt"
            };
            var outputFiles = new string[]
            {
                @"D:\Program Files\Files\TrackingTestingAndroid1.log",
                @"D:\Program Files\Files\TrackingTestingAndroid2.log",
                @"D:\Program Files\Files\TrackingTestingAndroid3.log",
                @"D:\Program Files\Files\TrackingTestingAndroid4.log",
                @"D:\Program Files\Files\TrackingTestingAndroid5.log"
            };
            start.SetNext(new AdbClientHandler()).SetNext(
                new PackageManagerHandler(@"D:\Program Files\Files\com.finchtechnologies.trackingtestingandroid.apk")).SetNext(
                new FileUploadHandler(inputFiles)).SetNext(
                //new CommandLineHandler()).SetNext(
                new FileDownloadHandler(outputFiles)).SetNext(null);
            start.Handle();
            Console.ReadKey();
            //PackageManagerHandler.appdirectory = @"D:\Program Files\Files\com.finchtechnologies.trackingtestingandroid.apk";
            //FileUploadHandler.inputfile = @"D:\Program Files\Files\Testing.txt";
            //FileDownloadHandler.outputfile = @"D:\Program Files\Files\TrackingTestingAndroid.log";
            //var server = new ADBServerHandler();
            //var client = new ADBClientHandler();
            //var package = new PackageManagerHandler();
            //
            //server.SetNext(client).SetNext(package);
            //
            //server.Handle();



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
