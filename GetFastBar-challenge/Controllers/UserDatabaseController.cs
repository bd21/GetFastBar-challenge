using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GetFastBar_challenge.Controllers
{
    public class UserDatabaseController : Controller
    {
        // GET: UserDatabase
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserDatabase/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserDatabase/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserDatabase/Create
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

        // GET: UserDatabase/Edit/5
        public ActionResult Edit(string ID, string customerID, string cardID, 
                                 string last4, string company, DateTime expiry)
        {
            var originalMovie = (from m in _db.MovieSet
                                 where m.Id == movieToEdit.Id
                                 select m).First();

            if (!ModelState.IsValid)
                return View(originalMovie);
            _db.ApplyPropertyChanges(originalMovie.EntityKey.EntitySetName, movieToEdit);
            _db.SaveChanges();

            return RedirectToAction("Index");


            return View();
        }

        // POST: UserDatabase/Edit/5
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

        // GET: UserDatabase/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserDatabase/Delete/5
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
    }
}
