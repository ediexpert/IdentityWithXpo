using DX.Data.Xpo.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityWithXpoLatest.ViewModels
{
    public class RoleViewModel 
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public void GetData(XPIdentityRole model)
        {
            model.Name = Name;            
        }
    }
}