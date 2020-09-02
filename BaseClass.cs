
namespace ReferAll
{
    public class RAHost
    {
        private static RAHost instance;
        public static RAHost Instance
        {
            get
            {
                if (instance == null)
                    instance = new RAHost();
                return instance;
            }
        }

        private bool isInitialized = false;
        private string baseUrl = "http://localhost";
        public string BaseDomain
        {
            get
            {
                return baseUrl + "/wp-json";
            }
        }

        public void Initialized(string url)
        {
            if (!isInitialized)
            {
                baseUrl = url;
                isInitialized = true;
            }
        }

    }
}
