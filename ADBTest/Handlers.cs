using System;
using System.IO;
using System.Net;
using System.Threading;
using SharpAdbClient;
using SharpAdbClient.DeviceCommands;

namespace ADBTest
{
    public abstract class BaseHandler
    {
        private BaseHandler nextHandler;

        protected ConsoleOutputReceiver Receiver { get; set; }

        public BaseHandler SetNext(BaseHandler handler)
        {
            nextHandler = handler;
            return handler;
        }

        public virtual BaseHandler Handle() => nextHandler?.Handle();
    }

    public class AdbServerHandler : BaseHandler
    {
        private readonly string serverDirectory;

        public AdbServerHandler(string directory) => serverDirectory = directory;

        public override BaseHandler Handle()
        {
            AdbServer server = new();
            server.StartServer(serverDirectory, restartServerIfNewer: false);
            Console.WriteLine(server.GetStatus());
            return base.Handle();
        }
    }

    public class AdbClientHandler : BaseHandler
    {
        public static AdbClient Client { get; } = new();
        public static DeviceData Device { get; private set; }

        public override BaseHandler Handle()
        {
            var devices = Client.GetDevices();
            Console.WriteLine("Select device:");
            for (int i = 0; i < devices.Count; i++)
            {
                Console.WriteLine($"{i + 1} - {devices[i]}");
            }

            Device = devices[int.Parse(Console.ReadLine() ?? "1") - 1];
            return base.Handle();
        }
    }

    public class PackageManagerHandler : BaseHandler
    {
        private readonly string appDirectory;

        public PackageManagerHandler(string directory) => appDirectory = directory;

        public override BaseHandler Handle()
        {
            PackageManager manager = new(AdbClientHandler.Client, AdbClientHandler.Device);
            manager.InstallPackage(appDirectory, reinstall: true);
            Receiver = new ConsoleOutputReceiver();
            return base.Handle();
        }
    }

    public class CommandLineHandler : BaseHandler
    {
        private readonly TimeSpan delay = TimeSpan.FromSeconds(3);

        private const string permCommand =
            "pm grant com.finchtechnologies.trackingtestingandroid android.permission.WRITE_EXTERNAL_STORAGE";

        private const string appStartCommand =
            "monkey -p com.finchtechnologies.trackingtestingandroid -c android.intent.category.LAUNCHER 1";

        private const string tapCommand = "input tap 360 640";


        public override BaseHandler Handle()
        {
            Thread.Sleep(delay);
            AdbClientHandler.Client.ExecuteRemoteCommand(permCommand, AdbClientHandler.Device, Receiver);
            Thread.Sleep(delay);
            AdbClientHandler.Client.ExecuteRemoteCommand(appStartCommand, AdbClientHandler.Device, Receiver);
            Thread.Sleep(delay);
            AdbClientHandler.Client.ExecuteRemoteCommand(tapCommand, AdbClientHandler.Device, Receiver);
            Thread.Sleep(delay);

            return base.Handle();
        }
    }

    public class FileUploadHandler : BaseHandler
    {
        private readonly string[] inputfile;
        private string fileName;
        private const string apkDirectory = "/sdcard/Databases/Standard/";

        public FileUploadHandler(string[] directory) => inputfile = directory;

        public override BaseHandler Handle()
        {
            var device = AdbClientHandler.Device;

            foreach (var file in inputfile)
            {
                fileName = file.Remove(0, file.LastIndexOf('\\') + 1);
                using var service =
                    new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device);
                using Stream stream = File.OpenRead(file);
                service.Push(stream, Path.Combine(apkDirectory, fileName), 444, DateTime.Now, null,
                    CancellationToken.None);
            }

            return base.Handle();
        }
    }

    public class FileDownloadHandler : BaseHandler
    {
        private readonly string[] outputfile;
        private readonly TimeSpan delay = TimeSpan.FromSeconds(1);

        private const string checkFileCommandTemplate =
            "FILE=/sdcard/DataBases/Emulated/{0} ; if test -f \"$FILE\"; then echo \"exist\" ; fi";

        private const string apkDirectory = "/sdcard/Databases/Emulated/";
        private string fileName;

        public FileDownloadHandler(string[] directory) => outputfile = directory;

        public override BaseHandler Handle()
        {
            var device = AdbClientHandler.Device;

            foreach (var file in outputfile)
            {
                Receiver = new ConsoleOutputReceiver();
                fileName = file.Remove(0, file.LastIndexOf('\\') + 1);
                var command = string.Format(checkFileCommandTemplate, fileName);
                while (Receiver.ToString() != "exist\r\n")
                {
                    AdbClientHandler.Client.ExecuteRemoteCommand(command, AdbClientHandler.Device, Receiver);
                    Console.WriteLine(Receiver.ToString());
                    Thread.Sleep(delay);
                }

                Console.WriteLine($"I found {file}");
                using var service =
                    new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device);
                using Stream stream = File.OpenWrite(file);
                service.Pull(Path.Combine(apkDirectory, fileName), stream, null, CancellationToken.None);
            }

            Console.WriteLine("Success");
            return base.Handle();
        }
    }
}