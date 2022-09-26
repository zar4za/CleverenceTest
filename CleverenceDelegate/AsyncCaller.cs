using System;

namespace CleverenceDelegate
{
    public class AsyncCaller
    {
        private readonly EventHandler _handler;

        public AsyncCaller(EventHandler handler)
        {
            _handler = handler;
        }

        public bool Invoke(int timeout, AsyncCallback callback, EventArgs args)
        {
            var res = _handler.BeginInvoke(null, args, callback, null);
            return res.AsyncWaitHandle.WaitOne(timeout);
        }
    }
}
