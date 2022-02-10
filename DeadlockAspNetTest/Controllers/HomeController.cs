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

        /// <summary>
        /// Demonstrate calling an async method using async/await
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AsyncAwait()
        {
            var result = await DataClient.GetDataAsync();
            return View(new ViewModel(result));
        }

        /// <summary>
        /// Demonstrate calling an async method using GetAwaiter().GetResult() which deadlocks
        /// </summary>
        /// <returns></returns>
        public ActionResult Deadlock()
        {
            var result = DataClient.GetDataAsync().GetAwaiter().GetResult();
            return View(new ViewModel(result));
        }

        /// <summary>
        /// Demonstrate calling a fake async method (actually returns synchronously) using GetAwaiter().GetResult()
        /// which doesn't deadlock since the data is returnned synchronously
        /// </summary>
        /// <returns></returns>
        public ActionResult CachedData()
        {
            var result = DataClient.GetCachedDataAsync().GetAwaiter().GetResult();
            return View(new ViewModel(result));
        }

        /// <summary>
        /// Demonstrate calling an async method that uses ConfigureAwait(false) using GetAwaiter().GetResult().
        /// ConfigureAwait(false) prevents the deadlock from happening! But doesn't resume with the same syncrhonization context
        /// (i.e. HttpContext.Current == null). But it suceeds in this method because we don't access anything in the sync context
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfigureAwaitOkay()
        {
            var result = DataClient.GetDataConfigureAwaitFalseAsync().GetAwaiter().GetResult();
            return View(new ViewModel(result));
        }

        /// <summary>
        /// Demonstrate calling an async method that uses ConfigureAwait(false) using GetAwaiter().GetResult().
        /// ConfigureAwait(false) prevents the deadlock from happening! But doesn't resume with the same syncrhonization context
        /// (i.e. HttpContext.Current == null). This method fails cuz HttpContext.Current is used but is null :(
        /// </summary>
        /// <returns></returns>
        public ActionResult ConfigureAwaitBad()
        {
            var result = DataClient.GetDataConfigureFalseWithHttpContextAsync().GetAwaiter().GetResult();
            return View(new ViewModel(result));
        }
    }
}