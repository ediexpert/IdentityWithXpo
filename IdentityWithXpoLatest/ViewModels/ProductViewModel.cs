using IdentityWithXpoLatest.Persistent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityWithXpoLatest.ViewModels
{
    public class ProductViewModel : BaseViewModel<Product>
    {
        public string Name { get; set; }
        public override void GetData(Product model)
        {
            model.Oid = ID;
            model.Name = Name;
        }
    }
}