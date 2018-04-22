using System;
using System.Collections.Generic;

namespace ImprimisCore
{
    public interface ISqlFilter
    {
        string GetMySqlString();
    }


    public enum StringTextFilterMode
    {
        Starts,
        Ends,
        Contains,
        Equal
    }

    public enum IntFilterMode
    {
        Minor,
        Equal,
        Major
    }

    public enum DateTimeFilterMode
    {
        After,
        Before,
        At
    }


    public class StringTextFilter : ISqlFilter
    {
        private string _columnName; //E' il campo della colonna che mi interessa filtrare
        private string _filterString; //E' il il valore che mi interessa cercare
        private StringTextFilterMode _filterMode;//E' la modalità di filtraggio (che inizia con, che finisce con, che contiene, che è esattamente uguale)

        //Uso il costurttore pubblico di default
        public StringTextFilter(string column, string filterstr, StringTextFilterMode mode)
        {
            _columnName = column;
            _filterString = filterstr;
            _filterMode = mode;
        }
        //Get (la set tengo privata)
        public string ColumnName { get => _columnName; }
        public string FilterString { get => _filterString; }
        public StringTextFilterMode FilterMode { get => _filterMode; }

        //Torna solo la stringa del filtro (condizione del WHERE)
        public string GetMySqlString()
        {
            var str = "";
            switch (FilterMode)
            {
                case StringTextFilterMode.Starts:
                    str = $"{ColumnName} LIKE '{FilterString}%'";
                    break;
                case StringTextFilterMode.Ends:
                    str = $"{ColumnName} LIKE '%{FilterString}'"; 
                    break;
                case StringTextFilterMode.Contains:
                    str = $"{ColumnName} LIKE '%{FilterString}%'"; 
                    break;
                case StringTextFilterMode.Equal:
                    str = $"{ColumnName} = '{FilterString}'"; 
                    break;
            }

            return str;
        }
    }


    public class IntFilter : ISqlFilter
    {
        private string _columnName;
        private int _filterInt;
        private IntFilterMode _filterMode;

        public IntFilter(string column, int filterint, IntFilterMode filtermode)
        {
            _columnName = column;
            _filterInt = filterint;
            _filterMode = filtermode;
        }

        public string ColumnName { get => _columnName; }
        public int FilterInt { get => _filterInt; }
        public IntFilterMode FilterMode { get => _filterMode; }

        //todo
        public string GetMySqlString()
        {
            var str = "";
            switch (FilterMode)
            {
                case IntFilterMode.Minor:
                    str = ColumnName + " < " + FilterInt;
                    break;
                case IntFilterMode.Equal:
                    str = ColumnName + " = " + FilterInt;
                    break;
                case IntFilterMode.Major:
                    str = ColumnName + " > " + FilterInt;
                    break;
            }
            return str;
        }
    }



  

    public class DateTimeFilter : ISqlFilter
    {
        private string _columnName;
        private DateTime _filterDateTime;
        private DateTimeFilterMode _filterMode;


        public DateTimeFilter(string column, DateTime filterdatetime, DateTimeFilterMode mode)
        {
            _columnName = column;
            _filterDateTime = filterdatetime;
            _filterMode = mode;
        }
        //Get (la set tengo privata)
        public string ColumnName { get => _columnName; }
        public DateTime FilterDateTime { get => _filterDateTime; }
        public DateTimeFilterMode FilterMode { get => _filterMode; }

        //todo
        public string GetMySqlString()
        {
            var str = "";
            var dateFormat = "yyyyMMdd HH:mm:ss";
            switch (FilterMode)
            {
                case DateTimeFilterMode.Before:
                    str = $"{ColumnName} < '{FilterDateTime.ToString(dateFormat)}'";
                    break;
                case DateTimeFilterMode.At:
                    str = ColumnName + " = " + "'" + FilterDateTime.ToString(dateFormat) + "'";
                    break;
                case DateTimeFilterMode.After:
                    str = $"{ColumnName} > '{FilterDateTime.ToString(dateFormat)}'";
                    break;
            }
            return str;
        }
    }



    public class AndCompositeFilter : ISqlFilter
    {
        private ISqlFilter[] _filters;

        public AndCompositeFilter(params ISqlFilter[] filters)
        {
            _filters = filters;
        }

        //Get
        public ISqlFilter[] Filters { get => _filters; }

        public string GetMySqlString()
        {
            var strs = new List<string>();
            foreach (var filter in Filters)
            {
                strs.Add(filter.GetMySqlString());
            }
            return string.Join(" AND ", strs);
        }
    }


    public class OrCompositeFilter : ISqlFilter
    {
        private ISqlFilter[] _filters;

        public OrCompositeFilter(params ISqlFilter[] filters)
        {
            _filters = filters;
        }

        //Get
        public ISqlFilter[] Filters { get => _filters; }

        public string GetMySqlString()
        {
            var strs = new List<string>();
            foreach (var filter in Filters)
            {
                strs.Add(filter.GetMySqlString());
            }
            return string.Join(" OR ", strs);
        }
    }


    //WHERE 1 = NO FILTRI 
    public class NoFilter : ISqlFilter
    {
        public string GetMySqlString()
        {
            return " 1 ";
        }
    }

}
