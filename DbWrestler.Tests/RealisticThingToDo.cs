using System;
using System.Linq;
using NUnit.Framework;
using Testy.Extensions;

namespace DbWrestler.Tests
{
    [TestFixture]
    public class RealisticThingToDo
    {
        [Test]
        public void CanDoThis()
        {
            var localDb = new LocalDb();

            var instance = localDb.GetInstance();

            instance.CreateIfNotExists();

            instance.GetDatabaseNames().Select(name => new {DatabaseName = name}).DumpTable();

            var database = instance.GetDatabase("test_db");

            database.CreateIfNotExists();

            database.DeleteIfExists();

            Console.WriteLine($@"This is the connection string:

    {database.ConnectionString}");
        }
    }
}