using Eticaret.DataAccess.EntityFramework;
using Eticaret.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eticaret.Business
{
    public class Test
    {
        //public Test()
        //{
        //    //DataAccess.DatabaseContext databaseContext = new DataAccess.DatabaseContext();
        //    //databaseContext.TradeUsers.ToList();
        //    //databaseContext.Categories.ToList();
        //    //databaseContext.Comments.ToList();
        //    Repository<Category> repository = new Repository<Category>();
        //    List<Category> categories = repository.List();
        //}
        //Repository<TradeUser> repositoryTradeUser = new Repository<TradeUser>();
        //Repository<Category> repositoryCategory = new Repository<Category>();
        //int result;
        //public void InsertTest()
        //{
        //    result = repositoryTradeUser.Insert(new TradeUser()
        //    {
        //        Name = "Gökhan",
        //        Surname = "Güler",
        //        Email = "gokhanguler@gmail.com",
        //        IsActive = true,
        //        IsAdmin = true,
        //        UserName = "cornivus",
        //        Password = "12345",
        //        CreateOn = DateTime.Now,
        //        ModifiedOn = DateTime.Now.AddMinutes(5),
        //        ModifiedUserName = "cornivus",
        //    });
        //}
        //public void UpdateTest()
        //{
        //    TradeUser tradeUser = repositoryTradeUser.find(p => p.UserName == "cornivus");
        //    if (tradeUser != null)
        //    {
        //        tradeUser.UserName = "p0ring";
        //        result = repositoryTradeUser.Save();

        //    }

        //}
        //public void DeleteTest()
        //{
        //    TradeUser tradeUser = repositoryTradeUser.find(p => p.Id == 1013);
        //    if (tradeUser != null)
        //    {
        //        repositoryTradeUser.Delete(tradeUser);
        //        repositoryTradeUser.Save();

        //    }

        //}
    }
}
