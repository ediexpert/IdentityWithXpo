using DevExpress.Data.Filtering;
using DevExpress.Web.Mvc;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentityWithXpoLatest.Controllers
{
    public class UserController : BaseXpoController
    {
        private static DX.Data.Xpo.XpoDatabase db;
        private static UnitOfWork uow;
        // GET: User
        private void InitUnitOfwork()
        {
            if (db == null)
                db = DomainObjects.ApplicationDbContext.Create();
            if (uow == null)
                uow = db.GetUnitOfWork();
        }
        public UserController()
        {
            InitUnitOfwork();
        }
        public ActionResult Index()
        {
            InitUnitOfwork();
            return View();
        }



        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            InitUnitOfwork();
            var model = new XPCollection<DomainObjects.XpoApplicationUser>(uow);
            return PartialView("_GridViewPartial", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] DX.Data.Xpo.Identity.XPIdentityUser item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to insert the new item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] DX.Data.Xpo.Identity.XPIdentityUser item)
        {
            var model = new object[0];
            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", model);
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete(System.String ID)
        {
            InitUnitOfwork();
            var model = new XPCollection<DomainObjects.XpoApplicationUser>(uow);
            if (ID != null)
            {
                try
                {
                    var user = uow.FindObject<DomainObjects.XpoApplicationUser>(CriteriaOperator.Parse("[Id] == ?", ID));
                    if (user != null)
                    {
                        uow.Delete(user);
                        uow.CommitChanges();
                    }

                    // Insert here a code to delete the item from your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GridViewPartial", model);
        }
    }
}