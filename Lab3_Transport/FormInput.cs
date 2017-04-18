using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3_Transport
{
    public partial class FormInput : Form
    {
        private DataGridView dataGridView_Solve;

        private uint iSellersCount;
        private uint iBuyersCount;

        public bool isOK = false;

        public FormInput()
        {
            InitializeComponent();
        }

        public FormInput(ref DataGridView dataGridView_Solve)
        {
            InitializeComponent();
            this.dataGridView_Solve = dataGridView_Solve;
        }

        private void button_Click_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(maskedTextBox_ACount.Text) || String.IsNullOrEmpty(maskedTextBox_BCount.Text))
                return;
            iSellersCount = Convert.ToUInt32(maskedTextBox_ACount.Text);
            iBuyersCount = Convert.ToUInt32(maskedTextBox_BCount.Text);

            //MessageBox.Show("iSellersCount: " + iSellersCount + "\r\niBuyersCount: " + iBuyersCount);

            DataGridViewTextBoxColumn dgvc = new DataGridViewTextBoxColumn();
            dgvc.HeaderText = "";
            dgvc.Name = "";
            dgvc.ReadOnly = true;
            dataGridView_Solve.Columns.Add(dgvc);

            for (int i = 0; i < iBuyersCount; ++i)
            {
                int iNum = i + 1;
                string sText = "B" + iNum.ToString();
                dataGridView_Solve.Columns.Add(sText, sText);
            }
            
            dataGridView_Solve.Columns.Add("Запасы", "Запасы");
            dataGridView_Solve.RowHeadersVisible = false;

            string[] sValues;
            // Draw first rows
            for (int i = 0; i < iSellersCount; ++i)
            {
                int iNum = i + 1;
                sValues = new string[iBuyersCount+2];
                sValues[0] = "A" + iNum.ToString();
                for (int j = 1; j < iBuyersCount + 2; ++j)
                    sValues[j] = "";

                dataGridView_Solve.Rows.Add(sValues);
            }

            // Last row с потребностями
            sValues = new string[iBuyersCount + 2];
            sValues[0] = "Потребности";
            for (int i = 1; i < iBuyersCount + 1; ++i)
                sValues[i] = "";
            dataGridView_Solve.Rows.Add(sValues);

            // Make right bottom corner uneditable
            int iLastRowPos = dataGridView_Solve.Rows.Count - 1;
            int iLastCellInLastRowPos = dataGridView_Solve.Rows[iLastRowPos].Cells.Count - 1;
            DataGridViewCell dgvc_Last = dataGridView_Solve.Rows[iLastRowPos].Cells[iLastCellInLastRowPos];
            dgvc_Last.ReadOnly = true;
            dgvc_Last.Style.ForeColor = Color.Aqua;
            dgvc_Last.Style.BackColor = Color.LightGray;

            dataGridView_Solve.Refresh();

            isOK = true;
            this.Close();
        }
    }
}
