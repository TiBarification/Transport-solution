using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3_Transport
{
    public partial class FormMain : Form
    {
        enum EMODE
        {
            NONE = 0,
            EDITOR,
            SOLVE
        }

        public event EventHandler EMODEChanged;
        public delegate void EventHandler(object sender, EventArgs e);
        private EMODE GLOBALMODE;
        

        public FormMain()
        {
            InitializeComponent();
            GLOBALMODE = EMODE.NONE;

            EMODEChanged += FormMain_EMODEChanged;
        }

        private void FormMain_EMODEChanged(object sender, EventArgs e)
        {
            if (sender != null)
            {
                if (GLOBALMODE == EMODE.EDITOR)
                {
                    toolStripButton_SolveTransport.Enabled = true;
                    dataGridView_Solve.ReadOnly = false;
                }
                else if (GLOBALMODE == EMODE.SOLVE)
                {
                    dataGridView_Solve.ReadOnly = true;
                    toolStripButton_SolveTransport.Enabled = false;
                }
            }
        }

        private void toolStripButton_New_Click(object sender, EventArgs e)
        {
            FormInput fiForm = new FormInput(ref dataGridView_Solve);
            fiForm.FormClosed += FiForm_FormClosed;
            fiForm.ShowDialog();
        }

        private void FiForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if ((sender as FormInput).isOK)
            {
                GLOBALMODE = EMODE.EDITOR;
                EMODEChanged?.Invoke(this, new EventArgs());
            }
        }

        private void toolStripButton_Clear_Click(object sender, EventArgs e)
        {
            // TODO Clear datagrid
            dataGridView_Solve.Rows.Clear();
            dataGridView_Solve.Columns.Clear();

            GLOBALMODE = EMODE.NONE;
            EMODEChanged?.Invoke(this, new EventArgs());

            dataGridView_Solve.Refresh();
        }

        private void toolStripButton_SolveTransport_Click(object sender, EventArgs e)
        {
            if (!GridEditorFilled()) return;
            TryCloseTransportSolvation();

            // Enter in solver mode
            GLOBALMODE = EMODE.SOLVE;
            EMODEChanged?.Invoke(this, new EventArgs());

            // Here our solvation will be
            CSolveTable csTable = new CSolveTable();
            csTable.FillTable(dataGridView_Solve);
            dataGridView_Solve.Columns.Clear();
            dataGridView_Solve.Rows.Clear();
            dataGridView_Solve.Refresh();
            dataGridView_Solve.ReadOnly = true;
            csTable.FormColumnsOnGrid(ref dataGridView_Solve);
            csTable.Solve();
        }

        private bool GridEditorFilled()
        {
            DataGridViewRowCollection dgvrc = dataGridView_Solve.Rows;
            bool bIsIgnored = false;
            foreach (DataGridViewRow dgvr in dgvrc)
            {
                DataGridViewCellCollection dgvcc = dgvr.Cells;

                foreach (DataGridViewCell dgvc in dgvcc)
                {
                    if (dgvc.ColumnIndex == 0 || (dgvr.Index == dgvrc.Count-1 && dgvc.ColumnIndex == dgvcc.Count-1))
                        bIsIgnored = true;
                    string text = (string)dgvc.EditedFormattedValue;
                    if (!bIsIgnored)
                    {
                        if (!Regex.IsMatch(text, @"^\d+$"))
                        {
                            MessageBox.Show("Some values have innopropriate symbols");
                            return false;
                        }
                        else if (String.IsNullOrEmpty(text) || String.IsNullOrWhiteSpace(text))
                        {
                            MessageBox.Show("Not enough cells was filled");
                            return false;
                        }
                    }
                    bIsIgnored = false;
                }
            }

            //MessageBox.Show("All required cells are filled");
            return true;
        }

        private void TryCloseTransportSolvation()
        {
            int iLastRowPos = dataGridView_Solve.Rows.Count - 1;
            int iLastCellInLastRowPos = dataGridView_Solve.Rows[iLastRowPos].Cells.Count - 1;
            DataGridViewCell dgvc_Last = dataGridView_Solve.Rows[iLastRowPos].Cells[iLastCellInLastRowPos];

            // Sum A
            int iSum_A = 0;
            string data;
            for (int i = 0; i < iLastRowPos; ++i)
            {
                data = (string)dataGridView_Solve[iLastCellInLastRowPos, i].Value;
                iSum_A += Convert.ToInt32(data);
            }

            int iSum_B = 0;
            for (int i = 1; i < iLastCellInLastRowPos; ++i)
            {
                data = (string)dataGridView_Solve[i, iLastRowPos].Value;
                iSum_B += Convert.ToInt32(data);
            }

            //MessageBox.Show("A Summ = " + iSum_A.ToString() + "\r\nB Summ = " + iSum_B.ToString());
            if (iSum_A < iSum_B)
            {
                int iColumns = dataGridView_Solve.Columns.Count;

                string[] values = new string[iColumns];
                for (int i = 0; i < iColumns; ++i)
                {
                    int iNum;
                    if (i == 0)
                    {
                        iNum = iLastRowPos + 1;
                        values[i] = "A" + iNum.ToString();
                    }
                    else if (i == iColumns - 1) // if it is a last item in current row
                    {
                        iNum = iSum_B - iSum_A;
                        values[i] = iNum.ToString();
                    }
                    else
                    {
                        values[i] = "0";
                    }
                }

                dataGridView_Solve.Rows.Insert(iLastRowPos, values);

                MessageBox.Show("Добавлен фиктивный поставщик", "Фиктивный поставщик", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (iSum_A > iSum_B)
            {
                DataGridViewTextBoxColumn dgvc = new DataGridViewTextBoxColumn();
                int iNum = iLastCellInLastRowPos + 1;
                dgvc.HeaderText = "B" + iNum.ToString();
                dgvc.Name = "B" + iNum.ToString();
                dgvc.DefaultCellStyle.NullValue = "0";
                dgvc.ReadOnly = true;
                dataGridView_Solve.Columns.Insert(iLastCellInLastRowPos, dgvc);

                dataGridView_Solve[iLastCellInLastRowPos, iLastRowPos].Value = (iSum_A - iSum_B).ToString();

                MessageBox.Show("Добавлен фиктивный потребитель", "Фиктивный потребитель", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Make datagridview uneditable
            dataGridView_Solve.ReadOnly = true;
        }

        private void toolStripButton_MyVar_Click(object sender, EventArgs e)
        {
            DataGridViewTextBoxColumn dgvc = new DataGridViewTextBoxColumn();
            dgvc.HeaderText = "";
            dgvc.Name = "";
            dgvc.ReadOnly = true;
            dataGridView_Solve.Columns.Add(dgvc);

            int iBuyersCount = 6;
            int iSellersCount = 5;

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
                sValues = new string[iBuyersCount + 2];
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
            dataGridView_Solve.ReadOnly = true;

            dataGridView_Solve[1, 0].Value = "2";
            dataGridView_Solve[1, 1].Value = "7";
            dataGridView_Solve[1, 2].Value = "8";
            dataGridView_Solve[1, 3].Value = "2";
            dataGridView_Solve[1, 4].Value = "6";
            dataGridView_Solve[1, 5].Value = "23";

            dataGridView_Solve[2, 0].Value = "8";
            dataGridView_Solve[2, 1].Value = "6";
            dataGridView_Solve[2, 2].Value = "9";
            dataGridView_Solve[2, 3].Value = "4";
            dataGridView_Solve[2, 4].Value = "9";
            dataGridView_Solve[2, 5].Value = "12";

            dataGridView_Solve[3, 0].Value = "3";
            dataGridView_Solve[3, 1].Value = "2";
            dataGridView_Solve[3, 2].Value = "4";
            dataGridView_Solve[3, 3].Value = "6";
            dataGridView_Solve[3, 4].Value = "7";
            dataGridView_Solve[3, 5].Value = "24";

            dataGridView_Solve[4, 0].Value = "7";
            dataGridView_Solve[4, 1].Value = "2";
            dataGridView_Solve[4, 2].Value = "11";
            dataGridView_Solve[4, 3].Value = "7";
            dataGridView_Solve[4, 4].Value = "9";
            dataGridView_Solve[4, 5].Value = "32";

            dataGridView_Solve[5, 0].Value = "2";
            dataGridView_Solve[5, 1].Value = "7";
            dataGridView_Solve[5, 2].Value = "5";
            dataGridView_Solve[5, 3].Value = "8";
            dataGridView_Solve[5, 4].Value = "3";
            dataGridView_Solve[5, 5].Value = "28";

            dataGridView_Solve[6, 0].Value = "6";
            dataGridView_Solve[6, 1].Value = "9";
            dataGridView_Solve[6, 2].Value = "3";
            dataGridView_Solve[6, 3].Value = "4";
            dataGridView_Solve[6, 4].Value = "5";
            dataGridView_Solve[6, 5].Value = "22";

            dataGridView_Solve[7, 0].Value = "25";
            dataGridView_Solve[7, 1].Value = "34";
            dataGridView_Solve[7, 2].Value = "32";
            dataGridView_Solve[7, 3].Value = "44";
            dataGridView_Solve[7, 4].Value = "55";

            GLOBALMODE = EMODE.EDITOR;
            EMODEChanged?.Invoke(this, new EventArgs());

            dataGridView_Solve.Refresh();
        }
    }
}
