using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeadlockWpfApp
{
    internal class DataClient
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
                var result = await client.GetStringAsync(DataUrl);
                return result;
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
                var result = await client.GetStringAsync(DataUrl).ConfigureAwait(false);
                return result;
            }
        }

        // Other ConfigureAwait(false) footguns:
        // - Taking in a callback/delegate that requires being on the same SyncContext
        // - Only calling ConfigureAwait(false) on the first async method and not all
    }
}
