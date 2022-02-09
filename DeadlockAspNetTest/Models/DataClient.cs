using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

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
    }
}