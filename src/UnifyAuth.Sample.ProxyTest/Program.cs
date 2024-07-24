using EntityFrameworkCore.BootKit;
using Microsoft.AspNetCore.HttpOverrides;
using UnifyAuth.Sample.ProxyTest.Models;

namespace UnifyAuth.Sample.ProxyTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSingleton<DatabaseSettings>(x=> new DatabaseSettings() { EnableSqlLog = false, EnableSensitiveDataLogging = false});
            var app = builder.Build();

            //var db = new Database();
            
            //AppDomain.CurrentDomain.SetData("Assemblies", new string[] { "UnifyAuth.Sample.ProxyTest" });
            //db.BindDbContext<INoSqlDbRecord, DbContext4MongoDb2>(new DatabaseBind
            //{
            //    MasterConnection = new MongoDbConnection("mongodb+srv://qualiumtech:h%23cSBkpHEd60@cluster0.oejx4.azure.mongodb.net/SampleDb?retryWrites=true&w=majority"),
            //    //CreateDbIfNotExist = true,
            //    ServiceProvider = app.Services,
            //    TableInterface = typeof(INoSqlDbRecord),
            //});
            //var collection = db.Collection<MongoDbCollection>();
            // Configure the HTTP request pipeline.

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost
            });

            app.MapControllers();

            app.Run();

        }
    }
}