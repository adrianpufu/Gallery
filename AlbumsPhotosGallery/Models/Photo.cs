using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbumsPhotosGallery.Models
{
    public class Photo
    {

        public int ID { get; set; }

        public int AlbumID { get; set; }

        public string FileName { get; set; }

        public virtual ICollection <PhotoVersion> PhotoVersions { get; set; }

    }
}