using MySqlConnector;
using System.Data;
using System.Data.Common;

namespace ProjectBase.Database.Connection
{
    public class DatabaseNonAsyncSelectReader
    {
        public static DataTable GetDataTable(DbCommand command)
        {
            // DataTable item;
            DataSet dataSet = new DataSet();
            using (
                MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter(command as MySqlCommand)
            )
            {
                dataSet = new DataSet();
                mySqlDataAdapter.Fill(dataSet, "table");
                DataTable table = new DataTable();
                dataSet.Tables.Add(table);
                return dataSet.Tables[0];
            }
        }
    }
}
