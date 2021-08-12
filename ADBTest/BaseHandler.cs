using SharpAdbClient;

namespace ADBTest
{
    public abstract class BaseHandler
    {
        private BaseHandler nextHandler;
        
        protected ConsoleOutputReceiver Receiver { get; set;}

        public BaseHandler SetNext(BaseHandler handler)
        {
            nextHandler = handler;
            return handler;
        }

        public virtual BaseHandler Handle()
        {
            if (nextHandler != null)
            {
                return nextHandler.Handle();
            }
            else
            {
                return null;
            }
        }
    }
}
