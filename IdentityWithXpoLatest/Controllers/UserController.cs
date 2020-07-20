using DevExpress.Data.Filtering;
using DevExpress.DataProcessing;
using DevExpress.Web.Mvc;
using DevExpress.Xpo;
using DX.Data.Xpo.Identity;
using IdentityWithXpoLatest.Models;
using IdentityWithXpoLatest.Persistent;
using IdentityWithXpoLatest.ViewModels;
using IdentityWithXpoLatest.XPO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentityWithXpoLatest.Controllers
{
    public class UserController : BaseXpoController<Product>
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public UserController()
        {

        }
        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }
        IEnumerable<UserViewModel> GetUsers()
        {

            return (from c in XpoSession.Query<XpoApplicationUser>().ToList()
                    select new UserViewModel() { UserName = c.UserName, Email = c.Email}).ToList();
        }
        IEnumerable<RoleViewModel> GetRoles()
        {

            return (from c in XpoSession.Query<XpoApplicationRole>().ToList()
                    select new RoleViewModel() { Name = c.Name }).ToList();
        }

        public ActionResult Index()
        {
           
            return View();
        }



        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {           
            ViewData["roles"] = GetRoles();
            return PartialView("_GridViewPartial", GetUsers());
        }
        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialUpdate([ModelBinder(typeof(DevExpressEditorsBinder))] UserViewModel model)
        {
            var appUser = UserManager.FindByEmail(model.Email);
            IdentityResult result;
            if (appUser == null)
            {
                result = CreateUser(new ApplicationUser { UserName = model.UserName, Email = model.Email }, model.RoleName);    
            }
            else
            {
                result = UpdateUser(appUser, model.RoleName);
            }
            if (!result.Succeeded)
            {
                ViewData["Errors"] = result.Errors;
            }
            return PartialView("_GridViewPartial", GetUsers());
        }

        private IdentityResult CreateUser(ApplicationUser appUser, string role)
        {
            var res = UserManager.Create(appUser);
            if (!res.Succeeded)
                return res;
            return UserManager.AddToRole(appUser.Id, role);
        }
        private IdentityResult UpdateUser(ApplicationUser appUser, string role)
        {
            ApplicationUser user = UserManager.FindByEmail(appUser.Email);
            if (user == null)
                return CreateUser(appUser, role);
            user.Email = appUser.Email;
            user.UserName = appUser.UserName;
            var result = UserManager.Update(user);
            if (!result.Succeeded)
                return result;
            foreach (var r in user.Roles)
            {
                UserManager.RemoveFromRole(user.Id, r.Name);
            }

            return UpdateRole(user.Id, role);
        }

        private IdentityResult UpdateRole(string userId, string roleName)
        {
            var user = UserManager.FindById(userId);
            RemovePreviousRoles(user);
            //if role does not exist then add the role
            XpoApplicationRole role = XpoSession.Query<XpoApplicationRole>()?.FirstOrDefault(x => x.Name == roleName);
            if (role == null)
            {
                role = new XpoApplicationRole(XpoSession);
                role.Name = roleName;
                XpoSession.CommitChanges();
            }
                
            return UserManager.AddToRole(user.Id, roleName);

        }

        private void RemovePreviousRoles(ApplicationUser user)
        {
            if (user.Roles.Count < 1)
                return;

            foreach (var role in user.Roles)
            {
                UserManager.RemoveFromRole(user.Id, role.Name);
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] UserViewModel model)
        {
            ApplicationUser user = UserManager.FindByEmail(model.Email);
            UserManager.Delete(user);
            return PartialView("_GridViewPartial", GetUsers());
        }
    }
}