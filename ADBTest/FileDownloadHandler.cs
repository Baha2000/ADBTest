using SharpAdbClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace ADBTest
{
    public class FileDownloadHandler : BaseHandler
    {
        public string Outputfile { get; private set; }

        public FileDownloadHandler(string directory)
        {
            Outputfile = directory;
        }

        public override BaseHandler Handle()
        {
            var device = ADBClientHandler.client.GetDevices().First();

            using (SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device))
            using (Stream stream = File.OpenWrite(Outputfile))
            {
                service.Pull("/sdcard/TrackingTestingAndroid.log", stream, null, CancellationToken.None);
            }
            System.Console.WriteLine("Success");
            return base.Handle();
        }
    }
}
