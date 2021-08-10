using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADBTest
{
    abstract class BaseHandler : IHandler
    {
        private IHandler nextHandler;

        public IHandler SetNext(IHandler handler)
        {
            this.nextHandler = handler;
            return handler;
        }

        public virtual object Handle(object request)
        {
            if (this.nextHandler != null)
            {
                return this.nextHandler.Handle(request);
            }
            else
            {
                return null;
            }
        }
    }
}
