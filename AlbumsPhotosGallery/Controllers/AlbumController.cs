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

namespace AlbumsPhotosGallery.Controllers
{
    public class AlbumController : Controller
    {
        private GalleryContext db = null;

        private string defaultThumbnailPath = "~/Images/Thumbnails/defaultThumbnail.png";

        public AlbumController()
        {
            db = new GalleryContext();
        }

        public ActionResult Index(int? pageSize, int? pageNumber)
        {
            var viewModel = GetViewModel(pageSize, pageNumber);

            return View("Index", viewModel);
        }

        private AlbumsViewModel GetViewModel(int? pageSize, int? pageNumber)
        {
            if (!pageSize.HasValue)
            {
                pageSize = 9;
            }

            if (!pageNumber.HasValue)
            {
                pageNumber = 1;
            }

            var count = db.Albums.Count();
            var albums = db.Albums.OrderBy(x => x.ID).Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();

            return new AlbumsViewModel
            {
                Albums = albums,
                PageSize = pageSize.Value,
                TotalPages = (int)Math.Ceiling((double)count / pageSize.Value),
                PageNumber = pageNumber.Value
            };
        }

        public ActionResult ViewAddAlbum()
        {
            return PartialView("_AddAlbum");
        }

        [HttpPost]
        public ActionResult SaveAlbum(string name, string userid)
        {
            SaveAlbumToDatabase(name, userid);

            return RedirectToAction("Index");
        }

        private void SaveAlbumToDatabase(string name, string userid)
        {
            var album = db.Albums.Create();

            album.Name = name;

            album.UserID = userid;

            db.Albums.Add(album);

            db.SaveChanges();

            CreateFirstThumbnail(album.ID);
        }

        private void CreateFirstThumbnail(int albumId)
        {
            var photo = db.Photos.Create();
            photo.FileName = "FirstThumbnail";
            photo.AlbumID = albumId;

            db.Photos.Add(photo);
            db.SaveChanges();

            var photoVersion = db.PhotoVersions.Create();
            photoVersion.Width = 400;
            photoVersion.Height = 300;
            photoVersion.Path = defaultThumbnailPath;
            photoVersion.Type = PhotoTypeEnum.Thumbnail;
            photoVersion.PhotoID = photo.ID;

            db.PhotoVersions.Add(photoVersion);
            db.SaveChanges();
        }

        public ActionResult ViewForAdd()
        {
            return View();
        }

        public ActionResult GetThumbnailPath(int albumId)
        {
            var path = defaultThumbnailPath;

            var photo = db.Photos.Where(x => x.AlbumID == albumId).FirstOrDefault();

            var photoVersion = db.PhotoVersions.Where(x => x.PhotoID == photo.ID && x.Type == PhotoTypeEnum.Thumbnail).FirstOrDefault();

            if (photoVersion != null)
            {
                path = photoVersion.Path;
            }
            return base.File(path, "image/jpeg"); 
        }

    }
}