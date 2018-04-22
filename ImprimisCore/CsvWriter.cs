using System.Collections.Generic;

namespace ImprimisCore
{
    public class CsvWriter
    {
        public string GetCsvRow(User user)
        {
            var str = "\"" + user.Email.Trim('"') + "\",\"" + user.Phone.Trim('"') + "\",\"" + user.Name.Trim('"') + "\"," + user.Counter + ",\"" + user.Notes?.Trim('"') + "\"," + user.TimeStamp; 
            return str;
        }

        public List<string> GetCsvRows(List<User> users)
        {
            var csvRows = new List<string>();
            foreach (var user in users)
                csvRows.Add(GetCsvRow(user));
            return csvRows;
        }
    }

   
    }
