using ImprimisCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Imprimis
{
    static class UserFilterProvider
    {
        public enum UserFilterProviderColumn
        {
            Email,
            Phone,
            Name,
            Counter,
            Notes,
            Timestamp,
            All, // tutti in OR --> ho immaginato una ricerca generica del tipo: cerca ovunque questa parola (in and non mi sembrava abbastanza realistica)
            None //no WHERE
        }

        public static UserFilterProviderColumn GetFilterColumnString()
        {
            Console.WriteLine("Inserisci il filtro per:");
            UserFilterProviderColumn? col = null;//converto l'enum in nullable
            while (col == null)
            {
                Console.WriteLine("1. Email");
                Console.WriteLine("2. Phone");
                Console.WriteLine("3. Name");
                Console.WriteLine("4. Counter");
                Console.WriteLine("5. Notes");
                Console.WriteLine("6. Timestamp");
                Console.WriteLine("7. Filtra su tutti i campi testuali(OR)");//qui andrebbero gestiti meglio i filtri compositi
                Console.WriteLine("8. Non filtrare");
                var key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case '1':
                        col = UserFilterProviderColumn.Email;
                        break;
                    case '2':
                        col = UserFilterProviderColumn.Phone;
                        break;
                    case '3':
                        col = UserFilterProviderColumn.Name;
                        break;
                    case '4':
                        col = UserFilterProviderColumn.Counter;
                        break;
                    case '5':
                        col = UserFilterProviderColumn.Notes;
                        break;
                    case '6':
                        col = UserFilterProviderColumn.Timestamp;
                        break;
                    case '7':
                        col = UserFilterProviderColumn.All;
                        break;
                    case '8':
                        col = UserFilterProviderColumn.None;
                        break;

                }
            }
            return col.Value;
        }


        public static ISqlFilter SqlTextStringFilter(UserFilterProviderColumn columnFilter)
        {
            string column;
            switch (columnFilter)
            {
                case UserFilterProviderColumn.Email:
                    Console.WriteLine(" Inserisci l'email o parte di essa:");
                    column = "email";
                    break;
                case UserFilterProviderColumn.Phone:
                    Console.WriteLine(" Inserisci il telefono o parte di esso:");
                    column = "phone";
                    break;
                case UserFilterProviderColumn.Name:
                    Console.WriteLine(" Inserisci il nome o parte di esso:");
                    column = "name";
                    break;
                case UserFilterProviderColumn.Notes:
                    Console.WriteLine(" Inserisci le note o parte di esse:");
                    column = "notes";
                    break;
                default: throw new Exception();

            }


            var filterValue = Console.ReadLine();
            StringTextFilterMode? mode = null;//non è più un enum ma un oggetto che ha un enum ma che può essere nullo
            while (mode == null)
            {
                Console.WriteLine("1. Contains");
                Console.WriteLine("2. Starts");
                Console.WriteLine("3. End");
                Console.WriteLine("4. Match esatto");
                var key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case '1':
                        mode = StringTextFilterMode.Contains;
                        break;
                    case '2':
                        mode = StringTextFilterMode.Starts;
                        break;
                    case '3':
                        mode = StringTextFilterMode.Ends;
                        break;
                    case '4':
                        mode = StringTextFilterMode.Equal;
                        break;
                }
            }

            var filter = new StringTextFilter(column, filterValue.Trim(), mode.Value);
            return filter;

        }

        public static ISqlFilter SqlIntFilter(UserFilterProviderColumn columnFilter)
        {
            string column = "counter";
            int? filterValue = null;
            while (!filterValue.HasValue)
            {
                Console.WriteLine(" Inserisci il counter:");
                var str = Console.ReadLine();
                if (int.TryParse(str, out var value))
                {
                    filterValue = value;
                }
            }
            
            IntFilterMode? mode = null;//non è più un enum ma un oggetto che ha un enum ma che può essere nullo
            while (mode == null)
            {
                Console.WriteLine("1. minore");
                Console.WriteLine("2. uguale");
                Console.WriteLine("3. maggiore");
                var key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case '1':
                        mode = IntFilterMode.Minor;
                        break;
                    case '2':
                        mode = IntFilterMode.Equal;
                        break;
                    case '3':
                        mode = IntFilterMode.Major;
                        break;
                }
            }

            var filter = new IntFilter(column, filterValue.Value, mode.Value);
            return filter;
        }

        public static ISqlFilter SqlDateTimeFilter(UserFilterProviderColumn columnFilter)
        {
            string column = "timestamp";
            DateTime? filterValue = null;
            while (!filterValue.HasValue)
            {
                Console.WriteLine(" Inserisci la data in formato standard:");
                var str = Console.ReadLine();
                if (DateTime.TryParse(str, out var value))
                {
                    filterValue = value;
                }
            }
            DateTimeFilterMode? mode = null;//non è più un enum ma un oggetto che ha un enum ma che può essere nullo
            while (mode == null)
            {
                Console.WriteLine("1. prima");
                Console.WriteLine("2. ora esatta");
                Console.WriteLine("3. dopo");
                var key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case '1':
                        mode = DateTimeFilterMode.Before;
                        break;
                    case '2':
                        mode = DateTimeFilterMode.At;
                        break;
                    case '3':
                        mode = DateTimeFilterMode.After;
                        break;
                }
            }

            var filter = new DateTimeFilter(column, filterValue.Value, mode.Value);
            return filter;
        }

        public static ISqlFilter SqlNoFilter()
        {
            return new NoFilter();
        }

        //Dovrò fare dei metodi per gestire in qualche modo tutte le combo dei filtri
        public static ISqlFilter SqlAllOrCompositeFilter()
        {
            Console.WriteLine(" Inserisci il testo che vuoi cercare:");
            var filterValue = Console.ReadLine();
         
            var email = new StringTextFilter("email", filterValue.Trim(), StringTextFilterMode.Contains);
            var phone = new StringTextFilter("phone", filterValue.Trim(), StringTextFilterMode.Contains);
            var name = new StringTextFilter("name", filterValue.Trim(), StringTextFilterMode.Contains);
            var notes = new StringTextFilter("notes", filterValue.Trim(), StringTextFilterMode.Contains);
            var orCompositeFilter = new OrCompositeFilter(email, phone, name, notes);
            return orCompositeFilter;
        }




        public static ISqlFilter GetMySqlFilter()
        {
            ISqlFilter sqlFilter;
            var columnFilter = GetFilterColumnString();
            switch (columnFilter)
            {
                case UserFilterProviderColumn.Email:
                    sqlFilter = SqlTextStringFilter(columnFilter);
                    break;
                case UserFilterProviderColumn.Phone:
                    sqlFilter = SqlTextStringFilter(columnFilter);
                    break;
                case UserFilterProviderColumn.Name:
                    sqlFilter = SqlTextStringFilter(columnFilter);
                    break;
                case UserFilterProviderColumn.Counter:
                    sqlFilter = SqlIntFilter(columnFilter);
                    break;
                case UserFilterProviderColumn.Notes:
                    sqlFilter = SqlTextStringFilter(columnFilter);
                    break;
                case UserFilterProviderColumn.Timestamp:
                    sqlFilter = SqlDateTimeFilter(columnFilter);
                    break;
                case UserFilterProviderColumn.All:
                    sqlFilter = SqlAllOrCompositeFilter();
                    break;
                case UserFilterProviderColumn.None:
                    sqlFilter = SqlNoFilter();
                    break;
                default:throw new Exception();


            }
            return sqlFilter;
        }

    }



    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    }
