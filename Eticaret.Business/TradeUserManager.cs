using Eticaret.Business.Abstract;
using Eticaret.Business.Results;
using Eticaret.Common.Helpers;
using Eticaret.Entities;
using Eticaret.Entities.Message;
using Eticaret.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.Business
{
    public class TradeUserManager : ManagerBase<TradeUser>
    {
        public BusinessErrorResult<TradeUser> tradeUserRegister(registerViewModel registerViewModel)
        {
            BusinessErrorResult<TradeUser> businessErrorResult = new BusinessErrorResult<TradeUser>();
            TradeUser tradeUser = find(p => p.UserName == registerViewModel.UserName || p.Email == registerViewModel.Email);
            if (tradeUser != null)
            {
                if (tradeUser.UserName == registerViewModel.UserName)
                {
                    businessErrorResult.AddError(EnumErrorMessages.UserNameAlreadyExits, "Kullanıcı adı kayıtlı.");
                }
                if (tradeUser.Email == registerViewModel.Email)
                {
                    businessErrorResult.AddError(EnumErrorMessages.EmailAlreadyExits, "E-Posta adresi kayıtlı.");
                }
            }
            else
            {
                int databaseResult = base.Insert(new TradeUser()
                {
                    UserName = registerViewModel.UserName,
                    ProfileImageFileName = "profil.jpg",
                    Email = registerViewModel.Email,
                    Password = registerViewModel.Password,
                    ActivateGuid = Guid.NewGuid(),
                    CreateOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    IsActive = false,
                    IsAdmin = false,
                });
                if (databaseResult > 0)
                {
                    businessErrorResult.Result = find(p => p.Email == registerViewModel.Email && p.UserName == registerViewModel.UserName);

                    string siteUrl = ConfigHelper.Get<string>("SiteRootUrl");
                    string activateUrl = $"{siteUrl}/Home/UserActivate/{businessErrorResult.Result.ActivateGuid}";
                    string body = $"Merhaba {businessErrorResult.Result.UserName};<br><br> Hesabınızı aktifleştirmek için <a href='{activateUrl}' target='_blank'>Tıklayınız</a> ";
                    MailHelper.SendMail(body, businessErrorResult.Result.Email, "ETrade Hesap Aktifleştirme");
                }
            }
            return businessErrorResult;
        }

        public BusinessErrorResult<TradeUser> GetUserById(int Id)
        {
            BusinessErrorResult<TradeUser> businessErrorResult = new BusinessErrorResult<TradeUser>
            {
                Result = find(p => p.Id == Id)
            };
            if (businessErrorResult.Result == null)
            {
                businessErrorResult.AddError(EnumErrorMessages.UserNotFound, "Kullanıcı Bulunamadı.");
            }
            return businessErrorResult;
        }

        public BusinessErrorResult<TradeUser> tradeUserLogin(loginViewModel loginViewModel)
        {
            BusinessErrorResult<TradeUser> businessErrorResult = new BusinessErrorResult<TradeUser>();
            businessErrorResult.Result = find(p => p.UserName == loginViewModel.UserName && p.Password == loginViewModel.Password);

            if (businessErrorResult.Result != null)
            {
                if (!businessErrorResult.Result.IsActive)
                {
                    businessErrorResult.AddError(EnumErrorMessages.UserrIsActive, "Hesabınız aktifleştirilmemiştir.");
                    businessErrorResult.AddError(EnumErrorMessages.CheckEmail, "Lütfen e-posta adresini kontrol ediniz.");
                }
            }
            else
            {
                businessErrorResult.AddError(EnumErrorMessages.UserNameOrPassWrong, "Kullanıcı adı ve şifre uyuşmamaktadır.");
            }
            return businessErrorResult;
        }

        public BusinessErrorResult<TradeUser> UpdateProfil(TradeUser tradeUserEditProfile)
        {
            TradeUser tradeUser = find(x => x.UserName == tradeUserEditProfile.UserName || x.Email == tradeUserEditProfile.Email);
            BusinessErrorResult<TradeUser> businessErrorResult = new BusinessErrorResult<TradeUser>();
            if (tradeUser != null && tradeUser.Id != tradeUserEditProfile.Id)
            {
                if (tradeUser.UserName == tradeUserEditProfile.UserName)
                {
                    businessErrorResult.AddError(EnumErrorMessages.UserNameAlreadyExits, "Kullanıcı adı kayıtlı");
                }
                if (tradeUser.Email == tradeUserEditProfile.Email)
                {
                    businessErrorResult.AddError(EnumErrorMessages.EmailAlreadyExits, "E-posta adresi kayıtlı.");
                }
                return businessErrorResult;
            }
            businessErrorResult.Result = find(x => x.Id == tradeUserEditProfile.Id);
            businessErrorResult.Result.Email = tradeUserEditProfile.Email;
            businessErrorResult.Result.Name = tradeUserEditProfile.Name;
            businessErrorResult.Result.Surname = tradeUserEditProfile.Surname;
            businessErrorResult.Result.Password = tradeUserEditProfile.Password;
            businessErrorResult.Result.UserName = tradeUserEditProfile.UserName;
            if (string.IsNullOrEmpty(tradeUserEditProfile.ProfileImageFileName) == false)
            {
                businessErrorResult.Result.ProfileImageFileName = tradeUserEditProfile.ProfileImageFileName;
            }
            if (base.Update(businessErrorResult.Result) == 0)
            {
                businessErrorResult.AddError(EnumErrorMessages.ProfileCouldNotUpdated, "Profil güncellenemedi.");
            }
            return businessErrorResult;
        }

        public BusinessErrorResult<TradeUser> RemoveUserById(int id)
        {
            BusinessErrorResult<TradeUser> businessErrorResult = new BusinessErrorResult<TradeUser>();
            TradeUser tradeUser = find(x => x.Id == id);

            if (tradeUser != null)
            {
                if (Delete(tradeUser) == 0)
                {
                    businessErrorResult.AddError(EnumErrorMessages.UserCouldNotRemove, "Kullanıcı silinemedi");
                    return businessErrorResult;
                }
            }
            else
            {
                businessErrorResult.AddError(EnumErrorMessages.UserCouldNotFind, "Kullanıcı bulunamadı.");
            }
            return businessErrorResult;
        }

        public BusinessErrorResult<TradeUser> ActivateUser(Guid activateGuid)
        {
            BusinessErrorResult<TradeUser> businessErrorResult = new BusinessErrorResult<TradeUser>();
            businessErrorResult.Result = find(p => p.ActivateGuid == activateGuid);
            if (businessErrorResult != null)
            {
                if (businessErrorResult.Result.IsActive)
                {
                    businessErrorResult.AddError(EnumErrorMessages.UserAlreadyActivate, "Kullanıcı zaten aktif edilmiştir.");
                }

                businessErrorResult.Result.IsActive = true;

                Update(businessErrorResult.Result);
            }
            else
            {
                businessErrorResult.AddError(EnumErrorMessages.ActivateIdDoesNotExits, "Aktifleştirilecek kullanıcı bulunamadı.");
            }

            return businessErrorResult;
        }


        //Method hidding
        public new BusinessErrorResult<TradeUser> Insert(TradeUser registerViewModel)
        {
            BusinessErrorResult<TradeUser> businessErrorResult = new BusinessErrorResult<TradeUser>();
            TradeUser tradeUser = find(p => p.UserName == registerViewModel.UserName || p.Email == registerViewModel.Email);
            businessErrorResult.Result = registerViewModel;
            if (tradeUser != null)
            {
                if (tradeUser.UserName == registerViewModel.UserName)
                {
                    businessErrorResult.AddError(EnumErrorMessages.UserNameAlreadyExits, "Kullanıcı adı kayıtlı.");
                }
                if (tradeUser.Email == registerViewModel.Email)
                {
                    businessErrorResult.AddError(EnumErrorMessages.EmailAlreadyExits, "E-Posta adresi kayıtlı.");
                }
            }
            else
            {
                businessErrorResult.Result.ProfileImageFileName = "profil.jpg";
                businessErrorResult.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(businessErrorResult.Result) == 0)
                {
                    businessErrorResult.AddError(EnumErrorMessages.UserCouldNotInserted, "Kullanıcı eklenemedi.");
                }
            }
            return businessErrorResult;
        }
        public new BusinessErrorResult<TradeUser> Update(TradeUser tradeUserEditUser)
        {
            TradeUser tradeUser = find(x => x.UserName == tradeUserEditUser.UserName || x.Email == tradeUserEditUser.Email);
            BusinessErrorResult<TradeUser> businessErrorResult = new BusinessErrorResult<TradeUser>();
            businessErrorResult.Result = tradeUserEditUser;

            if (tradeUser != null && tradeUser.Id != tradeUserEditUser.Id)
            {
                if (tradeUser.UserName == tradeUserEditUser.UserName)
                {
                    businessErrorResult.AddError(EnumErrorMessages.UserNameAlreadyExits, "Kullanıcı adı kayıtlı");
                }
                if (tradeUser.Email == tradeUserEditUser.Email)
                {
                    businessErrorResult.AddError(EnumErrorMessages.EmailAlreadyExits, "E-posta adresi kayıtlı.");
                }
                return businessErrorResult;
            }
            businessErrorResult.Result = find(x => x.Id == tradeUserEditUser.Id);
            businessErrorResult.Result.Email = tradeUserEditUser.Email;
            businessErrorResult.Result.Name = tradeUserEditUser.Name;
            businessErrorResult.Result.Surname = tradeUserEditUser.Surname;
            businessErrorResult.Result.Password = tradeUserEditUser.Password;
            businessErrorResult.Result.UserName = tradeUserEditUser.UserName;
            businessErrorResult.Result.IsActive = tradeUserEditUser.IsActive;
            businessErrorResult.Result.IsAdmin = tradeUserEditUser.IsAdmin;

            if (base.Update(businessErrorResult.Result) == 0)
            {
                businessErrorResult.AddError(EnumErrorMessages.UserCouldNotUpdated, "Kullanıcı güncellenemedi.");
            }
            return businessErrorResult;
        }
    }
}
