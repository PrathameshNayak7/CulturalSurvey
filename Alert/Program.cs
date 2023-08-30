using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Alert.Class;

namespace Alert
{
    public class Program
    {
    

         static string key { get; set; } = "A!9H*hi%Xj+YY4YP2@fun#007X_^";
        static string bash_key { get; set; } = "B$A!9HDhi%XYZ4YP2fun@007#X";

        public static void Main()
        {
            MainAsync().GetAwaiter().GetResult();
        }
        private static async Task MainAsync()
        {
            try
            {
                DateTime currentDay = DateTime.Parse(DateTime.Now.ToString("dd-MM-yyyy"));
                int timing = int.Parse(DateTime.Now.ToString("HH"));
                var PreviousDay = currentDay.AddDays(-1);
                 await Encrypt();
             // await RR_Generate_Link(7);
            // await RR_Send_Invitation(7, "Invitation");
        //  await RR_Send_Remider(7, "reminder", PreviousDay.ToString().Substring(0, 10));

                //  await Descrypt();
                //  await Generate_Link(2);
                //  await bash();
                //await Send_Invitation(2, "Invitation");

                //  await Send_Remider(2, "reminder",PreviousDay.ToString().Substring(0, 10));


                //--------- LEAD lead_Id testing------------

                long ldcount;
                long tid = 0;
                string LeadId = "LD2123655";
                string LID = "LD21";
                string pass = LeadId.Substring(4, LeadId.LastIndexOf(LID) + 5);
                ldcount =Convert.ToInt64(pass);//["ldcnt"]);
                tid = ldcount + 1;
                int length = 5;
                string asString = tid.ToString("D" + length); //"00050"
                LID = LID + asString;

            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
            }
        }
       
        public static async Task Generate_Link(int Survey_Id)
        {
            string status = "";
            Notify obj = new Notify();
            try
            {
                await obj.Generate_Link(Survey_Id);
            }
            catch (Exception ex)
            {
                status = "error" + ex.Message.ToString();
            }
        }
        public static async Task Send_Invitation(int Survey_Id, string Type)
        {
            string status = "";
            Notify obj = new Notify();
            try
            {
                await obj.Send_Invitation(Survey_Id, Type);
            }
            catch (Exception ex)
            {
                status = "error" + ex.Message.ToString();
            }
        }
        public static async Task Send_Remider(int Survey_Id, string Type,string PreviousDay)
        {
            string status = "";
            Notify obj = new Notify();
            try
            {
               await obj.Send_Reminder(Survey_Id, Type, PreviousDay.ToString());
            }
            catch (Exception ex)
            {
                status = "error" + ex.Message.ToString();
            }
        }

        public static async Task Generate_Unique_Code()
        {
          
            try
            {
              
                var generator = new RandomGenerator();              
                var randomPassword =generator.RandomPassword();
                var EncryPass = await Encrypt(randomPassword.ToString());
            
              //  var Encrttesting = await Encrypt("fun");
                string sPass = await GetMd5Hash(EncryPass.ToString());
                 
            }
            catch (Exception ex)
            {
               
            }
        }
        
        public static async Task Encrypt()
        {
            try
            {

                string encrypt6 = await Encrypt("0000X2") + "S";
                string encrypt5 = await Encrypt("0000X1") + "S";
                string encrypt1 = await Encrypt("0000X6")+"S";
                string encrypt2 = await Encrypt("0000X99") + "S";
                string encrypt3 = await Encrypt("0000X999") + "S";
                string encrypt4 = await Encrypt("0000X99999") + "S";
                string trimed = encrypt5.TrimEnd('S');
              //  string sPass = await GetMd5Hash(encrypt.ToString());
              //  string decrpt = await Decrypt(encrypt.ToString());
            }
            catch (Exception ex)
            {

            }
        }
        public static async Task bash()
        {
            try
            {
               
                string sPass = await GetMd5Hash("15KknavXfp4=");
              
            }
            catch (Exception ex)
            {

            }
        }

        public static async Task Descrypt()
        {

            try
            {
                //string sPass = await GetMd5Hash("kLjQ2AOq9x8=");
                string decrpt1 = await Decrypt("jpyFvYVhCCxa7ET+kRhBtg==");
                string decrpt2 = await Decrypt("jpyFvYVhCCy06Gk/E5D1Tg==");


            }
            catch (Exception ex)
            {

            }
        }

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
            string value = "";
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {  
                        byte[] cipherBytes = Convert.FromBase64String(cipher.Replace(' ', '+'));
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        value = UTF8Encoding.UTF8.GetString(bytes);
                     
                        //return UTF8Encoding.UTF8.GetString(bytes);

                        return value.ToString().Replace("0000X", ""); 

                    }
                }
            }
        }

        public static async Task<string> GetMd5Hash(string value)
        {
            try
            {
                var md5Hasher = MD5.Create();
                var data =  md5Hasher.ComputeHash(Encoding.Default.GetBytes(value+bash_key));
                var sBuilder = new StringBuilder();
                for (var i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
            catch (Exception ex)
            {

                return "error";
            }
        }
     

        public static async Task Previous_Day_Data_NotUpdated(string currentDay,string DisplayDate)
        {
            string status = "";
            Notify obj = new Notify();
            try
            {
                await obj.Reminder_Hospital_Bed_Data(currentDay.ToString(), DisplayDate.ToString());
            }
            catch (Exception ex)
            {
                status = "error" + ex.Message.ToString();
            }
        }

        public static async Task Email_Alert(string currentDay, string DisplayDate)
        {
            string status = "";
            Notify obj = new Notify();
            try
            {
                await obj.Email_Alert(currentDay.ToString(), DisplayDate.ToString());
            }
            catch (Exception ex)
            {
                status = "error" + ex.Message.ToString();
            }
        }


        #region RR
        public static async Task RR_Generate_Link(int Reward_Id)
        {
            string status = "";
            Notify obj = new Notify();
            try
            {
                await obj.RR_Generate_Link(Reward_Id);
            }
            catch (Exception ex)
            {
                status = "error" + ex.Message.ToString();
            }
        }

        public static async Task RR_Send_Invitation(int Reward_Id, string Type)
        {
            string status = "";
            Notify obj = new Notify();
            try
            {
                await obj.RR_Send_Invitation(Reward_Id, Type);
            }
            catch (Exception ex)
            {
                status = "error" + ex.Message.ToString();
            }
        }

        public static async Task RR_Send_Remider(int Reward_Id, string Type, string PreviousDay)
        {
            string status = "";
            Notify obj = new Notify();
            try
            {
                await obj.RR_Send_Reminder(Reward_Id, Type, PreviousDay.ToString());
            }
            catch (Exception ex)
            {
                status = "error" + ex.Message.ToString();
            }
        }
        #endregion
    }
}
