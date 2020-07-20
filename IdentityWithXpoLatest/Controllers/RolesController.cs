using DevExpress.Web.Mvc;
using DevExpress.Xpo;
using IdentityWithXpoLatest.Models;
using IdentityWithXpoLatest.Persistent;
using IdentityWithXpoLatest.ViewModels;
using IdentityWithXpoLatest.XPO;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentityWithXpoLatest.Controllers
{
    public class RolesController : BaseXpoController<Product>
    {
        private RoleManager<ApplicationRole> roleManager;
        public RolesController()
        {

        }
        public RolesController(RoleManager<ApplicationRole> roleMgr)
        {
            roleManager = roleMgr;
        }
        IEnumerable<RoleViewModel> GetRoles()
        {
            return (from c in XpoSession.Query<XpoApplicationRole>().ToList()
                    select new RoleViewModel() {ID = c.Id ,Name = c.Name }).ToList();
        }

        void SaveRole(ApplicationRole applicationRole)
        {
            XpoApplicationRole role = XpoSession.GetObjectByKey<XpoApplicationRole>(applicationRole.ID);
            if(role == null)
                role = new XpoApplicationRole(XpoSession);
            role.Name = applicationRole.Name;
            XpoSession.CommitChanges();
        }
        // GET: Roles
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            ViewData["roles"] = roleManager?.Roles;
            return PartialView("_GridViewPartial", GetRoles());
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialAddNew([ModelBinder(typeof(DevExpressEditorsBinder))] ApplicationRole role)
        {
            SaveRole(role);
            return PartialView("_GridViewPartial", GetRoles());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] ApplicationRole role)
        {
            SaveRole(role);
            return PartialView("_GridViewPartial", GetRoles());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] ApplicationRole role)
        {
            var appRole = XpoSession.GetObjectByKey<XpoApplicationRole>(role.ID);
            if(appRole != null)
            {
                XpoSession.Delete(appRole);
                XpoSession.CommitChanges();
            }
            return PartialView("_GridViewPartial", GetRoles());
        }
    }
}