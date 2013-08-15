using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ImageGallery.Model;
using ImageGallery.Data;
using ImageGallery.Repositories;
using ImageGallery.Services.Models;

namespace ImageGallery.Services.Controllers
{
    public class UsersController : ApiController
    {
        private IRepository<User> dbRepository;

        public UsersController()
        {
            var dbContext = new ImageGaleryContext();
            this.dbRepository = new DbRepository<User>(dbContext);
        }

        public UsersController(IRepository<User> repository)
        {
            this.dbRepository = repository;
        }

        // GET api/Users
        public IEnumerable<UserModel> GetUsers()
        {
            var users = this.dbRepository.All().AsEnumerable();

            return
                from user in users
                select new UserModel() { UserID = user.UserId, Name = user.UserName };
        }

        // GET api/Users/5
        public UserModel GetUser(int userId)
        {
            User user = this.dbRepository.Get(userId);
            if (user == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return new UserModel() { UserID = user.UserId, Name = user.UserName };
        }

        // PUT api/Users/5
        public HttpResponseMessage PutUser(int userId, User user)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (userId != user.UserId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                this.dbRepository.Update(userId, user);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Users
        public HttpResponseMessage PostUser(User user)
        {
            if (ModelState.IsValid)
            {
                this.dbRepository.Add(user);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = user.UserId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Users/5
        public HttpResponseMessage DeleteUser(int userId)
        {
            User user = this.dbRepository.Get(userId);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                this.dbRepository.Delete(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }
    }
}