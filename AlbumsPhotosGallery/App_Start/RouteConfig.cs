using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AlbumsPhotosGallery
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "SaveAlbum",
                url: "Save/SaveAlbum",
                defaults: new { controller = "Album", action = "SaveAlbum" }
            );

            routes.MapRoute(
                name: "SaveAlbum2",
                url: "{controller}/Save/SaveAlbum",
                defaults: new { controller = "Album", action = "SaveAlbum" }
            );

            routes.MapRoute(
                name: "ModalAlbums",
                url: "Modal/ViewAddAlbum",
                defaults: new { controller = "Album", action = "ViewAddAlbum" }
            );

            routes.MapRoute(
                name: "ModalAlbums2",
                url: "{controller}/Modal/ViewAddAlbum",
                defaults: new { controller = "Album", action = "ViewAddAlbum" }
            );

            routes.MapRoute(
                name: "ModalPhotos",
                url: "Modal/ViewAddPhotos",
                defaults: new { controller = "Photos", action = "ViewAddPhotos" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
