using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ImprimisCore
{
    public class UserRepository
    {
        public List<User> GetUsers(ISqlFilter filter)
        {
            //connestion string --> database è sul mio pc
            string connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                //Chiamo la dispose (using) per tutti i comandi disposable (chiude connection col database e il reader)
                using (var command = connection.CreateCommand())
                {
                    var userFilter = filter.GetMySqlString();
                    command.CommandText = "SELECT * FROM users WHERE " + userFilter;

                    using (var reader = command.ExecuteReader())
                    {
                        var users = new List<User>();

                        while (reader.Read())//è improntato sulla riga 0 quindi il while scorre le righe finchè ci sono righe
                        {
                            var user = new User();
                            user.Email = reader.GetString("email");
                            user.Phone = reader.GetString("phone");
                            user.Name = reader.GetString("name");
                            user.Counter = reader.GetInt32("counter");
                            //Solo il campo notes della tabella è nullable
                            var ordinal = reader.GetOrdinal("notes");
                            if (!reader.IsDBNull(ordinal))
                                user.Notes = reader.GetString(ordinal);
                            user.TimeStamp = reader.GetDateTime("timestamp");
                            users.Add(user);
                        }

                        return users;
                    }
                }
            }
        }

        private static string GetConnectionString()
        {
            MySqlConnectionStringBuilder conn_string = new MySqlConnectionStringBuilder();
            conn_string.Server = "localhost";
            conn_string.UserID = "marta";
            conn_string.Password = "marcello55";
            conn_string.Database = "db_marta";
            conn_string.SslMode = MySqlSslMode.None;
            string connectionString = conn_string.ConnectionString;
            return connectionString;
        }


        //Crea la tabella e la svuota ad ogni nuova chiamata
        const string CreateNewTableScript = @"
DROP TABLE IF EXISTS `users_new`;

CREATE TABLE `users_new` (
  `email` varchar(100) NOT NULL,
  `phone` varchar(100) NOT NULL,
  `name` varchar(100) NOT NULL,
  `counter` int(11) NOT NULL,
  `notes` text,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
";


        //Faccio la Insert 
        const string InsertScript = @"
INSERT INTO `db_marta`.`users_new` (
  `email`,
  `phone`,
  `name`,
  `counter`,
  `notes`,
  `timestamp`
)
VALUES
  (
    @email,
    @phone,
    @name,
    @counter,
    @notes,
    @timestamp
  );";


        public void SetNewUsersTable(List<User> users)
        {
            string connectionString = GetConnectionString();
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                //Creo tabella
                using (var command = connection.CreateCommand())
                {

                    command.CommandText = CreateNewTableScript;
                    command.ExecuteNonQuery();
                }

                foreach (var user in users)
                {
                    using (var command = connection.CreateCommand())
                    {
                        //INSERT
                        command.CommandText = InsertScript;
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@phone", user.Phone);
                        command.Parameters.AddWithValue("@name", user.Name);
                        command.Parameters.AddWithValue("@counter", user.Counter);
                        command.Parameters.AddWithValue("@notes", user.Notes);
                        command.Parameters.AddWithValue("@timestamp", user.TimeStamp);
                        command.ExecuteNonQuery();
                    }
                }




            }
        }
    }
}