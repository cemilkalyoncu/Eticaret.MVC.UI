using Eticaret.Business;
using Eticaret.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Eticaret.MVC.UI.Models
{
    public class CacheHelper
    {
        public static List<Category> GetCategoriesFromCache()
        {
            var result = WebCache.Get("category-cache");
            if (result == null)
            {
                CategoryManager categoryManager = new CategoryManager();
                result = categoryManager.List();
                WebCache.Set("category-cache", result,20,true);
            }
            return result;
        }
        public static void RemoveCategoriesFromCache()
        {
            Remove("category-remove");
        }
        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}