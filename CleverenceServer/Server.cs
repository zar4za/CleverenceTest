using System.Threading;

namespace CleverenceServer
{
    public static class Server
    {
        private static int _count = 0;
        private static readonly object _lockObject = new object();

        public static void AddToCount(int value)
        {
            lock (_lockObject)
            {
                _count += value;
                Thread.Sleep(200);
            }
        }

        public static int GetCount()
        {
            lock (_lockObject)
            {
                return _count;
            }
        }
    }
}
