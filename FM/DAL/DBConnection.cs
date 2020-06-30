using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace FM.DAL
{
    class DBConnection
    {
        
        private SQLiteConnectionStringBuilder stringBuilder = new SQLiteConnectionStringBuilder();

        private static DBConnection instance = null;
        public static DBConnection Instance
        {
            get
            {
                if (instance == null)
                    instance = new DBConnection();

                return instance;
            }
        }

        private DBConnection() 
        {
            stringBuilder.DataSource = Properties.Settings.Default.path;
        }

        public SQLiteConnection Connection => new SQLiteConnection(stringBuilder.ToString());

        public void SetDatabase(string path)
        {
            stringBuilder.DataSource = path;
        }
    }
}
