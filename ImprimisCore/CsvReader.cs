using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ImprimisCore
{
    public class CsvReader
    {
        public static string[] SplitCSV(string input)
        {
            Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);
            List<string> list = new List<string>();
            string curr = null;
            foreach (Match match in csvSplit.Matches(input))
            {
                curr = match.Value;
                if (0 == curr.Length)
                {
                    list.Add("");
                }

                list.Add(curr.TrimStart(','));
            }

            return list.ToArray();
        }

        public User ReadUser(string csvRow)
        {
            var stringUserList = SplitCSV(csvRow);
            var user = new User();
            user.Email = stringUserList[0]?.Trim('"');
            user.Phone = stringUserList[1]?.Trim('"');
            user.Name = stringUserList[2]?.Trim('"');
            user.Counter = int.Parse(stringUserList[3]);
            user.Notes = stringUserList[4]?.Trim('"');
            user.TimeStamp = DateTime.Parse(stringUserList[5]);
            return user;
        }

        public List<User> ReadUsers(List<string> csvRows)
        {
            var users = new List<User>();
            foreach (var row in csvRows)
                users.Add(ReadUser(row));
            return users;
        }

    }

}
