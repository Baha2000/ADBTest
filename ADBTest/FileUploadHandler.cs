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
}
