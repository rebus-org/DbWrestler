# DbWrestler

Makes it easy to wrestle with LocalDB.

It's just
```csharp
var localDb = new LocalDb();

var instance = localDb.GetInstance(instanceName: "MSSQLLocalDB");

instance.CreateIfNotExists();

var database = instance.GetDatabase(databaseName: "test_db");

database.CreateIfNotExists();

var connectionString = database.ConnectionString;

// off you go 👍

```
and off you go 👍