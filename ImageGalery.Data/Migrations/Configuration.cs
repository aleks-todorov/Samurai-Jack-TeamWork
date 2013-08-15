namespace ImageGallery.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ImageGallery.Model;

    public sealed class Configuration : DbMigrationsConfiguration<ImageGallery.Data.ImageGaleryContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ImageGallery.Data.ImageGaleryContext context)
        {
            //context.Users.AddOrUpdate(u => u.UserName,
            //    new User { UserName = "Pesho", Password = "asdasd" },
            //    new User { UserName = "Ivan", Password = "asdasd" },
            //    new User { UserName = "Gosho", Password = "asdasd" },
            //    new User { UserName = "Miro", Password = "asdasd" }
            //    );
        }
    }
}
