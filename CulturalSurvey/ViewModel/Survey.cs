using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Text;
using System.Security.Cryptography;

namespace CulturaSurvey.ViewModel
{
    public class Survey
    {
        public long survey_Id { get; set; }
        public string Survey_Name { get; set; }
        public string Kan_Survey_Name { get; set; }
        public string Tel_Survey_Name { get; set; }
        public string Eng_Welcome { get; set; }
        public string Kan_Welcome { get; set; }
        public string Tel_Welcome { get; set; }
        public string Eng_Instruction { get; set; }
        public string Kan_Instruction { get; set; }
        public string Tel_Instruction { get; set; }
        public int isAnonymous { get; set; }
        public int isEnabled { get; set; }

        public long User_Id { get; set; }
        public long Response_Id { get; set; }
        public string impact_unit { get; set; }
        public string department { get; set; }
        public string gender { get; set; }
        public string work_location { get; set; }
        public string Level { get; set; }
        public string tenuer { get; set; }
        public string age { get; set; }
        public string Language { get; set; }

        List<vmQuestions> questions = new List<vmQuestions>();

    }
    public class vmSurvey_Summary
    {
        public string Participants { get; set; }
        public string Submitted { get; set; }
        public string Pending { get; set; }
        public string CompletionPercentage { get; set; }

    }
    public class vmResponse_Main
    {       
     
        public string Impact_Unit { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }
        public string Work_Location { get; set; }
        public string Levels { get; set; }
        public string Tenuer { get; set; }
        public string Age { get; set; }
        public string Language { get; set; }      
    }
    public class vmResponse_Detail {
        public long id { get; set; }
        public int isOptions { get; set; }
        public int Rating { get; set; }
        public string Remark_Comment { get; set; }

    }
    public class vmMasters
    {
        public long FieldValue { get; set; }
        public string FieldText { get; set; }
    }
    public class Welcome
    {
        public long survey_Id { get; set; }
        public string Survey_Name { get; set; }
        public string Kan_Survey_Name { get; set; }
        public string Tel_Survey_Name { get; set; }
        public string Eng_Welcome { get; set; }
        public string Kan_Welcome { get; set; }
        public string Tel_Welcome { get; set; }
        public string Eng_Instruction { get; set; }
        public string Kan_Instruction { get; set; }
        public string Tel_Instruction { get; set; }
        public int isAnonymous { get; set; }
        public int isEnabled { get; set; }
    }
    public class Selections
    {
        public long User_Id { get; set; }
        public string impact_unit { get; set; }
        public string department { get; set; }
        public string gender { get; set; }
        public string work_location { get; set; }
        public string Level { get; set; }
        public string tenuer { get; set; }
        public string age { get; set; }
        public string Language { get; set; }
        public int isEnabled { get; set; }
    }
    public class vmQuestions
    {
        public long QSlno { get; set; }
        public long RSlno { get; set; }
        public string Q_Code_Display { get; set; }
        public string English { get; set; }
        public string kannada { get; set; }
        public string Telugu { get; set; }
        public string EQR { get; set; }
        public string KQR { get; set; }
        public string TQR { get; set; }
        public long isOptions { get; set; }

        public string Response { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public long isSaved { get; set; }
        public string Page_Group { get; set; }
    }

    public class dfCrypto
    {

     static string key { get; set; } = "A!9H*hi%Xj+YY4YP2@fun#007X_^";
    
      //  static string key { get; set; } = "fun@1234";

        static string bash_key { get; set; } = "B$A!9HDhi%XYZ4YP2fun@007#X";

        public string Encrypt(string password)
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
        public string Decrypt(string cipher)
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
                        //return UTF8Encoding.UTF8.GetString(bytes);
                        value = UTF8Encoding.UTF8.GetString(bytes);
                        return value.ToString().Replace("0000X", "");
                    }
                }
            }
        }
     
        public string GetMd5Hash(string value)
        {
            try
            {
                var md5Hasher = MD5.Create();
                var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value+bash_key));
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

        
    }
    public class vmTranslate_Response
    {

        public string Translated_Text { get; set; }
        public string Language_code { get; set; }

    }


}