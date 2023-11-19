namespace Core.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class MongoDbCollectionAttribute : Attribute
    {
        public string CollectionName { get; }
        public MongoDbCollectionAttribute(string collectionName)
        {
            CollectionName = collectionName;
        }
    }
}
