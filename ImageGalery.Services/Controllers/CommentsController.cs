using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ImageGallery.Model;
using ImageGallery.Data;
using ImageGallery.Repositories;
using ImageGallery.Services.Models;

namespace ImageGallery.Services.Controllers
{
    public class CommentsController : ApiController
    {
        private IRepository<Comment> dbRepository;

        public CommentsController()
        {
            var dbContext = new ImageGaleryContext();
            this.dbRepository = new DbRepository<Comment>(dbContext);
        }

        public CommentsController(IRepository<Comment> repository)
        {
            this.dbRepository = repository;
        }

        // GET api/Comments?userId=1
        public IEnumerable<CommentModel> GetComments(int userId)
        {
            var comments = this.dbRepository.All();

            return 
                from comment in comments
                where comment.AuthorId == userId
                   select new CommentModel()
                   {
                       CommentId = comment.CommentId,
                       Content = comment.Content
                   };
        }

        // GET api/Comments?userId=1&commentId=1
        public CommentModel GetComment(int userId, int commentId)
        {
            Comment comment = this.dbRepository.Get(commentId);
            if (comment == null || comment.AuthorId != userId)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return new CommentModel()
            {
                CommentId = comment.CommentId,
                Content = comment.Content
            };

        }

        // PUT api/Comments?userId=1&commentId=1
        public HttpResponseMessage PutComment(int userId, int commentId, Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (commentId != comment.CommentId || comment.AuthorId != userId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                this.dbRepository.Update(commentId, comment);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Comments?userId=1
        public HttpResponseMessage PostComment(int userId, Comment comment)
        {
            if (ModelState.IsValid && comment.AuthorId == userId)
            {
                this.dbRepository.Add(comment);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, comment);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = comment.CommentId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Comments?userId=1
        public HttpResponseMessage DeleteComment(int userId, int commentId)
        {
            Comment comment = this.dbRepository.Get(commentId);
            if (comment == null || comment.AuthorId != userId)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                this.dbRepository.Delete(comment);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, comment);
        }
    }
}