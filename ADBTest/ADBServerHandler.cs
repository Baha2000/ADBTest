namespace ADBTest
{
    class ADBServerHandler : BaseHandler
    {
        public override BaseHandler Handle()
        {
                SharpAdbClient.AdbServer server = new();
                var result = server.StartServer(@"C:\Info\Android\platform-tools\adb.exe", restartServerIfNewer: false);
                System.Console.WriteLine(server.GetStatus());
                SetNext(new ADBClientHandler());
                return base.Handle();
        }

    }
}
