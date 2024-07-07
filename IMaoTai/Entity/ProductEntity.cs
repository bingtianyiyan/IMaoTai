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

        public ProductEntity(string code, string title, string description, string img, DateTime created)
        {
            Code = code;
            Title = title;
            Description = description;
            Img = img;
            Created = created;
        }

        #endregion Construct

        #region Properties

        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Img { get; set; }

        public DateTime Created { get; set; }

        #endregion Properties
    }
}