using Client;
using MongoDB.Driver;
using System.Linq;

namespace Model
{
    public static class LongRunningJobStorage
    {
        private static readonly IMongoCollection<LongRunningJob> _jobStore;

        static LongRunningJobStorage()
        {
            var url = new MongoUrl("mongodb://admin:123456##@localhost/jarvis-longrunningjob?authSource=admin");
            var client = new MongoClient(url);
            var db = client.GetDatabase(url.DatabaseName);
            _jobStore = db.GetCollection<LongRunningJob>("LongRunningJobs");
        }

        public static void SaveJob(LongRunningJob job)
        {
            _jobStore.InsertOne(job);
        }

        public static LongRunningJob LoadJob(string id)
        {
            return _jobStore.AsQueryable().SingleOrDefault(j => j.Id == id);
        }
    }
}
