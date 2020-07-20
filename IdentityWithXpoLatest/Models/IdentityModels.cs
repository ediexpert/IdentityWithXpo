using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using DX.Data.Xpo.Identity;
using DevExpress.Xpo;
using DX.Data.Xpo.Identity.Persistent;
using System;

namespace IdentityWithXpoLatest.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : XPIdentityUser<string>
    {
        public ApplicationUser() { }

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
        //public string FacebookProfilePage { get; set; }

        //public override void Assign(object source, int loadingFlags)
        //{
        //	base.Assign(source, loadingFlags);
        //	//ApplicationUser src = source as ApplicationUser;
        //	//if (src != null)
        //	//{
        //	//	// additional properties here
        //	//	this.PropertyA = src.PropertyA;
        //	//	// etc.				
        //	//}
        //}
    }

    public class ApplicationUserMapper : XPUserMapper<ApplicationUser, XpoApplicationUser>
    {
        public override XpoApplicationUser Assign(ApplicationUser source, XpoApplicationUser destination)
        {
            var result = base.Assign(source, destination);
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
        public static DX.Data.Xpo.XpoDatabase Create()
        {
            return new DX.Data.Xpo.XpoDatabase("DefaultConnection");
        }
    }
    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public ApplicationDbContext()
    //        : base("DefaultConnection", throwIfV1Schema: false)
    //    {
    //    }

    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }
    //}

    public class ApplicationRole : XPIdentityRole
    {
        public ApplicationRole()
        { }
    }
    [MapInheritance(MapInheritanceType.ParentTable)]
    public class XpoApplicationRole : XpoDxRole
    {
        public XpoApplicationRole(Session session) : base(session)
        {
        }
    }
    public class ApplicationRoleMapper : XPRoleMapper<string, ApplicationRole, XpoApplicationRole>
    {
        public override Func<XpoApplicationRole, ApplicationRole> CreateModel => base.CreateModel;

        public override XpoApplicationRole Assign(ApplicationRole source, XpoApplicationRole destination)
        {
            XpoApplicationRole result = base.Assign(source, destination);
            return result;
        }

        public override string Map(string sourceField)
        {
            return base.Map(sourceField);
        }
    }

}