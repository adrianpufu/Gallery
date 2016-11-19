using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlbumsPhotosGallery.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace AlbumsPhotosGallery.DAL
{
    public class GalleryContext : DbContext
    {

        public GalleryContext() : base("GalleryContext")
        {
            
        }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Photo> Photos { get; set; }

        public DbSet<PhotoVersion> PhotoVersions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

        }
    }

}