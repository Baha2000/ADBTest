using SharpAdbClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace ADBTest
{
    class FileDownloadHandler : BaseHandler
    {
        public static string outputfile { get; set; }

        public override BaseHandler Handle()
        {
            var device = ADBClientHandler.client.GetDevices().First();

            using (SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device))
            using (Stream stream = File.OpenWrite(outputfile))
            {
                service.Pull("/sdcard/TrackingTestingAndroid.log", stream, null, CancellationToken.None);
            }
            System.Console.WriteLine("Success");
            return base.Handle();
        }
    }
}
