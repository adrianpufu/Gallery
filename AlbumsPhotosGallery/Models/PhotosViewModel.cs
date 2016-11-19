using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbumsPhotosGallery.Models
{
    public class PhotosViewModel
    {
        public ICollection<Photo> Photos { get; set; }

        public Album Album { get; set; }
    }
}