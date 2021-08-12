using System.Linq;
using SharpAdbClient;

namespace ADBTest
{
    public class PackageManagerHandler : BaseHandler
    {

        public string AppDirectory { get; private set; }

        public static ConsoleOutputReceiver Receiver { get; set;}

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
}
