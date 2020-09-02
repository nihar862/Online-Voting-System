using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using OVS.Models;

namespace Api.Controllers
{
    
    public class TestController : Controller
    {

        public ActionResult Index(string id,Voter v)
        {
            var identity = Session["VoterId"].ToString();
            TempData["ID"] = id;
            TempData["ID2"] = id;

            return View();
        }

        private VotingSystemDBEntities db = new VotingSystemDBEntities();
        public JsonResult SendOTP(string temp)
        {
           
                int otpValue = new Random().Next(100000, 999999);
                var status = "";
                try
                {
                    var no = Session["VoterId"].ToString();
                    string recipient = db.Voters.Where(u => u.VoterId == no).FirstOrDefault().Mobile_no.ToString();
                    string APIKey = "XmYGkbR/HYc-vVKOL0bG6dvRGnS5Tvff04iTUDROVN";

                    string message = "Your OTP Number is " + otpValue + " ";
                    String encodedMessage = HttpUtility.UrlEncode(message);

                    using (var webClient = new WebClient())
                    {
                        byte[] response = webClient.UploadValues("https://api.textlocal.in/send/", new NameValueCollection(){

                                             {"apikey" , APIKey},
                                             {"numbers" , recipient},
                                             {"message" , encodedMessage},
                                             {"sender" , "TXTLCL"}});

                        string result = System.Text.Encoding.UTF8.GetString(response);

                        var jsonObject = JObject.Parse(result);     

                        status = jsonObject["status"].ToString();

                        Session["CurrentOTP"] = otpValue;

                    }


                    return Json(status, JsonRequestBehavior.AllowGet);


                }
                catch (Exception e)
                {

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                    throw (e);
                }
           
        }

        public ActionResult EnterOTP()
        {
            
            return View();
        }

        [HttpPost]
        public JsonResult VerifyOTP(string otp)
        {
            bool result = false;

            string sessionOTP = Session["CurrentOTP"].ToString();

            if (otp == sessionOTP)
            {
                result = true;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
