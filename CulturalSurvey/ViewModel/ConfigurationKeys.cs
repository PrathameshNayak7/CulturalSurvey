using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CulturaSurvey.ViewModel
{
    public static class ConfigurationKeys
    {
        #region SkillingPaytmKeys
        public static string MerchantId = System.Configuration.ConfigurationManager.AppSettings["MerchantId"];

        public static string MerchantKey = System.Configuration.ConfigurationManager.AppSettings["MerchantKey"];

        public static string Website = System.Configuration.ConfigurationManager.AppSettings["Website"];

        public static string IndustryType = System.Configuration.ConfigurationManager.AppSettings["IndustryType"];

        public static string ChannelId = System.Configuration.ConfigurationManager.AppSettings["ChannelId"];
        #endregion SkillingPaytmKeys

        #region
        public static string StartupMerchantId = System.Configuration.ConfigurationManager.AppSettings["StartupMerchantId"];

        public static string StartupMerchantKey = System.Configuration.ConfigurationManager.AppSettings["StartupMerchantKey"];

        public static string StartupWebsite = System.Configuration.ConfigurationManager.AppSettings["StartupWebsite"];

        public static string StartupIndustryType = System.Configuration.ConfigurationManager.AppSettings["StartupIndustryType"];

        public static string StartupChannelId = System.Configuration.ConfigurationManager.AppSettings["StartupChannelId"];
        #endregion


    }
}