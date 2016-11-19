using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AlbumsPhotosGallery.DAL;

namespace AlbumsPhotosGallery.Models
{
    public class AlbumsViewModel
    {

        public IEnumerable<Album> Albums { get; set; }

    }

}