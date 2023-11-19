namespace Core.Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class MongoDbIndexesAttribute : Attribute
    {
        public string Index { get; }
        public MongoDbIndexesAttribute(string index)
        {
            Index = index;
        }
    }
}
