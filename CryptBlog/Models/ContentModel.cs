using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using CryptBlog.Entities;
using System.Web.Hosting;
using CryptBlog.Models;

namespace CryptBlog.Models
{
    public class ContentModel
    {
        SQLiteConnection db;
        string databasePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "test.db");

        public ContentModel()
        {
            db = new SQLiteConnection(databasePath);
            EncryptAll();
            if (db.GetTableInfo("ContentEntity").Count==0)
            {
                db.CreateTable<ContentEntity>();
            }
        }
        public void Insert(string title, string body)
        {
            var content = new ContentEntity() { Title = title, Body=body };
            db.Insert(content);
        }
        public TableQuery<ContentEntity> SelectAll()
        {
            var query = db.Table<ContentEntity>();
            return query;
        }
        public ContentEntity SelectWithID(int id)
        {
            var content = db.Query<ContentEntity>("select * from ContentEntity where Id="+id)[0];
            return content;
        }
        public int Update(ContentEntity objToUpdate)
        {
            return db.Update(objToUpdate);
        }
        public void EncryptAll()
        {
            var query = SelectAll().ToArray();
            EncryptionModel encryptionModel = new EncryptionModel();
            for (int i = 0; i < query.Count(); i++)
            {
                ContentEntity cryptedObj;
                cryptedObj=encryptionModel.AES_EncryptContentEntity(query[i], "OnurCryptKey000000000000");
                if (cryptedObj!=null)
                {
                    Update(cryptedObj);
                }
            }
        }
        public void InsertWithAESEncryption(string title, string body, string encryptionKey)
        {
            ContentEntity content = new ContentEntity() { Title = title, Body = body };
            EncryptionModel encryptionModel = new EncryptionModel();
            content = encryptionModel.AES_EncryptContentEntity(content, encryptionKey);
            db.Insert(content);
        }
    }
}