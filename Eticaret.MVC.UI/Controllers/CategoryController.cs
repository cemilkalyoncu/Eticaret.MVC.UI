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
    [Auth]
    [AuthAdmin]
    public class CategoryController : Controller
    {
        private CategoryManager categoryManager = new CategoryManager();
        public ActionResult Index()
        {
            return View(categoryManager.List());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            ModelState.Remove("CreateOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                categoryManager.Insert(category);
                CacheHelper.RemoveCategoriesFromCache();
                return RedirectToAction("Index");
            }

            return View(category);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            ModelState.Remove("CreateOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                Category categoryUp = categoryManager.find(x => x.Id == category.Id);
                categoryUp.Title = category.Title;
                categoryUp.Description = category.Description;
                categoryManager.Update(categoryUp);
                CacheHelper.RemoveCategoriesFromCache();

                return RedirectToAction("Index");
            }
            return View(category);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = categoryManager.find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = categoryManager.find(x => x.Id == id);
            categoryManager.Delete(category);
            CacheHelper.RemoveCategoriesFromCache();

            return RedirectToAction("Index");
        }

    }
}
