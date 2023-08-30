using System;
using System.ComponentModel.DataAnnotations;

namespace CulturaSurvey.ViewModel
{
    public class Login
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "User name")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class UserMenu
    {
        public long Id { get; set; }
        public string MenuCode { get; set; }
        public string url { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string Menutype { get; set; }
        public int SortOrder { get; set; }
        public long ParentId { get; set; }
        public string Menu_Icon { get; set; }
    }

    public class UserInfo
    {
        public long User_ID { get; set; }
        public long Refernce_Id { get; set; }
        public string Username { get; set; }
        public int User_Type { get; set; }

        public string email { get; set; }
        public string name { get; set; }
        public string image { get; set; }
    }

    public class UserDetails
    {
        public long RID { get; set; }
        public long SID { get; set; }
        public long UserId { get; set; }
        public string EmployeeName { get; set; }
        public int isCompleted_Survey { get; set; }

    }

    public enum User_Types
    {
        SuperAdmin = 1,
        Administrator = 2,
        HealthMonitors = 3,
        StockManager = 4,
        CallCentre = 5,
        ContactCentreAdmin = 6,
        NodalOfficer = 7,
        Commissioner = 8,
        Volunteer = 9
    };

    public class vmChangePassword
    {
        public long User_ID { get; set; }

        [Display(Name = "email Id")]
        public string eMailId { get; set; }

        [Required]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation does not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class vmBICharts
    {
        public int Slno { get; set; }
        public string category { get; set; }
        public string chat_url { get; set; }
       public string status { get; set; }
    }
    public class vmDF_Survey
    {
        public int slno
        {
            get; set;
        }
        public int Response_Id
        {
            get; set;
        }
        public string impact_unit
        {
            get; set;
        }
        public string department
        {
            get; set;
        }
        public string gender
        {
            get; set;
        }
        public string work_location
        {
            get; set;
        }
        public string Level
        {
            get; set;
        }
        public string tenuer
        {
            get; set;
        }
        public string age
        {
            get; set;
        }
        public string Language
        {
            get; set;
        }
        public int Academic_Slno
        {
            get; set;
        }
        public string AcademicCode
        {
            get; set;
        }
        public string Q_code
        {
            get; set;
        }
        public string Questions
        {
            get; set;
        }
      
        public string comb_code
        {
            get; set;
        }
        public string comb_description
        {
            get; set;
        }
        public string Org_Level
        {
            get; set;
        }
        public string FiveP
        {
            get; set;
        }
        public int rating
        {
            get; set;
        }
        public string rating_description
        {
            get; set;
        }
        public int isOption
        {
            get; set;
        }
        public string Comments
        {
            get; set;
        }
        public string Status
        {
            get; set;
        }
    }

    public class vmExotelNumbers
    {
        public int Slno { get; set; }
        public string Mobileno { get; set; }
        public string Message { get; set; }
        
    }

    public class vmSurveySummary
    {
        public string Department { get; set; }
        public string Participants { get; set; }
        public string Submitted { get; set; }
        public string Pending { get; set; }
        public string SubmissionPercentage { get; set; }

    }

    public class vmSurveyBICharts
    {
        public string category { get; set; }
        public string ChartURL { get; set; }
       public string UserType { get; set; }
    }
    public class vmSurveySummaryCount
    {
        public int TodayTotal { get; set; }
        public int OverAll { get; set; }
       
    }
    public class vmSurveyMaster
    {
        public int SurveyId { get; set; }
        public string SurveyName { get; set; }

    }

    public class vmSurveyComments
    {
        public string Code { get; set; }
        public string Level { get; set; }
        public string Priciples { get; set; }
        public string Questions { get; set; }
        public string Comments { get; set; }


    }
}