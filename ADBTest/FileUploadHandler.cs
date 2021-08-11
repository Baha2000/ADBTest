using SharpAdbClient;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace ADBTest
{
    public class FileUploadHandler : BaseHandler
    {
        public string Inputfile { get; private set; }

        public FileUploadHandler(string directory)
        {
            Inputfile = directory;
        }

        public override BaseHandler Handle()
        {
            var device = ADBClientHandler.client.GetDevices().First();

            using (SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device))
            using (Stream stream = File.OpenRead(Inputfile))
            {
                service.Push(stream, "/sdcard/Databases/Standard/config", 444, DateTime.Now, null, CancellationToken.None);
            }

            return base.Handle();
        }
    }
}
