using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

public class Notification
{
    
    public bool Successful
    {
        get;
        set;
    }

    public string Response
    {
        get;
        set;
    }
    public Exception Error
    {
        get;
        set;
    }
    public static string AndroidPush(string deviceId, string message, string title, string imageurls)
    {
        Notification result = new Notification();
       
        var serverApiKey = "AAAAGPzAQoc:APA91bGeJa_hW2sUnNjOMbKQ5dvHiCtqvwLdWCaWUPQ0dGnL4CA-bxNs6zsAKp7cEZiwV7dLRoQTMnQpS002VLQZjLftwg_4o1cV2CoKADDDlDhOypFMmQMN__E6ojlgjOxyvVYRO5mn";
        var senderId = "107319673479";
        try
        {
                    
            result.Successful = false;
            result.Error = null;

            var value = message;
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            tRequest.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            tRequest.Headers.Add(string.Format("Authorization: key={0}", serverApiKey));
            tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));

           
            string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.title=" + title + "&data.message="
          + value + "&data.time=" + System.DateTime.Now.ToString() + "&data.image=" + imageurls + "&registration_id=" + deviceId.ToString() + "";
            Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            tRequest.ContentLength = byteArray.Length;

            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);

                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                        {
                            String sResponseFromServer = tReader.ReadToEnd();
                            result.Response = sResponseFromServer;
                            result.Successful = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            result.Successful = false;
            result.Response = null;
            result.Error = ex;
        }

        return result.Response.ToString();
    }

 }