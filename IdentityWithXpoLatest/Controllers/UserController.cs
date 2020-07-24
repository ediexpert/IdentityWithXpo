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
                    select new UserViewModel() { 
                        UserName = c.UserName, 
                        Email = c.Email, 
                        Roles = c.Roles.Select(t => new RoleViewModel { ID = t.ID, Name = t.Name}).ToList() ,                       
                        RoleName = string.Join(", ", c.Roles.Select(t => t.Name).ToArray())
                    }).ToList();
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
            
            string[] selectedRoles = TokenBoxExtension.GetSelectedValues<string>("RoleName");
            var appUser = UserManager.FindByEmail(model.Email);
            List<IdentityResult> result;
            if (appUser == null)
            {
                result = CreateUser(new ApplicationUser { UserName = model.UserName, Email = model.Email }, selectedRoles);    
            }
            else
            {
                result = UpdateUser(appUser, selectedRoles);
            }
            if(result.Any(x => x.Succeeded == false))
            {
                foreach (var item in result.Where(x => x.Succeeded == false))
                {
                    ViewData["Errors"] += item.Errors + ","; 
                }
            }
            ViewData["roles"] = GetRoles();
            return PartialView("_GridViewPartial", GetUsers());
        }

        private List<IdentityResult> CreateUser(ApplicationUser appUser, string[] roles)
        {
            List<IdentityResult> results = new List<IdentityResult>();
            var res = UserManager.Create(appUser);
            if (!res.Succeeded)
            {
                results.Add(res);
                return results;
            }
            RemovePreviousRoles(appUser);
            foreach (var item in roles)
            {
                results.Add(UserManager.AddToRole(appUser.Id, item));
            }
            return results;
        }
        private List<IdentityResult> UpdateUser(ApplicationUser appUser, string[] roles)
        {
            List<IdentityResult> results = new List<IdentityResult>();
            ApplicationUser user = UserManager.FindByEmail(appUser.Email);
            if (user == null)
                return CreateUser(appUser, roles);
            user.Email = appUser.Email;
            user.UserName = appUser.UserName;
            var result = UserManager.Update(user);
            if (!result.Succeeded)
            {
                results.Add(result);
                return results;
            }

           RemovePreviousRoles(appUser);
            foreach (var item in roles)
            {
                results.Add(UpdateRole(user.Id, item.Trim()));
            }
            return results;
        }

        private IdentityResult UpdateRole(string userId, string roleName)
        {
            var user = UserManager.FindById(userId);
            //RemovePreviousRoles(user);
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
            var roles = UserManager.GetRoles(user.ID);
            if (roles.Count < 1)
                return;

            foreach (var role in roles)
            {
                UserManager.RemoveFromRole(user.Id, role);
            }
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult GridViewPartialDelete([ModelBinder(typeof(DevExpressEditorsBinder))] UserViewModel model)
        {
            ApplicationUser user = UserManager.FindByEmail(model.Email);
            UserManager.Delete(user);
            ViewData["roles"] = GetRoles();
            return PartialView("_GridViewPartial", GetUsers());
        }
    }
}