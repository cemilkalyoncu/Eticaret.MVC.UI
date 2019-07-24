using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Eticaret.Entities;

namespace Eticaret.DataAccess.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext databaseContext)
        {
            TradeUser admin = new TradeUser()
            {
                Name = "Cemil",
                Surname = "Kalyoncu",
                Email = "dangerous_sentenced@hotmail.com",
                IsActive = true,
                IsAdmin = true,
                UserName = "ETradeAdmin",
                Password = "12345",
                ProfileImageFileName = "profil.jpg",
                CreateOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "ETradeAdmin",
            };
            databaseContext.TradeUsers.Add(admin);
            TradeUser user = new TradeUser()
            {
                Name = "Cemil",
                Surname = "Kalyoncu",
                Email = "dangerous_sentenced@hotmail.com",
                IsActive = true,
                IsAdmin = false,
                UserName = "ETradeUser",
                Password = "12345",
                ProfileImageFileName = "profil.jpg",
                CreateOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "ETradeAdmin",
            };
            databaseContext.TradeUsers.Add(user);
            for (int i = 0; i < 15; i++)
            {
                TradeUser userAdmin = new TradeUser()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    IsActive = true,
                    IsAdmin = false,
                    UserName = $"user{i}",
                    Password = "12345",
                    ProfileImageFileName = "profil.jpg",
                    CreateOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUserName = $"user{i}"
                };
                databaseContext.TradeUsers.Add(userAdmin);
            }
            databaseContext.SaveChanges();

            List<TradeUser> tradeUserList = databaseContext.TradeUsers.ToList();

            for (int i = 0; i < 10; i++)
            {
                Category category = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreateOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUserName = "ETradeAdmin"
                };
                databaseContext.Categories.Add(category);


                for (int j = 0; j < FakeData.NumberData.GetNumber(5, 9); j++)
                {
                    TradeUser tradeUser = tradeUserList[FakeData.NumberData.GetNumber(0, tradeUserList.Count - 1)];
                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        CreateOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        Owner = tradeUserList[FakeData.NumberData.GetNumber(0, tradeUserList.Count - 1)],
                        ModifiedUserName = tradeUser.UserName
                    };
                    category.Notes.Add(note);


                    for (int k = 0; k < FakeData.NumberData.GetNumber(3, 5); k++)
                    {
                        TradeUser tradeUserOwner = tradeUserList[FakeData.NumberData.GetNumber(0, tradeUserList.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                            CreateOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            Owner = tradeUserList[FakeData.NumberData.GetNumber(0, tradeUserList.Count - 1)],
                            ModifiedUserName = tradeUserOwner.UserName
                        };
                        note.Comments.Add(comment);
                    }
                    databaseContext.SaveChanges();

                    for (int h = 0; h < note.LikeCount; h++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = tradeUserList[h],
                        };
                        note.Likes.Add(liked);
                    }
                }
            }
            databaseContext.SaveChanges();
        }
    }
}
