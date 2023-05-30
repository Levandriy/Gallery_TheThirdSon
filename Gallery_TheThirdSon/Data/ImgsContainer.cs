using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery_TheThirdSon.Data
{
    public class ImgsContainer : DbContext
    {
        public ImgsContainer()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public DbSet<Models.ImageModel> Images { get; set; }
        public DbSet<Models.SaveModel> Saves { get; set; }
        public DbSet<Models.TagModel> Tags { get; set; }
        public DbSet<Models.SavesList> SaveLists { get; set; }
        public DbSet<Models.TagsList> TagsLists { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=imgsapp.db");
        }
    }
}
