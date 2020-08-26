using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using ReferAll.UrlHash.Struct;

namespace ReferAll.UrlHash
{
    public class DeleteHash
    {
        #region Fields
        /// <summary>
        /// Instance of Delete Hash Class.
        /// </summary>
        private static DeleteHash instance;
        public static DeleteHash Instance
        {
            get
            {
                if (instance == null)
                    instance = new DeleteHash();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public DeleteHash()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void Delete(string wp_id, string session_key, string hash, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            dict.Add("hash", hash);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/referall/v1/urlhash/delete", content);
            response.EnsureSuccessStatusCode();

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                Token token = JsonConvert.DeserializeObject<Token>(result);

                bool success = token.status == "success" ? true : false;
                string data = token.status == "success" ? result : token.message;
                callback(success, data);
            }
            else
            {
                callback(false, "Network Error! Check your connection.");
            }
        }
        #endregion
    }
}
