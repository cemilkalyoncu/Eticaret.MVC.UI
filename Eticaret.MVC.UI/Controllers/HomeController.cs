using Eticaret.Business;
using Eticaret.Business.Results;
using Eticaret.Entities;
using Eticaret.Entities.ValueObjects;
using Eticaret.MVC.UI.Filters;
using Eticaret.MVC.UI.Models;
using Eticaret.MVC.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Eticaret.MVC.UI.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        private CategoryManager categoryManager = new CategoryManager();
        private NoteManager noteManager = new NoteManager();
        private TradeUserManager tradeUserManager = new TradeUserManager();

        public ActionResult Index()
        {
            return View(noteManager.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(p => p.ModifiedOn).ToList());
        }
        public ActionResult ByCategory(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Category cat = categoryManager.Find(x => x.Id == id.Value);

            //if (cat == null)
            //{
            //    return HttpNotFound();
            //    //return RedirectToAction("Index", "Home");
            //}

            //List<Note> notes = cat.Notes.Where(
            //    x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList()

            List<Note> notes = noteManager.ListQueryable().Where(
                x => x.IsDraft == false && x.CategoryId == Id).OrderByDescending(
                x => x.ModifiedOn).ToList();

            return View("Index", notes);
        }
        public ActionResult MostLiked()
        {
            return View("Index", noteManager.ListQueryable().OrderByDescending(p => p.LikeCount).ToList());
        }
        public ActionResult About()
        {
            return View();
        }
        [Auth]
        public ActionResult ShowProfile()
        {
            BusinessErrorResult<TradeUser> businessErrorResult = tradeUserManager.GetUserById(CurrentSession.UserSession.Id);
            if (businessErrorResult.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = businessErrorResult.Errors

                };
                return View("Error", errorNotifyObj);
            }
            return View(businessErrorResult.Result);
        }
        [Auth]
        public ActionResult EditProfile()
        {
            BusinessErrorResult<TradeUser> businessErrorResult = tradeUserManager.GetUserById(CurrentSession.UserSession.Id);
            if (businessErrorResult.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = businessErrorResult.Errors

                };
                return View("Error", errorNotifyObj);
            }
            return View(businessErrorResult.Result);
        }
        [Auth]
        [HttpPost]
        public ActionResult EditProfile(TradeUser tradeUserEditProfile, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null && (
                ProfileImage.ContentType == "image/jpeg" ||
                ProfileImage.ContentType == "image/jpg" ||
                ProfileImage.ContentType == "image/png"
                ))
                {
                    string fileName = $"tradeUser_{tradeUserEditProfile}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{fileName}"));
                    tradeUserEditProfile.ProfileImageFileName = fileName;
                }

                BusinessErrorResult<TradeUser> businessErrorResult = tradeUserManager.UpdateProfil(tradeUserEditProfile);
                if (businessErrorResult.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = businessErrorResult.Errors,
                        Title = "Profil Güncellenemedi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };
                    return View("Error", errorNotifyObj);
                }

                CurrentSession.Set<TradeUser>("login", businessErrorResult.Result);

                return RedirectToAction("ShowProfile");
            }
            return View(tradeUserEditProfile);

        }
        [Auth]
        public ActionResult DeleteProfile()
        {
            BusinessErrorResult<TradeUser> businessErrorResult = tradeUserManager.RemoveUserById(CurrentSession.UserSession.Id);
            if (businessErrorResult.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Profil Silinemedi.",
                    Items = businessErrorResult.Errors,
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(loginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                BusinessErrorResult<TradeUser> businessErrorResult = tradeUserManager.tradeUserLogin(loginViewModel);
                if (businessErrorResult.Errors.Count > 0)
                {
                    businessErrorResult.Errors.ForEach(p => ModelState.AddModelError("", p.Messages + p.WrongCode.GetHashCode()));
                    return View(loginViewModel);

                }
                CurrentSession.Set<TradeUser>("login", businessErrorResult.Result);
                return RedirectToAction("Index");
            }
            return View(loginViewModel);
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(registerViewModel registerViewModel)
        {

            if (ModelState.IsValid)
            {
                BusinessErrorResult<TradeUser> businessErrorResult = tradeUserManager.tradeUserRegister(registerViewModel);

                if (businessErrorResult.Errors.Count > 0)
                {
                    businessErrorResult.Errors.ForEach(p => ModelState.AddModelError("", p.Messages + p.WrongCode.GetHashCode()));
                    return View(registerViewModel);

                }
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login",
                };
                notifyObj.Items.Add("Lütfen e-posta adresinize gönderdiğimiz aktivasyon link'ine tıklayarak hesabınızı aktive ediniz. Hesabınızı aktive etmeden not ekleyemez ve beğenme yapamazsınız.");
                return View("Ok", notifyObj);
            }
            return View(registerViewModel);
        }
        public ActionResult UserActivate(Guid Id)
        {
            BusinessErrorResult<TradeUser> businessErrorResult = tradeUserManager.ActivateUser(Id);
            if (businessErrorResult.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem!",
                    Items = businessErrorResult.Errors

                };
                return View("Error", errorNotifyObj);
            }

            OkViewModel okNotifyObj = new OkViewModel()
            {
                Title = "Hesap aktifleştirildi.",
                RedirectingUrl = "/Home/Login"
            };
            okNotifyObj.Items.Add("Hesabınız Aktifleştirildi. Hesabınızda not paylaşabilir ve beğenme yapabilirsiniz.");
            return View("Ok", okNotifyObj);

        }
        public ActionResult Logout()
        {
            Session.Clear();
            return View();
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult HassError()
        {
            return View();
        }
    }
}