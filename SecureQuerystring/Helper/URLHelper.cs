using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SecureQuerystring.Helpers
{
    public static class CustomURLHelper
    {
        public static string GetDigest(string tamperProofParams)
        {
           // string  SecretSalt = "H3#@*ALMLLlk31q4l1ncL#@...";
            string Digest = string.Empty;
            string input = string.Concat(SecretSalt, tamperProofParams, SecretSalt);
            //The array of bytes that will contain the encrypted value of input
            byte[] hashedDataBytes = null;
            //The encoder class used to convert strPlainText to an array of bytes
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            //Create an instance of the MD5CryptoServiceProvider class
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            //Call ComputeHash, passing in the plain-text string as an array of bytes
            //The return value is the encrypted value, as an array of bytes
            hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(input));
            //Base-64 Encode the results and strip off ending '==', if it exists
            Digest = Convert.ToBase64String(hashedDataBytes).TrimEnd("=".ToCharArray());
            return Digest;
        }

        public static string CreateTamperProofURL(string url, string nonTamperProofParams, string tamperProofParams)
        {
            string tpURL = url;
            if (nonTamperProofParams.Length > 0 || tamperProofParams.Length > 0)
            {
                url += "?";
            }
            //Add on the tamper & non-tamper proof parameters, if any
            if (nonTamperProofParams.Length > 0)
            {
                url += nonTamperProofParams;
                if (tamperProofParams.Length > 0)
                    url += "&";
            }
            if (tamperProofParams.Length > 0)
                url += tamperProofParams;
            //Add on the tamper-proof digest, if needed
            if (tamperProofParams.Length > 0)
            {
                url += string.Concat("&Jack=", GetDigest(tamperProofParams));
            }
            return url;
        }

        public const string SecretSalt = "H3#@*ALMLLlk31q4l1ncL#@RFHF#N3fNM><#WH$O@#!FN#LNl33N#LNFl#J#Y$#IOHhnf;;3qrthl3q";
//public void Page_Load(object sender, EventArgs e)
//{
//    EnsureURLNotTampered(string.Format("TP1={0}&TP2={1}&TP3={2}", Request.QueryString("TP1"), Request.QueryString("TP2"), Request.QueryString("TP3")));
//}

//The secret salt...


   public static int EnsureURLNotTampered(string tamperProofParams, string receivedDigest)
   {
       //Determine what the digest SHOULD be
       string expectedDigest = GetDigestToDecode(tamperProofParams);
       //Any + in the digest passed through the querystring would be
       //convereted into spaces, so 'uncovert' them
      // string receivedDigest = Request.QueryString["Digest"];
       if (receivedDigest == null)
       {
           //Oh my, we didn't get a Digest!
          // Response.Write("YOU MUST PASS IN A DIGEST!");
          // Response.End();
       }
       else
       {
           receivedDigest = receivedDigest.Replace(" ", "+");
           //Now, see if the received and expected digests match up
           if (string.Compare(expectedDigest, receivedDigest) != 0)
           {
               //Don't match up, egad
              // Response.Write("THE URL HAS BEEN TAMPERED WITH.");
              // Response.End();
           return 1;
           }
       }
       return 0;
   }

   public static string GetDigestToDecode(string tamperProofParams)
   {
       string Digest = string.Empty;
       string input = string.Concat(SecretSalt, tamperProofParams, SecretSalt);

       //The array of bytes that will contain the encrypted value of input
       byte[] hashedDataBytes = null;

       //The encoder class used to convert strPlainText to an array of bytes
       System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();

       //Create an instance of the MD5CryptoServiceProvider class
       System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();

       //Call ComputeHash, passing in the plain-text string as an array of bytes
       //The return value is the encrypted value, as an array of bytes
       hashedDataBytes = md5Hasher.ComputeHash(encoder.GetBytes(input));

       //Base-64 Encode the results and strip off ending '==', if it exists
       Digest = Convert.ToBase64String(hashedDataBytes).TrimEnd("=".ToCharArray());

       return Digest;
   }



    }
}