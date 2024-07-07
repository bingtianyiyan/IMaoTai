using System;
using IMaoTai.Domain;

namespace IMaoTai.Entity
{
    public class ProductEntity
    {
        #region Construct

        public ProductEntity()
        {
        }

        public ProductEntity(string code, string title, string description, string img,string picV2, DateTime created)
        {
            Code = code;
            Title = title;
            Description = description;
            Img = img;
            PictureV2 = picV2;
            Created = created;
        }

        #endregion Construct

        #region Properties

        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Img { get; set; }
        public string PictureV2 { get; set; }

        public DateTime Created { get; set; }

        #endregion Properties
    }
}