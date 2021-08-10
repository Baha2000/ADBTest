using System.Linq;

namespace ADBTest
{
    class PackageManagerHandler : BaseHandler
    {
        public override object Handle(object request)
        {
            SharpAdbClient.AdbClient client = (SharpAdbClient.AdbClient)request;
            SharpAdbClient.DeviceCommands.PackageManager manager = new SharpAdbClient.DeviceCommands.PackageManager(client, client.GetDevices().First());
            manager.InstallPackage(@"C:\Info\Android\com.finchtechnologies.trackingtestingandroid.apk", reinstall: true);
            System.Console.WriteLine("Success");
            return base.Handle(manager);
        }

    }
}
