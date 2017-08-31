using Microsoft.Practices.EnterpriseLibrary.Data;
using SOAlutions.Utils.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Test.Service.DataAccess
{
    public interface ISampleRepository
    {
        string GetSampleText(int requestNumber);
        string GetSampleTextFromDatabase(int id);
        IEnumerable<TestObject> GetSampleCollectionFromDatabase();
    }

    public class SampleRepository : BaseRepository, ISampleRepository
    {
        public string GetSampleText(int requestNumber)
        {
            return String.Format("SampleRepository received {0} requests", requestNumber);
        }

        public string GetSampleTextFromDatabase(int id)
        {
            Database db = DatabaseFactory.CreateDatabase("Database1");
            DbCommand cmd = db.GetStoredProcCommand("TestProcedure");
            db.AddInParameter(cmd, "Id", DbType.Int32, id);

            TestObject sample = db.CreateObject<TestObject>(cmd, GenerateTestObjectFromReader);
            return sample.Value;
        }

        public IEnumerable<TestObject> GetSampleCollectionFromDatabase()
        {
            Database db = DatabaseFactory.CreateDatabase("Database1");
            DbCommand cmd = db.GetStoredProcCommand("TestProcedure");

            var samples = db.CreateCollection<TestObject>(cmd, GenerateTestObjectFromReader);
            return samples;
        }

        private TestObject GenerateTestObjectFromReader(IDataReader reader)
        {
            TestObject testObj = null;
            if (reader.Read())
            {
                testObj = new TestObject()
                {
                    ID = reader.GetInt32("Id"),
                    Value = reader.GetString("TestData"),
                };
            }
            return testObj;
        }
    }    
}