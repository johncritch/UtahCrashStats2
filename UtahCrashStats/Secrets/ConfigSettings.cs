using Newtonsoft.Json.Linq;
namespace UtahCrashStats.Secrets
{
    public class ConfigSettings : IConfigSettings
    {
        private string _ClientId;
        private string _ClientSecret;
        private string _CrashDbConnection;
        private string _StoryDbConnection;
        public ConfigSettings()
        {
            Init();
        }
        public void Init()
        {
            var secretValues = JObject.Parse(SecretsManager.GetSecret());
            if (secretValues != null)
            {
                _ClientId = secretValues["Authentication:Google:ClientId"].ToString();
                _ClientSecret = secretValues["Authentication:Google:ClientSecret"].ToString();
                _CrashDbConnection = secretValues["CrashDbConnection"].ToString();
                _StoryDbConnection = secretValues["StoryDbConnection"].ToString();
            }
        }
        public string ClientId
        {
            get
            {
                return _ClientId;
            }
            set
            {
                _ClientId = value;
            }
        }
        public string ClientSecret
        {
            get
            {
                return _ClientSecret;
            }
            set
            {
                _ClientSecret = value;
            }
        }
        public string CrashDbConnection
        {
            get
            {
                return _CrashDbConnection;
            }
            set
            {
                _CrashDbConnection = value;
            }
        }
        public string StoryDbConnection
        {
            get
            {
                return _StoryDbConnection;
            }
            set
            {
                _StoryDbConnection = value;
            }
        }
    }
    public interface IConfigSettings
    {
        string ClientId
        {
            get;
            set;
        }
        string ClientSecret
        {
            get;
            set;
        }
        string CrashDbConnection
        {
            get;
            set;
        }
        string StoryDbConnection
        {
            get;
            set;
        }
    }
}