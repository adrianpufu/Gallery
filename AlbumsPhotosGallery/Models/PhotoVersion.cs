using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbumsPhotosGallery.Models
{
    public class PhotoVersion
    {

        public int ID { get; set; }

        public int PhotoID { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string Path { get; set; }

        public PhotoTypeEnum Type { get; set; }

    }
}