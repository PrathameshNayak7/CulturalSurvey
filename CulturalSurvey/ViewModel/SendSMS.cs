using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml;
using System.Threading.Tasks;
namespace CulturaSurvey.ViewModel
{
    public class SendSMS
    {
        private string SID = null;
        private string token = null;
        private string key = null;
        public SendSMS(string SID, string key, string token)
        {
            this.SID = "dfmail";
            this.key = "dfmail";
            this.token = "ddac450064312c03a44ff94d301cf7eabdbd62bd";
        }

        public string execute(string from, string to, string Body)
        {
            Exotel_Response exotel = new Exotel_Response();
            try
            {

                Dictionary<string, string> postValues = new Dictionary<string, string>();
                postValues.Add("From", from);
                postValues.Add("To", to);
                postValues.Add("Body", Body);
                postValues.Add("DltEntityId", "1001425560000014479");
                String postString = "";
                foreach (KeyValuePair<string, string> postValue in postValues)
                {
                    postString += postValue.Key + "=" + WebUtility.UrlEncode(postValue.Value) + "&";
                }

                postString = postString.TrimEnd('&');
                ServicePointManager.ServerCertificateValidationCallback = delegate
                {
                    return true;
                };
                //string smsURL = "http://api.exotel.in/v1/Accounts/dfmail/Sms/send";
                string smsURL = "https://53dadcf2fe17ad9b2e7256b14c99ee98e80e7d7ef377571a:53b019c7f4a6a26aa9c2bcd47e128437b7053d4109894e10@api.exotel.com/v1/Accounts/dfmail/Sms/send";
                HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(smsURL);
                objRequest.Credentials = new NetworkCredential(this.key, this.token);
                objRequest.Method = "POST";
                objRequest.ContentLength = postString.Length;
                objRequest.ContentType = "application/x-www-form-urlencoded";

                StreamWriter opWriter = null;
                opWriter = new StreamWriter(objRequest.GetRequestStream());
                opWriter.Write(postString);
                opWriter.Close();

                HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
                string postResponse = null;
                List<string> list = new List<string>();
                using (StreamReader responseStream = new StreamReader(objResponse.GetResponseStream()))
                {
                    postResponse = responseStream.ReadToEnd();
                    responseStream.Close();
                }
                string[] lines = postResponse.Split('\n');
                postResponse = lines[3].ToString();
                //foreach (string line in lines)
                //{
                //    Console.WriteLine(line);
                //}
                return postResponse;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }

        }
    }

    public class Exotel_Response
    {
        public string Status_Code
        {
            get; set;
        }
        public string Status_Description
        {
            get; set;
        }
    }
}