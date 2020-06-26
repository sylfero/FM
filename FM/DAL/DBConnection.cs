using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FM.DAL
{
    class DBConnection
    {
        private MySqlConnectionStringBuilder stringBuilder = new MySqlConnectionStringBuilder();

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

        public MySqlConnection connection => new MySqlConnection(stringBuilder.ToString());

        private DBConnection()
        {
            stringBuilder.UserID = Properties.Settings.Default.userID;
            stringBuilder.Password = Properties.Settings.Default.paswd;
            stringBuilder.Server = Properties.Settings.Default.server;
            stringBuilder.Database = Properties.Settings.Default.database;
            stringBuilder.Port = Properties.Settings.Default.port;
        }
    }
}
