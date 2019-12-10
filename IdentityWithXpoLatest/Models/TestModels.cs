using DevExpress.Xpo;
using System;

namespace IdentityWithXpoLatest.Models
{
    //[RuleCombinationOfPropertiesIsUnique("SampleRule", DefaultContexts.Save, "Description, Id")]
    //public class SampleClass : XPObject
    //{
    //    public SampleClass(Session session) : base(session) { }

    //    int id;
    //    public int Id
    //    {
    //        get => id;
    //        set => SetPropertyValue(nameof(Id), ref id, value);
    //    }

    //    string description;
    //    public string Description
    //    {
    //        get => description;
    //        set => SetPropertyValue(nameof(Description), ref description, value);
    //    }
    //}


    public class Employee : XPObject
    {
        public Employee(Session session) : base(session) { }
        private string _Name;
        public string Name
        {
            get => _Name;
            set => SetPropertyValue(nameof(Name), ref _Name, value);
        }


        private DateTime _DateOfBirth;
        public DateTime DateOfBirth
        {
            get => _DateOfBirth;
            set => SetPropertyValue(nameof(DateOfBirth), ref _DateOfBirth, value);
        }

        private int _Age;
        [NonPersistent]
        public int Age
        {
            get
            {
                var diff = DateTime.Now - DateOfBirth;
                _Age = (int)(diff.Days - diff.Days % 365) / 365;
                return _Age;
            }

        }

    }
}