using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfakimRegistrationProj.Models;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OfakimRegistrationProj.Controllers
{
    public class UserController : Controller
    {
        public static IEnumerable<User> Users;
        private static SemaphoreSlim semaphore = new SemaphoreSlim(1);

        [HttpGet]
        public ActionResult RegisterUser(int id = 0)
        {
            return View("RegisterUser", new User());
        }
        [HttpPost]
        public ActionResult RegisterUser(User userModel)
        {
            semaphore.Wait();
            semaphore.Release();
            return View("ShowAllUser", Users);
        }
        [HttpPost]
        public JsonResult PutUser(string order)
        {
            bool isOrderOk;
            semaphore.Wait();
            using (AppDBEntities AppDB = new AppDBEntities())
            {  
                var userModel = JsonConvert.DeserializeObject<User>(order);
                isOrderOk = AppDB.Simplevalidations(userModel);
                Users = AppDB.UpdateAndGetUsers(userModel, isOrderOk);
            }
            semaphore.Release();
            return Json(isOrderOk.ToString());
        }
    }
}