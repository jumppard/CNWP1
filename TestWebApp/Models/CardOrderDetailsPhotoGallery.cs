using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestWebApp.Models
{
    public class CardOrderDetailsPhotoGallery
    {
        public List<string> PhotosNames { get; set; }

        public CardOrderDetailsPhotoGallery() { PhotosNames = new List<string>(); }

        public List<string> GetPhotosNames() { return PhotosNames; }
        public void AddPhotoNameToList(string photoName) { PhotosNames.Add(photoName); }
    }
}