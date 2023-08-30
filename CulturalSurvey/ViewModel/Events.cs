using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CulturaSurvey.ViewModel
{
    public class vmEvents
    {
        public long Event_Id { get; set; }
        public string Event_Name { get; set; }
        public string About_Event { get; set; }
        public string Terms_Conditions { get; set; }
        public string Registration_Form { get; set; }
        public int isCollect_Fees { get; set; }
        public int isCamuRegistration { get; set; }
        public int Registration_Fees { get; set; }
        public int isEnabled { get; set; }
        public string status { get; set; }
    }

    public class vmAddCash
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Created_Date { get; set; }
        public string AddCashStatus { get; set; }
        public string status { get; set; }
    }

    //Cash Module
    public class vmCashPayment
    {
        public long User_Id { get; set; }
        public string UserName { get; set; }
        public string Total_College { get; set; }
        public long Total_Reg { get; set; }
        public long Pending_Std_Count { get; set; }
        public long Total_Paid_Amount { get; set; }
        public string status { get; set; }
        public string First_Name { get; set; }
        public string Father_Name { get; set; }
        public string Last_Name { get; set; }
        public string Name_On_Certificate { get; set; }
        public string College_Name { get; set; }
        public string Payment_Date { get; set; }
        public string Contact_No { get; set; }
        public string Email_Id { get; set; }
        public string Gender { get; set; }
        public string Registration_Id { get; set; }
        public long Amount { get; set; }
        public string Payment_Type { get; set; }
        public string Remarks { get; set; }
        public string Created_Date { get; set; }
        public string isAccountStatus { get; set; }
        public string AccountConfirm_Date { get; set; }
        public long Id { get; set; }
        public long Collected_Amount { get; set; }
        public long Submitted_Amount { get; set; }
        public long Pending_Amount { get; set; }
        public long Account_Confirmed { get; set; }
        public long Account_Pending { get; set; }
        public string Incharge_Name { get; set; }


        public string Application_No { get; set; }
        public int isCashReceived { get; set; }
        public string CashReceivedStatusDate { get; set; }
        public string State_Name { get; set; }
        public string Dist_Name { get; set; }
        public string Taluka_Name { get; set; }
        public string Qualification_Name { get; set; }
        public long Year_Of_Passing { get; set; }
        public long Father_Contact { get; set; }
        public string isBacklogs { get; set; }
        public string Backlogs_nos { get; set; }
        public string Source_of_Information { get; set; }
        public string Reference_Name { get; set; }

        public long p_New_ID { get; set; }



        public long Total_Registrations { get;set; }
        public long Receive_Confirmation_Pending { get;set; }
        public long Confirmation_Pending_Amt { get;set; }
        public long Recieved_Count { get;set; }
        public long Deposited_Amt { get;set; }
        public long Deposite_Pending { get;set; }
        public long Account_Confirmaed { get;set; }
        
    }

    public class vmRegistration
    {
        public long Registration_Id { get; set; }
        public long Event_Id { get; set; }

        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string DOB { get; set; }
        public string Age { get; set; }
        public string Name_On_Certificate { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
        
        public string Contact_No { get; set; }

        [Display(Name = "Father/Guardian Contact No")]
        public string Whatsapp_No { get; set; }
        public string College_Name { get; set; }

        public string College_Type_Id { get; set; }
        public string College_Id { get; set; }

        public string Gender { get; set; }

        [Required]
        [Display(Name = "Email Id")]
        public string Email_Id { get; set; }
        public string Qualification_Id { get; set; }
        public string State_Id { get; set; }
        public string District_id { get; set; }
        public string Taluka_Id { get; set; }
        public long Village_Id { get; set; }
        public string Year_Of_Passing { get; set; }
        public string BYN { get; set; }

        [Display(Name = "Backlogs")]
        public string BYesNumber { get; set; }
        public string Source { get; set; }
        public int isApplied_Skill_Training { get; set; }
        public string Application_Number { get; set; }
        public int isAccept_Terms { get; set; }
        public int isPayment_Done { get; set; }
        public string Institution_Id { get; set; }
        public string Program_Id { get; set; }
        public string Degree_Id { get; set; }
        public string Academic_Id { get; set; }

        public string Incharge_Id { get; set; }
        public string Reference_Name { get; set; }

        [Required]
        [Display(Name = "Application Fees")]
        public double Amount { get; set; }
        public int isRegistration_Completed { get; set; }
        public string RESPCODE { get; set; }

        public string Application_Status { get; set; }
    }

  
    public class vmCamuRegistration
    {
        public long Registration_Id { get; set; }
        public long Event_Id { get; set; }

        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string DOB { get; set; }
        public string Age { get; set; }
        public string Name_On_Certificate { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
       

        public string Contact_No { get; set; }

        [Display(Name = "Whatsapp No")]
        public string Whatsapp_No { get; set; }
        public string College_Name { get; set; }

        public string Gender { get; set; }

        [Required]
        [Display(Name = "Email Id")]
        public string Email_Id { get; set; }
        public string Qualification_Id { get; set; }
        public long State_Id { get; set; }
        public long District_id { get; set; }
        public long Taluka_Id { get; set; }
        public long Village_Id { get; set; }
        public string Year_Of_Passing { get; set; }
        public string Source { get; set; }
        public int BYN { get; set; }

        [Display(Name = "Backlogs")]
        public string BYesNumber { get; set; }

        public int isApplied_Skill_Training { get; set; }
        public string Application_Number { get; set; }
        public int isAccept_Terms { get; set; }
        public int isPayment_Done { get; set; }

        [Required]
        [Display(Name = "Entrance Fees")]
        public double Amount { get; set; }
        public int isRegistration_Completed { get; set; }
        public string RESPCODE { get; set; }
    }
    public class PaymentResponseViewModel
    {

        public string MID { get; set; }
        public string ORDERID { get; set; }
        public string CUST_ID { get; set; }

        [Display(Name = "Transaction Amount")]
        public string TXNAMOUNT { get; set; }
        public string CURRENCY { get; set; }

        [Display(Name = "Transaction ID")]
        public string TXNID { get; set; }


        [Display(Name = "UTR No")]
        public string BANKTXNID { get; set; }
        public string STATUS { get; set; }

        [Display(Name = "Response Code")]
        public string RESPCODE { get; set; }

        [Display(Name = "Response Message")]
        public string RESPMSG { get; set; }

        [Display(Name = "Transaction Date")]
        public string TXNDATE { get; set; }
        public string GATEWAYNAME { get; set; }
        public string BANKNAME { get; set; }


        [Display(Name = "Payment Mode")]
        public string PAYMENTMODE { get; set; }

      
        public string CHECKSUMHASH { get; set; }

    }
    public class vmQualification
    {
        public string Qualification_Id { get; set; }
        public string Qualification_Name { get; set; }
        public string status { get; set; }
    }
    public class vmCourseType
    {
        public string CourseType_Id { get; set; }
        public string CourseType_Name { get; set; }
        public string status { get; set; }
    }
   
    public class vmState
    {
        public string State_Id { get; set; }
        public string State_Name { get; set; }
        public string status { get; set; }
    }
    public class vmDistrict
    {
        public string District_Id { get; set; }
        public string District_Name { get; set; }
        public string status { get; set; }
    }
    public class vmTaluka
    {
        public string Taluka_Id { get; set; }
        public string Taluka_Name { get; set; }
        public string status { get; set; }
    }
    public class vmVillage
    {
        public string Village_Id { get; set; }
        public string Village_Name { get; set; }
        public string status { get; set; }
    }

    public class vmCollege
    {
        public string College_Id { get; set; }
        public string College_Name { get; set; }
        public string status { get; set; }
    }
    public class vmInstitution
    {
        public string Institution_Id { get; set; }
        public string Institution_Name { get; set; }
        public string status { get; set; }
    }
    public class vmPrograms
    {
        public string Program_Id { get; set; }
        public string Program_Name { get; set; }
        public string status { get; set; }
    }
    public class vmDegree
    {
        public string Degree_Id { get; set; }
        public string Degree_Name { get; set; }
        public double Application_Fees { get; set; }
        public string status { get; set; }
    }
    public class vmStaffs
    {
        public string Staff_Id { get; set; }
        public string Staff_Name { get; set; }   
        public string status { get; set; }
    }

    public class vmAcademic
    {
        public string Academic_Id { get; set; }
        public string Academic_Code { get; set; }
        public string status { get; set; }
    }
    public static class PaytmParamKeys
    {
        public static string MId = "MID";
        public static string WEBSITE = "WEBSITE";
        public static string INDUSTRY_TYPE_ID = "INDUSTRY_TYPE_ID";
        public static string CHANNEL_ID = "CHANNEL_ID";
        public static string ORDER_ID = "ORDER_ID";
        public static string CUST_ID = "CUST_ID";
        public static string MOBILE_NO = "MOBILE_NO";
        public static string EMAIL = "EMAIL";
        public static string TXN_AMOUNT = "TXN_AMOUNT";
        public static string CALLBACK_URL = "CALLBACK_URL";
    }

    public static class PaytmUrl
    {
        public static string StagingUrl = "https://securegw-stage.paytm.in/order/process";
        public static string ProductionUrl = "https://securegw.paytm.in/order/process";
    }

    public static class PathLocation
    {
        public static string PaytmTemplate = "/Content/Templates/PaytmCheckOutPage.html";
    }

    public static class ResponseCodes
    {

    }

    public class vmRegistrationReport
    {
        public long Registration_Id { get; set; }
        public long Event_Id { get; set; }
        public string Registration_Link { get; set; }
        public string Name { get; set; }
       public string Name_On_Certificate { get; set; }
        public string Contact_No { get; set; }
        public string Whatsapp_No { get; set; }
        public string DOB { get; set; }
        public string Age { get; set; }
        
        public string Email_Id { get; set; }
        public string College_Name { get; set; }
        public int isPayment_Done { get; set; }
        public string isPayment_Completed { get; set; }
        public string Bank_Transaction_Id { get; set; }
        public string Transaction_Amount { get; set; }
        public string Transaction_Date { get; set; }
        public int isRegistration_Completed { get; set; }
        public string Gender { get; set; }
        public string Qualification { get; set; }
        public string Year_Of_Passing { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string Taluka { get; set; }
        public string Village { get; set; }
        public string Source { get; set; }
        public string Skilling_Student { get; set; }
        public string Application_No { get; set; }
        public string  Accept_Terms { get; set; }
        public string Created_Date { get; set; }
        public string Created_DateTime { get; set; }
        public string isVerified { get; set; }
        public string Verified_Date { get; set; }

        public string Verified_By { get; set; }
        public string SyncToCamu { get; set; }
        public string Sync_Date { get; set; }
        public string Application_Status { get; set; }
        public string isBacklogs { get; set; }
        public string Backlogs_nos { get; set; }
        public string Institution_Name { get; set; }
        public string Admission_Year { get; set; }
        public string Program_Name { get; set; }
        public string Degree_Name { get; set; }
        public string Reporting_Managers { get; set; }
        public string Reference_Name { get; set; }

        public string Father_Name { get; set; }
        public string Father_Contact { get; set; }

        public string Division_Name { get; set; }
        public string status { get; set; }


        public string isCashReceived { get; set;}
        public string CashReceivedStatusDate { get; set;}
        public string CashReceivedBy { get; set;}
    }

    public class GoogleAccessToken

    {

        public string access_token { get; set; }

        public string token_type { get; set; }

        public int expires_in { get; set; }

        public string id_token { get; set; }

        public string refresh_token { get; set; }

    }
    public class GoogleUserOutputData

    {

        public string id { get; set; }

        public string name { get; set; }

        public string given_name { get; set; }

        public string email { get; set; }

        public string picture { get; set; }

    }
}