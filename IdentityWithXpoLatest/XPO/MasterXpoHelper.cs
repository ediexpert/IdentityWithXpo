using DevExpress.Xpo;
using DevExpress.Xpo.DB;

namespace WaWiXPO.Helper.Master
{
    public static class MasterXpoHelper
    {
        static string _connectionString { get; set; }
        public static Session GetNewSession()
        {
            return new Session(DataLayer);
        }

        public static UnitOfWork GetNewUnitOfWork()
        {
            return new UnitOfWork(DataLayer);
        }

        private readonly static object lockObject = new object();

        static volatile IDataLayer fDataLayer;
        static IDataLayer DataLayer
        {
            get
            {
                if (fDataLayer == null)
                {
                    lock (lockObject)
                    {
                        if (fDataLayer == null)
                        {
                            fDataLayer = GetDataLayer();
                        }
                    }
                }
                return fDataLayer;
            }
        }

        private static IDataLayer GetDataLayer()
        {
            return XpoDefault.GetDataLayer(_connectionString, AutoCreateOption.DatabaseAndSchema);

            //Use below code when want to use WCF service for cache data store.
            //return XpoDefault.GetDataLayer("http://localhost:59370/DataService.svc", AutoCreateOption.DatabaseAndSchema);
        }

        public static void InitiateDataLayer(string connectionString)
        {
            if (fDataLayer == null)
            {
                lock (lockObject)
                {
                    if (fDataLayer == null)
                    {
                        _connectionString = connectionString;
                        fDataLayer = GetDataLayer();
                    }
                }
            }
        }
    }
}
