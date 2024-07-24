using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace UnifyAuth.Sample.ProxyTest.Models
{
    [BsonIgnoreExtraElements]
    public class MongoDbCollection : INoSqlDbRecord
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
    }

    public interface INoSqlDbRecord
    {
    }
}
