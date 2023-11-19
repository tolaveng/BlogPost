using MongoDB.Bson;

namespace Core.Domain.Documents
{
    public interface IDocument
    {
        ObjectId Id { get; set; }
    }
}
