using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace DeadlockAspNetTest.Models
{
    public static class DataClient
    {
        private const string DataUrl = "https://login.microsoftonline.com/common/v2.0/.well-known/openid-configuration";

        /// <summary>
        /// A method that actually awaits some for some asynchronous operation to complete
        /// </summary>
        /// <returns>a string of data</returns>
        public static async Task<string> GetDataAsync()
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(DataUrl);
            }
        }

        /// <summary>
        /// A method that returns a task but actually completes synchronously (e.g. data is returned from cache)
        /// </summary>
        /// <returns>a string of cached data</returns>
        public static Task<string> GetCachedDataAsync()
        {
            return Task.FromResult("some synchronous cached data");
        }

        /// <summary>
        /// A method that makes an asynchronous API call and calls ConfigureAwait(false) on the API method
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetDataConfigureAwaitFalseAsync()
        {
            using (var client = new HttpClient())
            {
                return await client.GetStringAsync(DataUrl).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// A method that makes an asynchronous API call and calls ConfigureAwait(false) on the API method,
        /// but also tries to access System.Web.HttpContext.Current right after calling ConfigureAwait(false)
        /// </summary>
        /// <returns></returns>
        public static async Task<string> GetDataConfigureFalseWithHttpContextAsync()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync(DataUrl).ConfigureAwait(false);
                Console.WriteLine(System.Web.HttpContext.Current.Request.QueryString); // Will blow up!
                return result;
            }
        }
    }
}