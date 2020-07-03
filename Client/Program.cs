using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire.States;
using Model;
using MongoDB.Driver;
using System;

namespace Client
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

            GlobalConfiguration
               .Configuration
               .UseMongoStorage(
                    url.ToString(),
                    storageOptions);

            string command;
            do
            {
                Console.Write("Insert Command:");
                command = Console.ReadLine();
                ExecuteCommand(command);
            } while (!string.IsNullOrEmpty(command));
        }

        private static void ExecuteCommand(string command)
        {
            if (!String.IsNullOrWhiteSpace(command))
            {
                var duration = Int32.Parse(command);
                string queueName = duration < 6 ? "fast" : "slow";
                var job = new LongRunningJob()
                {
                    Id = Guid.NewGuid().ToString(),
                    Type = "Command/" + command,
                    Duration = duration,
                };
                LongRunningJobStorage.SaveJob(job);

                var client = new BackgroundJobClient();
                var state = new EnqueuedState(queueName);

                client.Create(() => LongRunningJobExecutor.Execute(job.Id), state);
            }
        }
    }
}