using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityWithXpoLatest.Models;

namespace IdentityWithXpoLatest.ViewModels
{
    public class EmployeeViewModel : BaseViewModel<Models.Employee>
    {
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }
        public override void GetData(Employee model)
        {
            model.Name = Name;
            model.DateOfBirth = DateOfBirth;
        }
    }
}