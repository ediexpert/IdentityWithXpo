using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityWithXpoLatest.XPO
{
    public class XpoConnectionHelper
    {
        static DX.Data.Xpo.XpoDatabase Database;
        public static void InitiateDataLayer(string connectionStringName)
        {
            Database = new DX.Data.Xpo.XpoDatabase(connectionStringName);
        }
        public static UnitOfWork GetNewUnitOfWork()
        {
            return Database.GetUnitOfWork();
        }
        
    }
}