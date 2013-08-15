using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using ImageGallery.Services.Controllers;
using ImageGallery.Repositories;
using ImageGallery.Model;
using ImageGallery.Data;

namespace ImageGallery.Services.DependencyResolvers
{
    public class DbDependencyResolver : IDependencyResolver
    {
        ImageGaleryContext context = new ImageGaleryContext();

        IDependencyScope IDependencyResolver.BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(AlbumsController))
            {
                var repository = new DbRepository<Album>(context);
                return new AlbumsController(repository);
            }

            else if (serviceType == typeof(CommentsController))
            {
                var repository = new DbRepository<Comment>(context);
                return new CommentsController(repository);
            }

            else if (serviceType == typeof(ImagesController))
            {
                var imageRepository = new DbRepository<Image>(context);
                var albumRepository = new DbRepository<Album>(context);
                return new ImagesController(imageRepository, albumRepository);
            }

            else if (serviceType == typeof(UsersController))
            {
                var repository = new DbRepository<User>(context);
                return new UsersController(repository);
            }

            else
            {
                return null;
            }
        }

        IEnumerable<object> IDependencyScope.GetServices(Type serviceType)
        {
            return new List<object>();
        }

        void IDisposable.Dispose()
        {
        }
    }
}