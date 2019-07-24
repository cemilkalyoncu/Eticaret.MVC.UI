using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Eticaret.Business;
using Eticaret.Entities;
using Eticaret.MVC.UI.Filters;
using Eticaret.MVC.UI.Models;

namespace Eticaret.MVC.UI.Controllers
{
    [Exc]
    public class NoteController : Controller
    {
        NoteManager noteManager = new NoteManager();
        CategoryManager categoryManager = new CategoryManager();
        LikedManager likedManager = new LikedManager();
        [Auth]
        public ActionResult Index()
        {
            var notes = noteManager.ListQueryable().Include("Category").Include("Owner").Where(
                x => x.Owner.Id == CurrentSession.UserSession.Id).OrderByDescending(
                x => x.ModifiedOn);
            return View(notes.ToList());
        }
        [Auth]
        public ActionResult MyLikedNotes()
        {
            var notes = likedManager.ListQueryable().Include("LikedUser").Include("Note").Where(
                x => x.LikedUser.Id == CurrentSession.UserSession.Id).Select(
                x => x.Note).Include("Category").Include("Owner").OrderByDescending(
                x => x.ModifiedOn);

            return View("Index", notes.ToList());
        }
        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }
        [Auth]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {

            ModelState.Remove("CreateOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.UserSession;
                noteManager.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }
        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }
        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {

            ModelState.Remove("CreateOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                Note noteUp = noteManager.find(x => x.Id == note.Id);
                noteUp.IsDraft = note.IsDraft;
                noteUp.CategoryId = note.CategoryId;
                noteUp.Text = note.Text;
                noteUp.Title = note.Title;
                noteManager.Update(noteUp);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }
        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }
        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = noteManager.find(x => x.Id == id);
            noteManager.Delete(note);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            if (CurrentSession.UserSession != null)
            {
                List<int> likedNoteIds = likedManager.List(
                         x => x.LikedUser.Id == CurrentSession.UserSession.Id &&
                            ids.Contains(x.Note.Id)).Select(
                         x => x.Note.Id).ToList();
                return Json(new { result = likedNoteIds });
            }
            else
            {
                return Json(new { result = new List<int>() });
            }
        }
        [HttpPost]
        public ActionResult SetLikeState(int noteid, bool liked)
        {
            int res = 0;

            if (CurrentSession.UserSession == null)
                return Json(new { hasError = true, errorMessage = "Beğenme işlemi için giriş yapmalısınız.", result = 0 });

            Liked like =
                likedManager.find(x => x.Note.Id == noteid && x.LikedUser.Id == CurrentSession.UserSession.Id);

            Note note = noteManager.find(x => x.Id == noteid);

            if (like != null && liked == false)
            {
                res = likedManager.Delete(like);
            }
            else if (like == null && liked == true)
            {
                res = likedManager.Insert(new Liked()
                {
                    LikedUser = CurrentSession.UserSession,
                    Note = note
                });
            }

            if (res > 0)
            {
                if (liked)
                {
                    note.LikeCount++;
                }
                else
                {
                    note.LikeCount--;
                }

                res = noteManager.Update(note);

                return Json(new { hasError = false, errorMessage = string.Empty, result = note.LikeCount });
            }

            return Json(new { hasError = true, errorMessage = "Beğenme işlemi gerçekleştirilemedi.", result = note.LikeCount });
        }
        public ActionResult GetNoteText(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Note note = noteManager.find(x => x.Id == id);

            if (note == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialNoteText", note);
        }
    }
}
