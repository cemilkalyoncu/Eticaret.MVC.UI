using Eticaret.Business;
using Eticaret.Entities;
using Eticaret.MVC.UI.Filters;
using Eticaret.MVC.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Eticaret.MVC.UI.Controllers
{
    [Exc]
    public class CommentController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CommentManager commentManager = new CommentManager();
        public ActionResult ShowNoteComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //veri tabanına bi kez istek atmak icin bu sekilde sorgu atarak commentside ekliyoruz
            Note note = noteManager.ListQueryable().Include("Comments").FirstOrDefault(x => x.Id == id);

            if (note == null)
            {
                return HttpNotFound();

            }
            return PartialView("_PartialComments", note.Comments);
        }
        [Auth]
        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = commentManager.find(x => x.Id == id);
            if (comment == null)
            {
                return new HttpNotFoundResult();
            }
            comment.Text = text;
            if (commentManager.Update(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = commentManager.find(x => x.Id == id);
            if (comment == null)
            {
                return new HttpNotFoundResult();
            }
            if (commentManager.Delete(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }
        [Auth]
        [HttpPost]
        public ActionResult Create(Comment comment, int? noteid)
        {
            ModelState.Remove("CreateOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                if (noteid == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Note note = noteManager.find(x => x.Id == noteid);

                if (note == null)
                {
                    return new HttpNotFoundResult();
                }
                comment.Note = note;
                comment.Owner = CurrentSession.UserSession;
                if (commentManager.Insert(comment) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
    }
}