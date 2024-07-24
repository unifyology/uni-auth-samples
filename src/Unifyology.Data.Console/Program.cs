using EntityFrameworkCore.BootKit;

var db = new Database();
//AppDomain.CurrentDomain.SetData("Assemblies", new string[] { "EntityFrameworkCore.BootKit.UnitTest" });
db.BindDbContext<IDbRecord, DbContext4MongoDb>(new DatabaseBind
{
    MasterConnection = new MongoDbConnection("mongodb+srv://qualiumtech:h%23cSBkpHEd60@cluster0.oejx4.azure.mongodb.net/SamplesDb?retryWrites=true&w=majority"),
    CreateDbIfNotExist = true,
});

Console.WriteLine("Hello, World!");
