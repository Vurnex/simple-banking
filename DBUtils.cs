using System;
using System.Collections.Generic;
using System.Text;
using MySqlConnector;

namespace App440Project
{
    static class DBUtils
    {
        public static MySqlConnection CreateConnection()
        {
            String connStr = @"Connection Details Here";

            MySqlConnection conn = new MySqlConnection(connStr);

            return conn;

        }

        public static MySqlDataReader getAccounts(MySqlConnection conn)
        {
            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = @"SELECT *
                                FROM `Database1`.`Accounts`
                                WHERE User = 'John';";

            cmd.Connection = conn;

            MySqlDataReader rows = cmd.ExecuteReader();

            return rows;
        }

        public static MySqlDataReader getBills(MySqlConnection conn)
        {
            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = @"SELECT * FROM `Database1`.`Bills`;";

            cmd.Connection = conn;

            MySqlDataReader rows = cmd.ExecuteReader();

            return rows;
        }

        public static MySqlDataReader getCheckingTransactions(MySqlConnection conn)
        {
            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = @"SELECT * FROM Database1.CheckingTransactions;";

            cmd.Connection = conn;

            MySqlDataReader rows = cmd.ExecuteReader();

            return rows;
        }

        public static MySqlDataReader getSavingsTransactions(MySqlConnection conn)
        {
            MySqlCommand cmd = new MySqlCommand();

            cmd.CommandText = @"SELECT * FROM Database1.SavingsTransactions;";

            cmd.Connection = conn;

            MySqlDataReader rows = cmd.ExecuteReader();

            return rows;
        }


    }
}
