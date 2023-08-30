using CulturaSurvey.Infrastructure;

using CulturaSurvey.ViewModel;
using DataLayer;
using MySql.Data.MySqlClient;
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
using Amazon.Runtime.CredentialManagement;
using ClosedXML.Excel;
using System.IO;
using System.Configuration;

namespace CulturaSurvey.Controllers
{
    public class HomeController : Controller
    {
      
    


        [HttpGet]
        public ActionResult Login()
        {
            return View("Login", new Login());
        }

      
     
        public ActionResult Error()
        {
            return View("Error", "Home");
        }


       
    
    }
}