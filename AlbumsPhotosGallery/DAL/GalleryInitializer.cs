using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlbumsPhotosGallery.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace AlbumsPhotosGallery.DAL
{
    public class GalleryInitializer : System.Data.Entity.
    DropCreateDatabaseIfModelChanges<GalleryContext>
    {
        protected override void Seed(GalleryContext context)
        {
            var albums = new List<Album>
                {
                new Album{Name="Carson",UserID="Alexander"},
 new
Album{Name="Carson",UserID="Alexander"},
 new
Album{Name="Carson",UserID="Alexander"},
 new
Album{Name="Carson",UserID="Alexander"} };

            albums.ForEach(s => context.Albums.Add(s));
            context.SaveChanges();
            var photos = new List<Photo>
 {
 new Photo{AlbumID=1050,FileName="Chemistry"},
  new Photo{AlbumID=1050,FileName="Chemistry"},
  new Photo{AlbumID=1050,FileName="Chemistry"},
  new Photo{AlbumID=1050,FileName="Chemistry"}
 };
            photos.ForEach(s => context.Photos.Add(s));
            context.SaveChanges();
            var versions = new List<PhotoVersion>
 {
 new PhotoVersion{PhotoID=1,Width=400,Height=300,Type=PhotoTypeEnum.Thumbnail},
 new PhotoVersion{PhotoID=1,Width=400,Height=300,Type=PhotoTypeEnum.Thumbnail},
 new PhotoVersion{PhotoID=1,Width=400,Height=300,Type=PhotoTypeEnum.Thumbnail},
 new PhotoVersion{PhotoID=1,Width=400,Height=300,Type=PhotoTypeEnum.Thumbnail},
 };
            versions.ForEach(s => context.PhotoVersions.Add(s));
            context.SaveChanges();

        }
    }

}