using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite;

namespace CryptBlog.Entities
{
    public class ContentEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string EncryptedBody { get; set; }
        public byte[] EncryptionKey { get; set; }
        public byte[] InitialVector { get; set; }
    }
}