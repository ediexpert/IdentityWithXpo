using System;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using System.Configuration;

public static class XpoHelper
{
    private static string ConnectionStringName;
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
                        fDataLayer = GetDataLayer(ConnectionStringName);
                    }
                }
            }
            return fDataLayer;
        }
    }

    public static IDataLayer GetDataLayer(string connectionStringName)
    {
        ConnectionStringName = connectionStringName;
        XpoDefault.Session = null;
        string conn = ConfigurationManager.ConnectionStrings[ConnectionStringName].ConnectionString;
        conn = XpoDefault.GetConnectionPoolString(conn);
        XPDictionary dict = new ReflectionDictionary();
        IDataStore store = XpoDefault.GetConnectionProvider(conn, AutoCreateOption.SchemaAlreadyExists);
        dict.GetDataStoreSchema(System.Reflection.Assembly.GetExecutingAssembly());
        IDataLayer dl = new ThreadSafeDataLayer(dict, store);
        return dl;
    }
}