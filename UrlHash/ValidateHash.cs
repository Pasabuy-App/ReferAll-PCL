using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using ReferAll.UrlHash.Struct;

namespace ReferAll.UrlHash
{
    public class ValidateHash
    {
        #region Fields
        /// <summary>
        /// Instance of Validate Hash Class.
        /// </summary>
        private static ValidateHash instance;
        public static ValidateHash Instance
        {
            get
            {
                if (instance == null)
                    instance = new ValidateHash();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public ValidateHash()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void Validate(string wp_id, string session_key, string hash, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            dict.Add("hash", hash);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/referall/v1/urlhash/validate", content);
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
