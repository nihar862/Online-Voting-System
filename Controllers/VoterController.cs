using OVS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OVS.Controllers
{
    
    public class VoterController : Controller
    {
        private VotingSystemDBEntities db = new VotingSystemDBEntities();
        // GET: Voter
        [Authorize(Roles = "Admin,Voter")]
        public ActionResult UserProfile()
        {
            return View();
        }

       
        [HttpGet]
        [Authorize(Roles = "Admin,Voter,admin,voter")]
        public ActionResult VotingPortal(Candidate c)
        {
            var voter = Session["VoterId"].ToString();
            var id = db.Voters.Where(u => u.VoterId == voter).FirstOrDefault().City.ToString();    
            return View(db.Candidates.Where(x=>x.City.Contains(id)).ToList());
        }

        [Authorize(Roles ="Admin,admin")]
        [HttpGet]
        public ActionResult SearchPortal(string city,string name,string posi)
        {
            return View(db.Candidates.Where(x=>x.City.Contains(city) || x.Party.Motto.Contains(name) || x.Position.Contains(posi)).ToList());
        }


        public ActionResult CandidateList()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Voter,admin,voter")]
        public ActionResult CountVote(Voter v)
        {
            var voter = Session["VoterId"].ToString();
            var id = TempData["ID"].ToString();
            var Candidate = db.Candidates.Single(x => x.Candidate_ID == id);
            string count = Candidate.Vote.ToString();
            int num = int.Parse(count) + 1;
            TempData["num2"] = num.ToString();
            var isVoted = Session["isVoted"].ToString();

            if (isVoted == "False")
            {
                v = new Voter()
                {
                    VoterId = voter,
                    isVoted = true
                };
                db.Voters.Attach(v);
                db.Entry(v).Property(x => x.isVoted).IsModified = true;
                db.SaveChanges();
                
                return RedirectToAction("Voting", "Voter");
            }
            else
            {
                return RedirectToAction("Home", "Home");
            }

      
        }

        public ActionResult Voting(Candidate c)
        {
            var id = TempData["ID2"].ToString();  
            c = new Candidate()
            {
                Candidate_ID = id,
                Vote = TempData["num2"].ToString()       
            };
            db.Candidates.Attach(c);
            db.Entry(c).Property(x => x.Vote).IsModified = true;
            db.SaveChanges();

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }

        [Authorize(Roles = "Admin,Voter,admin,voter")]
        public ActionResult DeclareResult(Candidate c,Voter v)
        {
            var id = Session["VoterId"].ToString();
            var voter = db.Voters.Where(u => u.VoterId == id).FirstOrDefault();

            var cy = db.Voters.Where(u => u.VoterId == id).FirstOrDefault().City.ToString();
            

            if(voter.Role == "Admin")
            {
                return View(db.Candidates.ToList());
            }

            else if (voter.isVoted == true && voter.isAllowed == false)
            {
                return View(db.Candidates.Where(x => x.City.Contains(cy)).ToList());
            }
            else if (voter.isVoted == false && voter.isAllowed == true)
            {
                return RedirectToAction("notVoted","Voter");
            }
            else if(voter.isVoted == false && voter.isVoted == false)
            {
                return RedirectToAction("notVotedinEvent", "Voter");
            }
            else
            {
                return RedirectToAction("noResult", "Voter");
            }
        }

        public ActionResult noResult()
        {
            return View();
        }
        public ActionResult notVoted()
        {
            return View();
        }

        public ActionResult notVotedinEvent()
        {
            return View();
        }

        public ActionResult VotingStart(Voter v)
        {
            var isAllow = db.Voters.Where(x => x.isAllowed == false).ToList();

            foreach (var i in isAllow)
            {
                i.isAllowed = true;                
            }           
            db.SaveChanges();
            return RedirectToAction("SearchPortal", "Voter");
        }

        public ActionResult VotingEnd(Voter v)
        {
            var isAllow = db.Voters.Where(x => x.isAllowed == true).ToList();
            foreach (var i in isAllow)
            {
                i.isAllowed = false;
            }
            db.SaveChanges();
            return RedirectToAction("SearchPortal", "Voter");
        }

        public ActionResult Reset(Candidate c,Voter v)
        {
            var reset = db.Candidates.ToList();
            var votereset = db.Voters.ToList();
            foreach (var i in reset)
            {
                i.Vote = "0";
            }
            foreach(var it in votereset)
            {
                it.isVoted = false;
            }
            db.SaveChanges();
            return RedirectToAction("SearchPortal", "Voter");
        }
    }
    
}