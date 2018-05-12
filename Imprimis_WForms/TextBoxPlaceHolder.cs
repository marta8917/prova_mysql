using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Imprimis_WForms
{
    class TextBoxPlaceHolder
    {
        public TextBox TxtBox { get; }
        public string PlaceHolderText { get; }//Testo del segna posto della text box

        public TextBoxPlaceHolder(TextBox textBox, string placeHolderText)
        {
            TxtBox = textBox;
            PlaceHolderText = placeHolderText;
        }


        public void AddText(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(TxtBox.Text))
            {
                TxtBox.Text = PlaceHolderText;
                //TxtBox.ForeColor QUI METTERE IL COLORE GRIGINO CHIARO
            }
                
            
        }

        public void RemovePlaceHolderText(object sender, EventArgs e)
        {

            TxtBox.Text = "";
            //RIPRISTIARE COLORE NERO 
        }

        public void Set()
        {
            if (!TxtBox.Focused)
                AddText(null, null);//se non ho il focus devo scrivere il place holder
            TxtBox.GotFocus += RemovePlaceHolderText;
            TxtBox.LostFocus += AddText;
            
        }

        public string GetValue()
        {
            if (TxtBox.Text == PlaceHolderText)
                return "";
            else
                return TxtBox.Text;
        }
    }
}
