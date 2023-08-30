using CulturaSurvey.ViewModel;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace CulturaSurvey.Controllers
{
    public class CashRegController : Controller
    {

        #region Declaration
        string connection = System.Configuration.ConfigurationManager.ConnectionStrings["MDMConnection"].ToString();
        string pattern = @"[^a-zA-Z0-9]";
        #endregion

        // GET: CashModule
        public ActionResult Index()
        {
            return View();
        }


        #region Student Cash Recieve Module
        public ActionResult Receivable()
        {
            return View();
        }     


        public ActionResult GetCamuClusterHead()
        {
            List<vmCashPayment> lst = new List<vmCashPayment>();
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            try
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GETCAMU_CLUSTERHEAD";
                    cmd.Parameters.AddWithValue("p_UserID", User_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmCashPayment
                            {
                                User_Id = long.Parse(dt.Rows[i]["User_Id"].ToString()),
                                UserName = dt.Rows[i]["UserName"].ToString()                               
                            });

                        }
                        Json(lst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmCashPayment { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public ActionResult LoadCashReceivable(string Event_ID, string Cluster_Head, string Verification_Year,string Payment_Status, string Verification_Institute, string Verification_Course)
        {
            List<vmCashPayment> lst = new List<vmCashPayment>();
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            try
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_PAYMENT_RECEIVABLE";
                    cmd.Parameters.AddWithValue("p_UserID", User_Id);
                    cmd.Parameters.AddWithValue("p_Event_Id", Event_ID);
                    cmd.Parameters.AddWithValue("p_Cluster_Head", Cluster_Head);
                    cmd.Parameters.AddWithValue("p_Payment_Status", Payment_Status);
                    cmd.Parameters.AddWithValue("p_Academic_Year", Verification_Year);
                    cmd.Parameters.AddWithValue("p_Institute_Id", Verification_Institute);
                    cmd.Parameters.AddWithValue("p_Course_Id", Verification_Course);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmCashPayment
                            {
                                User_Id = long.Parse(dt.Rows[i]["User_Id"].ToString()),
                                UserName = dt.Rows[i]["UserName"].ToString(),
                                Total_College = dt.Rows[i]["Total_College"].ToString(),
                                Total_Reg = long.Parse(dt.Rows[i]["Total_Reg"].ToString()),
                                Pending_Std_Count = long.Parse(dt.Rows[i]["Pending_Std_Count"].ToString()),
                                Total_Paid_Amount = long.Parse(dt.Rows[i]["Total_Paid_Amount"].ToString())
                            });

                        }
                        Json(lst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmCashPayment { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }


        public ActionResult LoadCashDetails(string Event_ID, string Cluster_Head, string Verification_Year, string Payment_Status, string Verification_Institute, string Verification_Course)
        {
            List<vmCashPayment> lst = new List<vmCashPayment>();
            try
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_CASHPAYMENT_DETAILS";
                    cmd.Parameters.AddWithValue("p_Event_Id", Event_ID);
                    cmd.Parameters.AddWithValue("p_Cluster_Head", Cluster_Head);                    
                    cmd.Parameters.AddWithValue("p_Payment_Status", Payment_Status);
                    cmd.Parameters.AddWithValue("p_Academic_Year", Verification_Year);
                    cmd.Parameters.AddWithValue("p_Institute_Id", Verification_Institute);
                    cmd.Parameters.AddWithValue("p_Course_Id", Verification_Course);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmCashPayment
                            {                                
                                Registration_Id = dt.Rows[i]["Registration_Id"].ToString(),
                                First_Name = dt.Rows[i]["First_Name"].ToString(),
                                Father_Name = dt.Rows[i]["Father_Name"].ToString(),
                                Last_Name = dt.Rows[i]["Last_Name"].ToString(),
                                College_Name = dt.Rows[i]["College_Name"].ToString(),
                                Payment_Date = dt.Rows[i]["Payment_Date"].ToString(),
                                Contact_No = dt.Rows[i]["Contact_No"].ToString(),
                                Email_Id = dt.Rows[i]["Email_Id"].ToString(),
                                Gender = dt.Rows[i]["Gender"].ToString()                                
                            });

                        }
                        Json(lst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmCashPayment { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }


        public ActionResult UpdateCashDetails(List<vmCashPayment> SelectedValues)
        {
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                foreach (var i in SelectedValues)
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand("USP_UPDATE_CASHSTATUS", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;                       
                        cmd.Parameters.AddWithValue("p_StudentID", i.Registration_Id);
                        cmd.Parameters.AddWithValue("p_User_Id", User_Id);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateCashRejectStatus(long savedID, string rejectRemark)
        {
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("USP_UPDATE_CASH_REJECTSTATUS", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_Registration_Id", savedID);
                    cmd.Parameters.AddWithValue("p_Reject_Remark", rejectRemark);
                    cmd.ExecuteNonQuery();
                }
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Amount Submission To Account

        public ActionResult Incharge_History()
        {
            return View();
        }

        public ActionResult Amount_Submission(long Id,long Pay_Amount,string Payment_Type,string Payment_Remark)
        {
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            string Reg_Payment_Remarks = Regex.Replace(Payment_Remark, pattern, "");
            using (MySqlConnection con = new MySqlConnection(connection))
            {
                
                using (MySqlCommand cmd = new MySqlCommand("USP_COLLECTED_AMOUNT_SUBMISSION", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_Id", Id);
                    cmd.Parameters.AddWithValue("p_UserID", User_Id);
                    cmd.Parameters.AddWithValue("p_Amount", Pay_Amount);
                    cmd.Parameters.AddWithValue("p_Payment_Type", Payment_Type);
                    cmd.Parameters.AddWithValue("p_Remarks", Reg_Payment_Remarks);
                    int status = cmd.ExecuteNonQuery();
                    if(status == 1)
                    {
                        return Json(status, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(status, JsonRequestBehavior.AllowGet);
                    }
                }
            }                  
            
        }


        public JsonResult Load_SubmissionDetails()        
        {
            List<vmCashPayment> lst = new List<vmCashPayment>();
            try
            {
                long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_LOAD_SUBMISSION_DATA_REPORT";
                    cmd.Parameters.AddWithValue("p_Id", 0);
                    cmd.Parameters.AddWithValue("p_UserID", User_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmCashPayment
                            {
                                Id = long.Parse(dt.Rows[i]["Id"].ToString()),
                                Amount = long.Parse(dt.Rows[i]["Amount"].ToString()),
                                Payment_Type = dt.Rows[i]["Payment_Type"].ToString(),
                                Remarks = dt.Rows[i]["Remarks"].ToString(),
                                Created_Date = dt.Rows[i]["Created_Date"].ToString(),
                                isAccountStatus = dt.Rows[i]["isAccountStatus"].ToString(),
                                AccountConfirm_Date = dt.Rows[i]["AccountConfirm_Date"].ToString()
                            });

                        }
                        Json(lst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmCashPayment { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Load_InchargeHistory()
        {
            List<vmCashPayment> lst = new List<vmCashPayment>();
            try
            {
                long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_SUBMITTED_AMOUNT_HISTORY";
                    cmd.Parameters.AddWithValue("p_UserID", User_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmCashPayment
                            {
                                Collected_Amount = long.Parse(dt.Rows[i]["Collected_Amount"].ToString()),
                                Submitted_Amount = long.Parse(dt.Rows[i]["Submitted_Amount"].ToString()),
                                Pending_Amount = long.Parse(dt.Rows[i]["Pending_Amount"].ToString())
                            });

                        }
                        Json(lst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmCashPayment { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Edit_Submitted_Amt(long Id)
        {
            List<vmCashPayment> lst = new List<vmCashPayment>();
            try
            {
                long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_LOAD_SUBMISSION_DATA_REPORT";
                    cmd.Parameters.AddWithValue("p_Id", Id);
                    cmd.Parameters.AddWithValue("p_UserID", User_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmCashPayment
                            {
                                Id = long.Parse(dt.Rows[i]["Id"].ToString()),
                                Amount = long.Parse(dt.Rows[i]["Amount"].ToString()),
                                Payment_Type = dt.Rows[i]["Payment_Type"].ToString(),
                                Remarks = dt.Rows[i]["Remarks"].ToString()
                            });

                        }
                        Json(lst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmCashPayment { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }




        #endregion



        #region Account Reconsoliate the submitted amount

        public ActionResult Account()
        {
            return View();
        }

        public JsonResult Load_AcountSummary()
        {
            List<vmCashPayment> lst = new List<vmCashPayment>();
            try
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_ACCOUNT_SUMMARY";
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmCashPayment
                            {
                                User_Id = long.Parse(dt.Rows[i]["User_Id"].ToString()),
                                Total_Reg = long.Parse(dt.Rows[i]["Total_Reg"].ToString()),
                                UserName = dt.Rows[i]["UserName"].ToString(),
                                Collected_Amount = long.Parse(dt.Rows[i]["Collected_Amount"].ToString()),
                                Submitted_Amount = long.Parse(dt.Rows[i]["Submitted_Amount"].ToString()),
                                Pending_Amount = long.Parse(dt.Rows[i]["Pending_Amount"].ToString()),
                                Account_Confirmed = long.Parse(dt.Rows[i]["Account_Confirmed"].ToString()),
                                Account_Pending = long.Parse(dt.Rows[i]["Account_Pending"].ToString())
                            });

                        }
                        Json(lst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmCashPayment { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Reconsoliate_Details(long Id)
        {
            List<vmCashPayment> lst = new List<vmCashPayment>();
            try
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_ACCOUNT_RECONSOLIATE_DETAILS";
                    cmd.Parameters.AddWithValue("p_UserID", Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmCashPayment
                            {
                                Id = long.Parse(dt.Rows[i]["Id"].ToString()),
                                Incharge_Name = dt.Rows[i]["Incharge_Name"].ToString(),
                                Amount = long.Parse(dt.Rows[i]["Amount"].ToString()),
                                Payment_Type = dt.Rows[i]["Payment_Type"].ToString(),
                                Created_Date = dt.Rows[i]["Created_Date"].ToString(),
                                Remarks = dt.Rows[i]["Remarks"].ToString(),
                                isAccountStatus = dt.Rows[i]["isAccountStatus"].ToString(),
                                AccountConfirm_Date = dt.Rows[i]["AccountConfirm_Date"].ToString()
                            });

                        }
                        Json(lst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmCashPayment { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Update_AccountStatus(List<vmCashPayment> SelectedValues)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    foreach (var i in SelectedValues)
                    {
                        try
                        {
                            con.Open();
                            using (MySqlCommand cmd = new MySqlCommand("USP_UPDATE_ACCOUNTSTATUS", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("p_Id", i.Id);
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        catch (Exception ex) { }
                    }

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion



        #region Cash Payment Report

        public ActionResult CashPayment_Report()
        {
            return View();
        }

        public JsonResult GetClusterHead()
        {
            List<vmCashPayment> lst = new List<vmCashPayment>();
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            try
            {
                DataTable dt = new DataTable();
                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_USER_CLUSTERHEAD";
                    cmd.Parameters.AddWithValue("p_UserID", User_Id);
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmCashPayment
                            {
                                User_Id = long.Parse(dt.Rows[i]["User_Id"].ToString()),
                                UserName = dt.Rows[i]["UserName"].ToString()
                            });

                        }
                        Json(lst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmCashPayment { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetCashRegistrationReportList(string Event_Id, string Cluster_Head, string PaymentStatus, string Academic_Year, string Institute_Id, string Course_Id)
        {
            List<vmRegistrationReport> lst = new List<vmRegistrationReport>();
            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_CAMU_CASH_REGISTRATION_DETAILED_REPORTS";
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
                                isCashReceived = dt.Rows[i]["isCashReceived"].ToString(),
                                CashReceivedStatusDate = dt.Rows[i]["CashReceivedStatusDate"].ToString(),
                                CashReceivedBy = dt.Rows[i]["CashReceivedBy"].ToString(),

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

        #endregion



        #region Bulk Deposite

        public ActionResult Bulk_Deposite()
        {
            return View();
        }


        public ActionResult GetStudentsList(string Event_Id, string Cluster_Head, string PaymentStatus, string Academic_Year, string Institute_Id, string Course_Id)
        {
            List<vmRegistrationReport> lst = new List<vmRegistrationReport>();
            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_GET_CAMU_CASH_REGISTRATION_FOR_BULK_DEPOSIT";
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
                                Transaction_Amount = dt.Rows[i]["Transaction_Amount"].ToString(),
                                Transaction_Date = dt.Rows[i]["Transaction_Date"].ToString(),
                                Created_Date = dt.Rows[i]["Created_Date"].ToString()

                            });

                        }
                    }
                    else
                    {
                        return Json(lst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmRegistrationReport { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Bulk_Amount_Submission(List<vmCashPayment> SelectedValues, long Pay_Amount, string Payment_Type, string Payment_Remark)
        {
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());

            using (MySqlConnection con = new MySqlConnection(connection))
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand("USP_COLLECTED_AMOUNT_SUBMISSION", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    
                    cmd.Parameters.AddWithValue("p_Id", 0);
                    cmd.Parameters.AddWithValue("p_UserID", User_Id);
                    cmd.Parameters.AddWithValue("p_Amount", Pay_Amount);
                    cmd.Parameters.AddWithValue("p_Payment_Type", Payment_Type);
                    cmd.Parameters.AddWithValue("p_Remarks", Payment_Remark);

                    MySqlParameter pNewIDParam = new MySqlParameter("@p_New_ID", MySqlDbType.Int64);
                    pNewIDParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(pNewIDParam);

                    cmd.ExecuteNonQuery();

                    long lastInsertID = (long)pNewIDParam.Value;

                    return RedirectToAction("Cash_Submit_To_Account", new { SelectedValues = SelectedValues , lastInsertID} );
                }
            }            

        }


        public ActionResult Cash_Submit_To_Account(List<vmCashPayment> SelectedValues, long lastInsertID)
        {
            long User_Id = long.Parse(Request.Cookies["glogininfo"]["User_Id"].ToString());
            using (MySqlConnection con = new MySqlConnection(connection))
            {               
                foreach (var i in SelectedValues)
                {
                    con.Open();
                    using (MySqlCommand cmd = new MySqlCommand("USP_SSR_UPDATE_BULK_DEPOSIT", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_Registration_Id", i.Registration_Id);
                        cmd.Parameters.AddWithValue("p_User_Id", User_Id);
                        cmd.Parameters.AddWithValue("p_DepositeBatch_Id", lastInsertID);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

            }

            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        #endregion Timer in secret key 


        #region
        public ActionResult Secretkey()
        {
            return View();
        }

        #endregion



        #region CASH REPORT

        public ActionResult Cash_Report()
        {
            return View();
        }


        public ActionResult Cash_Summary()
        {
            List<vmCashPayment> lst = new List<vmCashPayment>();
            try
            {
                DataTable dt = new DataTable();

                using (MySqlConnection con = new MySqlConnection(connection))
                {
                    con.Open();
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "USP_CASH_REPORT";                    
                    cmd.Connection = con;
                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lst.Add(new vmCashPayment
                            {
                                UserName = dt.Rows[i]["UserName"].ToString(),
                                Total_Registrations = long.Parse(dt.Rows[i]["Total_Registrations"].ToString()),
                                Receive_Confirmation_Pending = long.Parse(dt.Rows[i]["Receive_Confirmation_Pending"].ToString()),
                                Confirmation_Pending_Amt = long.Parse(dt.Rows[i]["Confirmation_Pending_Amt"].ToString()),
                                Recieved_Count = long.Parse(dt.Rows[i]["Recieved_Count"].ToString()),
                                Collected_Amount = long.Parse(dt.Rows[i]["Collected_Amount"].ToString()),
                                Deposited_Amt = long.Parse(dt.Rows[i]["Deposited_Amt"].ToString()),
                                Deposite_Pending = long.Parse(dt.Rows[i]["Deposite_Pending"].ToString()),
                                Account_Confirmaed = long.Parse(dt.Rows[i]["Account_Confirmaed"].ToString())
                            });

                        }
                    }
                    else
                    {
                        return Json(lst, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                lst.Add(new vmCashPayment { status = "Error" });
            }

            return Json(lst, JsonRequestBehavior.AllowGet);

        }

        #endregion





    }
}