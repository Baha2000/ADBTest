﻿using SharpAdbClient;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

namespace ADBTest
{
    class FileUploadHandler : BaseHandler
    {
        public static string inputfile { get; set; }

        public override BaseHandler Handle()
        {
            var device = ADBClientHandler.client.GetDevices().First();

            using (SyncService service = new SyncService(new AdbSocket(new IPEndPoint(IPAddress.Loopback, AdbClient.AdbServerPort)), device))
            using (Stream stream = File.OpenRead(inputfile))
            {
                service.Push(stream, "/sdcard/Databases/Standard/config", 444, DateTime.Now, null, CancellationToken.None);
            }

            SetNext(new CommandLineHandler());
            return base.Handle();
        }
    }
}