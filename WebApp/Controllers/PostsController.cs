﻿using Core.Application.DTOs;
using Core.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<PostDto>>> Get(int pageNo, int? pageSize = 50)
        {
            var pageable = new Pageable(pageNo, pageSize.HasValue ? pageSize.Value : 50);
            pageable.SortBy = "PublishedDateTime";
            var posts = await postService.GetPosts(pageable);
            return Ok(posts);
        }

        [Route("tag/{tag}")]
        [HttpGet]
        public async Task<ActionResult<Pagination<PostDto>>> GetByTag(string tag, int pageNo, int? pageSize = 50)
        {
            var pageable = new Pageable(pageNo, pageSize.HasValue ? pageSize.Value : 50);
            pageable.SortBy = "PublishedDateTime";
            var posts = await postService.GetPostsByTag(tag, pageable);
            return Ok(posts);
        }


        [Route("id/{id}")]
        [HttpGet]
        public async Task<ActionResult<PostDto>> GetById(string id)
        {
            if (!ObjectId.TryParse(id, out var objectId)) {
                return NotFound($"{id} not found.");
            }   
            var post = await postService.GetPost(objectId);
            if (post != null) {
                return Ok(post);
            }
            return NotFound($"{id} not found.");
        }

        [Route("{path}")]
        [HttpGet]
        public async Task<ActionResult<PostDto>> GetByPath(string path)
        {
            if (path == null || path.Trim() == "") return NotFound();

            var post = await postService.GetByPath(path);
            if (post != null)
            {
                return Ok(post);
            }
            return NotFound($"{path} not found.");
        }
    }
}
