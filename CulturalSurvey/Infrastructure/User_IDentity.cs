using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace CulturaSurvey.Infrastructure
{
    [Serializable]
    public class User_IDentity : MarshalByRefObject, IIdentity, IPrincipal
    {
        private readonly FormsAuthenticationTicket _ticket;

        public User_IDentity(FormsAuthenticationTicket ticket)
        {
            _ticket = ticket;
        }

        public string AuthenticationType
        {
            get { return "User"; }
        }

        public bool IsAuthenticated
        {
            get { return true; }
        }

        public string Name
        {
            get { return _ticket.Name; }
        }

        //public string User_Name
        //{
        //    get { return _ticket.User_Name; }
        //}

        public string User_ID
        {
            get { return _ticket.UserData; }
        }

        public bool IsInRole(string role)
        {
            return Roles.IsUserInRole(role);
        }

        public IIdentity Identity
        {
            get { return this; }
        }
    }
}