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
using System.IO;
using System.Web;

namespace ImageGallery.Services.Controllers
{
    public class ImagesController : ApiController
    {

        private IRepository<Image> imageRepository;
        private IRepository<Album> albumRepository;

        public ImagesController()
        {
            var dbContext = new ImageGaleryContext();
            this.imageRepository = new DbRepository<Image>(dbContext);
            this.albumRepository = new DbRepository<Album>(dbContext);
        }

        public ImagesController(IRepository<Image> imageRepository, IRepository<Album> albumRepository)
        {
            this.imageRepository = imageRepository;
            this.albumRepository = albumRepository;
        }

        // GET api/Images?userId=1
        public IEnumerable<ImageModel> GetImages(int userId)
        {
            var images = this.imageRepository.All().Include(a => a.Album);
            return
                from image in images
                where image.Album.UserId == userId
                select new ImageModel()
                {
                    ImageId = image.ImageId,
                    ImageName = image.ImageName,
                    URL = image.URL,
                    AlbumId = image.AlbumId,
                    Path = image.Path
                };
        }

        // GET api/Images?userId=1&imageId=1
        public ImageModel GetImage(int userId, int imageId)
        {
            Image image = this.imageRepository.Get(imageId);
            if (image == null || image.Album.UserId != userId)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return new ImageModel()
            {
                ImageId = image.ImageId,
                ImageName = image.ImageName,
                URL = image.URL,
                AlbumId = image.AlbumId,
                Path = image.Path
            };
        }

        // PUT api/Images?userId=1&albumId=1&imageId=1
        public HttpResponseMessage PutImage(int userID, int albumId, int imageId, Image image)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            Album album = this.albumRepository.Get(albumId);

            if (image == null || imageId != image.ImageId || album == null || album.UserId != userID)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                image.Album = album;
                image.AlbumId = album.AlbumId;
                this.imageRepository.Update(imageId, image);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // //POST api/Images?userId=1
        //public HttpResponseMessage PostImage(int userId, int albumId, Image image)
        //{
        //    Album album = this.albumRepository.Get(albumId);

        //    if (ModelState.IsValid && album.UserId == userId)
        //    {
        //        image.Album = album;
        //        image.AlbumId = album.AlbumId;
        //        this.imageRepository.Add(image);
        //        ImageModel imageModel = GetImage(userId, image.ImageId);

        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, imageModel);
        //        response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = image.ImageId }));
        //        return response;
        //    }
        //    else
        //    {
        //        return Request.CreateResponse(HttpStatusCode.BadRequest);
        //    }
        //}

        //POST api/Images?userId=1
        public HttpResponseMessage PostImage(int userId, int albumId, Image image)
        {
            Album album = this.albumRepository.Get(albumId);

            string path = HttpContext.Current.Server.MapPath("~/App_Data/");
            var imageUrl = FileStorageAPI.UploadFile(image, path);

            if (ModelState.IsValid && album.UserId == userId)
            {
                image.Album = album;
                image.AlbumId = album.AlbumId;
                image.URL = imageUrl;
                this.imageRepository.Add(image);
                ImageModel imageModel = GetImage(userId, image.ImageId);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, imageModel);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = image.ImageId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Images?userId=1&albumId=1&imageId=1
        public HttpResponseMessage DeleteImage(int userId, int albumId, int imageId)
        {
            Image image = this.imageRepository.Get(imageId);
            Album album = this.albumRepository.Get(albumId);

            if (image == null || album.UserId != userId)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            ImageModel imageModel = GetImage(userId, image.ImageId);

            try
            {
                this.imageRepository.Delete(image);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, imageModel);
        }
    }
}