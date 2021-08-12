using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using SharpAdbClient;

namespace ADBTest
{
    public abstract class BaseHandler
    {
        private BaseHandler _nextHandler;

        protected ConsoleOutputReceiver Receiver { get; set; }

        public BaseHandler SetNext(BaseHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual BaseHandler Handle()
        {
            if (_nextHandler != null)
            {
                return _nextHandler.Handle();
            }
            else
            {
                return null;
            }
        }
    }

    public class AdbServerHandler : BaseHandler
    {
        private string ServerDirectory { get; set; }

        public AdbServerHandler(string directory)
        {
            ServerDirectory = directory;
        }

        public override BaseHandler Handle()
        {
            AdbServer server = new();
            server.StartServer(ServerDirectory, restartServerIfNewer: false);
            Console.WriteLine(server.GetStatus());
            return base.Handle();
        }
    }

    public class AdbClientHandler : BaseHandler
    {
        public static AdbClient Client { get; private set; }

        public override BaseHandler Handle()
        {
            Client = new AdbClient();
            var devices = Client.GetDevices();
            foreach (var device in devices)
            {
                Console.WriteLine(device);
            }

            return base.Handle();
        }
    }

    public class PackageManagerHandler : BaseHandler
    {
        private string AppDirectory { get; set; }

        public PackageManagerHandler(string directory)
        {
            AppDirectory = directory;
        }

        public override BaseHandler Handle()
        {
            SharpAdbClient.DeviceCommands.PackageManager manager = new(AdbClientHandler.Client,
                AdbClientHandler.Client.GetDevices().First());
            manager.InstallPackage(AppDirectory, reinstall: true);
            Receiver = new();
            return base.Handle();
        }
    }

    public class CommandLineHandler : BaseHandler
    {
        private const int Time = 3000;

        private const string PermCommand =
            "pm grant com.finchtechnologies.trackingtestingandroid android.permission.WRITE_EXTERNAL_STORAGE";

        private const string AppStartCommand =
            "monkey -p com.finchtechnologies.trackingtestingandroid -c android.intent.category.LAUNCHER 1";

        private const string TapCommand = "input tap 360 640";


        public override BaseHandler Handle()
        {
            Thread.Sleep(Time);
            AdbClientHandler.Client.ExecuteRemoteCommand(PermCommand, AdbClientHandler.Client.GetDevices().First(),
                Receiver);
            Thread.Sleep(Time);
            AdbClientHandler.Client.ExecuteRemoteCommand(AppStartCommand, AdbClientHandler.Client.GetDevices().First(),
                Receiver);
            Thread.Sleep(Time);
            AdbClientHandler.Client.ExecuteRemoteCommand(TapCommand, AdbClientHandler.Client.GetDevices().First(),
                Receiver);
            Thread.Sleep(Time);

            return base.Handle();
        }
    }

    public class FileUploadHandler : BaseHandler
    {
        private string[] Inputfile { get; set; }
        private string FileName { get; set; }
        private const string ApkDirectory = "/sdcard/Databases/Emulated/";

        public FileUploadHandler(string[] directory)
        {
            Inputfile = directory;
        }

        public override BaseHandler Handle()
        {
            var device = AdbClientHandler.Client.GetDevices().First();

            foreach (var file in Inputfile)
            {
                FileName = file.Remove(0, file.LastIndexOf('\\') + 1);
                using var service =
                    new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device);
                using Stream stream = File.OpenRead(file);
                {
                    service.Push(stream, ApkDirectory + FileName, 444, DateTime.Now, null, CancellationToken.None);
                }
            }

            return base.Handle();
        }
    }

    public class FileDownloadHandler : BaseHandler
    {
        private string[] Outputfile { get; set; }
        private const int Time = 1000;
        private const string PrefixCommand = "FILE=/sdcard/DataBases/Emulated/";
        private const string PostfixCommand = " ; if test -f \"$FILE\"; then echo \"exist\" ; fi";
        private const string ApkDirectory = "/sdcard/Databases/Emulated/";
        private string FileName { get; set; }

        public FileDownloadHandler(string[] directory)
        {
            Outputfile = directory;
        }

        public override BaseHandler Handle()
        {
            var device = AdbClientHandler.Client.GetDevices().First();

            foreach (string file in Outputfile)
            {
                Receiver = new ConsoleOutputReceiver();
                FileName = file.Remove(0, file.LastIndexOf('\\') + 1);
                while (Receiver.ToString() != "exist\r\n")
                {
                    AdbClientHandler.Client.ExecuteRemoteCommand(PrefixCommand + FileName + PostfixCommand,
                        AdbClientHandler.Client.GetDevices().First(), Receiver);
                    Console.WriteLine(Receiver.ToString());
                    Thread.Sleep(Time);
                }

                Console.WriteLine($"I found {file}");
                using var service =
                    new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device);
                using Stream stream = File.OpenWrite(file);
                {
                    service.Pull(ApkDirectory + FileName, stream, null, CancellationToken.None);
                }
            }

            Console.WriteLine("Success");
            return base.Handle();
        }
    }
}