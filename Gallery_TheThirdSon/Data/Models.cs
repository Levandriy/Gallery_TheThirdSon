using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery_TheThirdSon.Data
{
    public class Models
    {
        public class ImageModel
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Title { get; set; }
            public string Extention { get; set; }
            public string Description { get; set; } = "No description";
            public byte[] Data { get; set; }
            public DateTime DateUploaded { get; set; } = DateTime.Now;
        }
        public class SaveModel
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Title { get; set; } = "New save";
            public string Description { get; set; } = "No description";
            public DateTime DateCreated { get; set; } = DateTime.Now;
            public DateTime DateUpdated { get; set; } = DateTime.Now;
        }
        public class TagModel
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Title { get; set; }
            public string Description { get; set; } = "No description";
        }
        public class TagsList
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public Guid Image { get; set; }
            public Guid Tag { get; set; }
        }
        public class SavesList
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public Guid Image { get; set; }
            public Guid Save { get; set; }
        }
    }
}
