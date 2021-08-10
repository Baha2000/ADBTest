namespace ADBTest
{
    class ADBClientHandler : BaseHandler
    {
        public override object Handle(object request)
        {
            SharpAdbClient.AdbClient client = new();
            var devices = client.GetDevices();
            foreach (var device in devices)
            {
                System.Console.WriteLine(device);
            }
            return base.Handle(client);
        }
    }
}
