using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SQLite;

namespace CryptBlog.Entities
{
    public class SignatureEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Description { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}