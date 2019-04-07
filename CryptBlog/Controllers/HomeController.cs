using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CryptBlog.Models;
using SQLite;
using CryptBlog.Entities;
using Newtonsoft.Json;
using System.Text;

namespace CryptBlog.Controllers
{
    public class HomeController : Controller
    {
        public ContentModel contentModel = new ContentModel();
        public CommentModel commentModel = new CommentModel();
        public SignatureModel signatureModel = new SignatureModel();

        public ActionResult Index()
        {
            var contentsQuery =contentModel.SelectAll();
            ViewBag.Contents = EncodeJsString(JsonConvert.SerializeObject(contentsQuery));
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Content(string title, string body, string eBody, string eKey, string iVec)
        {
            ViewBag.Title = title;
            ViewBag.Body = body;
            ViewBag.EncryptedBody = eBody;
            ViewBag.EncryptionKey = eKey;
            ViewBag.InitialVector = iVec;
            return PartialView("~/Views/Home/Partial/ContentPartial.cshtml");
        }

        [HttpPost]
        public JsonResult GetComments()
        {
            var comments = commentModel.SelectAll();
            return Json(comments, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSignatures()
        {
            var signatures = signatureModel.SelectAllOnlyPublicKey();
            return Json(signatures, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult VerifyComment(int commentId, string publicKey)
        {
            return Json(signatureModel.VerifyComment(commentId,publicKey), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddRandomComment(int id)
        {
            RandomCommentModel rcm = new RandomCommentModel();
            commentModel.Insert(id, rcm.generatedRandom, "Secure Random", JsonConvert.SerializeObject(rcm.freq, Formatting.Indented));
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public string EncodeJsString(string s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\'':
                        sb.Append("\\\'");
                        break;
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");

            return sb.ToString();
        }
    }
}