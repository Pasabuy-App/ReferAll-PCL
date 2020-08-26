using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;
using ReferAll.UrlHash.Struct;

namespace ReferAll.Coupons
{
    public class DeleteCoupons
    {
        #region Fields
        /// <summary>
        /// Instance of Delete Coupons Class.
        /// </summary>
        private static DeleteCoupons instance;
        public static DeleteCoupons Instance
        {
            get
            {
                if (instance == null)
                    instance = new DeleteCoupons();
                return instance;
            }
        }
        #endregion
        #region Constructor
        /// <summary>
        /// Web service for communication to our Backend.
        /// </summary>
        HttpClient client;
        public DeleteCoupons()
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

            var response = await client.PostAsync(BaseClass.BaseDomainUrl + "/referall/v1/coupons/delete", content);
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
