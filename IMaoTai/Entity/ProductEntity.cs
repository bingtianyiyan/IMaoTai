using System;
using IMaoTai.Domain;

namespace IMaoTai.Entity
{
    public class ProductEntity
    {
        #region Fields
        private string _code;
        private string _title;
        private string _description;
        private string _img;
        private DateTime _created;


        #endregion


        #region Construct

        public ProductEntity()
        {
        }

        public ProductEntity(string code, string title, string description, string img, DateTime created)
        {
            _code = code;
            _title = title;
            _description = description;
            _img = img;
            _created = created;
        }

        #endregion

        #region Properties

        public string Code { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Img { get; set; }

        public DateTime Created { get; set; }


        #endregion





    }
}
