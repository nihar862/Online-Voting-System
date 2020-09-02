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

namespace OVS.Controllers
{
    [Authorize(Roles = "Admin,admin")]
    public class CandidateController : Controller
    {
        private VotingSystemDBEntities db = new VotingSystemDBEntities();

        // GET: Candidate
        public ActionResult Index()
        {
            var candidates = db.Candidates.Include(c => c.Party);
            return View(candidates.ToList());
        }

        // GET: Candidate/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidate candidate = db.Candidates.Find(id);
            if (candidate == null)
            {
                return HttpNotFound();
            }
            return View(candidate);
        }

        // GET: Candidate/Create
        public ActionResult Create()
        {
            ViewBag.Party_id = new SelectList(db.Parties, "Party_ID", "Motto");
            return View();
        }

        // POST: Candidate/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Candidate candidate)
        {
            string fileName = Path.GetFileNameWithoutExtension(candidate.ImageFile2.FileName);
            string ext = Path.GetExtension(candidate.ImageFile2.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + ext;
            candidate.Image = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            candidate.ImageFile2.SaveAs(fileName);

            if (ModelState.IsValid)
            {
                candidate.Vote = "0";
                db.Candidates.Add(candidate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Party_id = new SelectList(db.Parties, "Party_ID", "Motto", candidate.Party.Motto);
            return View(candidate);
        }

        // GET: Candidate/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidate candidate = db.Candidates.Find(id);
            if (candidate == null)
            {
                return HttpNotFound();
            }
            ViewBag.Party_id = new SelectList(db.Parties, "Party_ID", "Password", candidate.Party_id);
            return View(candidate);
        }

        // POST: Candidate/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Candidate_ID,Password,Name,Mobile_no,City,State,Email_id,Aadhar_Id,Party_id,DOB,Image")] Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(candidate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Party_id = new SelectList(db.Parties, "Party_ID", "Password", candidate.Party_id);
            return View(candidate);
        }

        // GET: Candidate/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidate candidate = db.Candidates.Find(id);
            if (candidate == null)
            {
                return HttpNotFound();
            }
            return View(candidate);
        }

        // POST: Candidate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Candidate candidate = db.Candidates.Find(id);
            db.Candidates.Remove(candidate);
            db.SaveChanges();
            return RedirectToAction("Index");
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
