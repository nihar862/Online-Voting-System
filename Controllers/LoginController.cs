using OVS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OVS.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        VotingSystemDBEntities db = new VotingSystemDBEntities();
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Voter voters)
        {
            using (db)
            {
                if (voters.VoterId != null && voters.Password != null)
                {
                    var valid = db.Voters.Where(u => u.VoterId == voters.VoterId && u.Password == voters.Password).FirstOrDefault();
                    if (valid == null)
                    {
                        ModelState.AddModelError("", "Username or Password Incorrect");
                        return View();
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(valid.VoterId, false);
                        Session["VoterId"] = valid.VoterId.ToString();
                        
                        Session["isVoted"] = valid.isVoted.ToString();
                        Session["isAllow"] = valid.isAllowed.ToString();       
                        
                 
                        if (valid.Role == "Voter")
                             return RedirectToAction("VotingPortal", "Voter");
                        else
                             return RedirectToAction("SearchPortal","Voter");
                    }
                }
                else
                {
                    return View();
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Remove("VoterId");
            Session.Remove("City");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Login");
        }
    }
}