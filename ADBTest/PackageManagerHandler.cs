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
        public static string appdirectory { get; set; }

        public static ConsoleOutputReceiver receiver { get; set;}

        public override BaseHandler Handle()
        {
            SharpAdbClient.DeviceCommands.PackageManager manager = new(ADBClientHandler.client, ADBClientHandler.client.GetDevices().First());
            manager.InstallPackage(appdirectory, reinstall: true);
            receiver = new();
            SetNext(new FileUploadHandler());
            return base.Handle();
        }
    }
}
