namespace ADBTest
{
    public class ADBServerHandler : BaseHandler
    {
        public string ServerDirectory { get; private set; }

        public ADBServerHandler(string directory)
        {
            ServerDirectory = directory;
        }
        public override BaseHandler Handle()
        {
                SharpAdbClient.AdbServer server = new();
                server.StartServer(ServerDirectory, restartServerIfNewer: false);
                System.Console.WriteLine(server.GetStatus());
                return base.Handle();
        }

    }
}
