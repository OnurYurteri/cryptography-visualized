using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CryptBlog.Entities;
using System.Web.Hosting;
using System.IO;
using SQLite;
using System.Security.Cryptography;
using System.Text;

namespace CryptBlog.Models
{
    public class SignatureModel
    {
        SQLiteConnection db;
        string databasePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "test.db");
        CommentModel commentModel = new CommentModel();

        public SignatureModel()
        {
            db = new SQLiteConnection(databasePath);
            if (db.GetTableInfo("SignatureEntity").Count == 0)
            {
                db.CreateTable<SignatureEntity>();
            }
        }

        public TableQuery<SignatureEntity> SelectAll()
        {
            var query = db.Table<SignatureEntity>();
            return query;
        }

        public IEnumerable<SignatureEntity> SelectAllOnlyPublicKey()
        {
            var query = db.Query<SignatureEntity>("select Id, Description, PublicKey from SignatureEntity");
            return query;
        }

        public SignatureEntity SelectWithID(int id)
        {
            var signature = db.Query<SignatureEntity>("select * from SignatureEntity where Id=" + id)[0];
            return signature;
        }

        public void Insert(string description, string publicKey, string privateKey)
        {
            var signature = new SignatureEntity() { Description = description, PublicKey = publicKey, PrivateKey = privateKey };
            db.Insert(signature);
        }

        public void CreateNewSignature(string description)
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048);
            string newPublicKey = RSA.ToXmlString(false);
            string newPrivateKey = RSA.ToXmlString(true);
            Insert(description, newPublicKey, newPrivateKey);
        }

        public bool SignComment(int signatureId, int commentId)
        {
            CommentEntity commentToSign = commentModel.SelectWithID(commentId);
            if (commentToSign.SignedBy != 0)
            {
                return false;
            }
            SignatureEntity signatureToSign = SelectWithID(signatureId);
            if (signatureToSign == null)
            {
                return false;
            }
            commentToSign.SignedData = SignString(commentToSign.Title, signatureToSign.PrivateKey);
            commentToSign.SignedBy = signatureId;
            if (commentModel.Update(commentToSign) == 0)
            {
                return false;
            }
            return true;
        }

        private string SignString(string commentString, string privateKey)
        {
            byte[] signedBytes;

            using (var rsa = new RSACryptoServiceProvider())
            {
                var encoder = new UTF8Encoding();
                byte[] originalData = encoder.GetBytes(commentString);
                rsa.FromXmlString(privateKey);
                signedBytes = rsa.SignData(originalData, CryptoConfig.MapNameToOID("SHA512"));
                rsa.PersistKeyInCsp = false;
            }
            return Convert.ToBase64String(signedBytes);
        }

        public bool VerifyComment(int commentId, string publicKey)
        {
            CommentEntity comment = commentModel.SelectWithID(commentId);
            return VerifyString(comment.Title, comment.SignedData, publicKey);
        }

        private bool VerifyString(string commentString, string signedData, string publicKey)
        {
            bool success = false;
            using (var rsa = new RSACryptoServiceProvider())
            {
                var encoder = new UTF8Encoding();
                byte[] bytesToVerify = encoder.GetBytes(commentString);
                byte[] signedBytes = Convert.FromBase64String(signedData);
                try
                {
                    rsa.FromXmlString(publicKey);
                }
                catch (Exception)
                {
                    return false;
                }
                success = rsa.VerifyData(bytesToVerify, CryptoConfig.MapNameToOID("SHA512"), signedBytes);
                rsa.PersistKeyInCsp = false;
            }
            return success;
        }

    }
}