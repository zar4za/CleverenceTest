using CleverenceDelegate;
using NUnit.Framework;
using System;

namespace CleverenceDelegateTests
{
    public class AsyncCallerTest
    {
        [Test]
        public void Invoke_WithTimeout1000_ReturnsTrue()
        {
            var handler = new EventHandler((sender, args) => { });
            var asyncCaller = new AsyncCaller(handler);
            var result = asyncCaller.Invoke(1000, null, null);
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void Invoke_WithTimeout0_ReturnsFalse()
        {
            var handler = new EventHandler((sender, args) => { });
            var asyncCaller = new AsyncCaller(handler);
            var result = asyncCaller.Invoke(0, null, null);
            Assert.That(result, Is.EqualTo(false));
        }
    }
}