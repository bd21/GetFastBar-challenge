using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Stripe;
using Microsoft.AspNet.Identity;

namespace GetFastBar_challenge.Controllers
{
    public class AspNetUserController : Controller
    {
        private CorrectModel db = new CorrectModel();
        private string cardId;
        private string stripeCustomerId;


        // GET: AspNetUser
        public ActionResult Index()
        {
            return View(db.AspNetUsers.ToList());
        }

        // GET: AspNetUser/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // GET: AspNetUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AspNetUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,StripeCustomerId,StripeCardId,CreditCardLastFour,CreditCardCompany,CreditCardExpires")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspNetUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspNetUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCustomer(string stripeEmail, string stripeToken)
        {
            //create customer with stripeToken
            var myCustomer = new StripeCustomerCreateOptions();
            myCustomer.Email = stripeEmail;
            myCustomer.SourceToken = stripeToken;

            //get customerID
            var customerService = new StripeCustomerService();
            StripeCustomer stripeCustomer = customerService.Create(myCustomer);
            stripeCustomerId = stripeCustomer.Id;

            //get cardId from stripeCustomer
            cardId = stripeCustomer.DefaultSourceId;

            //get card information from cardId
            var myCard = new StripeCardCreateOptions();
            myCard.SourceToken = cardId;

            var cardService = new StripeCardService();
            StripeCard stripeCard = cardService.Get(stripeCustomerId, cardId);


            //somehow save the customerID, cardID, CC4, company name, expiry object into the database
            string customerID = stripeCustomer.Id;
            string cardID = stripeCustomer.DefaultSourceId;
            string cc4 = stripeCard.Last4;
            string company = stripeCard.Brand;
            int month = Convert.ToInt32(stripeCard.ExpirationMonth);
            int year = Convert.ToInt32(stripeCard.ExpirationYear);
            DateTime expiry = new DateTime(year, month, 1);

            var currentUserID = User.Identity.GetUserId();
            this.Edit((String)currentUserID, customerID, cardID, cc4, company, expiry);
            TempData["notice"] = "Card successfully added!";
            return RedirectToAction("Index", "Manage");

        }
            // GET: AspNetUser/Edit/5
            public ActionResult Edit(string id, string customerID, string cardID, string CC4, 
                                 string companyName, DateTime expiration)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            //right here is where we are updating the database
            AspNetUser user = db.AspNetUsers.Find(id);
            user.StripeCustomerId = customerID;
            user.StripeCardId = cardID;
            user.CreditCardLastFour = CC4;
            user.CreditCardCompany = companyName;
            user.CreditCardExpires = expiration;
            db.SaveChanges();

            return View(aspNetUser);
        }

        // POST: AspNetUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,StripeCustomerId,StripeCardId,CreditCardLastFour,CreditCardCompany,CreditCardExpires")] AspNetUser aspNetUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aspNetUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aspNetUser);
        }

        // GET: AspNetUser/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUser);
        }

        // POST: AspNetUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            db.AspNetUsers.Remove(aspNetUser);
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
