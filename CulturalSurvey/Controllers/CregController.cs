using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using ClosedXML.Excel;
using System.IO;
using System.Configuration;
using paytm;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Web.Services.Description;
using CulturaSurvey.ViewModel;
using DataLayer;
using MySql.Data.MySqlClient;
using Microsoft.SqlServer.Server;
using System.Numerics;
using System.EnterpriseServices.CompensatingResourceManager;

namespace CulturaSurvey.Controllers
{
    public class CregController : Controller
    {
        public abstract class ControllerBase : IController
        {

            /// <summary>Gets the dynamic view data dictionary.</summary>  
            /// <returns>The dynamic view data dictionary.</returns>  
            public dynamic ViewBag { get; }

            public void Execute(RequestContext requestContext)
            {
                throw new NotImplementedException();
            }
        }

        #region Declaration

        string connection = System.Configuration.ConfigurationManager.ConnectionStrings["MDMConnection"].ToString();
        #endregion
        // GET: CamuRegistration
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult testR()
        {
            return View();
        }
        public ActionResult Verification()
        {
            return View();
        }
        public ActionResult Listing()
        {
            return View();
        }
        public ActionResult Analytics()
        {
            return View();
        }
        public ActionResult Closed()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

      

        public ActionResult Submitted(string n)
        {
            ViewBag.Name = n.ToString();
            return View();
        }
        public ActionResult ThankYou()
        {
            return View();
        }

        [Route("camu")]
        public async Task<ActionResult> CamuRegistration(string id)
        {
            try
            {
                dfCrypto crp = new dfCrypto();
                DBLayer DL = new DBLayer();
                // Made modification of routing because of full path its not look good.

                if (id.Contains("$$"))
                {
                    id = id.Replace("$$", "/");
                }
                long Event_Id = long.Parse(crp.Decrypt(id.ToString().TrimEnd('S')));
                //long Event_Id = long.Parse(crp.Decrypt(Request.QueryString["EID"].ToString().TrimEnd('S')));
                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_EID", Event_Id.ToString())
                };
                DataTable dt = await DL.ExecuteAdapterAsync(0, connection, "USP_GET_EVENT_DETAILS", queryParams);

                if (dt.Rows.Count > 0)
                {

                    if (dt.Rows[0]["isEnabled"].ToString() == "0")
                    {
                        return Redirect("/Creg/Closed");
                    }
                    else if (dt.Rows[0]["isEnabled"].ToString() == "1")
                    {
                        MySqlParameter[] queryParams1 = new MySqlParameter[] {
                    new MySqlParameter("p_Event_Id", Event_Id),
                      };
                        int status = await DL.ExecuteAsync(connection, "USP_UPDATE_EVENT_LINK_CLICK", queryParams1);

                        FormsAuthTicket(dt.Rows[0]["event_id"].ToString(), false);
                        ViewBag.hdnEventId = dt.Rows[0]["event_id"].ToString();

                        HttpCookie eventloginifo = new HttpCookie("eventloginifo");
                        eventloginifo.Values.Add("EID", dt.Rows[0]["event_id"].ToString());
                        eventloginifo.Values.Add("IsFees", dt.Rows[0]["isCollect_Fees"].ToString());
                        eventloginifo.Values.Add("Fees", dt.Rows[0]["Registration_Fees"].ToString());
                        eventloginifo.Values.Add("isCollegeDropdown", dt.Rows[0]["isCollegeDropdown"].ToString());
                        eventloginifo.Values.Add("isCamuRegistration", dt.Rows[0]["isCamuRegistration"].ToString());
                        eventloginifo.Expires = DateTime.Now.AddHours(8);
                        Response.Cookies.Add(eventloginifo);
                        ViewBag.hdnFees = dt.Rows[0]["isCollect_Fees"].ToString();
                        ViewBag.hdnRID = "0";
                        Response.Cookies["RID"].Value = "0";
                        ViewBag.hdnFees = dt.Rows[0]["Registration_Fees"].ToString();
                        ViewBag.hdnisCollegeDropdown = dt.Rows[0]["isCollegeDropdown"].ToString();
                        ViewBag.hdnisCamuRegistration = dt.Rows[0]["isCamuRegistration"].ToString();
                        return View("Index");
                    }
                    else
                    {
                        return Redirect("/Creg/Error");
                    }
                }
                else
                {
                    return Redirect("/Creg/Error");
                }
            }
            catch (Exception ex)
            {
                return Redirect("/Creg/Error?msg=" + ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetEventDetails(int Event_ID)
        {

            try
            {
                dfCrypto crp = new dfCrypto();
                DBLayer DL = new DBLayer();
                //   long Event_Id = long.Parse(crp.Decrypt(Request.QueryString["EID"].ToString().TrimEnd('S')));

                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_EID",Event_ID)
                };
                DataTable dt = await DL.ExecuteAdapterAsync(0, connection, "USP_GET_EVENT_DETAILS", queryParams);
                vmEvents list = (from DataRow dr in dt.Rows
                                 select new vmEvents()
                                 {
                                     Event_Id = int.Parse(dr["event_id"].ToString()),
                                     Event_Name = dr["Event_Name"].ToString(),
                                     About_Event = dr["about_event"].ToString(),
                                     Terms_Conditions = dr["terms_conditions"].ToString(),
                                     Registration_Form = dr["registration_form"].ToString(),
                                     isCollect_Fees = int.Parse(dr["isCollect_Fees"].ToString()),
                                     Registration_Fees = int.Parse(dr["Registration_Fees"].ToString()),
                                     isEnabled = int.Parse(dr["isEnabled"].ToString())
                                 }).FirstOrDefault();
                if (dt.Rows.Count > 0)
                {
                    ViewBag.Event_Name = dt.Rows[0]["Event_Name"].ToString();
                    return Json(new { Status = true, Message = "Succes", list }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = false, Message = "Invalid Registration Form", list }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
        private void FormsAuthTicket(string Mapping_Id, bool persistanceFlag)
        {
            try
            {

                var User_ID = Mapping_Id;
                var userData = User_ID.ToString(CultureInfo.InvariantCulture);
                var authTicket = new FormsAuthenticationTicket(1, //version
                                                            Mapping_Id.ToString(), // user name
                                                            DateTime.Now,             //creation
                                                            DateTime.Now.AddHours(12), //Expiration
                                                            false, //Persistent
                                                            userData);

                var encTicket = FormsAuthentication.Encrypt(authTicket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

            }
            catch (Exception ex)
            {

            }
        }

        #region Google singing

        public async Task<ActionResult> GoogleLogin(string email, string name, string image)
        {
            try
            {
                DBLayer DL = new DBLayer();
                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_MailId", email.ToString())
                };
                DataTable dt = await DL.ExecuteAdapterAsync(0, connection, "USP_SKILLING_LOGIN_GOOGLE_VALIDATION", queryParams);


                if (dt.Rows.Count > 0)
                {
                    FormsAuthTicket(email, false);

                    HttpCookie glogininfo = new HttpCookie("glogininfo");
                    glogininfo.Values.Add("User_Id", dt.Rows[0]["User_Id"].ToString());
                    glogininfo.Values.Add("Username", dt.Rows[0]["username"].ToString());
                    glogininfo.Values.Add("Mail_id", dt.Rows[0]["mail_id"].ToString());
                    glogininfo.Values.Add("Mobileno", dt.Rows[0]["mobileno"].ToString());
                    glogininfo.Values.Add("isAdmin", dt.Rows[0]["isAdmin"].ToString());
                    glogininfo.Expires = DateTime.Now.AddHours(24);
                    Response.Cookies.Add(glogininfo);
                    ViewBag.hdnisAdmin = dt.Rows[0]["isAdmin"].ToString();
                    var info = "Creg";
                    return Json(info, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("false", JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public async Task<ActionResult> SSOLogin(string email)
        {
            try
            {
                DBLayer DL = new DBLayer();
                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_MailId", email.ToString())
                };
                DataTable dt = await DL.ExecuteAdapterAsync(0, connection, "USP_SKILLING_LOGIN_GOOGLE_VALIDATION", queryParams);


                if (dt.Rows.Count > 0)
                {
                    FormsAuthTicket(email, false);

                    HttpCookie glogininfo = new HttpCookie("glogininfo");
                    glogininfo.Values.Add("User_Id", dt.Rows[0]["User_Id"].ToString());
                    glogininfo.Values.Add("Username", dt.Rows[0]["username"].ToString());
                    glogininfo.Values.Add("Mail_id", dt.Rows[0]["mail_id"].ToString());
                    glogininfo.Values.Add("Mobileno", dt.Rows[0]["mobileno"].ToString());
                    glogininfo.Values.Add("isAdmin", dt.Rows[0]["isAdmin"].ToString());
                    glogininfo.Expires = DateTime.Now.AddHours(24);
                    Response.Cookies.Add(glogininfo);
                    ViewBag.hdnisAdmin = dt.Rows[0]["isAdmin"].ToString();

                    return Redirect("/creg/Verification");
                }
                else
                {
                    return Redirect("/creg/Error");

                }
            }
            catch (Exception ex)
            {
                return Redirect("/creg/Error");
            }

        }

        public ActionResult ErrorPageAction()
        {
            return Redirect("/Creg/Error");
        }

        #endregion

        [HttpPost]
        public async Task<ActionResult> Save_Registration(vmRegistration Reg)
        {
            long Event_Id = long.Parse(Request.Cookies["eventloginifo"]["EID"].ToString());
            long Registration_Id = long.Parse(Request.Cookies["RID"].Value);
            string _URL_Link = ConfigurationManager.AppSettings["Reg_Link"].ToString();

            string Response_Id = "";
            string Encrypted_Reg = "";
            dfCrypto crp = new dfCrypto();
            DBLayer DL = new DBLayer();
            if (string.IsNullOrEmpty(Reg.Reference_Name))
            {
                Reg.Reference_Name = "";
            }
            try
            {

                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_Event_Id", Event_Id),
                    new MySqlParameter("p_Registration_Id", Registration_Id),
                    new MySqlParameter("p_First_Name",Regex.Replace(Reg.First_Name.ToString(), "'", "`").Trim()),
                    new MySqlParameter("p_Last_Name", Regex.Replace(Reg.Last_Name.ToString(), "'", "`").Trim()),
                    new MySqlParameter("p_Name_On_Certificate", Regex.Replace(Reg.Name_On_Certificate.ToString(), "'", "`").Trim()),
                    new MySqlParameter("p_DOB", Reg.DOB.ToString().ToString()),
                    new MySqlParameter("p_Age", Reg.Age.ToString()),
                    new MySqlParameter("p_Contact_No", Reg.Contact_No),
                    new MySqlParameter("p_Email_Id", Reg.Email_Id),
                    new MySqlParameter("p_Gender", Reg.Gender),
                    new MySqlParameter("p_Qualification_Id", Reg.Qualification_Id),
                    new MySqlParameter("p_Year_Of_Passing", Reg.Year_Of_Passing),
                    new MySqlParameter("p_Father_Name",Regex.Replace(Reg.Middle_Name.ToString(), "'", "`").Trim()),
                    new MySqlParameter("p_Father_Contact", Reg.Whatsapp_No),
                    new MySqlParameter("p_State_Id", Reg.State_Id),
                    new MySqlParameter("p_District_id", Reg.District_id),
                    new MySqlParameter("p_Taluka_Id", Reg.Taluka_Id),
                    new MySqlParameter("p_College_Type_Id", Reg.College_Type_Id),
                    new MySqlParameter("p_College_Id",Reg.College_Id.ToString()),
                    new MySqlParameter("p_isBacklogs",Reg.BYN.ToString()),
                    new MySqlParameter("p_Backlogs_nos",Reg.BYesNumber.ToString()),
                    new MySqlParameter("p_Source_of_Information", Reg.Source),
                    new MySqlParameter("p_Institution_Id", Reg.Institution_Id.ToString()),
                    new MySqlParameter("p_Degree_Id", Reg.Degree_Id.ToString()),
                    new MySqlParameter("p_Program_Id", Reg.Program_Id.ToString()),
                    new MySqlParameter("p_Academic_Id", Reg.Academic_Id.ToString()),
                    new MySqlParameter("p_Incharge_Id", Reg.Incharge_Id.ToString()),
                    new MySqlParameter("p_Reference_Name", Reg.Reference_Name.ToString()),
                    new MySqlParameter("p_isAccept_Terms", Reg.isAccept_Terms),
                    new MySqlParameter("p_isPayment_Done", Reg.isPayment_Done),
                    new MySqlParameter("p_Application_Status", "Enquiry"),
                };
                Response_Id = await DL.ExecuteAsyncWithOutPara(connection, "USP_SAVE_CAMU_ONLINE_REGISTRATION", queryParams);
                Response.Cookies["RID"].Value = Response_Id.ToString();
                ViewBag.hdnRID = Response_Id;
                string userencrypt = "";
                userencrypt = crp.Encrypt("0000X" + Response_Id.ToString()) + "S";
                if (Reg.Registration_Id == 0)
                {
                    Encrypted_Reg = _URL_Link + crp.Encrypt("0000X" + Response_Id.ToString()) + "S";
                    string status = Save_Action_Alert(Reg.First_Name.ToString(), Reg.Contact_No, "SMS", "SMS Not Sent", "000", "Student Registration Page", "Info", Response_Id.ToString(), Encrypted_Reg);
                    #region Jobdrive_text
                    //string SMS_Message = "";
                    //SMS_Message =   "Dear " + Reg.First_Name + "," + "\n";
                    //SMS_Message += "Refer Job Drive link : "+ Encrypted_Reg + " to completer further payment process - Deshpande Foundation " + "\n";
                    //SendAction_SMS(Reg.First_Name, Reg.Contact_No, SMS_Message.ToString(), "Job Drive Registration No: " + Response_Id, "Info",Response_Id.ToString(), Encrypted_Reg);

                    //    await SendMail_PaymentLink(Reg.First_Name,Encrypted_Reg,Reg.Email_Id);

                    #endregion
                    #region Registration
                    string SMS_Message = "";
                    SMS_Message = "Dear " + Reg.First_Name + "," + "\n";
                    SMS_Message += "Refer link : " + Encrypted_Reg + " to completer further references - Deshpande Foundation " + "\n";
                    SendAction_SMS(Reg.First_Name, Reg.Contact_No, SMS_Message.ToString(), "Registration No: " + Response_Id, "Info", Response_Id.ToString(), Encrypted_Reg);

                    await SendMail_PaymentLink(Reg.First_Name, Encrypted_Reg, Reg.Email_Id, "Enquiry");
                    #endregion
                }
                //  return View(Encrypted_Reg);
                return Json(new { Status = true, Message = userencrypt.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public async Task<ActionResult> Save_Registration_Draft(vmRegistration Reg)
        {
            long Event_Id = long.Parse(Request.Cookies["eventloginifo"]["EID"].ToString());
            long Registration_Id = long.Parse(Request.Cookies["RID"].Value);
            string _URL_Link = ConfigurationManager.AppSettings["Reg_Link"].ToString();

            string Response_Id = "";
            string Encrypted_Reg = "";
            dfCrypto crp = new dfCrypto();
            DBLayer DL = new DBLayer();
            if (string.IsNullOrEmpty(Reg.Reference_Name))
            {
                Reg.Reference_Name = "";
            }
            if (string.IsNullOrEmpty(Reg.Gender))
            {
                Reg.Gender = "";
            }
            if (string.IsNullOrEmpty(Reg.Qualification_Id))
            {
                Reg.Qualification_Id = "";
            }
            if (string.IsNullOrEmpty(Reg.Year_Of_Passing))
            {
                Reg.Year_Of_Passing = "";
            }
            if (string.IsNullOrEmpty(Reg.Middle_Name))
            {
                Reg.Middle_Name = "";
            }
            if (string.IsNullOrEmpty(Reg.Whatsapp_No))
            {
                Reg.Whatsapp_No = "";
            }
            if (string.IsNullOrEmpty(Reg.State_Id))
            {
                Reg.State_Id = "";
            }
            if (string.IsNullOrEmpty(Reg.District_id))
            {
                Reg.District_id = "";
            }
            if (string.IsNullOrEmpty(Reg.Taluka_Id))
            {
                Reg.Taluka_Id = "";
            }
            if (string.IsNullOrEmpty(Reg.College_Type_Id))
            {
                Reg.College_Type_Id = "";
            }
            if (string.IsNullOrEmpty(Reg.College_Id))
            {
                Reg.College_Id = "";
            }
            if (string.IsNullOrEmpty(Reg.Source))
            {
                Reg.Source = "";
            }
            if (string.IsNullOrEmpty(Reg.Institution_Id))
            {
                Reg.Institution_Id = "";
            }
            if (string.IsNullOrEmpty(Reg.Program_Id))
            {
                Reg.Program_Id = "";
            }
            if (string.IsNullOrEmpty(Reg.Degree_Id))
            {
                Reg.Degree_Id = "";
            }
            if (string.IsNullOrEmpty(Reg.Academic_Id))
            {
                Reg.Academic_Id = "";
            }
            if (string.IsNullOrEmpty(Reg.Incharge_Id))
            {
                Reg.Incharge_Id = "";
            }

            try
            {
                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_Event_Id", Event_Id),
                    new MySqlParameter("p_Registration_Id", Registration_Id),
                    new MySqlParameter("p_First_Name",Regex.Replace(Reg.First_Name.ToString(), "'", "`").Trim()),
                    new MySqlParameter("p_Last_Name", Regex.Replace(Reg.Last_Name.ToString(), "'", "`").Trim()),
                    new MySqlParameter("p_Name_On_Certificate", Regex.Replace(Reg.Name_On_Certificate.ToString(), "'", "`").Trim()),
                    new MySqlParameter("p_DOB", Reg.DOB.ToString().ToString()),
                    new MySqlParameter("p_Age", Reg.Age.ToString()),
                    new MySqlParameter("p_Contact_No", Reg.Contact_No),
                    new MySqlParameter("p_Email_Id", Reg.Email_Id),
                    new MySqlParameter("p_Gender", Reg.Gender),
                    new MySqlParameter("p_Qualification_Id", Reg.Qualification_Id),
                    new MySqlParameter("p_Year_Of_Passing", Reg.Year_Of_Passing),
                    new MySqlParameter("p_Father_Name",Regex.Replace(Reg.Middle_Name.ToString(), "'", "`").Trim()),
                    new MySqlParameter("p_Father_Contact", Reg.Whatsapp_No),
                    new MySqlParameter("p_State_Id", Reg.State_Id),
                    new MySqlParameter("p_District_id", Reg.District_id),
                    new MySqlParameter("p_Taluka_Id", Reg.Taluka_Id),
                    new MySqlParameter("p_College_Type_Id", Reg.College_Type_Id),
                    new MySqlParameter("p_College_Id",Reg.College_Id.ToString()),
                    new MySqlParameter("p_isBacklogs",Reg.BYN.ToString()),
                    new MySqlParameter("p_Backlogs_nos",Reg.BYesNumber.ToString()),
                    new MySqlParameter("p_Source_of_Information", Reg.Source),
                    new MySqlParameter("p_Institution_Id", Reg.Institution_Id.ToString()),
                    new MySqlParameter("p_Degree_Id", Reg.Degree_Id.ToString()),
                    new MySqlParameter("p_Program_Id", Reg.Program_Id.ToString()),
                    new MySqlParameter("p_Academic_Id", Reg.Academic_Id.ToString()),
                    new MySqlParameter("p_Incharge_Id", Reg.Incharge_Id.ToString()),
                    new MySqlParameter("p_Reference_Name", Reg.Reference_Name.ToString()),
                    new MySqlParameter("p_isAccept_Terms", Reg.isAccept_Terms),
                    new MySqlParameter("p_isPayment_Done", Reg.isPayment_Done),
                    new MySqlParameter("p_Application_Status","Draft"),
                };
                Response_Id = await DL.ExecuteAsyncWithOutPara(connection, "USP_SAVE_CAMU_ONLINE_REGISTRATION", queryParams);
                Response.Cookies["RID"].Value = Response_Id.ToString();
                ViewBag.hdnRID = Response_Id;
                string userencrypt = "";
                userencrypt = crp.Encrypt("0000X" + Response_Id.ToString()) + "S";
                if (Reg.Registration_Id == 0)
                {
                    Encrypted_Reg = _URL_Link + crp.Encrypt("0000X" + Response_Id.ToString()) + "S";
                    string status = Save_Action_Alert(Reg.First_Name.ToString(), Reg.Contact_No, "SMS", "SMS Not Sent", "000", "Student Registration Page", "Info", Response_Id.ToString(), Encrypted_Reg);
                    #region Jobdrive_text
                    //string SMS_Message = "";
                    //SMS_Message =   "Dear " + Reg.First_Name + "," + "\n";
                    //SMS_Message += "Refer Job Drive link : "+ Encrypted_Reg + " to completer further payment process - Deshpande Foundation " + "\n";
                    //SendAction_SMS(Reg.First_Name, Reg.Contact_No, SMS_Message.ToString(), "Job Drive Registration No: " + Response_Id, "Info",Response_Id.ToString(), Encrypted_Reg);

                    //    await SendMail_PaymentLink(Reg.First_Name,Encrypted_Reg,Reg.Email_Id);

                    #endregion
                    #region Registration
                    string SMS_Message = "";
                    SMS_Message = "Dear " + Reg.First_Name + "," + "\n";
                    SMS_Message += "Refer link : " + Encrypted_Reg + " to completer further details - Deshpande Foundation " + "\n";
                    SendAction_SMS(Reg.First_Name, Reg.Contact_No, SMS_Message.ToString(), "Registration No: " + Response_Id, "Info", Response_Id.ToString(), Encrypted_Reg);

                    await SendMail_PaymentLink(Reg.First_Name, Encrypted_Reg, Reg.Email_Id, "Draft");
                    #endregion
                }
                //  return View(Encrypted_Reg);
                return Json(new { Status = true, Message = userencrypt.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public async Task<ActionResult> Save_Registration_ClusterHead(vmRegistration Reg)
        {
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            long Event_Id = Reg.Event_Id;
            long Registration_Id = Reg.Registration_Id;


            string Response_Id = "";

            dfCrypto crp = new dfCrypto();
            DBLayer DL = new DBLayer();
            if (string.IsNullOrEmpty(Reg.Reference_Name))
            {
                Reg.Reference_Name = "";
            }
            try
            {
                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_Event_Id", Event_Id),
                    new MySqlParameter("p_Registration_Id", Registration_Id),
                    new MySqlParameter("p_First_Name",Regex.Replace(Reg.First_Name.ToString(), "'", "`").Trim()),
                    new MySqlParameter("p_Last_Name", Regex.Replace(Reg.Last_Name.ToString(), "'", "`").Trim()),
                    new MySqlParameter("p_Name_On_Certificate", Regex.Replace(Reg.Name_On_Certificate.ToString(), "'", "`").Trim()),
                    new MySqlParameter("p_DOB", Reg.DOB.ToString().ToString()),
                    new MySqlParameter("p_Age", Reg.Age.ToString()),
                    new MySqlParameter("p_Contact_No", Reg.Contact_No),
                    new MySqlParameter("p_Email_Id", Reg.Email_Id),
                     new MySqlParameter("p_Gender", Reg.Gender),
                      new MySqlParameter("p_Qualification_Id", Reg.Qualification_Id),
                      new MySqlParameter("p_Year_Of_Passing", Reg.Year_Of_Passing),
                     new MySqlParameter("p_Father_Name",Regex.Replace(Reg.Middle_Name.ToString(), "'", "`").Trim()),
                    new MySqlParameter("p_Father_Contact", Reg.Whatsapp_No),
                    new MySqlParameter("p_State_Id", Reg.State_Id),
                    new MySqlParameter("p_District_id", Reg.District_id),
                    new MySqlParameter("p_Taluka_Id", Reg.Taluka_Id),
                    new MySqlParameter("p_College_Type_Id", Reg.College_Type_Id),
                    new MySqlParameter("p_College_Id",Reg.College_Id.ToString()),
                    new MySqlParameter("p_isBacklogs",Reg.BYN.ToString()),
                    new MySqlParameter("p_Backlogs_nos",Reg.BYesNumber.ToString()),
                    new MySqlParameter("p_Source_of_Information", Reg.Source),
                    new MySqlParameter("p_Institution_Id", Reg.Institution_Id.ToString()),
                    new MySqlParameter("p_Degree_Id", Reg.Degree_Id.ToString()),
                    new MySqlParameter("p_Program_Id", Reg.Program_Id.ToString()),
                    new MySqlParameter("p_Academic_Id", Reg.Academic_Id.ToString()),
                    new MySqlParameter("p_Incharge_Id", Reg.Incharge_Id.ToString()),
                    new MySqlParameter("p_Reference_Name", Reg.Reference_Name.ToString()),
                    new MySqlParameter("p_User_Id", User_Id),
                };
                Response_Id = await DL.ExecuteAsyncWithOutPara(connection, "USP_SAVE_CAMU_ONLINE_REGISTRATION_CLUSTERHEAD", queryParams);
                Response.Cookies["RID"].Value = Response_Id.ToString();
                ViewBag.hdnRID = Response_Id;
                //  return View(Encrypted_Reg);
                return Json(new { Status = true, Message = "Verified Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }

        [Route("creg")]
        public async Task<ActionResult> Registration_Edit()
        {
            try
            {
                dfCrypto crp = new dfCrypto();
                DBLayer DL = new DBLayer();
                long Registration_Id = long.Parse(crp.Decrypt(Request.QueryString["RID"].ToString().TrimEnd('S')));
                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_RID", Registration_Id.ToString())
                };
                DataTable dt = await DL.ExecuteAdapterAsync(0, connection, "USP_GET_REGISTRATION_DETAILS", queryParams);

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["isEnabled"].ToString() == "0")
                    {
                        return Redirect("/Creg/Closed");
                    }
                    else if (dt.Rows[0]["isEnabled"].ToString() == "1")
                    {
                        if (dt.Rows[0]["isRegistration_Completed"].ToString() == "0") //0 means not completed
                        {
                            FormsAuthTicket(dt.Rows[0]["Registration_Id"].ToString(), false);
                            ViewBag.hdnEventId = dt.Rows[0]["Event_Id"].ToString();
                            HttpCookie eventloginifo = new HttpCookie("eventloginifo");
                            eventloginifo.Values.Add("EID", dt.Rows[0]["Event_Id"].ToString());
                            eventloginifo.Values.Add("IsFees", dt.Rows[0]["isCollect_Fees"].ToString());
                            eventloginifo.Values.Add("Fees", dt.Rows[0]["Registration_Fees"].ToString());
                            eventloginifo.Expires = DateTime.Now.AddHours(8);
                            Response.Cookies.Add(eventloginifo);
                            Response.Cookies["RID"].Value = Registration_Id.ToString();
                            ViewBag.hdnRID = Registration_Id.ToString();
                            ViewBag.hdnFees = dt.Rows[0]["Registration_Fees"].ToString();
                            return View("Index");
                        }
                        else if (dt.Rows[0]["isRegistration_Completed"].ToString() == "1")
                        {
                            // ViewBag.Name = dt.Rows[0]["Registered_Name"].ToString();
                            return Redirect("/Creg/Submitted?n=" + dt.Rows[0]["Registered_Name"].ToString());
                        }
                        else
                        {
                            return Redirect("/Creg/Error");
                        }

                    }
                    else
                    {
                        return Redirect("/Creg/Error");
                    }
                }
                else
                {
                    return Redirect("/Creg/Error");
                }
            }
            catch (Exception ex)
            {
                return Redirect("/Creg/Error?msg=" + ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetMobile_Validation(string Mobile_No, string First_Name)
        {

            try
            {

                DBLayer DL = new DBLayer();
                long Event_Id = long.Parse(Request.Cookies["eventloginifo"]["EID"].ToString());
                //   long Event_Id = long.Parse(crp.Decrypt(Request.QueryString["EID"].ToString().TrimEnd('S')));

                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_EID",Event_Id),
                     new MySqlParameter("p_Contact_No",Mobile_No),
                     new MySqlParameter("p_First_Name",Regex.Replace(First_Name.ToString(), "'", "`").Trim())
                };
                DataTable dt = await DL.ExecuteAdapterAsync(0, connection, "USP_VALIDATE_EVENT_MOBILENO", queryParams);

                if (dt.Rows.Count > 0)
                {

                    return Json(new { Status = true, Message = "Exists" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = false, Message = "NotExists" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetMoreRegistrationDetails(long Registration_Id)
        {

            try
            {
                dfCrypto crp = new dfCrypto();
                DBLayer DL = new DBLayer();
                //   long Event_Id = long.Parse(crp.Decrypt(Request.QueryString["EID"].ToString().TrimEnd('S')));

                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_Registration_Id",Registration_Id)
                };
                DataTable dt = await DL.ExecuteAdapterAsync(0, connection, "USP_GET_CAMU_ONLINE_REGISTRATION_DETAILS", queryParams);
                vmRegistration list = (from DataRow dr in dt.Rows
                                       select new vmRegistration()
                                       {
                                           First_Name = dr["First_Name"].ToString(),
                                           Last_Name = dr["Last_Name"].ToString(),
                                           Name_On_Certificate = dr["Name_On_Certificate"].ToString(),
                                           DOB = dr["DOB"].ToString(),
                                           Age = dr["Age"].ToString(),
                                           Contact_No = dr["contact_No"].ToString(),
                                           Email_Id = dr["Email_Id"].ToString(),
                                           Gender = dr["Gender"].ToString(),
                                           Qualification_Id = dr["Qualification_Id"].ToString(),
                                           Year_Of_Passing = dr["Year_Of_Passing"].ToString(),
                                           Middle_Name = dr["Father_Name"].ToString(),
                                           Whatsapp_No = dr["Father_Contact"].ToString(),
                                           State_Id = dr["state_Id"].ToString(),
                                           District_id = dr["district_Id"].ToString(),
                                           Taluka_Id = dr["Taluka_Id"].ToString(),
                                           College_Type_Id = dr["College_Type_Id"].ToString(),
                                           College_Id = dr["College_Id"].ToString(),
                                           BYN = dr["isBacklogs"].ToString(),
                                           BYesNumber = dr["Backlogs_nos"].ToString(),
                                           Source = dr["Source_of_Information"].ToString(),
                                           Institution_Id = dr["Institution_Id"].ToString(),
                                           Program_Id = dr["Program_Id"].ToString(),
                                           Degree_Id = dr["Degree_Id"].ToString(),
                                           Academic_Id = dr["Academic_Id"].ToString(),
                                           Incharge_Id = dr["Incharge_Id"].ToString(),
                                           Reference_Name = dr["Reference_Name"].ToString(),
                                           isAccept_Terms = int.Parse(dr["isAccept_Terms"].ToString()),
                                           isPayment_Done = int.Parse(dr["isPayment_Done"].ToString()),
                                           isRegistration_Completed = int.Parse(dr["isRegistration_Completed"].ToString()),
                                           Application_Status = dr["Camu_Application_Status"].ToString(),
                                       }).FirstOrDefault();
                if (dt.Rows.Count > 0)
                {
                    return Json(new { Status = true, Message = "success", list }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = false, Message = "Invalid Registration Form", list }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> MakePaymentRequest(vmRegistration model)
        {
            try
            {
                long Fees = long.Parse(Request.Cookies["eventloginifo"]["Fees"].ToString());
                long Registration_Id = long.Parse(Request.Cookies["RID"].Value);
                string _CallBack = ConfigurationManager.AppSettings["Reg_CallBack"].ToString();
                string Order_Id = Guid.NewGuid().ToString();
                var callbackUrl = _CallBack.ToString() + Registration_Id;
                /* initialize a TreeMap object */
                Dictionary<string, string> paytmParams = new Dictionary<string, string>
                {
                    // Merchant Id
                    { PaytmParamKeys.MId, ConfigurationKeys.MerchantId},

                    // Website
                    { PaytmParamKeys.WEBSITE, ConfigurationKeys.Website },

                    // Industry Type Id
                    { PaytmParamKeys.INDUSTRY_TYPE_ID, ConfigurationKeys.IndustryType },

                    // Chanel Id
                    { PaytmParamKeys.CHANNEL_ID, ConfigurationKeys.ChannelId },

                    // Order id generated from website
                    { PaytmParamKeys.ORDER_ID, Order_Id.ToString() },

                    //{ PaytmParamKeys.ORDER_ID,Registration_Id.ToString() },

                    // Customer Id generated from Website
                    //{ PaytmParamKeys.CUST_ID, Registration_Id.ToString() },

                      //{ PaytmParamKeys.CUST_ID, Guid.NewGuid().ToString() },
                    { PaytmParamKeys.CUST_ID, Registration_Id.ToString() },
                    // From model
                    { PaytmParamKeys.MOBILE_NO, model.Contact_No.Trim() },

                    // From model
                    { PaytmParamKeys.EMAIL, model.Email_Id.Trim() },

                    // From model
                    { PaytmParamKeys.TXN_AMOUNT, Fees.ToString() },

                    // Response will be sent to this url
                    { PaytmParamKeys.CALLBACK_URL, callbackUrl}
                };

                DBLayer db = new DBLayer();
                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_Registration_Id", Registration_Id.ToString()),
                    new MySqlParameter("p_Order_Id",Order_Id.ToString()),
                };
                int status = await db.ExecuteAsync(connection, "USP_ONLINE_REGISTRATION_UPDATE_ORDER_ID", queryParams);


                string checksum = CheckSum.generateCheckSum(ConfigurationKeys.MerchantKey, paytmParams);//merchant Key
                /* Prepare HTML Form and Submit to Paytm */
                string outputHtml = "";
                outputHtml += "<html>";
                outputHtml += "<head>";
                outputHtml += "<title>Merchant Checkout Page</title>";
                outputHtml += "</head>";
                outputHtml += "<body>";
                outputHtml += "<center><h1>Please do not refresh this page...</h1></center>";
                outputHtml += "<form method='post' action='" + PaytmUrl.ProductionUrl + "' name='paytm_form'>";
                foreach (string key in paytmParams.Keys)
                {
                    outputHtml += "<input type='hidden' name='" + key + "' value='" + paytmParams[key] + "'>";
                }
                outputHtml += "<input type='hidden' name='CHECKSUMHASH' value='" + checksum + "'>";
                outputHtml += "</form>";
                outputHtml += "<script type='text/javascript'>";
                outputHtml += "document.paytm_form.submit();";
                outputHtml += "</script>";
                outputHtml += "</body>";
                outputHtml += "</html>";

                ViewBag.HtmlData = outputHtml;
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public async Task<ActionResult> Get_Registration_Details()
        {
            try
            {
                dfCrypto crp = new dfCrypto();

                long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());

                //string Event_Id = crp.Decrypt(id.ToString().TrimEnd('S'));
                string Event_Id = "1";
                DBLayer DL = new DBLayer();
                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_User_Id",User_Id),
                    new MySqlParameter("p_Event_Id",Event_Id)
                };
                DataTable dt = await DL.ExecuteAdapterAsync(0, connection, "USP_GET_CAMU_ONLINE_REGISTRATION_REPORTS", queryParams);
                return View(dt);

            }
            catch (Exception ex)
            {
                return Redirect("/Creg/Error");
            }
        }


        public async Task<ActionResult> GetPaytmResponse(PaymentResponseViewModel model)
        {
            try
            {
                long Registration_Id = long.Parse(Request.QueryString["rid"].ToString());
                //   long Registration_Id = long.Parse(Request.Cookies["RID"].Value);
                string paytmChecksum = "";
                Dictionary<string, string> paytmParams = new Dictionary<string, string>();
                foreach (string key in Request.Form.Keys)
                {
                    if (key.Equals("CHECKSUMHASH"))
                    {
                        paytmChecksum = Request.Form[key];
                    }
                    else
                    {
                        paytmParams.Add(key.Trim(), Request.Form[key].Trim());
                    }
                }

                bool isValidChecksum = CheckSum.verifyCheckSum(ConfigurationKeys.MerchantKey, paytmParams, paytmChecksum);
                if (isValidChecksum)
                {
                    string Custom_Status = "";
                    if (model.RESPCODE == "01")
                    {
                        Custom_Status = "success";
                    }
                    else
                    {
                        ViewBag.Message = "Payment Failed";
                        ViewBag.ErrorDetail = model.RESPMSG;
                        Custom_Status = "Failed";
                    }
                    try
                    {
                        DBLayer DL = new DBLayer();
                        MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_Registration_Id", Registration_Id),
                    new MySqlParameter("p_MID", model.MID),
                    new MySqlParameter("p_Order_Id",model.ORDERID),
                    new MySqlParameter("p_Transaction_Id", model.TXNID),
                    new MySqlParameter("p_Transaction_Amount",model.TXNAMOUNT),
                    new MySqlParameter("p_Bank_Transaction_Id", model.BANKTXNID),
                    new MySqlParameter("p_Status", model.STATUS),
                    new MySqlParameter("p_Response_Code", model.RESPCODE),
                    new MySqlParameter("p_Response_Message",model.RESPMSG),
                    new MySqlParameter("p_Transaction_Date",model.TXNDATE),
                    new MySqlParameter("p_Gateway_Name", model.GATEWAYNAME),
                    new MySqlParameter("p_Bank_Name", model.BANKNAME),
                    new MySqlParameter("p_Payment_Mode", model.PAYMENTMODE),
                    new MySqlParameter("p_Check_Sum_Mash", model.CHECKSUMHASH),
                    new MySqlParameter("p_PaymentForDepartment", "Skilling Registration"),
                    new MySqlParameter("p_Custom_Status", Custom_Status.ToString()),
                };
                        int status = await DL.ExecuteAsync(connection, "USP_SAVE_PAYMENT_DETAILS", queryParams);
                        if (model.RESPCODE == "01")
                        {
                            if (status > 0)
                            {
                                ViewBag.Message = "Registration and Payment Successfull";

                            }
                            else
                            {
                                ViewBag.Message = "Payment Successfull but Registration Pending Contact 9738433247";
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ViewBag.Message = "Registration Failed :" + ex.Message.ToString();
                        //  return Json(new { Status = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    ViewBag.Message = "Checksum Mismatched";
                }
            }
            catch (Exception ex)
            {
            }

            return View(model);
        }


        public async Task<ActionResult> Save_Feedback(string Rating, string Comment)
        {

            try
            {
                if (Rating == "")
                {
                    Rating = "5";
                }

                DBLayer DL = new DBLayer();
                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_Feedback_Type", "Skilling Registration"),
                    new MySqlParameter("p_Rating", Rating.ToString()),
                    new MySqlParameter("p_Original_Comment", Regex.Replace(Comment, "'", "`")),
                    new MySqlParameter("p_Comment",  Regex.Replace(Comment, "'", "`")),
                    new MySqlParameter("p_Language_Code", "en"),

                };
                int status = await DL.ExecuteAsync(connection, "USP_WEB_SAVE_FEEDBACK", queryParams);

                return Json(new { Status = true, Message = "Succes" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task SendMail_PaymentLink(string Name, string URL_Link, string Mail_ID, string App_Type)
        {
            string Message = "";
            Message = "<div style='font-size:11pt;'>";
            Message += "Dear " + Name + ",<br>";
            if (App_Type == "Draft")
            {
                Message += "<p>Application is in draft</p>";
                Message += "<p><h4>Proceed to complete your registration by using the link :&nbsp; " + URL_Link + "</h4></p>";
            }
            else
            {

                Message += "<p><h4>Make the payment to complete the registration  :&nbsp; " + URL_Link + "</h4></p>";
            }


            Message += "<p>Thank you </p>";
            Message += "</div>";
            AlternateView avHtml = AlternateView.CreateAlternateViewFromString(Message.ToString(), null, MediaTypeNames.Text.Html);

            // Add the alternate views instead of using MailMessage.Body
            MailMessage m = new MailMessage();
            m.AlternateViews.Add(avHtml);

            // Address and send the message
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            m.From = new MailAddress("dftech@dfmail.org", "Deshpande Skilling");


            m.To.Add(Mail_ID);
            if (App_Type == "Draft")
            {
                m.Subject = "Skilling Registration Draft";
            }
            else
            {
                m.Subject = "Make payment to complete the registration";
            }

            m.IsBodyHtml = true;
            m.Body = Message.ToString();
            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = true;
            SmtpServer.Credentials = new System.Net.NetworkCredential("dftech@dfmail.org", "Deshpande@#123");
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(m);
        }

        public async Task SendMail(string Email_ID, string Name)
        {
            using (MailMessage mm = new MailMessage("dftech@dfmail.org", Email_ID.ToString()))
            {
                string body = PopulateBody(Name.ToString());


                mm.Subject = "Skilling Registration Details";
                mm.Body = body.ToString();
                mm.IsBodyHtml = true;
                mm.Bcc.Add(new MailAddress("sharad.tech@dfmail.org"));
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new NetworkCredential
                    {
                        UserName = "dftech@dfmail.org",
                        Password = "Deshpande@#123"
                    };

                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mm);

                }
            }
        }
        private string PopulateBody(string userName)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("/Views/Event/JobDrive_EmailTemplate.htm")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userName);

            return body;
        }
        public void SendAction_SMS(string Name, string Mobile_No, string Message, string Title, string Message_Type, string User_ID, string URL_Link)
        {
            try
            {
                string SMS_Id = "";
                string status = "";
                SendSMS s = new SendSMS("dfmail", "dfmail", "ddac450064312c03a44ff94d301cf7eabdbd62bd");
                string response = s.execute("08047091456", Mobile_No, Message.ToString());
                var matches = Regex.Matches(response, @"(?<=<Sid>)(.+?)(?=</)");
                foreach (Match m in matches)
                {
                    SMS_Id = m.Groups[1].ToString();
                }
                if (SMS_Id != "")
                {
                    status = Save_Action_Alert(Name, Mobile_No, "SMS", Message, SMS_Id, Title, Message_Type, User_ID.ToString(), URL_Link);
                }
                else
                {
                    status = "failed to send sms";
                }
            }
            catch (Exception)
            {


            }
        }

        public string Save_Action_Alert(string Name, string ContactNo, string Alert_Type, string Alert_Message, string Response_Id, string Title, string Message_Type, string User_Id, string URL_Link)
        {
            //@Sharad

            string status = "";
            try
            {
                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_INSERT_ALERT_DETAILS";
                    cmd.Parameters.AddWithValue("@p_Title", Title.ToString());
                    cmd.Parameters.AddWithValue("@p_Message_Type", Message_Type.ToString());
                    cmd.Parameters.AddWithValue("p_Receiver_Name", Regex.Replace(Name.ToString(), "'", "`").Trim());
                    cmd.Parameters.AddWithValue("p_Contact_No", ContactNo);
                    cmd.Parameters.AddWithValue("p_Alert_Type", Alert_Type);
                    cmd.Parameters.AddWithValue("p_Alert_Message", Regex.Replace(Alert_Message.ToString(), "'", "`").Trim());
                    cmd.Parameters.AddWithValue("p_URL_Link", URL_Link);
                    cmd.Parameters.AddWithValue("p_Response_Id", Response_Id);
                    cmd.Parameters.AddWithValue("p_User_Id", User_Id);
                    cmd.Connection = con;

                    int i = cmd.ExecuteNonQuery();
                    if (i > 0)
                    {
                        status = "success";
                    }
                    else
                    {
                        status = "Unable to Save Request";
                    }
                }
            }
            catch (Exception ex)
            {
                status = "error";
            }
            return status;
            //@Sharad
        }

        public ActionResult Load_Qualification(long Event_Id)
        {
            //@Sharad
            List<vmQualification> lst = new List<vmQualification>();

            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_MASTER_QUALIFICATION";
                    cmd.Parameters.AddWithValue("p_Event_Id", Event_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmQualification
                            {
                                Qualification_Id = dt.Rows[i]["Qualification_Id"].ToString(),
                                Qualification_Name = dt.Rows[i]["Qualification_Name"].ToString(),

                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmQualification { status = "There are no Qualification List" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmQualification { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetCamuStates()
        {
            //@Sharad
            List<vmState> lst = new List<vmState>();

            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_MASTER_CAMU_STATE";
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmState
                            {
                                State_Id = dt.Rows[i]["Id"].ToString(),
                                State_Name = dt.Rows[i]["State_Name"].ToString(),
                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmState { status = "There are no State List" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmState { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCamuDistrict(string State_Id)
        {
            //@Sharad
            List<vmDistrict> lst = new List<vmDistrict>();

            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_MASTER_CAMU_DISTRICT";
                    cmd.Parameters.AddWithValue("p_State_Id", State_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmDistrict
                            {
                                District_Id = dt.Rows[i]["Id"].ToString(),
                                District_Name = dt.Rows[i]["District_Name"].ToString(),
                            });
                        }

                    }
                    else
                    {
                        lst.Add(new vmDistrict { status = "There are no District List" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmDistrict { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }
        public ActionResult GetCamuTaluka(string District_Id)
        {
            //@Sharad
            List<vmTaluka> lst = new List<vmTaluka>();

            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_MASTER_CAMU_TALUKA";
                    cmd.Parameters.AddWithValue("p_District_Id", District_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmTaluka
                            {
                                Taluka_Id = dt.Rows[i]["Taluka_ID"].ToString(),
                                Taluka_Name = dt.Rows[i]["Taluka_Name"].ToString(),
                            });
                        }

                    }
                    else
                    {
                        lst.Add(new vmTaluka { status = "There are no Taluka List" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmTaluka { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCamuCollegeType()
        {
            //@Sharad
            List<vmCourseType> lst = new List<vmCourseType>();

            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_MASTER_CAMU_COLLEGE_TYPE";
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmCourseType
                            {
                                CourseType_Id = dt.Rows[i]["CT_Id"].ToString(),
                                CourseType_Name = dt.Rows[i]["Name"].ToString(),

                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmCourseType { status = "There are no CourseType List" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmCourseType { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCamuColleges(string State_Id, string District_Id, string Taluka_Id, string CollegeType_Id)
        {
            //@Sharad
            List<vmCollege> lst = new List<vmCollege>();

            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_MASTER_CAMU_COLLEGE";
                    cmd.Parameters.AddWithValue("p_State_Id", State_Id);
                    cmd.Parameters.AddWithValue("p_Distrcit_Id", District_Id);
                    cmd.Parameters.AddWithValue("p_Taluka_Id", Taluka_Id);
                    cmd.Parameters.AddWithValue("p_CollegeType_Id", CollegeType_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmCollege
                            {
                                College_Id = dt.Rows[i]["College_Id"].ToString(),
                                College_Name = dt.Rows[i]["College_Name"].ToString(),

                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmCollege { status = "There are no College List" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmCollege { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCamuInstitutions()
        {
            //@Sharad
            List<vmInstitution> lst = new List<vmInstitution>();

            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_MASTER_CAMU_INSTITUTE";
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmInstitution
                            {
                                Institution_Id = dt.Rows[i]["Institute_Id"].ToString(),
                                Institution_Name = dt.Rows[i]["Institute_Name"].ToString(),
                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmInstitution { status = "There are no Institutions List" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmInstitution { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCamuProgram(string Institute_Id)
        {
            //@Sharad
            List<vmPrograms> lst = new List<vmPrograms>();

            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_MASTER_CAMU_PROGRAM";
                    cmd.Parameters.AddWithValue("p_Institute_Id", Institute_Id);

                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmPrograms
                            {
                                Program_Id = dt.Rows[i]["Program_Id"].ToString(),
                                Program_Name = dt.Rows[i]["Program_Name"].ToString(),
                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmPrograms { status = "There are no Courses" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmPrograms { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCamuDegree(string Institute_Id, string Program_Id)
        {
            //@Sharad
            List<vmDegree> lst = new List<vmDegree>();

            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_MASTER_PROGRAM_DEGREE";
                    cmd.Parameters.AddWithValue("p_Institute_Id", Institute_Id);
                    cmd.Parameters.AddWithValue("p_Program_Id", Program_Id);

                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmDegree
                            {
                                Degree_Id = dt.Rows[i]["Degree_Id"].ToString(),
                                Degree_Name = dt.Rows[i]["Degree_Name"].ToString(),
                                Application_Fees = double.Parse(dt.Rows[i]["Application_Fees"].ToString()),
                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmDegree { status = "There are no Fellowship" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmDegree { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCamuStaff(string Institute_Id, string Program_Id)
        {
            //@Sharad
            List<vmStaffs> lst = new List<vmStaffs>();

            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_MASTER_CAMU_STAFF_DETAILS";
                    cmd.Parameters.AddWithValue("p_Institute_Id", Institute_Id);
                    cmd.Parameters.AddWithValue("p_Program_Id", Program_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmStaffs
                            {
                                Staff_Id = dt.Rows[i]["Staff_Id"].ToString(),
                                Staff_Name = dt.Rows[i]["Staff_Name"].ToString(),

                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmStaffs { status = "There are no Staff" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmStaffs { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }



        public ActionResult GetCamuAcademic(string Institute_Id)
        {
            //@Sharad
            List<vmAcademic> lst = new List<vmAcademic>();

            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_MASTER_CAMU_ACADEMIC_YEAR";
                    cmd.Parameters.AddWithValue("p_Institute_Id", Institute_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmAcademic
                            {
                                Academic_Id = dt.Rows[i]["Academic_Id"].ToString(),
                                Academic_Code = dt.Rows[i]["Academic_code"].ToString(),
                            });
                        }
                    }
                    else
                    {
                        lst.Add(new vmAcademic { status = "There are no Year" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmAcademic { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }


        #region Staff Verification

        public ActionResult GetCamuClusterHead(string Event_Id)
        {
            //@Sharad
            List<vmStaffs> lst = new List<vmStaffs>();
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_CLUSTERHEAD";
                    cmd.Parameters.AddWithValue("p_Event_Id", Event_Id);
                    cmd.Parameters.AddWithValue("p_User_Id", User_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmStaffs
                            {
                                Staff_Id = dt.Rows[i]["User_Id"].ToString(),
                                Staff_Name = dt.Rows[i]["UserName"].ToString(),

                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmStaffs { status = "There are no Staff" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmStaffs { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetCamuEventMaster()
        {
            //@Sharad
            List<vmEvents> lst = new List<vmEvents>();
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_EVENT_LIST";

                    cmd.Parameters.AddWithValue("p_User_Id", User_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmEvents
                            {
                                Event_Id = long.Parse(dt.Rows[i]["Event_Id"].ToString()),
                                Event_Name = dt.Rows[i]["Event_Name"].ToString(),

                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmEvents { status = "There are no forms" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmEvents { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetOnlineRegistrationList(string Event_Id, string Cluster_Head, string Payment_Status, string Academic_Year, string Institute_Id, string Course_Id)
        {
            //@Sharad
            List<vmRegistrationReport> lst = new List<vmRegistrationReport>();
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            try
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_CAMU_ONLINE_REGISTRATION_VERIFICATION_LISTING";
                    cmd.Parameters.AddWithValue("p_User_Id", Cluster_Head);
                    cmd.Parameters.AddWithValue("p_Event_Id", Event_Id);
                    cmd.Parameters.AddWithValue("p_Payment_Status", Payment_Status);
                    cmd.Parameters.AddWithValue("p_Academic_Year", Academic_Year);
                    cmd.Parameters.AddWithValue("p_Institute_Id", Institute_Id);
                    cmd.Parameters.AddWithValue("p_Course_Id", Course_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmRegistrationReport
                            {
                                Registration_Id = long.Parse(dt.Rows[i]["Registration_Id"].ToString()),

                                Registration_Link = dt.Rows[i]["Registration_Link"].ToString(),
                                Name = dt.Rows[i]["Name"].ToString(),

                                Name_On_Certificate = dt.Rows[i]["Name_On_Certificate"].ToString(),
                                Contact_No = dt.Rows[i]["Contact_No"].ToString(),
                                Email_Id = dt.Rows[i]["Email_Id"].ToString(),
                                Gender = dt.Rows[i]["Gender"].ToString(),
                                State = dt.Rows[i]["State"].ToString(),
                                District = dt.Rows[i]["District"].ToString(),
                                Taluka = dt.Rows[i]["Taluka"].ToString(),
                                College_Name = dt.Rows[i]["College_Name"].ToString(),
                                isPayment_Completed = dt.Rows[i]["isPayment_Completed"].ToString(),
                                Bank_Transaction_Id = dt.Rows[i]["Bank_Transaction_Id"].ToString(),
                                Transaction_Amount = dt.Rows[i]["Transaction_Amount"].ToString(),
                                Transaction_Date = dt.Rows[i]["Transaction_Date"].ToString(),
                                Created_Date = dt.Rows[i]["Created_Date"].ToString(),
                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmRegistrationReport { status = "There are no students" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmRegistrationReport { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetOnlineRegistrationReportList(string Event_Id, string Cluster_Head, string PaymentStatus, string Academic_Year, string Institute_Id, string Course_Id)
        {
            //@Sharad
            List<vmRegistrationReport> lst = new List<vmRegistrationReport>();
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandText = "USP_GET_CAMU_ONLINE_REGISTRATION_DETAILED_REPORTS_LATEST";
                    cmd.CommandText = "USP_GET_CAMU_ONLINE_REGISTRATION_DETAILED_REPORTS";
                    cmd.Parameters.AddWithValue("p_User_Id", Cluster_Head);
                    cmd.Parameters.AddWithValue("p_Event_Id", Event_Id);
                    cmd.Parameters.AddWithValue("p_Payment_Status", PaymentStatus);
                    cmd.Parameters.AddWithValue("p_Academic_Year", Academic_Year);
                    cmd.Parameters.AddWithValue("p_Institute_Id", Institute_Id);
                    cmd.Parameters.AddWithValue("p_Course_Id", Course_Id);

                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmRegistrationReport
                            {
                                Registration_Id = long.Parse(dt.Rows[i]["Registration_Id"].ToString()),
                                Event_Id = long.Parse(dt.Rows[i]["Event_Id"].ToString()),
                                Registration_Link = dt.Rows[i]["Registration_Link"].ToString(),
                                Name = dt.Rows[i]["Name"].ToString(),
                                DOB = dt.Rows[i]["DOB"].ToString(),
                                Age = dt.Rows[i]["Age"].ToString(),
                                Name_On_Certificate = dt.Rows[i]["Name_On_Certificate"].ToString(),
                                Contact_No = dt.Rows[i]["Contact_No"].ToString(),
                                Email_Id = dt.Rows[i]["Email_Id"].ToString(),
                                Gender = dt.Rows[i]["Gender"].ToString(),
                                Qualification = dt.Rows[i]["Qualification"].ToString(),
                                Year_Of_Passing = dt.Rows[i]["Year_Of_Passing"].ToString(),
                                State = dt.Rows[i]["State"].ToString(),
                                District = dt.Rows[i]["District"].ToString(),
                                Taluka = dt.Rows[i]["Taluka"].ToString(),
                                College_Name = dt.Rows[i]["College_Name"].ToString(),
                                isPayment_Done = int.Parse(dt.Rows[i]["isPayment_Done"].ToString()),
                                isPayment_Completed = dt.Rows[i]["isPayment_Completed"].ToString(),
                                Bank_Transaction_Id = dt.Rows[i]["Bank_Transaction_Id"].ToString(),
                                Transaction_Amount = dt.Rows[i]["Transaction_Amount"].ToString(),
                                Transaction_Date = dt.Rows[i]["Transaction_Date"].ToString(),
                                isRegistration_Completed = int.Parse(dt.Rows[i]["isRegistration_Completed"].ToString()),
                                Source = dt.Rows[i]["Source"].ToString(),
                                Accept_Terms = dt.Rows[i]["Accept_Terms"].ToString(),
                                Created_Date = dt.Rows[i]["Created_Date"].ToString(),
                                Created_DateTime = dt.Rows[i]["Created_DateTime"].ToString(),
                                isVerified = dt.Rows[i]["isVerified"].ToString(),
                                Verified_Date = dt.Rows[i]["Verified_Date"].ToString(),
                                Verified_By = dt.Rows[i]["Verified_By"].ToString(),
                                SyncToCamu = dt.Rows[i]["SyncToCamu"].ToString(),
                                Sync_Date = dt.Rows[i]["Sync_Date"].ToString(),
                                Application_No = dt.Rows[i]["Application_No"].ToString(),
                                Application_Status = dt.Rows[i]["Camu_Application_Status"].ToString(),
                                Father_Name = dt.Rows[i]["Father_Name"].ToString(),
                                Father_Contact = dt.Rows[i]["Father_Contact"].ToString(),
                                isBacklogs = dt.Rows[i]["isBacklogs"].ToString(),
                                Backlogs_nos = dt.Rows[i]["Backlogs_nos"].ToString(),
                                // Institution_Name= dt.Rows[i]["Institution_Name"].ToString(),
                                // Admission_Year = dt.Rows[i]["Admission_Year"].ToString(),
                                // Program_Name = dt.Rows[i]["Program_Name"].ToString(),
                                // Degree_Name = dt.Rows[i]["Degree_Name"].ToString(),
                                // Reporting_Managers = dt.Rows[i]["Reporting_Managers"].ToString(),
                                Reference_Name = dt.Rows[i]["Reference_Name"].ToString(),
                                Division_Name = dt.Rows[i]["Division_Name"].ToString(),

                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmRegistrationReport { status = "There are no students" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmRegistrationReport { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public async Task<ActionResult> creg_report()
        {
            try
            {
                dfCrypto crp = new dfCrypto();

                long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());

                //string Event_Id = crp.Decrypt(id.ToString().TrimEnd('S'));

                //DBLayer DL = new DBLayer();
                //MySqlParameter[] queryParams = new MySqlParameter[] {
                //    new MySqlParameter("p_User_Id",User_Id),
                //    new MySqlParameter("p_Event_Id",1)
                //};
                //DataTable dt = await DL.ExecuteAdapterAsync(0, connection, "USP_GET_CAMU_ONLINE_REGISTRATION_REPORTS", queryParams);
                return View();
                // return View(dt);

            }
            catch (Exception ex)
            {
                return Redirect("/Creg/Error");
            }
        }


        public ActionResult GetCamuInstitutionsVerification()
        {
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            //@Sharad
            List<vmInstitution> lst = new List<vmInstitution>();

            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_MASTER_CAMU_INSTITUTE_VERIFICATION";
                    cmd.Parameters.AddWithValue("p_User_Id", User_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmInstitution
                            {
                                Institution_Id = dt.Rows[i]["Institute_Id"].ToString(),
                                Institution_Name = dt.Rows[i]["Institute_Name"].ToString(),
                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmInstitution { status = "There are no Institutions List" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmInstitution { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetCamuProgramVerification(string Institute_Id)
        {
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            //@Sharad
            List<vmPrograms> lst = new List<vmPrograms>();

            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_MASTER_CAMU_PROGRAM_VERIFICATION";
                    cmd.Parameters.AddWithValue("p_User_Id", User_Id);
                    cmd.Parameters.AddWithValue("p_Institute_Id", Institute_Id);

                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmPrograms
                            {
                                Program_Id = dt.Rows[i]["Program_Id"].ToString(),
                                Program_Name = dt.Rows[i]["Program_Name"].ToString(),
                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmPrograms { status = "There are no Courses" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmPrograms { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }
        #endregion

        public ActionResult AddCash()
        {
            return View();
        }
        public ActionResult CashValidation()
        {
            return View();
        }
        public ActionResult FieldAnalytics()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> CashAddGetMobile_Validation(string Name, string Mobile_No)
        {
            try
            {

                DBLayer DL = new DBLayer();


                MySqlParameter[] queryParams = new MySqlParameter[] {
                 new MySqlParameter("p_Name",Regex.Replace(Name.ToString(), "'", "`").Trim()),
                     new MySqlParameter("p_Mobile_No",Mobile_No),

                };
                DataTable dt = await DL.ExecuteAdapterAsync(0, connection, "USP_VALIDATE_CASH_ADD_MOBILENO", queryParams);

                if (dt.Rows.Count > 0)
                {

                    return Json(new { Status = true, Message = "Exists" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Status = false, Message = "NotExists" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save_CashReceived(vmAddCash Reg)
        {
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            long Id = Reg.Id;
            int Response_Id = 0;
            dfCrypto crp = new dfCrypto();
            DBLayer DL = new DBLayer();
            try
            {
                MySqlParameter[] queryParams = new MySqlParameter[] {
                    new MySqlParameter("p_Id", Id),
                    new MySqlParameter("p_Name",Regex.Replace(Reg.Name.ToString(), "'", "`").Trim()),
                    new MySqlParameter("p_MobileNo", Reg.MobileNo),
                    new MySqlParameter("p_User_Id", User_Id),
                };
                Response_Id = await DL.ExecuteAsync(connection, "USP_SAVE_ONLINE_REGISTRATION_CASH_ADD", queryParams);


                return Json(new { Status = true, Message = "Save Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Status = false, Message = "Error" }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GetAdd_CashList(string Academic_Year, string Type)
        {
            //@Sharad
            List<vmAddCash> lst = new List<vmAddCash>();
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            try
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_CAMU_ONLINE_REGISTRATION_CASH_LIST";
                    cmd.Parameters.AddWithValue("p_User_Id", User_Id);
                    cmd.Parameters.AddWithValue("p_Type", Type);
                    cmd.Parameters.AddWithValue("p_Year", Academic_Year);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmAddCash
                            {
                                Id = long.Parse(dt.Rows[i]["Id"].ToString()),
                                Name = dt.Rows[i]["Student_Name"].ToString(),
                                MobileNo = dt.Rows[i]["MobileNo"].ToString(),
                                Created_Date = dt.Rows[i]["Created_Date"].ToString(),
                                AddCashStatus = dt.Rows[i]["Status"].ToString(),
                                status = "success"

                            });

                        }
                    }
                    else
                    {
                        lst.Add(new vmAddCash { status = "No data" });
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmAddCash { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }       

    }
}