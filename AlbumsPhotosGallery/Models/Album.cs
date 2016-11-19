using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AlbumsPhotosGallery.Models
{
    public class Album
    {

        public int ID { get; set; }

        public string Name { get; set; }

        public string UserID { get; set; }

        public virtual ICollection <Photo> Photos { get; set; }

    }
}