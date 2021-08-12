using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using SharpAdbClient;

namespace ADBTest
{
    public class ADBServerHandler : BaseHandler
    {
        public string ServerDirectory { get; private set; }

        public ADBServerHandler(string directory)
        {
            ServerDirectory = directory;
        }
        public override BaseHandler Handle()
        {
                SharpAdbClient.AdbServer server = new();
                server.StartServer(ServerDirectory, restartServerIfNewer: false);
                System.Console.WriteLine(server.GetStatus());
                return base.Handle();
        }

    }
    
    public class ADBClientHandler : BaseHandler
    {
        public static SharpAdbClient.AdbClient client { get; private set; }

        public override BaseHandler Handle()
        {
            client = new();
            var devices = client.GetDevices();
            foreach (var device in devices)
            {
                System.Console.WriteLine(device);
            }
            return base.Handle();
        }
    }
    
    public class PackageManagerHandler : BaseHandler
    {

        public string AppDirectory { get; private set; }

        //public static ConsoleOutputReceiver Receiver { get; set;}

        public PackageManagerHandler(string directory)
        {
            AppDirectory = directory;
            
        }

        public override BaseHandler Handle()
        {
            SharpAdbClient.DeviceCommands.PackageManager manager = new(ADBClientHandler.client, ADBClientHandler.client.GetDevices().First());
            manager.InstallPackage(AppDirectory, reinstall: true);
            Receiver = new();
            return base.Handle();
        }
    }
    
    public class CommandLineHandler : BaseHandler
    {
        private const int time = 3000;

        public override BaseHandler Handle()
        {
            Thread.Sleep(time);
            ADBClientHandler.client.ExecuteRemoteCommand("pm grant com.finchtechnologies.trackingtestingandroid android.permission.WRITE_EXTERNAL_STORAGE", ADBClientHandler.client.GetDevices().First(), Receiver);
            Thread.Sleep(time);
            ADBClientHandler.client.ExecuteRemoteCommand("monkey -p com.finchtechnologies.trackingtestingandroid -c android.intent.category.LAUNCHER 1", ADBClientHandler.client.GetDevices().First(), Receiver);
            Thread.Sleep(time);
            ADBClientHandler.client.ExecuteRemoteCommand("input tap 360 640", ADBClientHandler.client.GetDevices().First(), Receiver);
            Thread.Sleep(time);

            return base.Handle();
        }
    }
    
    public class FileUploadHandler : BaseHandler
    {
        public string[] Inputfile { get; private set; }

        public FileUploadHandler(string[] directory)
        {
            Inputfile = directory;
        }

        public override BaseHandler Handle()
        {
            var device = ADBClientHandler.client.GetDevices().First();

            foreach (string file in Inputfile)
            {
                using (SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device))
                using (Stream stream = File.OpenRead(file))
                {
                    //service.Push(stream, $"/sdcard/Databases/Standard/{file.Remove(0, file.LastIndexOf('\\') + 1)}", 444, DateTime.Now, null, CancellationToken.None);
                    service.Push(stream, $"/sdcard/Databases/Emulated/{file.Remove(0, file.LastIndexOf('\\') + 1)}", 444, DateTime.Now, null, CancellationToken.None);
                }
            }
            return base.Handle();
        }
    }
    
    public class FileDownloadHandler : BaseHandler
    {
        public string[] Outputfile { get; private set; }

        public FileDownloadHandler(string[] directory)
        {
            Outputfile = directory;
        }

        public override BaseHandler Handle()
        {
            var device = ADBClientHandler.client.GetDevices().First();

            foreach (string file in Outputfile)
            {
                //while (PackageManagerHandler.Receiver.ToString() != "exist")
                //{
                Receiver = new();// вот это смотри
                ADBClientHandler.client.ExecuteRemoteCommand($"FILE=/sdcard/DataBases/Emulated/{file.Remove(0, file.LastIndexOf('\\') + 1)} ; if test -f \"$FILE\"; then echo \"exist\" ; fi", ADBClientHandler.client.GetDevices().First(), Receiver);
                System.Console.WriteLine(Receiver.ToString());
                //}
                System.Console.WriteLine($"I found {file}");
                using (SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device))
                using (Stream stream = File.OpenWrite(file))
                {
                    service.Pull($"/sdcard/Databases/Emulated/{file.Remove(0, file.LastIndexOf('\\') + 1)}", stream, null, CancellationToken.None);
                }
            }
            System.Console.WriteLine("Success");
            return base.Handle();
        }
    }
    
    
}
