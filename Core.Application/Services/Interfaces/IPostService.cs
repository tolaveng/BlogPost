using Core.Application.DTOs;
using MongoDB.Bson;

namespace Core.Application.Services.Interfaces
{
    public interface IPostService
    {
        Task<Pagination<PostDto>> GetPosts(Pageable pagable, string? searchText);
        Task<PostDto> GetPost(ObjectId postId);
        Task<bool> CreatePost(PostDto postDto);
        Task<bool> UpdatePost(PostDto postDto);
        Task<bool> DeletePost(ObjectId postId);
        Task<bool> ArchivedPost(ObjectId postId);

    }
}
