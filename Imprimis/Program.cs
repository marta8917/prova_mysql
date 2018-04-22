using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System;
using ImprimisCore;

namespace Imprimis
{
    class Program
    {
        static void Main(string[] args)
        {
            // STEP 1: QUERY SU DATABASE CON FILTRO IN INPUT E CREAZIONE DI UN FILE CSV CON I DATI ESTRATTI ///////////////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            Console.WriteLine("Ciao Utente, scrivi un filtro rispetto al quale estrarre i dati!");
           
            var filter = UserFilterProvider.GetMySqlFilter();
            //UserRepository usa la stringa (condizione) del filtro per recuperare l'elenco degli users risultanti e restituisce una lista di user
            var userRepo1 = new UserRepository();
            var usersList = userRepo1.GetUsers(filter);

            var csvWriter = new CsvWriter();
            // Ogni user rappresenta di fatto una riga del csv che devo scrivere quindi converto la lista di user in una lista di string
            var userListString = csvWriter.GetCsvRows(usersList);

            var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "marta_users.csv");
            using (var file = System.IO.File.CreateText(filePath))//using = dispose (chiude)
            {
                foreach (var row in userListString)
                {
                    file.WriteLine(row);
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


            // STEP 2: A PARTIRE DL FILE CSV, RISCRIVO UN DATABASE CON I RISULTATI DELLE QUERY INSERITE DALL-UTENTE ///////////////////////////////////////
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            var csvReader = new CsvReader();
            var csvRows = new List<string>();
            List<User> readUsers;
            using (var file = System.IO.File.OpenText(filePath))
            {
                var line = file.ReadLine();
                while (line != null)
                {
                    csvRows.Add(line);
                    line = file.ReadLine();
                }
                readUsers = csvReader.ReadUsers(csvRows);
            }

            //dalla lista di user ora aggiungo al repository un tabella 
            var userRepo2 = new UserRepository();
            userRepo2.SetNewUsersTable(readUsers);
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////



        }
    }
}
