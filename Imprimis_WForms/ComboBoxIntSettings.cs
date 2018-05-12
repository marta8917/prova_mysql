using ImprimisCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Imprimis_WForms
{
    class ComboBoxIntSettings
    {
        public ComboBox Combo { get; }

        public ComboBoxIntSettings(ComboBox combo)
        {
            Combo = combo;
        }

        public void SetDropList()
        {
            List<string> lista = new List<string> { "Equal", "Minor", "Major" };
            Combo.DataSource = lista;
        }

        public IntFilterMode GetValue()
        {
            IntFilterMode mode = IntFilterMode.Equal; //Defalt == equal
            var counterValue = Combo.SelectedValue;
            switch (counterValue)
            {
                case "Minor":
                    mode = IntFilterMode.Minor;
                    break;
                case "Major":
                    mode = IntFilterMode.Major;
                    break;
                //case "Equal":
                //    mode = IntFilterMode.Equal;
                //    break;
            }

            return mode;
        }

    }
}
