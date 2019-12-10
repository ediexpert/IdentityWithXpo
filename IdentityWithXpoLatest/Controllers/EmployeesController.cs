using DevExpress.Web.Mvc;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace IdentityWithXpoLatest.Controllers
{
    public class EmployeesController : BaseXpoController<Models.Employee>
    {

        // GET: Employees

        private IEnumerable<ViewModels.EmployeeViewModel> GetEmployees()
        {
            return (from c in XpoSession.Query<Models.Employee>().ToList()
                    select new ViewModels.EmployeeViewModel() { ID = c.Oid, Name = c.Name, DateOfBirth = c.DateOfBirth, Age = c.Age }).ToList();
        }
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {

            return PartialView("_GridViewPartial", GetEmployees());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] ViewModels.EmployeeViewModel item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Save(item);
                    // Insert here a code to insert the new item in your model
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", GetEmployees());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] ViewModels.EmployeeViewModel item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to update the item in your model
                    Save(item);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("_GridViewPartial", GetEmployees());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] ViewModels.EmployeeViewModel item)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    // Insert here a code to delete the item from your model
                    Delete(item);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("_GridViewPartial", GetEmployees());
        }
    }
}