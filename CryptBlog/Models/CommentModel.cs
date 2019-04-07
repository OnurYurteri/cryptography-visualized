using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CryptBlog.Entities;
using System.Web.Hosting;
using SQLite;
using System.IO;

namespace CryptBlog.Models
{
    public class CommentModel
    {
        SQLiteConnection db;
        string databasePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "test.db");

        public CommentModel()
        {
            db = new SQLiteConnection(databasePath);
            if (db.GetTableInfo("CommentEntity").Count == 0)
            {
                db.CreateTable<CommentEntity>();
            }
        }

        public void Insert(int contentId, string title, string body, string analysis)
        {
            var comment = new CommentEntity() { ContentId=contentId, Title=title, Body=body, Analysis=analysis };
            db.Insert(comment);
        }

        public TableQuery<CommentEntity> SelectAll()
        {
            var query = db.Table<CommentEntity>();
            return query;
        }

        public CommentEntity SelectWithID(int id)
        {
            var comment = db.Query<CommentEntity>("select * from CommentEntity where Id=" + id)[0];
            return comment;
        }
        public int Update(CommentEntity objToUpdate)
        {
            return db.Update(objToUpdate);
        }


    }
}