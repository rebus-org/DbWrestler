using System;
using System.Linq;
using NUnit.Framework;
using Testy;
using Testy.Extensions;

namespace DbWrestler.Tests
{
    [TestFixture]
    public class SimpleTests : FixtureBase
    {
        [Test]
        public void CanGetInstanceNames()
        {
            new LocalDb().GetInstanceNames().Select(name => new {InstanceName = name}).DumpTable();
        }

        [Test]
        public void CanCheckExistence_DoesNotExist()
        {
            var instance = new LocalDb();

            var localDbDatabase = instance.GetInstance(Guid.NewGuid().ToString("N"));

            Assert.That(localDbDatabase.Exists, Is.False);
        }

        [Test]
        public void CanCheckExistence_CreateAndCheck()
        {
            var instance = new LocalDb();

            var localDbDatabase = instance.GetInstance(Guid.NewGuid().ToString("N"));

            var existsBeforeAnything = localDbDatabase.Exists;

            localDbDatabase.Create();

            var existsAfterCreate = localDbDatabase.Exists;

            localDbDatabase.Delete();

            var existsAfterDelete = localDbDatabase.Exists;

            Assert.That(existsBeforeAnything, Is.False);
            Assert.That(existsAfterCreate, Is.True);
            Assert.That(existsAfterDelete, Is.False);
        }
    }
}
