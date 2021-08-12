using System;

namespace ADBTest
{
    internal class Program
    {
        private static void Main()
        {
            var start = new AdbServerHandler(@"D:\Program Files\platform-tools\adb.exe");
            var inputFiles = new string[]
            {
                @"D:\Program Files\Files\Testing1.txt",
                @"D:\Program Files\Files\Testing2.txt",
                @"D:\Program Files\Files\Testing3.txt",
                @"D:\Program Files\Files\Testing4.txt",
                @"D:\Program Files\Files\Testing5.txt"
            };
            var outputFiles = new string[]
            {
                @"D:\Program Files\Files\TrackingTestingAndroid1.log",
                @"D:\Program Files\Files\TrackingTestingAndroid2.log",
                @"D:\Program Files\Files\TrackingTestingAndroid3.log",
                @"D:\Program Files\Files\TrackingTestingAndroid4.log",
                @"D:\Program Files\Files\TrackingTestingAndroid5.log"
            };
            start.SetNext(new AdbClientHandler()).SetNext(
                new PackageManagerHandler(@"D:\Program Files\Files\com.finchtechnologies.trackingtestingandroid.apk")).SetNext(
                new FileUploadHandler(inputFiles)).SetNext(
                //new CommandLineHandler()).SetNext(
                new FileDownloadHandler(outputFiles)).SetNext(null);
            start.Handle();
            Console.ReadKey();
        }
    }
}
