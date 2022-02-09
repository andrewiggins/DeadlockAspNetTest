using DeadlockAspNetTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DeadlockAspNetTest.Controllers
{
    public class HomeController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AsyncAwait()
        {
            var result = await DataClient.GetDataAsync();
            return View(new ViewModel(result));
        }

        public ActionResult Deadlock()
        {
            var result = DataClient.GetDataAsync().GetAwaiter().GetResult();
            return View(new ViewModel(result));
        }

        public ActionResult CachedData()
        {
            var result = DataClient.GetCachedDataAsync().GetAwaiter().GetResult();
            return View(new ViewModel(result));
        }
    }
}