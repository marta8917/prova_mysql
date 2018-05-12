using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImprimisCore;

namespace Imprimis_WForms
{
    class ComboBoxTextStringSettings
    {
        public ComboBox Combo { get; }
   
        public ComboBoxTextStringSettings(ComboBox combo)
        {
            Combo = combo;
        }

        //public void AddDropList(object sender, EventArgs e)
        //{
        //    Combo.Items.Add("Starts");
        //    Combo.Items.Add("Ends");
        //    Combo.Items.Add("Contains");
        //    Combo.Items.Add("Equal");
        //}

        public void SetDropList()
        {
            List<string> lista = new List<string> { "Contains", "Starts", "Ends", "Equal" };
            Combo.DataSource = lista;
        }

        public StringTextFilterMode GetValue()
        {
            StringTextFilterMode mode = StringTextFilterMode.Contains;//Defalt== contains
            var emailValue = (string)Combo.SelectedValue;
            switch (emailValue)
            {
                case "Starts":
                    mode = StringTextFilterMode.Starts;
                    break;
                case "Ends":
                    mode = StringTextFilterMode.Ends;
                    break;

                //case "Contains":
                //    mode = StringTextFilterMode.Contains;
                //    break;

                case "Equals":
                    mode = StringTextFilterMode.Equal;
                    break;
            }

            return mode;
        }




    }
}