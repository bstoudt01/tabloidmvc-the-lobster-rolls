﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }
        // GET: CommentsController
        public ActionResult Index(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);
            List<Comment> comments = _commentRepository.GetAllCommentsByPostId(id);

            PostCommentViewModel vm = new PostCommentViewModel
            {
               
                Post = post,
                Comments = comments
            };

            return View(vm);
        }

  
        // GET: CommentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post, Comment comment)
        {
            
            try
            {
                int userId = GetCurrentUserProfileId();
                comment.UserProfileId = userId;
                comment.PostId = post.Id;
                comment.CreateDateTime = DateTime.Now;
                _commentRepository.AddComment(comment);
                return RedirectToAction("Index", new { id = post.Id });
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Comment comment)
        {
            int userId = GetCurrentUserProfileId();
            comment.UserProfileId = userId;
            //comment.PostId =
            comment.CreateDateTime = DateTime.Now;
            _commentRepository.UpdateComment(comment);
            return RedirectToAction(nameof(Index));
            //try
            //{
                
            //}
            //catch
            //{
            //    return View();
            //}
        }

        // GET: CommentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}