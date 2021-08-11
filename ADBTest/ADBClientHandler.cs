namespace ADBTest
{
    class ADBClientHandler : BaseHandler
    {
        public static SharpAdbClient.AdbClient client { get; private set; }

        public override BaseHandler Handle()
        {
            client = new();
            var devices = client.GetDevices();
            foreach (var device in devices)
            {
                System.Console.WriteLine(device);
            }
            return base.Handle();
        }
    }
}
