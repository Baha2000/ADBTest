using SharpAdbClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace ADBTest
{
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
                    PackageManagerHandler.Receiver = new();// вот это смотри
                    ADBClientHandler.client.ExecuteRemoteCommand($"FILE=/sdcard/DataBases/Emulated/{file.Remove(0, file.LastIndexOf('\\') + 1)} ; if test -f \"$FILE\"; then echo \"exist\" ; fi", ADBClientHandler.client.GetDevices().First(), PackageManagerHandler.Receiver);
                    System.Console.WriteLine(PackageManagerHandler.Receiver.ToString());
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
