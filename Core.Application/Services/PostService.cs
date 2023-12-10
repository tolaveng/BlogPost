using AutoMapper;
using Core.Application.DTOs;
using Core.Application.Services.Interfaces;
using Core.Domain.Documents;
using Core.Domain.IRepositories;
using MongoDB.Bson;

namespace Core.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IMongoRepository<Post> respository;
        private readonly IMapper mapper;

        public PostService(IMongoRepository<Post> mongoRespository, IMapper mapper)
        {
            this.respository = mongoRespository;
            this.mapper = mapper;
        }

        public async Task<bool> ArchivedPost(ObjectId postId)
        {
            try
            {
                var post = await respository.FindByIdAsync(postId);
                if (post != null)
                {
                    post.IsArhived = true;
                    await respository.ReplaceOneAsync(post);
                    return true;
                }
            } catch (Exception) { }
            return false;
        }

        public async Task<bool> CreatePost(PostDto postDto)
        {
            try
            {
                if (postDto.Id == null || postDto.Id == ObjectId.Empty)
                {
                    postDto.Id = ObjectId.GenerateNewId();
                }
                var post = mapper.Map<Post>(postDto);
                await respository.InsertOneAsync(post);
                return true;
            } catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeletePost(ObjectId postId)
        {
            try
            {
                await respository.DeleteByIdAsync(postId);
                return true;
            } catch (Exception) { }
            return false;
        }

        public async Task<PostDto> GetPost(ObjectId postId)
        {
            try
            {
                var post = await respository.FindByIdAsync(postId);
                if (post == null) return null;
                return mapper.Map<PostDto>(post);
            } catch (Exception) { 
                return null;
            }
        }

        public async Task<Pagination<PostDto>> GetPosts(Pagable pagable)
        {
            try
            {
                var queryable = respository.AsQueryable();
                var count = queryable.Count();
                var totalPages = (int)Math.Ceiling((decimal)count / pagable.PageSize);

                var posts = queryable.Skip(pagable.Skip).Take(pagable.PageSize).ToList();
                var items = mapper.Map<List<PostDto>>(posts);
                

                return new Pagination<PostDto>()
                {
                    Items = items,
                    HasNext = pagable.PageNo < totalPages,
                    TotalPages = totalPages,
                    Count = count,
                };
            } catch (Exception) { }
            return null;
        }

        public async Task<bool> UpdatePost(PostDto postDto)
        {
            try
            {
                var post = mapper.Map<Post>(postDto);
                await respository.ReplaceOneAsync(post);
                return true;
            } catch (Exception) {
                return false;
            }
        }
    }
}
