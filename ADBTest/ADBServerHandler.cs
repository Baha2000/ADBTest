namespace ADBTest
{
    class ADBServerHandler : BaseHandler
    {
        public static string serverdirectory { get; set; }
        public override BaseHandler Handle()
        {
                SharpAdbClient.AdbServer server = new();
                var result = server.StartServer(@"C:\Info\Android\platform-tools\adb.exe", restartServerIfNewer: false);
                System.Console.WriteLine(server.GetStatus());
                return base.Handle();
        }

    }
}
