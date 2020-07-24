using IdentityWithXpoLatest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityWithXpoLatest.ViewModels
{
    public class UserViewModel : BaseViewModel<XpoApplicationUser>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public override void GetData(XpoApplicationUser model)
        {
            model.UserName = UserName;
            model.Email = Email;
        }
    }
}