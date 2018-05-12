using ImprimisCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Imprimis_WForms
{
    class ComboBoxDateTimeSettings
    {
        private ComboBox Combo { get; }
       

        public ComboBoxDateTimeSettings(ComboBox dateTime)
        {
            Combo = dateTime;
        }


        public void SetDropList()
        {
            List<string> lista = new List<string> { "At", "Before", "After" };
            Combo.DataSource = lista;
        }

        public DateTimeFilterMode GetValue()
        {
            DateTimeFilterMode mode = DateTimeFilterMode.At; //Defalt == at
            var dateTimeValue = Combo.SelectedValue;
            switch (dateTimeValue)
            {
                case "Before":
                    mode = DateTimeFilterMode.Before;
                    break;
                case "After":
                    mode = DateTimeFilterMode.After;
                    break;
                    //case "At":
                    //    mode = DateTimeFilterMode.At;
                    //    break;
            }

            return mode;
        }

    }
}
