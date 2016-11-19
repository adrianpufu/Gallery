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
        
        private string defaultThumbnailPath = "/Photos/defaultThumbnail.jpg";

        public AlbumController()
        {
            db = new GalleryContext();
        }

        public ActionResult Index()
        {
            AlbumsViewModel viewModel = new AlbumsViewModel();

            viewModel.Albums = db.Albums.ToList();

            return View("Index", viewModel);
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