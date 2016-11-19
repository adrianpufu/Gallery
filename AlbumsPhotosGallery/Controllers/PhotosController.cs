using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AlbumsPhotosGallery.DAL;
using AlbumsPhotosGallery.Models;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using ImageMagick;

namespace AlbumsPhotosGallery.Controllers
{
    public class PhotosController : Controller
    {
        private GalleryContext db = null;

        private string defaultThumbnailPath = "~/Images/Thumbnails/defaultThumbnail.png";

        public PhotosController()
        {
            db = new GalleryContext();
        }

        public ActionResult Index(int? id)
        {
            PhotosViewModel viewModel = new PhotosViewModel();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Album album = db.Albums.Find(id);

            if (album == null)
            {
                return HttpNotFound();
            }

            var photos = db.Photos.Where(x => x.AlbumID == album.ID).ToList();

            if (photos != null)
            {

                viewModel.Photos = photos;

                viewModel.Album = album;
            }
            else
            {
                var defaultPhoto = GetDefaultPhoto(album);

                viewModel.Photos.Add(defaultPhoto);

                viewModel.Album = album;
            }

            return View("Index", viewModel);
        }

        private Photo GetDefaultPhoto(Album album)//trebuie modificata asta
        {
            var defaultPhoto = new Photo();

            defaultPhoto.AlbumID = album.ID;

            var photoVersion = new PhotoVersion();

            photoVersion.Path = defaultThumbnailPath;

            photoVersion.Type = PhotoTypeEnum.Thumbnail;

            defaultPhoto.PhotoVersions.Add(photoVersion);

            return defaultPhoto;
        }

        public ActionResult GetPhoto(int photoid, PhotoTypeEnum phototype)
        {
            var photo = db.Photos.Find(photoid);

            if (photo == null)
            {
                return base.File(defaultThumbnailPath, "image/jpeg");
            }

            var photoVersion = db.PhotoVersions.Where(x => x.PhotoID == photo.ID && x.Type == phototype).FirstOrDefault();

            if (photoVersion == null)
            {
                return base.File(defaultThumbnailPath, "image/jpeg");
            }

            return base.File(photoVersion.Path, "image/jpeg");
        }

        public ActionResult ViewAddPhotos()
        {
            return PartialView("_AddPhotos");
        }

        [HttpPost]
        public ActionResult SaveImage(int albumId)
        {
            try
            {
                var files = this.Request.Files;

                if (files != null && files.Count > 0)
                {
                    for (var i = 0; i < files.Count; i++)
                    {
                        var path = SaveFileToDisk(files[i]);

                        var name = Path.GetFileNameWithoutExtension(files[i].FileName);

                        var photoThumbnail = ResizeImage(files[i], 400, 300, PhotoTypeEnum.Thumbnail);

                        var photoMedium = ResizeImage(files[i], 1000, 800, PhotoTypeEnum.Medium);

                        MagickImageInfo info = new MagickImageInfo(files[i].FileName);

                        var photoOriginal = CreatePhotoVersion(path, PhotoTypeEnum.Original, info.Width, info.Height);

                        var photoId = SaveFileToDatabase(albumId, name);

                        SavePhotoVersionToDatabase(photoId, photoThumbnail);

                        SavePhotoVersionToDatabase(photoId, photoMedium);

                        SavePhotoVersionToDatabase(photoId, photoOriginal);
                    }
                }
            }
            catch (Exception ex) { return Json("Error While Saving. " + ex.Message); }

            return Json(new
            {
                Success = true
            });
        }

        private PhotoVersion ResizeImage(HttpPostedFileBase file, int typeWidth, int typeHeight, PhotoTypeEnum type)
        {
            using (MagickImage image = new MagickImage(file.FileName)) //Nu stiu exact daca merge asa direct din HttpPostedFileBase
            {
                MagickImageInfo info = new MagickImageInfo(file.FileName);
                
                //fit in dimensiunile respective
                //MagickGeometry size = new MagickGeometry(100, 100);

                if (info.Width > info.Height)
                {
                    image.Resize(0, typeHeight);

                    //image.Resize(size);
                }
                else
                {
                    image.Resize(typeWidth, 0);

                    //image.Resize(size);
                }
                return saveImageToDisk(image, type, file.FileName);
            }
        }

        private PhotoVersion saveImageToDisk(MagickImage image, PhotoTypeEnum type, string fileName)
        {
            var photoName = Guid.NewGuid().ToString() + Path.GetExtension(fileName);

            var path = Path.Combine(Server.MapPath("~/Images/Mediums"), photoName);

            switch (type)
            {
                case PhotoTypeEnum.Thumbnail:

                    path = Path.Combine(Server.MapPath("~/Images/Thumbnails"), photoName);

                    image.Write(path);

                    break;

                case PhotoTypeEnum.Medium:

                    image.Write(path);

                    break;
                default:
                    break;
            }

            return CreatePhotoVersion(path, type, image.Width, image.Height);
        }

        private PhotoVersion CreatePhotoVersion(string path, PhotoTypeEnum type, int width, int height)
        {
            var photoVersion = new PhotoVersion();
            photoVersion.Width = width;
            photoVersion.Height = height;
            photoVersion.Path = path;
            photoVersion.Type = type;

            return photoVersion;
        }

        private void SavePhotoVersionToDatabase(int photoId, PhotoVersion version)
        {
            var photoVersion = db.PhotoVersions.Create();
            photoVersion.Width = version.Width;
            photoVersion.Height = version.Height;
            photoVersion.Path = version.Path;
            photoVersion.Type = version.Type;
            photoVersion.PhotoID = photoId;

            db.PhotoVersions.Add(photoVersion);
            db.SaveChanges();
        }

        private string SaveFileToDisk(HttpPostedFileBase file)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            var path = Path.Combine(Server.MapPath("~/Images/Originals"), fileName);

            file.SaveAs(path);

            return path;
        }

        private int SaveFileToDatabase(int albumId, string name)
        {
            var photo = db.Photos.Create();
            photo.FileName = name;
            photo.AlbumID = albumId;

            db.Photos.Add(photo);
            db.SaveChanges();

            return photo.ID;
        }

    }
}