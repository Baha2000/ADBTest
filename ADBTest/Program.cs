using System;

namespace ADBTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var start = new ADBServerHandler(@"D:\Program Files\platform-tools\adb.exe");
            string[] Inputfiles = new string[5];

            Inputfiles[0] = @"D:\Program Files\Files\Testing1.txt";
            Inputfiles[1] = @"D:\Program Files\Files\Testing2.txt";
            Inputfiles[2] = @"D:\Program Files\Files\Testing3.txt";
            Inputfiles[3] = @"D:\Program Files\Files\Testing4.txt";
            Inputfiles[4] = @"D:\Program Files\Files\Testing5.txt";
            start.SetNext(new ADBClientHandler()).SetNext(
                new PackageManagerHandler(@"D:\Program Files\Files\com.finchtechnologies.trackingtestingandroid.apk")).SetNext(
                new FileUploadHandler(Inputfiles)).SetNext(
                new CommandLineHandler()).SetNext(
                new FileDownloadHandler(@"D:\Program Files\Files\TrackingTestingAndroid.log"));
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
