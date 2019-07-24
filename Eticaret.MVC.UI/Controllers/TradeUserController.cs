using Eticaret.Business;
using Eticaret.Business.Results;
using Eticaret.Entities;
using Eticaret.MVC.UI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Eticaret.MVC.UI.Controllers
{
    [Exc]
    [Auth]
    [AuthAdmin]
    public class TradeUserController : Controller
    {
        private TradeUserManager tradeUserManager = new TradeUserManager();
        public ActionResult Index()
        {
            return View(tradeUserManager.List());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TradeUser tradeUser = tradeUserManager.find(x => x.Id == id.Value);
            if (tradeUser == null)
            {
                return HttpNotFound();
            }
            return View(tradeUser);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TradeUser tradeUser)
        {
            ModelState.Remove("CreateOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                BusinessErrorResult<TradeUser> bussinesErrorResult=  tradeUserManager.Insert(tradeUser);
                if (bussinesErrorResult.Errors.Count >0)
                {
                    bussinesErrorResult.Errors.ForEach(x => ModelState.AddModelError("", x.Messages));
                    return View(tradeUser);
                }
                return RedirectToAction("Index");
            }

            return View(tradeUser);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TradeUser tradeUser = tradeUserManager.find(x => x.Id == id.Value);
            if (tradeUser == null)
            {
                return HttpNotFound();
            }
            return View(tradeUser);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TradeUser tradeUser)
        {
            ModelState.Remove("CreateOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                BusinessErrorResult<TradeUser> bussinesErrorResult = tradeUserManager.Update(tradeUser);
                if (bussinesErrorResult.Errors.Count > 0)
                {
                    bussinesErrorResult.Errors.ForEach(x => ModelState.AddModelError("", x.Messages));
                    return View(tradeUser);
                }
                return RedirectToAction("Index");
            }

            return View(tradeUser);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TradeUser tradeUser = tradeUserManager.find(x => x.Id == id.Value);
            if (tradeUser == null)
            {
                return HttpNotFound();
            }
            return View(tradeUser);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TradeUser tradeUser = tradeUserManager.find(x => x.Id == id);
            tradeUserManager.Delete(tradeUser);
            return RedirectToAction("Index");
        }

    }

}