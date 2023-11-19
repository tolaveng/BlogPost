
using Core.Domain.Attributes;

namespace Core.Domain.Documents
{
    [MongoDbCollection("Settings")]
    [MongoDbIndexes("Name")]
    public class Setting : Document
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
