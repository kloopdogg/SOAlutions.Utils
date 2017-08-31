using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Service
{
    public class BaseRepository
    {
        static bool IsFactorySet = false;
        static BaseRepository()
        {
            if (!IsFactorySet)
            {
                DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
                IsFactorySet = true;
            }
        }
    }
}
