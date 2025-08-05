using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventoryTracker.Serializer
{
    public interface ISQLiteSerializable
    {
        string TableName { get; }
        Dictionary<string, object> ToSqlParams();
        void FromSqlReader(SQLiteDataReader reader);
    }
}
