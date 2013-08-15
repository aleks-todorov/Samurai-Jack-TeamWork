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
using System.Web;

namespace ImageGallery.Services.Controllers
{
    public class AlbumsController : ApiController
    {
        private IRepository<Album> dbRepository;

        public AlbumsController()
        {
            var dbContext = new ImageGaleryContext();
            this.dbRepository = new DbRepository<Album>(dbContext);
        }

        public AlbumsController(IRepository<Album> repository)
        {
            this.dbRepository = repository;
        }

        // GET api/Albums?userId=1
        public IEnumerable<AlbumModel> GetAlbums(int userId)
        {
            var albums = this.dbRepository.All().Include(a => a.User);
            return
                from album in albums
                where album.UserId == userId
                select new AlbumModel()
                {
                    AlbumId = album.AlbumId,
                    AlbumName = album.AlbumName,
                    ImagesCount = album.Images.Count,
                    AlbumCount = album.Albums.Count
                };
        }

        // GET api/Albums?userId=1&albumId=1
        public AlbumFullModel GetAlbum(int userId, int albumId)
        {
            Album album = this.dbRepository.Get(albumId);
            if (album == null || album.UserId != userId)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return new AlbumFullModel()
            {
                AlbumID = album.AlbumId,
                AlbumName = album.AlbumName,
                UserId = album.UserId,
                Images =
                    from image in album.Images
                    select new ImageModel()
                    {
                        ImageId = image.ImageId,
                        ImageName = image.ImageName,
                        URL = image.URL
                    },
                Albums =
                    from subAlbum in album.Albums
                    select new AlbumModel()
                    {
                        AlbumId = subAlbum.AlbumId,
                        AlbumName = subAlbum.AlbumName,
                        AlbumCount = subAlbum.Albums.Count,
                        ImagesCount = subAlbum.Images.Count
                    }
            };
        }

        // PUT api/Albums?userId=1&albumId=1
        public HttpResponseMessage PutAlbum(int userId, int albumId, Album album)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (album == null || albumId != album.AlbumId || userId != album.UserId)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                this.dbRepository.Update(albumId, album);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //Post 
        public HttpResponseMessage PostAlbum(int userId, Album album)
        {
            if (ModelState.IsValid && userId == album.UserId)
            {
                this.dbRepository.Add(album);
                FileStorageAPI.CreateFolder(album);
                string path = HttpContext.Current.Server.MapPath("~/App_Data/");

                foreach (var image in album.Images)
                {
                    FileStorageAPI.UploadFile(image, path);
                }

                var albumModel = new AlbumFullModel()
            {
                AlbumID = album.AlbumId,
                AlbumName = album.AlbumName,
                UserId = album.UserId,
                Images =
                    from image in album.Images
                    select new ImageModel()
                    {
                        ImageId = image.ImageId,
                        ImageName = image.ImageName,
                        URL = image.URL
                    },
                Albums =
                    from subAlbum in album.Albums
                    select new AlbumModel()
                    {
                        AlbumId = subAlbum.AlbumId,
                        AlbumName = subAlbum.AlbumName,
                        AlbumCount = subAlbum.Albums.Count,
                        ImagesCount = subAlbum.Images.Count
                    }
            };
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, albumModel);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = album.AlbumId }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Albums?userId=1&albumId=1
        public HttpResponseMessage DeleteAlbum(int userID, int albumId)
        {
            Album album = this.dbRepository.Get(albumId);
            if (album == null || album.UserId != userID)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                this.dbRepository.Delete(album);
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, album);
        }
    }
}