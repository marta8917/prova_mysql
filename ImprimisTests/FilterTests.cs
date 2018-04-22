using ImprimisCore;
using NUnit.Framework;
using System;

namespace ImprimisTests
{
    [TestFixture]
    public class FilterTests
    {
        //string
        [Test]
        public void when_requesting_starting_with_string_returns_correct_sql_string()
        {
            var filter = new StringTextFilter("email", "0", StringTextFilterMode.Starts);
            var result = filter.GetMySqlString();
            Assert.That(result, Is.EqualTo("email LIKE '0%'"));
        }

        [Test]
        public void when_requesting_ending_with_string_returns_correct_sql_string()
        {
            var filter = new StringTextFilter("email", ".com", StringTextFilterMode.Ends);
            var result = filter.GetMySqlString();
            Assert.That(result, Is.EqualTo("email LIKE '%.com'"));
        }



        [Test]
        public void when_requesting_exact_string_returns_correct_sql_string()
        {
            var filter = new StringTextFilter("email", "l.sileoni@inwind.it", StringTextFilterMode.Equal);
            var result = filter.GetMySqlString();
            Assert.That(result, Is.EqualTo("email = 'l.sileoni@inwind.it'"));
           
        }


        [Test]
        public void when_requesting_contains_string_returns_correct_sql_string()
        {
            var filter = new StringTextFilter("phone", "2", StringTextFilterMode.Contains);
            var result = filter.GetMySqlString();
            Assert.That(result, Is.EqualTo("phone LIKE '%2%'"));

        }

        //int
        [Test]
        public void when_requesting_minor_int_returns_correct_sql_string()
        {
            var filter = new IntFilter("counter", 2, IntFilterMode.Minor);
            var result = filter.GetMySqlString();
            Assert.That(result, Is.EqualTo("counter < 2"));
        }


        [Test]
        public void when_requesting_equal_int_returns_correct_sql_string_bis()
        {
            var filter = new IntFilter("counter", 1, IntFilterMode.Equal);
            var result = filter.GetMySqlString();
            Assert.That(result, Is.EqualTo("counter = 1"));
        }

        [Test]
        public void when_requesting_major_int_returns_correct_sql_string_bis()
        {
            var filter = new IntFilter("counter", 1, IntFilterMode.Major);
            var result = filter.GetMySqlString();
            Assert.That(result, Is.EqualTo("counter > 1"));
        }

        //Text
        [Test]
        public void when_requesting_contains_text_returns_correct_sql_string()
        {
            var filter = new StringTextFilter("notes", "a", StringTextFilterMode.Contains);
            var result = filter.GetMySqlString();
            Assert.That(result, Is.EqualTo("notes LIKE '%a%'"));

        }


        [Test]
        public void when_requesting_equal_text_returns_correct_sql_string()
        {
            var filter = new StringTextFilter("notes", "", StringTextFilterMode.Equal);
            var result = filter.GetMySqlString();
            Assert.That(result, Is.EqualTo("notes = ''"));

        }


        //DateTime
        [Test]
        public void when_requesting_at_datetime_returns_correct_sql_string()
        {
            var datetime = new DateTime(2018, 04, 16, 13, 24, 32);
            var filter = new DateTimeFilter("timestamp", datetime, DateTimeFilterMode.At);
            var result = filter.GetMySqlString();
            Assert.That(result, Is.EqualTo("timestamp = '20180416 13:24:32'"));

        }

        [Test]
        public void when_requesting_before_datetime_returns_correct_sql_string()
        {
            var datetime = new DateTime(2018, 04, 16, 13, 24, 32);
            var filter = new DateTimeFilter("timestamp", datetime, DateTimeFilterMode.Before);
            var result = filter.GetMySqlString();
            Assert.That(result, Is.EqualTo("timestamp < '20180416 13:24:32'"));

        }


        [Test]
        public void when_requesting_after_datetime_returns_correct_sql_string()
        {
            var datetime = new DateTime(2018, 04, 16, 13, 24, 32);
            var filter = new DateTimeFilter("timestamp", datetime, DateTimeFilterMode.After);
            var result = filter.GetMySqlString();
            Assert.That(result, Is.EqualTo("timestamp > '20180416 13:24:32'"));

        }


        //CompositeFilters
        [Test]
        public void when_requesting_after_datetime_and_equal_mail_returns_correct_sql_string()
        {
            var datetime = new DateTime(2018, 04, 16, 13, 24, 32);
            ISqlFilter[] isqlFilter = { new DateTimeFilter("timestamp", datetime, DateTimeFilterMode.After),
                                        new StringTextFilter("email", "l.sileoni@inwind.it", StringTextFilterMode.Equal)};

            var andCompositeFilter = new AndCompositeFilter(isqlFilter);
            var result = andCompositeFilter.GetMySqlString();
            Assert.That(result, Is.EqualTo("timestamp > '20180416 13:24:32' AND email = 'l.sileoni@inwind.it'"));
        }


        [Test]
        public void when_requesting_before_datetime_or_contains_string_or_major_int_returns_correct_sql_string()
        {
            var datetime = new DateTime(2018, 04, 16, 13, 24, 32);

            ISqlFilter[] isqlFilter = { new DateTimeFilter("timestamp", datetime, DateTimeFilterMode.Before),
                                        new StringTextFilter("phone", "2", StringTextFilterMode.Contains),
                                        new IntFilter("counter", 1, IntFilterMode.Major)};

            var orCompositeFilter = new OrCompositeFilter(isqlFilter);//in realtà il costruttore prende params, quindi basta l'elenco senza costruirsiper forza l'array
            var result = orCompositeFilter.GetMySqlString();
            Assert.That(result, Is.EqualTo("timestamp < '20180416 13:24:32' OR phone LIKE '%2%' OR counter > 1"));
        }


        [Test]
        public void when_requesting_before_datetime_or_contains_string_and_major_int_returns_correct_sql_string()
        {
            var datetime = new DateTime(2018, 04, 16, 13, 24, 32);

            ISqlFilter[] isqlFilter = { new DateTimeFilter("timestamp", datetime, DateTimeFilterMode.Before),
                                        new StringTextFilter("phone", "2", StringTextFilterMode.Contains)};

            var orCompositeFilter = new OrCompositeFilter(isqlFilter);
            var andFilter = new AndCompositeFilter(orCompositeFilter, new IntFilter("counter", 1, IntFilterMode.Major));
            var result = andFilter.GetMySqlString();
            Assert.That(result, Is.EqualTo("timestamp < '20180416 13:24:32' OR phone LIKE '%2%' AND counter > 1"));
        }




    }
}
