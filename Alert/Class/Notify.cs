﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Net.Mime;
using Alert.Class;

namespace Alert.Class
{
    class Notify
    {
        public string connection = ConfigurationManager.ConnectionStrings["Notify"].ToString();
        public string RRconnection = ConfigurationManager.ConnectionStrings["RR"].ToString();
        public string Mail_Sub = ConfigurationManager.AppSettings["Mail_Sub"].ToString();
        public string URL = ConfigurationManager.AppSettings["URL"].ToString();
        public string RR_URL = ConfigurationManager.AppSettings["RR_URL"].ToString();
        public static string key = ConfigurationManager.AppSettings["key"].ToString();
        public static string bash_key = ConfigurationManager.AppSettings["bash_key"].ToString();
      
        public async Task Generate_Link(int survey_id)
        {
            string Link_Id = "";
            string URL_Link = "";
            try
            {
                DBLayer db = new DBLayer();
                MySqlParameter[] Qry1 = new MySqlParameter[] {
                    new MySqlParameter("p_Survey_Id", survey_id)
                };
                DataTable dt = await db.ExecuteAdapterAsync(0, connection, "USP_GET_SURVEY_USER_LIST", Qry1);
               for(int i=0; i < dt.Rows.Count; i++)
                {

                    Link_Id = await  Encrypt("0000X"+dt.Rows[i]["Mapping_Slno"].ToString())+"S";
                    URL_Link = URL + Link_Id;
                    MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_Mapping_Slno", long.Parse(dt.Rows[i]["Mapping_Slno"].ToString())),
                    new MySqlParameter("p_URL_Link", URL_Link)
                };
                   int Response_Id = await db.ExecuteAsync(connection, "USP_UPDATE_USER_LINK", queryParams);
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task Send_Invitation(int survey_id,string Type)
        {
 
            string URL_Link = "";
            string Mapping_Id = "";
            string Mail_Id = "";
            string Employee_Name = "";
            string Survey_Name = "";
            string Message = "";
            string Survey_StartDate = "";
            string Survey_EndDate = "";
            int Survey_Year = 0;
            try
            {
                DBLayer db = new DBLayer();
                MySqlParameter[] Qry1 = new MySqlParameter[] {
                    new MySqlParameter("p_Survey_Id", survey_id),
                    new MySqlParameter("p_Type", Type)
                };
                DataTable dt = await db.ExecuteAdapterAsync(0, connection, "USP_GET_SEND_SURVEY_LINK_MAIL", Qry1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Mapping_Id= dt.Rows[i]["Mapping_Slno"].ToString();
                    Survey_Name = dt.Rows[i]["Survey_Name"].ToString();
                    Employee_Name = dt.Rows[i]["Employee_Name"].ToString();
                    Mail_Id = dt.Rows[i]["email_Id"].ToString().Trim();
                    URL_Link = dt.Rows[i]["URL_Link"].ToString();
                    Survey_StartDate = dt.Rows[i]["Survey_StartDate"].ToString();
                    Survey_EndDate = dt.Rows[i]["Survey_EndDate"].ToString();
                    Survey_Year = int.Parse(dt.Rows[i]["Survey_Year"].ToString());
                    Message = "<div style='font-size:11pt;'>";
                    Message += "Dear " + Employee_Name + ",<br>";
                    Message += "<p>Welcome to the Culture Survey "+ Survey_Year+ ". </p>";              
                    Message += "<p><h4>Please click here to take the survey  :&nbsp; " + URL_Link+"</h4></p>";
                    Message += "<p>For technical assistance, you may connect with Mr. Sharad Noolvi on 9738433247 or email him at sharad.tech@dfmail.org.";
                     Message += "<br>Please note that the survey will be live from " + Survey_StartDate+" till "+  Survey_EndDate+" , after which this link will be disabled.</p>";
                    Message += "<p>Thank you </p>";
                     Message += "</div>";
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(Message.ToString(), null, MediaTypeNames.Text.Html);

                    // Add the alternate views instead of using MailMessage.Body
                    MailMessage m = new MailMessage();
                    m.AlternateViews.Add(avHtml);

                    // Address and send the message
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    m.From = new MailAddress("culturesurvey@dfmail.org", "Cultural Survey");
                    m.To.Add(Mail_Id);
                    m.Subject = "Welcome to the Culture Survey " + Survey_Year;
                    m.IsBodyHtml = true;
                    m.Body = Message.ToString();
                    SmtpServer.Port = 587;
                    SmtpServer.UseDefaultCredentials = true;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("culturesurvey@dfmail.org", "dfTech@1234");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(m);
                    MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_Mapping_Slno",Mapping_Id)           
                };
                  int Response_Id = await db.ExecuteAsync(connection, "USP_UPDATE_INVITATION_SENT", queryParams);
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task Send_Reminder(int survey_id, string Type, string PreviousDay)
        {

            string URL_Link = "";
            string Mapping_Id = "";
            string Mail_Id = "";
            string Employee_Name = "";
            string Survey_Name = "";
            string Message = "";
            int Reminder = 0;
            int isCompleted_Survey = 0;
            int Completion_Percentage = 0;
            int Survey_Year = 0;
            string Survey_EndDate = "";
            try
            {
                DBLayer db = new DBLayer();
                MySqlParameter[] Qry1 = new MySqlParameter[] {
                    new MySqlParameter("p_Survey_Id", survey_id),
                    new MySqlParameter("p_Type", Type)
                };
                DataTable dt = await db.ExecuteAdapterAsync(0, connection, "USP_GET_SEND_SURVEY_LINK_MAIL", Qry1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Mapping_Id = dt.Rows[i]["Mapping_Slno"].ToString();
                    Survey_Name = dt.Rows[i]["Survey_Name"].ToString();
                    Employee_Name = dt.Rows[i]["Employee_Name"].ToString();
                    Mail_Id = dt.Rows[i]["email_Id"].ToString();
                    URL_Link = dt.Rows[i]["URL_Link"].ToString();
                    Survey_EndDate = dt.Rows[i]["Survey_EndDate"].ToString();
                    Reminder = int.Parse(dt.Rows[i]["Gentle_Remiders"].ToString());
                    Completion_Percentage = int.Parse(dt.Rows[i]["_CompletedPercentage"].ToString());
                    Survey_Year = int.Parse(dt.Rows[i]["Survey_Year"].ToString());
                    isCompleted_Survey = int.Parse(dt.Rows[i]["isCompleted_Survey"].ToString());
                    Message = "<div style='font-size:11pt;'>";
                    Message += "Dear " + Employee_Name + ", ";
                    Message += "<br><b> " + PreviousDay.ToString() + "</b> day update on the DF staff participation in the Culture Survey  " + Survey_Year + ": <b>" + Completion_Percentage + " % .</b>";
                    Message += "<br> We appreciate your participation in the survey and request the employees who have not yet taken the survey, to do so at the earliest.";

                    if (isCompleted_Survey == 0)
                    {
                        Message += "<h4><b>Please click here to take the survey  :&nbsp;</b>" + URL_Link + "</h4>";
                    }
                    Message += "<div> <br><br>";
                    Message += "<hr><p style='text-align:center;font-size:x-small;'>Auto generated mail from Technology Tool @ <b>Deshpande Foundation</b>  <hr></p>";
                    Message += "</div>";
                    //  Message += "<center></br> <img src='https://www.deshpandefoundationindia.org/images/dflogo.png' style='width:200px;height:70px;' /></center>";
                    Message += "</div>";
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(Message.ToString(), null, MediaTypeNames.Text.Html);

                    // Add the alternate views instead of using MailMessage.Body
                    MailMessage m = new MailMessage();
                    m.AlternateViews.Add(avHtml);

                    // Address and send the message
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    m.From = new MailAddress("culturesurvey@dfmail.org", "Cultural Survey");
                    m.To.Add(Mail_Id);
                    m.Subject = "Culture Survey " + Survey_Year + " | Daily Update";
                    m.IsBodyHtml = true;
                    m.Body = Message.ToString();

                    SmtpServer.Port = 587;
                    SmtpServer.UseDefaultCredentials = true;

                    SmtpServer.Credentials = new System.Net.NetworkCredential("culturesurvey@dfmail.org", "dfTech@1234");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(m);

                    MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_Mapping_Slno",Mapping_Id)
                };
                    int Response_Id = await db.ExecuteAsync(connection, "USP_WEB_UPDATE_GENTLE_REMINDER", queryParams);
                }

            }
            catch (Exception ex)
            {

            }
        }


        #region R&R
        public async Task RR_Generate_Link(int Reward_Id)
        {
            string Link_Id = "";
            string URL_Link = "";
            try
            {
                DBLayer db = new DBLayer();
                MySqlParameter[] Qry1 = new MySqlParameter[] {
                    new MySqlParameter("p_Reward_Id", Reward_Id)
                };
                DataTable dt = await db.ExecuteAdapterAsync(0, RRconnection, "USP_GET_RR_USER_LIST", Qry1);
                Console.WriteLine("Total Count " + dt.Rows.Count);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                   
                    Link_Id = await Encrypt("0000X" + dt.Rows[i]["User_Id"].ToString()) + "S";
                    URL_Link = RR_URL + Link_Id;
                    MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_User_Id", long.Parse(dt.Rows[i]["User_Id"].ToString())),
                    new MySqlParameter("p_URL_Link", URL_Link)
                };
                    
                    int Response_Id = await db.ExecuteAsync(RRconnection, "USP_UPDATE_USER_LINK", queryParams);
                    Console.WriteLine("Update Started :" + i);
                }
                Console.WriteLine("Update Closed ");

            }
            catch (Exception ex)
            {

            }
        }

        public async Task RR_Send_Invitation(int survey_id, string Type)
        {

            string URL_Link = "";
            string Mapping_Id = "";
            string Mail_Id = "";
            string Employee_Name = "";
            string Survey_Name = "";
            string Message = "";
            string Survey_StartDate = "";
            string Survey_EndDate = "";
            string Reward_Month = "";
            string Greetings = "";
            string Objectives = "";
            string Key_Points = "";
            string Reward_Paramers = "";
            string Regards = "";
            string Logo1 = "";
            string Logo2 = "";
            string Main_Subject = "";
            int Survey_Year = 0;
            try
            {
                DBLayer db = new DBLayer();
                MySqlParameter[] Qry1 = new MySqlParameter[] {
                    new MySqlParameter("p_Reward_Id", survey_id),
                    new MySqlParameter("p_Type", Type)
                };
                DataTable dt = await db.ExecuteAdapterAsync(0, RRconnection, "USP_GET_SEND_REWARD_LINK_MAIL", Qry1);
                Console.WriteLine("Total User :" + dt.Rows.Count);
                for (int i = 0; i < dt.Rows.Count; i++)
                {   
                    Mapping_Id = dt.Rows[i]["User_Id"].ToString();
                    Survey_Name = dt.Rows[i]["Reward_Name"].ToString();
                    Employee_Name = dt.Rows[i]["Employee_Name"].ToString();
                    Mail_Id = dt.Rows[i]["Email_Id"].ToString().Trim();
                    URL_Link = dt.Rows[i]["URL_Link"].ToString();
                    Survey_StartDate = dt.Rows[i]["Reward_StartDate"].ToString();
                    Survey_EndDate = dt.Rows[i]["Reward_EndDate"].ToString();
                    Survey_Year = int.Parse(dt.Rows[i]["Survey_Year"].ToString());
                    Reward_Month = dt.Rows[i]["Reward_Month"].ToString();
                    Greetings = dt.Rows[i]["Greeting"].ToString();
                    Objectives = dt.Rows[i]["Objective"].ToString();
                    Key_Points = dt.Rows[i]["Key_Points"].ToString();
                    Reward_Paramers = dt.Rows[i]["Parameters"].ToString();
                    Regards = dt.Rows[i]["Regards"].ToString();
                    Logo1 = dt.Rows[i]["Logo_1"].ToString();
                    Logo2 = dt.Rows[i]["Logo_2"].ToString();
                    Main_Subject = dt.Rows[i]["Mail_Subject"].ToString();
                    Message = "<div style='font-size:11pt;'>";
                    Message += "Dear " + Employee_Name + ",<br>";
                    Message += Greetings;
                    Message += Objectives;
                    Message += Key_Points;
                    Message += Reward_Paramers;
                    Message += "<p><h4>Link  :&nbsp; " + URL_Link + "</h4></p>";
                    Message += Regards;
                    Message += "</div>";

                    Message += "<div> <br><br>";
                    Message += "<hr><p style='text-align:center;font-size:x-small;'>Auto generated mail from Technology Tool @ <b><a href='https://www.deshpandefoundation.org/' target='_blank'>Deshpande Foundation</a></b>  <hr></p>";
                    Message += "</div>";
                   //Message += "<center></br> <img src='"+Logo1+"' />" +
                   //     "<img src=src='" + Logo2 + "' /></center>";
                    Message += "</div>";
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(Message.ToString(), null, MediaTypeNames.Text.Html);

                    // Add the alternate views instead of using MailMessage.Body
                    MailMessage m = new MailMessage();
                    m.AlternateViews.Add(avHtml);

                    // Address and send the message
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    m.From = new MailAddress("recognition@dfmail.org", Reward_Month+ " Recognition");
                    m.To.Add(Mail_Id);
                    m.Subject = Main_Subject;
                    m.IsBodyHtml = true;
                    m.Body = Message.ToString();
                    SmtpServer.Port = 587;
                    SmtpServer.UseDefaultCredentials = true;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("recognition@dfmail.org", "dfTech@1234");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(m);
                    MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_Mapping_Slno",Mapping_Id)
                };
                    int Response_Id = await db.ExecuteAsync(RRconnection, "USP_UPDATE_INVITATION_SENT", queryParams);

                    Console.WriteLine("Invitation Sent to :" + Mail_Id);
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task RR_Send_Reminder(int Reward_id, string Type, string PreviousDay)
        {

            string URL_Link = "";
            string Mapping_Id = "";
            string Mail_Id = "";
            string Employee_Name = "";
            string Survey_Name = "";
            string Message = "";
            int Reminder = 0;
            string Reward_Month = "";
            int isCompleted_Survey = 0;
            int Completion_Percentage = 0;
            int Survey_Year = 0;
            string Main_Subject = "";
            string Survey_EndDate = "";
            try
            {
                DBLayer db = new DBLayer();
                MySqlParameter[] Qry1 = new MySqlParameter[] {
                   new MySqlParameter("p_Reward_Id", Reward_id),
                    new MySqlParameter("p_Type", Type)
                };
                DataTable dt = await db.ExecuteAdapterAsync(0, RRconnection, "USP_GET_SEND_REWARD_LINK_MAIL", Qry1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Mapping_Id = dt.Rows[i]["User_Id"].ToString();
                    Survey_Name = dt.Rows[i]["Reward_Name"].ToString();
                    Employee_Name = dt.Rows[i]["Employee_Name"].ToString();
                    Mail_Id = dt.Rows[i]["Email_Id"].ToString();
                    URL_Link = dt.Rows[i]["URL_Link"].ToString();
                    Survey_EndDate = dt.Rows[i]["Reward_StartDate"].ToString();
                    Reminder = int.Parse(dt.Rows[i]["Gentle_Remiders"].ToString());
                    Reward_Month = dt.Rows[i]["Reward_Month"].ToString();
                    Completion_Percentage = int.Parse(dt.Rows[i]["_CompletedPercentage"].ToString());
                    Survey_Year = int.Parse(dt.Rows[i]["Survey_Year"].ToString());
                    isCompleted_Survey = int.Parse(dt.Rows[i]["isCompleted"].ToString());
                    Main_Subject = dt.Rows[i]["Mail_Subject"].ToString();
                    Message = "<div style='font-size:11pt;'>";
                    Message += "Dear " + Employee_Name + ", <br>";
                 
                    Message += dt.Rows[i]["Reminder_Mail_Format"].ToString();

                    if (isCompleted_Survey == 0)
                    {
                        Message += "<h4><b>Please click here to take the Assessment  :&nbsp;</b>" + URL_Link + "</h4>";
                    }
                    Message += "<div> <br><br>";
                    Message += "<hr><p style='text-align:center;font-size:x-small;'>Auto generated mail from Technology Tool @ <b><a href='https://www.deshpandefoundation.org/' target='_blank'>Deshpande Foundation</a></b>  <hr></p>";
                    Message += "</div>";
                    //  Message += "<center></br> <img src='https://www.deshpandefoundationindia.org/images/dflogo.png' style='width:200px;height:70px;' /></center>";
                    Message += "</div>";
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(Message.ToString(), null, MediaTypeNames.Text.Html);

                    // Add the alternate views instead of using MailMessage.Body
                    MailMessage m = new MailMessage();
                    m.AlternateViews.Add(avHtml);

                    // Address and send the message
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    m.From = new MailAddress("recognition@dfmail.org", Reward_Month + " Recognition");
                    m.To.Add(Mail_Id);
                    m.Subject = Main_Subject;
                    m.IsBodyHtml = true;
                    m.Body = Message.ToString();

                    SmtpServer.Port = 587;
                    SmtpServer.UseDefaultCredentials = true;

                    SmtpServer.Credentials = new System.Net.NetworkCredential("recognition@dfmail.org", "dfTech@1234");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(m);

                    MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_User_Id",Mapping_Id)
                };
                    int Response_Id = await db.ExecuteAsync(RRconnection, "USP_WEB_UPDATE_GENTLE_REMINDER", queryParams);


                    Console.WriteLine("Invitation Sent to :" + Mail_Id);
                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task RR_Send_Consoliated_Report(int survey_id, string PreviousDay)
        {

            string URL_Link = "";
            string Mapping_Id = "";
            string Mail_Id = "";
            string Employee_Name = "";
            string Survey_Name = "";
            string Message = "";
            int Reminder = 0;
            int isCompleted_Survey = 0;
            int Completion_Percentage = 0;
            int Survey_Year = 0;
            string Survey_EndDate = "";
            try
            {
                DBLayer db = new DBLayer();
                MySqlParameter[] Qry1 = new MySqlParameter[] {
                    new MySqlParameter("p_Survey_Id", survey_id)
                  
                };
                DataTable dt = await db.ExecuteAdapterAsync(0, connection, "USP_GET_SEND_REWARD_LINK_MAIL", Qry1);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Mapping_Id = dt.Rows[i]["User_Id"].ToString();
                    Survey_Name = dt.Rows[i]["Reward_Name"].ToString();
                    Employee_Name = dt.Rows[i]["Employee_Name"].ToString();
                    Mail_Id = dt.Rows[i]["Email_Id"].ToString();
                    URL_Link = dt.Rows[i]["URL_Link"].ToString();
                    Survey_EndDate = dt.Rows[i]["Reward_StartDate"].ToString();
                    Reminder = int.Parse(dt.Rows[i]["Gentle_Remiders"].ToString());
                    Completion_Percentage = int.Parse(dt.Rows[i]["_CompletedPercentage"].ToString());
                    Survey_Year = int.Parse(dt.Rows[i]["Survey_Year"].ToString());
                    isCompleted_Survey = int.Parse(dt.Rows[i]["isCompleted"].ToString());
                    Message = "<div style='font-size:11pt;'>";
                    Message += "Dear " + Employee_Name + ", ";
                    Message += "<br><b> " + PreviousDay.ToString() + "</b> day update on the staff participation in the Assessment  " + Survey_Year + ": <b>" + Completion_Percentage + " % .</b>";
                    Message += "<br> We appreciate your participation in the survey and request the employees who have not yet taken the Assessment, to do so at the earliest.";

                    if (isCompleted_Survey == 0)
                    {
                        Message += "<h4><b>Please click here to take the Assessment  :&nbsp;</b>" + URL_Link + "</h4>";
                    }
                    Message += "<div> <br><br>";
                    Message += "<hr><p style='text-align:center;font-size:x-small;'>Auto generated mail from Technology Tool @ <a href='https://www.deshpandefoundation.org/'> <b>Deshpande Foundation</b></a>  <hr></p>";
                    Message += "</div>";
                    //  Message += "<center></br> <img src='https://www.deshpandefoundationindia.org/images/dflogo.png' style='width:200px;height:70px;' /></center>";
                    Message += "</div>";
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(Message.ToString(), null, MediaTypeNames.Text.Html);

                    // Add the alternate views instead of using MailMessage.Body
                    MailMessage m = new MailMessage();
                    m.AlternateViews.Add(avHtml);

                    // Address and send the message
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    m.From = new MailAddress("recognition@dfmail.org", "Assessment");
                    m.To.Add(Mail_Id);
                    m.Subject = "Assessment " + Survey_Name + " " + "Daily Update";
                    m.IsBodyHtml = true;
                    m.Body = Message.ToString();

                    SmtpServer.Port = 587;
                    SmtpServer.UseDefaultCredentials = true;

                    SmtpServer.Credentials = new System.Net.NetworkCredential("recognition@dfmail.org", "dfTech@1234");
                    SmtpServer.EnableSsl = true;

                    SmtpServer.Send(m);

                    MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_User_Id",Mapping_Id)
                };
                    int Response_Id = await db.ExecuteAsync(connection, "USP_WEB_UPDATE_GENTLE_REMINDER", queryParams);
                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        public static async Task<string> Encrypt(string password)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateEncryptor())
                    {
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(password);
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }
        public static async Task<string> Decrypt(string cipher)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipher);
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return UTF8Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }
        public async Task Reminder_Hospital_Bed_Data(string AlertDate, string DispalyDate)
        {

            DBLayer db = new DBLayer();
            string status = "";
            string Message = "";
            string Title = "";
            Title = "Reminder - " + DispalyDate.ToString();
            MySqlParameter[] Qry1 = new MySqlParameter[] {
                    new MySqlParameter("p_Alert_Date", AlertDate)
                };
            DataTable dt1 = await db.ExecuteAdapterAsync(0, connection, "USP_ALERT_HOSPITAL_DATA_NOT_ENTERED", Qry1);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                if (dt1.Rows[i]["DeviceId"].ToString() != "0")
                {
                    Message = "Reminder" + "\n";
                    Message += DispalyDate.ToString() + "\n";
                    Message += "Dear " + dt1.Rows[i]["Employee_Name"].ToString() + "\n";
                    Message += " You have not updated Hospital Details for - " + DispalyDate.ToString() + "\n";
                    Message += " Kindly enter details ASAP.";
                    string Response = Notification.AndroidPush(dt1.Rows[i]["DeviceId"].ToString(), Message.ToString().TrimEnd(','), "Reminder", "Empty");
                    MySqlParameter[] queryParams = new MySqlParameter[] {
                         new MySqlParameter("@p_Title",Title.ToString()),
                        new MySqlParameter("@p_Message_Type","Reminder"),
                        new MySqlParameter("@p_Receiver_Name", dt1.Rows[i]["Employee_Name"].ToString()),
                        new MySqlParameter("@p_Contact_No", "0"),
                        new MySqlParameter("@p_Alert_Type", "NOTIFICATION"),
                        new MySqlParameter("@p_Alert_Message", Regex.Replace(Message.ToString().TrimEnd(','), "'", "`").Trim()),
                        new MySqlParameter("@p_Response_Id", Response.ToString()),
                        new MySqlParameter("@p_User_Id", dt1.Rows[i]["User_Id"].ToString()),
                        };
                    int iRecords = await db.ExecuteAsync(connection, "USP_INSERT_ALERT_DETAILS", queryParams);
                    status = "success";
                }
            }
        }


        public async Task Email_Alert(string AlertDate, string DisplayDate)
        {

            DBLayer db = new DBLayer();


            MySqlParameter[] Qry1 = new MySqlParameter[] {
             new MySqlParameter("p_Date", AlertDate)
            };
            DataTable dt = await db.ExecuteAdapterAsync(0, connection, "USP_GET_ALERT_EMAIL_SUMMARY", Qry1);
            string textbody = "";

            textbody = "<h2 style='color:#666666;font-family:Century Gothic;text-align:center;'> Hospital Summary on  " + DisplayDate + " " + "</h2></br></br> <table border=" + 0 + " cellpadding=" + 5 + "px cellspacing=" + 1 + "PX width = " + 100 + "%><tr bgcolor='#4f81bd' style='color: white;text-align:center;font-size: 13px;font-family:Arial;letter-spacing:4px'><td><b>Updated Hospitals</b></td><td> <b>Not Updated Hospitals</b> </td></tr>";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                textbody += "<tr bgcolor='#c6d9f1' style='font-size: 18px;font-family:Arial'><td style='text-align:center;'>" + dt.Rows[i]["Total_Updated"].ToString() + "</td><td style='text-align:center;'> " + dt.Rows[i]["Not_Updated"].ToString() + "</td></tr>";
            }
            textbody += "</table></br></br><hr>";
            //-----------End of Summary---------

            int count = 0;
            int status = 0;
            MySqlParameter[] Qry2 = new MySqlParameter[] {
                    new MySqlParameter("p_Date", AlertDate),
                           new MySqlParameter("p_Flag", 1) // 1 indicate  Updated
                };
            DataTable dt1 = await db.ExecuteAdapterAsync(0, connection, "USP_GET_ALERT_EMAIL_HOSPITAL_DETAILS", Qry2);
            if (dt1.Rows.Count > 0)
            {
                status = 1;
                textbody += "<table><tr><td><h2 style='color:#666666;font-family:Century Gothic;text-align:center;'>Updated Hospitals Data on " + " " + dt1.Rows[0]["Last_Updated_Date"].ToString() + " " + "</h2></td><td><h2 style='color:#666666;font-family:Century Gothic;text-align:center;'>Not Updated Hospitals on " + " " + DisplayDate + " " + "</h2></td></tr></br>";

                textbody += "<tr><td style='vertical-align:top;'>";
                textbody += "<div style='height:350px;min-height:150px;overflow:auto;'>";
                textbody += "<table border=" + 0 + " cellpadding=" + 3 + "px cellspacing=" + 1 + "PX width = " + 100 + "%'><tr bgcolor='#6699cc' style='color: white;text-align:center;letter-spacing:2px;'><td>";
                textbody += "<b>slno</b></td><td> <b>Hospital Name</b> </td> <td> <b>Incharge Name</b> </td><td> <b>Mobile No</b> </td><td> <b>Gen Vacant</b> </td><td> <b>Oxy Vacant</b> </td><td> <b>ICU Vacant</b> </td><td> <b>Vent Vacant</b> </td></tr>";
              
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    count = count + 1;

                    textbody += "<tr bgcolor='#c6d9f1'><td>" + count + "</td><td> " + dt1.Rows[i]["Hospital_Name"].ToString() + "</td><td> " + dt1.Rows[i]["Employee_Name"].ToString() + "</td><td style='text-align:center'> " + dt1.Rows[i]["Employee_Phone"].ToString() + "</td>";
                    textbody += "<td style='text-align:center'> " + dt1.Rows[i]["Total_General"].ToString() + "</td><td style='text-align:center'> " + dt1.Rows[i]["Total_Oxygen"].ToString() + "</td><td style='text-align:center'> " + dt1.Rows[i]["Total_ICU"].ToString() + "</td><td style='text-align:center'> " + dt1.Rows[i]["Total_Ventilator"].ToString() + "</td></tr>";
                }
                textbody += "</table></div></td>";
            }        
         
            count = 0;


            MySqlParameter[] Qry3 = new MySqlParameter[] {
                    new MySqlParameter("p_Date", AlertDate),
                           new MySqlParameter("p_Flag",0) // 0 Indicate not Updated Hospital Data
                };
            DataTable dt2 = await db.ExecuteAdapterAsync(0, connection, "USP_GET_ALERT_EMAIL_HOSPITAL_DETAILS", Qry3);
            if (dt2.Rows.Count > 0)
            {
                status = 1;
                textbody += "<td style='vertical-align:top;'>";
                textbody += "<div style='height:350px;min-height:150px;overflow:auto;'>";
                textbody += "<table border=" + 0 + " cellpadding=" + 3 + "px cellspacing=" + 1 + "PX width = " + 100 + "%>";
                textbody += "<tr bgcolor='#cc6666' style='color: white;text-align:center;letter-spacing:2px;'><td><b>slno</b></td><td> <b>Hospital Name</b> </td> <td> <b>Incharge Name</b> </td><td> <b>Mobile No</b> </td></tr>";

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    count = count + 1;
                    textbody += "<tr bgcolor='#ffcccc'><td>" + count + "</td><td> " + dt2.Rows[i]["Hospital_Name"].ToString() + "</td><td> " + dt2.Rows[i]["Employee_Name"].ToString() + "</td><td style='text-align:center'> " + dt2.Rows[i]["Employee_Phone"].ToString() + "</td></tr>";
                }
                textbody += "</table></div></td></tr></table></div>";
            }
            if (status == 1)
            {
                 SendMailToHeadMembers("raghavendra.tech@dfmail.org", textbody, "Hospital Summary Details on " + DisplayDate.ToString());
                //SendMailToHeadMembers("sharad.tech@dfmail.org", textbody, "Hospital Summary Details on " + DisplayDate.ToString());
            }         

        }
        public static void SendMailToHeadMembers(string to, string msg, string Subject)
        {
            try
            {
                MailMessage m = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                m.From = new MailAddress("sharad.tech@dfmail.org", "");
                m.To.Add(to);
                m.To.Add(new MailAddress(to));
                //m.CC.Add(new MailAddress("skambi@dfmail.org"));
                //m.CC.Add(new MailAddress("raghavendra.tech@dfmail.org"));
                //m.CC.Add(new MailAddress("gopal.bgk@dfmail.org"));
                m.CC.Add(new MailAddress("sharad.tech@dfmail.org"));
                m.CC.Add(new MailAddress("mallikarjun.tech@dfmail.org"));
                //Attachment imgCertificate = new Attachment(FilePath);
                //m.Attachments.Add(imgCertificate);   
                m.Subject = Subject.ToString();
                m.IsBodyHtml = true;
                m.Body = msg;
                SmtpServer.Port = 587;

                SmtpServer.Credentials = new System.Net.NetworkCredential("sharad.tech@dfmail.org", "fun.9986901247");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(m);
            }
            catch (Exception ex)
            {

            }
        }

    }
}
