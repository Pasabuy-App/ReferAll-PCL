using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using ReferAll.Controller.Struct;

namespace ReferAll.Visits
{
    public class Visits
    {
        #region Fields
        /// <summary>
        /// Instance of Visit Class.
        /// </summary>
        private static Visits instance;
        public static Visits Instance
        {
            get
            {
                if (instance == null)
                    instance = new Visits();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public Visits()
        {
            client = new HttpClient();
        }
        #endregion
        #region Insert Methods
        public async void Insert(string wp_id, string session_key, string mkey, string hash, Action<bool, string> callback)
        {
            var dict = new Dictionary<string, string>();
                dict.Add("wpid", wp_id);
                dict.Add("snky", session_key);
                dict.Add("mkey", mkey);
                dict.Add("hash", hash);
            var content = new FormUrlEncodedContent(dict);

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/referall/v1/visits/insert", content);
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
