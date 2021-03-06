﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Blog.API.Data;
using Blog.API.Dtos;
using Blog.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class BlogItemsController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IBlogRepository _repo;

        public BlogItemsController(DataContext context, IMapper mapper, IBlogRepository repo)
        {
            _context = context;
            _mapper = mapper;
            _repo = repo;
        }

        // Get Users Blogs
        [HttpGet("myBlogs")]
        public async Task<IActionResult> get()
        {
            // Users ID
            var userId = GetUsersId();

            var blogs = await _repo.GetMyBlogs(userId);

            return Ok(blogs);

        }

        // Get All Blogs
        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            var blogs = await _repo.GetAllBlogs();

            return Ok(blogs);

        }


        [HttpPost]
        public async Task<IActionResult> add(BlogItemDto blogItemDto)
        {
            // Users ID
            await _repo.AddBlog(blogItemDto);


            return Ok("Blog Created");
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> put(int id, BlogItemDto blogItemDto)
        {
            // Users ID
            var userId = GetUsersId();

            if (blogItemDto.UserId != userId || blogItemDto.Id != id)
            {
                return BadRequest("Can't update blog");
            }

            await _repo.Update(id, blogItemDto);

            return Ok("Blog Updated");
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> delete(int id)
        {
            var blogToDelete = await _context.Blogs.FindAsync(id);

            // Users ID
            var userId = GetUsersId();

            var success = await _repo.Delete(id, userId);
            if (success != null)
                return Ok("Deleted Item");


            return BadRequest("You Cant Delete that blog");
        }
    }
}