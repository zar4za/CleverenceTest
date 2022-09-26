using CleverenceServer;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace CleverenceServerTests
{
    public class ServerTest
    {
        private int _expectedCount;
        private int _size;
        private int _addCount;

        [SetUp]
        public void Setup()
        {
            _expectedCount = Server.GetCount();
            _size = 50;
            _addCount = 10;
        }

        [Test]
        public void GetCount_MultipleThreads_GetSameValue()
        {
            Server.AddToCount(_addCount);
            _expectedCount += _addCount;
            Thread.Sleep(100);


            Assert.Multiple(() =>
            {
                Parallel.For(0, _size, new ParallelOptions() { MaxDegreeOfParallelism = _size }, (i) =>
                {
                    Assert.That(Server.GetCount(), Is.EqualTo(_expectedCount));
                });
            });
        }

        [Test]
        public void AddToCount_MultipleThreads_FinalCountEqualsExpected()
        {
            Parallel.For(0, _size, new ParallelOptions() { MaxDegreeOfParallelism = _size }, (i) =>
            {
                Server.AddToCount(_addCount);
            });

            _expectedCount += _size * _addCount;

            Assert.That(Server.GetCount(), Is.EqualTo(_expectedCount));
        }

        [Test]
        public void AddToCount_MultipleThreads_ExecTimeIsApproximateToSynchronous()
        {
            var parallel = Stopwatch.StartNew();
            Parallel.For(0, _size, new ParallelOptions() { MaxDegreeOfParallelism = _size }, (i) =>
            {
                Server.AddToCount(_addCount);
            });
            parallel.Stop();

            var sync = Stopwatch.StartNew();
            for (int i = 0; i < _size; i++)
            {
                Server.AddToCount(_addCount);
            }
            sync.Stop();

            var deltaMilliseconds = Math.Abs(sync.ElapsedMilliseconds - parallel.ElapsedMilliseconds);
            Assert.That(deltaMilliseconds, Is.LessThan(500));
        }

        [Test]
        [TestCase(7)]
        public void GetCount_MultipleThreads_WaitsForWrite(int step)
        {
            _expectedCount = Server.GetCount();
            Thread.Sleep(100);

            Assert.Multiple(() =>
            {
                Parallel.For(0, _size, new ParallelOptions() { MaxDegreeOfParallelism = _size }, (i) =>
                {
                    Assert.That(Server.GetCount(), Is.EqualTo(_expectedCount));

                    if (i % step == 0)
                    {
                        Server.AddToCount(_addCount);
                        _expectedCount += _addCount;
                    }
                });
            });
        }
    }
}