using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.Owin.Hosting;
using MongoDB.Driver;
using System;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var storageOptions = new MongoStorageOptions
            {
                MigrationOptions = new MongoMigrationOptions
                {
                    MigrationStrategy = new MigrateMongoMigrationStrategy(),
                    BackupStrategy = new NoneMongoBackupStrategy()
                }
            };
            var url = new MongoUrl("mongodb://admin:123456##@localhost/jarvis-hangfire-test?authSource=admin");
            var mongoStorage = new MongoStorage(
                MongoClientSettings.FromConnectionString(url.ToString()),
                url.DatabaseName,
                storageOptions);

            var optionsSlow = new BackgroundJobServerOptions
            {
                // This is the default value
                WorkerCount = 1,
                Queues = new[] { "slow" }
            };

            var optionsFast = new BackgroundJobServerOptions
            {
                // This is the default value
                WorkerCount = 5,
                Queues = new[] { "fast" }
            };

            JobStorage.Current = mongoStorage;
            WebApp.Start<Startup>("http://+:46001");

            using (var serverSlow = new BackgroundJobServer(optionsSlow, mongoStorage))
            using (var serverFast = new BackgroundJobServer(optionsFast, mongoStorage))
            {
                Console.WriteLine("Press a key to close");
                Console.ReadKey();
            }
        }
    }
}
