using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OVS.Models;
using System.IO;
using System.Xml.Linq;

namespace OVS.Controllers
{
    [Authorize(Roles = "Admin,admin")]
    public class RegistrationController : Controller
    {
        
        private VotingSystemDBEntities db = new VotingSystemDBEntities();

        // GET: Voter
        
        public ActionResult VoterList()
        {
            return View(db.Voters.ToList());
        }


        // GET: Voter/Details/5
        
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voter voter = db.Voters.Find(id);
            if (voter == null)
            {
                return HttpNotFound();
            }
            return View(voter);
        }

        // GET: Voter/Create
        
        public ActionResult Register()
        {
            return View();
        }

        // POST: Voter/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Voter voter)
        {
            string fileName = Path.GetFileNameWithoutExtension(voter.ImageFile.FileName);
            string ext = Path.GetExtension(voter.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + ext;
            voter.Image = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            voter.ImageFile.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                voter.isVoted = false;
                voter.isAllowed = false;
                db.Voters.Add(voter);
                db.SaveChanges();
                return RedirectToAction("VoterList");
            }

            return View(voter);
        }

 
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Voter voter = db.Voters.Find(id);
            if (voter == null)
            {
                return HttpNotFound();
            }
            return View(voter);
        }

        // POST: Voter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public ActionResult DeleteConfirmed(string id)
        {
            Voter voter = db.Voters.Find(id);
            db.Voters.Remove(voter);
            db.SaveChanges();
            return RedirectToAction("VoterList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
