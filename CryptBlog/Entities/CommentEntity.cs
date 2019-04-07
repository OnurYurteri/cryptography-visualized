using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite;

namespace CryptBlog.Entities
{
    public class CommentEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ContentId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Analysis { get; set; }
        public int SignedBy { get; set; }
        public string SignedData { get; set; }
    }
    
}