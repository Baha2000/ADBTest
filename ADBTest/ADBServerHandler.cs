namespace ADBTest
{
    class ADBServerHandler : BaseHandler
    {
        public override object Handle(object request)
        {
                SharpAdbClient.AdbServer server = new();
                var result = server.StartServer(@"C:\Info\Android\platform-tools\adb.exe", restartServerIfNewer: false);
                System.Console.WriteLine(server.GetStatus());
                return base.Handle(server);
        }

    }
}
