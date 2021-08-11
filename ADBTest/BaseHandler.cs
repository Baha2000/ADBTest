namespace ADBTest
{
    abstract class BaseHandler
    {
        private static BaseHandler nextHandler;

        public static BaseHandler SetNext(BaseHandler handler)
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
