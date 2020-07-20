using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityWithXpoLatest.Persistent
{
    public class Product : XPObject
    {
        public Product(Session session) : base(session)
        {
        }
        private string _Name;
        public string Name
        {
            get => _Name;
            set => SetPropertyValue(nameof(Name), ref _Name, value);
        }
    }
}