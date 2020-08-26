using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using ReferAll.UrlHash.Struct;

namespace ReferAll.UrlHash
{
    public class CreateHash
    {
        #region Fields
        /// <summary>
        /// Instance of Create Hash Class.
        /// </summary>
        private static CreateHash instance;
        public static CreateHash Instance
        {
            get
            {
                if (instance == null)
                    instance = new CreateHash();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public CreateHash()
        {
            client = new HttpClient();
        }
        #endregion
        #region Methods
        public async void Create(string wp_id, string session_key, string type, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
            dict.Add("wpid", wp_id);
            dict.Add("snky", session_key);
            dict.Add("type", type);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/referall/v1/urlhash/create", content);
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
