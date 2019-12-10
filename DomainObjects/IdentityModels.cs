using DevExpress.Xpo;
using DX.Data.Xpo.Identity;
using DX.Data.Xpo.Identity.Persistent;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DomainObjects
{
    public class ApplicationUser : XPIdentityUser<string>
    {
        public ApplicationUser() { }
        // use below code to add more properties
        public string ProfilePictureSrcset { get; set; }
        public string FacebookProfilePage { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class XpoApplicationUser : XpoDxUser
    {
        public XpoApplicationUser(Session session) : base(session)
        {
        }
        // use below code to add more properties
        private string _ProfilePictureSrcset;
        public string ProfilePictureSrcset
        {
            get => _ProfilePictureSrcset;
            set => SetPropertyValue(nameof(ProfilePictureSrcset), ref _ProfilePictureSrcset, value);
        }

        private string _FacebookProfilePage;
        public string FacebookProfilePage
        {
            get => _FacebookProfilePage;
            set => SetPropertyValue(nameof(FacebookProfilePage), ref _FacebookProfilePage, value);
        }
    }
    public class ApplicationUserMapper : XPUserMapper<ApplicationUser, XpoApplicationUser>
    {
        public override XpoApplicationUser Assign(ApplicationUser source, XpoApplicationUser destination)
        {
            var result = base.Assign(source, destination);
            // use below sample to assign properties
            result.FacebookProfilePage = source.FacebookProfilePage;
            result.ProfilePictureSrcset = source.ProfilePictureSrcset;
            return result;
        }

        public override string Map(string sourceField)
        {
            return base.Map(sourceField);
        }

        public override Func<XpoApplicationUser, ApplicationUser> CreateModel => base.CreateModel;
    }

    public class ApplicationDbContext
    {
        public static string ConnectionString = "DefaultConnection";
        public static DX.Data.Xpo.XpoDatabase Create()
        {
            return new DX.Data.Xpo.XpoDatabase(ConnectionString);
        }
    }
}
