using ImprimisCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Imprimis_WForms
{
    public partial class Form1 : Form
    {
        private ComboBoxTextStringSettings _comboTextSettings;
        private ComboBoxIntSettings _comboIntSettings;
        private ComboBoxDateTimeSettings _comboDateTimeSettings;
        private TextBoxPlaceHolder _textBoxPlaceHolderEmail;
        private TextBoxPlaceHolder _textBoxPlaceHolderPhone;
        private TextBoxPlaceHolder _textBoxPlaceHolderName;
        private TextBoxPlaceHolder _textBoxPlaceHolderNotes;



        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            _textBoxPlaceHolderEmail = new TextBoxPlaceHolder(txtEmail, "Inserisci la mail o parte di essa");
            _textBoxPlaceHolderEmail.Set();
            _textBoxPlaceHolderPhone = new TextBoxPlaceHolder(txtPhone, "Inserisci il  telefono o parte di esso");
            _textBoxPlaceHolderPhone.Set();
            _textBoxPlaceHolderName = new TextBoxPlaceHolder(txtName, "Inserisci il nome o parte di esso");
            _textBoxPlaceHolderName.Set();
            _textBoxPlaceHolderNotes = new TextBoxPlaceHolder(txtNotes, "Inserisci le note");
            _textBoxPlaceHolderNotes.Set();


            _comboTextSettings = new ComboBoxTextStringSettings(comboEmail);
            _comboTextSettings.SetDropList();

            _comboTextSettings = new ComboBoxTextStringSettings(comboPhone);
            _comboTextSettings.SetDropList();

            _comboTextSettings = new ComboBoxTextStringSettings(comboName);
            _comboTextSettings.SetDropList();

            _comboIntSettings = new ComboBoxIntSettings(comboCounter);
            _comboIntSettings.SetDropList();

            _comboTextSettings = new ComboBoxTextStringSettings(comboNotes);
            _comboTextSettings.SetDropList();

            _comboDateTimeSettings = new ComboBoxDateTimeSettings(comboDateTime);
            _comboDateTimeSettings.SetDropList();

            dateTimePicker.Enabled = checkBox1.Checked;
        }

        private void btnCerca_Click(object sender, EventArgs e)
        {
            var filters = new List<ISqlFilter>();
            if (_textBoxPlaceHolderEmail.GetValue() != "")
            {
                filters.Add(new StringTextFilter("email", txtEmail.Text, _comboTextSettings.GetValue()));
            }
            if (_textBoxPlaceHolderPhone.GetValue() != "")
            {
                filters.Add(new StringTextFilter("phone", txtPhone.Text, _comboTextSettings.GetValue()));
            }
            if (_textBoxPlaceHolderName.GetValue() != "")
            {
                filters.Add(new StringTextFilter("name", txtName.Text, _comboTextSettings.GetValue()));
            }
            if (numericUpDown1.Value != 0)
            {
                int val = (int)numericUpDown1.Value;
                filters.Add(new IntFilter("counter", val, _comboIntSettings.GetValue()));
            }
            if (_textBoxPlaceHolderNotes.GetValue() != "")
            {
                filters.Add(new StringTextFilter("notes", txtNotes.Text, _comboTextSettings.GetValue()));
            }
            if (checkBox1.Checked)
            {
                filters.Add(new DateTimeFilter("timestamp", dateTimePicker.Value, _comboDateTimeSettings.GetValue()));
            }




            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // SELEZIONE FILEPATH
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = "Csv files|*.csv";
            saveFileDialog.ShowDialog();
            var filePath = saveFileDialog.FileName;


            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // SCRIVO
            var filter = new AndCompositeFilter(filters.ToArray());
            var userRepo1 = new UserRepository();
            var usersList = userRepo1.GetUsers(filter);
            var csvWriter = new CsvWriter();
            var userListString = csvWriter.GetCsvRows(usersList);

            using (var file = System.IO.File.CreateText(filePath))//using = dispose (chiude)
            {
                foreach (var row in userListString)
                {
                    file.WriteLine(row);
                }
            }

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker.Enabled = checkBox1.Checked;
        }
    }
}
