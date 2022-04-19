using Job_portal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Job_portal.Controllers
{
    public class CompanyController : Controller
    {
        job_DatabaseEntities1 db = new job_DatabaseEntities1();
        // GET: Company
        public ActionResult Index()
        {
            try
            {
                Response.Cache.SetNoStore();
                TempData["companyname"] = Session["companyname"].ToString();
                //show posted job
                var getpostedjob = db.PostJob_tb.ToList();
                ViewBag.showpostedjob = getpostedjob;

                //count total job seeker
                var countjobseeker = db.jobseeker_tb.Count();
                ViewBag.showcountjobseeker = countjobseeker;

                //count total company
                var countcompany = db.Company_tb.Count();
                ViewBag.showcountcompany = countcompany;

                //count total posted job
                var countpostedjob = db.PostJob_tb.Count();
                ViewBag.showcountpostedjob = countpostedjob;

                //count total apply job
                var countapplyjob = db.ApplyJob_tb.Count();
                ViewBag.showcountapplyjob = countapplyjob;
            }
            catch (Exception ex)
            {
                ViewBag.ss = ex.ToString();
                return RedirectToAction("login","Company");
            }
            return View();
        }


        public ActionResult companyRegistration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult companyRegistration(Company_tb comtb)
        {
            if(ModelState.IsValid)
            {
                db.Entry(comtb).State = EntityState.Added;
                comtb.EntryDate = DateTime.Now;
                comtb.Status = 1;
                db.SaveChanges();
                return RedirectToAction("login");
            }
            return View(comtb);
        }
        public ActionResult login()
        {
            if (Session["comid"] != null)
            {
                return RedirectToAction("Index", "Company");
            }
            return View();
        }
        [HttpPost]
        public ActionResult login(Company_tb comtblogin)
        {
            try
            {
                var a = db.Company_tb.Where(l => l.UserName == comtblogin.UserName && l.Password == comtblogin.Password).SingleOrDefault();
                if (a != null)
                {
                    Session["comid"] = a.CID.ToString();
                    //ViewBag.jstatus = "not applied";
                    Session["companyname"] = a.CNAME.ToString();

                    return RedirectToAction("index","Company");
                }
                else
                {
                    TempData["msg"] = "Invalid User Name or Password";
                    return RedirectToAction("login", "Company");
                }
            }
            catch (Exception ex)
            {
                ViewBag.s = ex.ToString();
                return RedirectToAction("login", "Company");
            }
        }

        public ActionResult AddJobPosting()
        {
            ViewBag.showjobcat = new SelectList(db.job_categorytb, "job_cat_id", "job_cat_name");
            ViewBag.showjoblocation = new SelectList(db.Job_location, "JobLocation_ID", "Location_Name");

            return View();
        }
        [HttpPost]
        public ActionResult AddJobPosting(PostJob_tb pstb, DateTime dates)
        {
            if(ModelState.IsValid)
            {
                db.Entry(pstb).State = EntityState.Added;
                pstb.Company_ID = Convert.ToInt32(Session["comid"]);
                pstb.JobStatus = 0;
                pstb.EntryDate = DateTime.Now;
                pstb.LastApplyDate = dates;
                db.SaveChanges();
                TempData["success"] = "Job Post Successfully Done";
                return RedirectToAction("AddJobPosting");
            }
            return View(pstb);
        }

        public ActionResult ViewJobPosting()
        {
            int CompanyID = Convert.ToInt32(Session["comid"]);
            var ShowPostedJob = db.PostJob_tb.Where(x => x.Company_ID == CompanyID).ToList();
            return View(ShowPostedJob);
        }


      
        public ActionResult CheckJobSeeker()
        {
            int idd = Convert.ToInt32(Session["comid"]);
            var p = db.ApplyJob_tb.Where(U => U.Company_ID == idd).ToList();
            ViewBag.p = p;
            return View();
        }
        public ActionResult CheckJobSeekerdata(int id)
        {
            var p = db.jobseeker_tb.Where(U => U.JS_ID == id).ToList();
            var i = db.Education_tb.Where(U => U.Job_seeker_ID == id).ToList();
            ViewBag.i = i;
            ViewBag.p = p;
            
            return View();
        }

        public ActionResult CompanyInfo()
        {
            int CompanyID = Convert.ToInt32(Session["comid"]);
            var checkcompany_info = db.Company_tb.Where(x => x.CID == CompanyID).ToList();
            return View(checkcompany_info);
        }

        public ActionResult companyfullDetailDetail(int id)
        {
            var companyDetails = db.Company_tb.Find(id);
            return View(companyDetails);
        }



        // GET: Company/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Company/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Company/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Company/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Company/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Company/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        //logout
        public ActionResult logout()
        {
            Session.Abandon();
            Session.Remove("comid");
            Session.RemoveAll();

            return RedirectToAction("login","Company");
        }
    }
}
