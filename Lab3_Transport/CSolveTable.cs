using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3_Transport
{
    public class CSolveTable
    {
        private DataGridView dgv;

        private List<List<CTransportCell>> lTable;
        private List<uint> lStorage;
        private List<uint> lNeeds;
        private int?[] lUKoef;
        private int?[] lVKoef;

        private bool bOptimalPlan;

        private List<CTransportCell> lCorners;
        private List<CTransportCell> lPath;

        public CSolveTable()
        {
            lTable = new List<List<CTransportCell>>();
            lStorage = new List<uint>();
            lNeeds = new List<uint>();
            bOptimalPlan = false;
        }

        public void FillTable(DataGridView dataSolve)
        {
            DataGridViewRowCollection dataRowCollection = dataSolve.Rows;
            int iLastColumnIndex = dataSolve.Columns.Count - 1;
            int iPosX = 0;
            foreach (DataGridViewRow dgvr in dataRowCollection)
            {
                if (dgvr.Index == dataRowCollection.Count-1) // if it was a last row in datagrid, it contains needs
                {
                    // We need to exclude last column
                    foreach (DataGridViewCell dgvc in dgvr.Cells)
                    {
                        if (dgvc.ColumnIndex == 0) continue; // Skip first column of row
                        if (dgvc.ColumnIndex == iLastColumnIndex) break; // Just exit

                        lNeeds.Add(Convert.ToUInt32(dgvc.Value)); // All our needs filled
                    }
                }
                else
                {
                    List<CTransportCell> lTransportCells = new List<CTransportCell>();
                    int iPosY = 0;
                    foreach (DataGridViewCell dgvc in dgvr.Cells)
                    {
                        if (dgvc.ColumnIndex == 0) continue; // Skip first column of row
                        if (dgvc.ColumnIndex == iLastColumnIndex) // if it was a last column of row, we add those values to list of storage
                        {
                            lStorage.Add(Convert.ToUInt32(dgvc.Value));
                        }
                        else
                        {
                            // Else, we need to fill our List<List<CTransportCell>>
                            
                            CTransportCell cell = new CTransportCell(Convert.ToUInt32(dgvc.Value), iPosX, iPosY);
                            if (cell.iTariff == 0)
                                cell.bArtificial = true;
                            lTransportCells.Add(cell);
                            iPosY++;
                        }
                    }
                    lTable.Add(lTransportCells);
                }

                iPosX++;
            }
        }

        public void Solve()
        {
            ResourceAllocation();
            while (bOptimalPlan == false)
            {
                ClearAllCustomData();
                PotencialRedistribution();

                bOptimalPlan = true;
                List<CTransportCell> lNonOptimal = new List<CTransportCell>();
                foreach (List<CTransportCell> lCTC in lTable)
                {
                    foreach (CTransportCell ctc in lCTC)
                    {
                        if (ctc.iActualValue != null) continue;

                        int iPosX = ctc.iPosX;
                        int iPosY = ctc.iPosY;

                        int iRes = (int)lUKoef[iPosX] + (int)lVKoef[iPosY] - (int)ctc.iTariff;
                        if (iRes > 0)
                        {
                            ctc.iDiff = iRes;
                            lNonOptimal.Add(ctc);
                            if (bOptimalPlan)
                                bOptimalPlan = false;
                        }
                    }
                }

                if (bOptimalPlan) break;

                int iMax = lNonOptimal.Max(x => x.iDiff);
                var result = lNonOptimal.Select(x => x).Where(x => x.iDiff == iMax);
                CTransportCell startCell = result.OrderBy(x => x.iTariff).FirstOrDefault();
                startCell.bStarter = true;
                startCell.bCorner = true;
                startCell.bSign = true;
                lCorners.Add(startCell);

                // Check lCorners on actual info
                FindCorners();


                // After corners checked, we need to order and show path to them.
                FindPath();

                OutPutInDataGrid(true, true);

                // Change values, depending on +(true) or -(false) on bSign
                MakeChanges();
            }

            OutPutInDataGrid(true, false);
        }

        private void MakeChanges()
        {
            uint iMin = (uint)lCorners.Where(x => x.bSign == false).Min(x => x.iActualValue);
            foreach (CTransportCell ctc in lCorners)
            {
                if (ctc.bSign == true) // +
                {
                    if (ctc.iActualValue == null)
                        ctc.iActualValue = iMin;
                    else
                        ctc.iActualValue += iMin;

                    if (ctc.iActualValue == 0 || ctc.bZeroValue)
                    {
                        //ctc.iActualValue = null;
                        ctc.bZeroValue = false;
                        ctc.iActualValue = iMin;
                    }
                }
                else if (ctc.bSign == false) // -
                {
                    if (ctc.iActualValue == 0 || ctc.bZeroValue)
                    {
                        ctc.iActualValue = null;
                        ctc.bZeroValue = false;
                    }
                    else
                    {
                        ctc.iActualValue -= iMin;
                    }

                    if (ctc.iActualValue == 0)
                    {
                        ctc.iActualValue = null;
                        ctc.bZeroValue = false;
                    }
                }
            }

            // Clear all values 

            //ClearAllCustomData();
        }

        private void ClearAllCustomData()
        {
            foreach (List<CTransportCell> lCTC in lTable)
            {
                foreach (CTransportCell ctc in lCTC)
                {
                    ctc.ClearCustomInfo();
                }
            }
        }

        private void FindPath()
        {
            lPath = new List<CTransportCell>();
            lCorners = lCorners.OrderByDescending(x => x.bStarter).ToList();
            Stack<CTransportCell> sCTC = new Stack<CTransportCell>();
            sCTC.Push(lCorners[0]);

            while (lCorners.Any(x => x.bSign == null))
            {
                CTransportCell ctc = sCTC.Pop();
                sCTC.Push(ctc);

                int iPosX = ctc.iPosX;
                int iPosY = ctc.iPosY;

                // By rows
                CTransportCell ctcPartner = lCorners.Find(x => x.iPosX == iPosX && x.iPosY != iPosY);

                // Set sign for teammates on rows
                if (ctcPartner.bSign == null)
                {
                    if (ctc.bSign == true)
                        ctcPartner.bSign = false;
                    else if (ctc.bSign == false)
                        ctcPartner.bSign = true;
                    sCTC.Push(ctcPartner);
                }

                int iCount;
                if (iPosY < ctcPartner.iPosY)
                {
                    iCount = ctcPartner.iPosY - iPosY;
                    lPath.AddRange(lTable[iPosX].GetRange(iPosY, iCount));
                }
                else
                {
                    iCount = iPosY - ctcPartner.iPosY;
                    lPath.AddRange(lTable[iPosX].GetRange(ctcPartner.iPosY, iCount));
                }

                // By columns
                ctcPartner = lCorners.Find(x => x.iPosY == iPosY && x.iPosX != iPosX);

                // Set sign for teaammates on columns
                if (ctcPartner.bSign == null)
                {
                    if (ctc.bSign == true)
                        ctcPartner.bSign = false;
                    else if (ctc.bSign == false)
                        ctcPartner.bSign = true;
                    sCTC.Push(ctcPartner);
                }

                if (iPosX < ctcPartner.iPosX) // наша ячейка выше чем вторая
                {
                    for (int i = iPosX; i <= ctcPartner.iPosX; ++i)
                    {
                        lPath.Add(lTable[i][iPosY]);
                    }
                }
                else // наша ячейка ниже искомой
                {
                    for (int i = ctcPartner.iPosX; i <= iPosX; ++i)
                    {
                        lPath.Add(lTable[i][iPosY]);
                    }
                }
            }
            /*
            foreach (CTransportCell ctc in lCorners)
            {
                int iPosX = ctc.iPosX;
                int iPosY = ctc.iPosY;

                // By rows
                CTransportCell ctcPartner = lCorners.Find(x => x.iPosX == iPosX && x.iPosY != iPosY);
                
                // Set sign for teammates on rows
                if (ctcPartner.bSign == null)
                {
                    if (ctc.bSign == true)
                        ctcPartner.bSign = false;
                    else if (ctc.bSign == false)
                        ctcPartner.bSign = true;
                }

                int iCount;
                if (iPosY < ctcPartner.iPosY)
                {
                    iCount = ctcPartner.iPosY - iPosY;
                    lPath.AddRange(lTable[iPosX].GetRange(iPosY, iCount));
                }
                else
                {
                    iCount = iPosY - ctcPartner.iPosY;
                    lPath.AddRange(lTable[iPosX].GetRange(ctcPartner.iPosY, iCount));
                }

                // By columns
                ctcPartner = lCorners.Find(x => x.iPosY == iPosY && x.iPosX != iPosX);

                // Set sign for teaammates on columns
                if (ctcPartner.bSign == null)
                {
                    if (ctc.bSign == true)
                        ctcPartner.bSign = false;
                    else if (ctc.bSign == false)
                        ctcPartner.bSign = true;
                }

                if (iPosX < ctcPartner.iPosX) // наша ячейка выше чем вторая
                {
                    for (int i = iPosX; i <= ctcPartner.iPosX; ++i)
                    {
                        lPath.Add(lTable[i][iPosY]);
                    }
                }
                else // наша ячейка ниже искомой
                {
                    for (int i = ctcPartner.iPosX; i <= iPosX; ++i)
                    {
                        lPath.Add(lTable[i][iPosY]);
                    }
                }
            } */

            // Distinct in List
            lPath = lPath.Distinct().ToList();
            foreach (CTransportCell ctc in lPath)
            {
                // make it a path cell
                ctc.bIsPathCell = true;
            }
        }

        private void PotencialRedistribution()
        {
            int iIndexOfRowWithMaxValues = FindRowWithMaxValues(); // It finds index of row which contains maximum values
            // This row will be with potencial U = 0
            lUKoef = new int?[lStorage.Count];
            lVKoef = new int?[lNeeds.Count];

            lUKoef[iIndexOfRowWithMaxValues] = 0;
            for (int i = 0; i < lTable[iIndexOfRowWithMaxValues].Count; ++i)
            {
                uint? iValue = lTable[iIndexOfRowWithMaxValues][i].iActualValue;
                uint iTariff = lTable[iIndexOfRowWithMaxValues][i].iTariff;
                if (iValue != null)
                    lVKoef[i] = (int?)(iTariff);
            }

            // NEED TO REMAKE
            bool bMode = true;
            int iOldUKoefCount = lUKoef.Select(x => x).Where(y => y != null).ToList().Count;
            int iOldVKoefCount = lVKoef.Select(x => x).Where(y => y != null).ToList().Count;

            while (lUKoef.Any(x => x == null) || lVKoef.Any(x => x == null))
            {
                int iNewUKoefCount = iOldUKoefCount;
                int iNewVKoefCount = iOldVKoefCount;
                if (bMode) // Horizontal moving
                {
                    for (int i = 0; i < lVKoef.Length; ++i)
                    {
                        int? koef = lVKoef[i];
                        if (koef != null)
                        {
                            for (int j = 0; j < lTable.Count; ++j)
                            {
                                if (lTable[j][i].iActualValue != null)
                                {
                                    if (lUKoef[j] != null) continue;
                                    int iTariff = (int)(lTable[j][i].iTariff);
                                    int? UTariff = iTariff - koef;
                                    lUKoef[j] = UTariff;
                                    iNewUKoefCount++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < lUKoef.Length; ++i)
                    {
                        int? koef = lUKoef[i];
                        if (koef != null)
                        {
                            for (int j = 0; j < lTable[i].Count; ++j)
                            {
                                if (lTable[i][j].iActualValue != null)
                                {
                                    if (lVKoef[j] != null) continue;
                                    int iTariff = (int)(lTable[i][j].iTariff);
                                    int? VTariff = iTariff - koef;
                                    lVKoef[j] = VTariff;
                                    iNewVKoefCount++;
                                }
                            }
                        }
                    }
                }

                if (iOldUKoefCount == iNewUKoefCount && iOldVKoefCount == iNewVKoefCount)
                {
                    // черт, мы попали в цикл
                    // нужно найти пустые клетки, имеющие хотя бы один потенциал в строке/столбце
                    
                    Func<CTransportCell> GetFictiveCell = delegate ()
                    {
                        foreach (List<CTransportCell> lCTC in lTable)
                        {
                            foreach (CTransportCell cTC in lCTC)
                            {
                                if (cTC.iActualValue == null)
                                {
                                    if ((lUKoef[cTC.iPosX] == null && lVKoef[cTC.iPosY] != null) || (lUKoef[cTC.iPosX] != null && lVKoef[cTC.iPosY] == null))
                                    {
                                        return cTC;
                                    }
                                }
                            }
                        }

                        return null;
                    };

                    CTransportCell fictive = GetFictiveCell();

                    fictive.bZeroValue = true;
                    fictive.iActualValue = 0;
                    fictive.bCorner = true;
                    lCorners.Add(fictive);

                    int iTariff = (int)fictive.iTariff;
                    if (lUKoef[fictive.iPosX] == null)
                    {
                        lUKoef[fictive.iPosX] = iTariff - lVKoef[fictive.iPosY];
                        iNewUKoefCount++;
                        bMode = false;
                    }
                    else
                    {
                        lVKoef[fictive.iPosY] = iTariff - lUKoef[fictive.iPosX];
                        iNewVKoefCount++;
                        bMode = true;
                    }

                    iOldUKoefCount = iNewUKoefCount;
                    iOldVKoefCount = iNewVKoefCount;


                    continue;
                }

                iOldUKoefCount = iNewUKoefCount;
                iOldVKoefCount = iNewVKoefCount;

                bMode = !bMode;
            }
        }

        private int FindRowWithMaxValues()
        {
            List<int> lValues = new List<int>();

            lCorners = new List<CTransportCell>();

            foreach (List<CTransportCell> lCTC in lTable)
            {
                // List of not empty values
                List<CTransportCell> lCells = lCTC.Select(x => x).Where(y => y.iActualValue != null).ToList();
                foreach (CTransportCell ctc in lCells)
                {
                    ctc.bCorner = true;
                    lCorners.Add(ctc);
                }
                // Add all cells, that have Actual values
                //lCorners.Concat(lCells);

                int iCount = lCells.Count;
                lValues.Add(iCount);
            }

            return lValues.IndexOf(lValues.Max());
        }

        public void FormColumnsOnGrid(ref DataGridView dgv)
        {
            //dgv.CellBorderStyle = DataGridViewCellBorderStyle.SunkenHorizontal;
            dgv.Columns.Add("", ""); // For A1, A2, ...
            dgv.Columns.Add("", ""); // For U1, U2, ...
            uint i = 1;
            foreach (uint value in lNeeds)
            {
                string text = "B" + i.ToString();
                dgv.Columns.Add(text, text);
                i++;
            }
            dgv.Columns.Add("Storage", "Запасы");
            this.dgv = dgv;
            OutPutInDataGrid(false, false);
        }

        private void OutPutInDataGrid(bool bIncludePotentials, bool bColor)
        {
            // Add empty row for potentials V1, V2, ...
            string[] values;
            int iRow;
            if (bIncludePotentials)
            {
                values = new string[dgv.Columns.Count];
                values[0] = "";
                values[1] = "";
                int i = 2; // to start from 2
                for (iRow = 0; iRow < lVKoef.Length; ++iRow)
                    values[i++] = "V= " + lVKoef[iRow].ToString();
                values[i] = ""; // To keep last column empty
                dgv.Rows.Add(values);
            }
            else
                dgv.Rows.Add();

            
            iRow = 0;
            foreach (List<CTransportCell> lCTC in lTable) // for each row in this list
            {
                DataGridViewRow dgvr = new DataGridViewRow();

                DataGridViewTextBoxCell dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = "A" + (iRow + 1).ToString();
                dgvr.Cells.Add(dgvCell);
                dgvCell = new DataGridViewTextBoxCell();
                if (bIncludePotentials)
                    dgvCell.Value = "U" + (iRow + 1).ToString() + "= " + lUKoef[iRow].ToString();
                else
                    dgvCell.Value = "";
                dgvr.Cells.Add(dgvCell);
                //uint iStart = 2; // start from this position
                foreach (CTransportCell ctc in lCTC)
                {
                    dgvCell = new DataGridViewTextBoxCell();
                    StringBuilder sb = new StringBuilder();

                    if (ctc.iActualValue != null)
                        sb.Append(ctc.iActualValue.ToString() + " ");
                    sb.Append("[" + ctc.iTariff.ToString() + "]");
                    if (ctc.iDiff > 0)
                        sb.Append("\r\n{" + ctc.iDiff.ToString() + "}");
                    dgvCell.Value = sb.ToString();

                    // Color cells with values
                    if (bColor)
                    {
                        if (ctc.bIsPathCell)
                        {
                            dgvCell.Style.BackColor = System.Drawing.Color.Yellow;
                        }
                        if (ctc.bCorner)
                        {
                            dgvCell.Style.BackColor = System.Drawing.Color.Orange;
                            if (ctc.bSign == true)
                                dgvCell.Value += "(+)";
                            else if (ctc.bSign == false)
                                dgvCell.Value += "(-)";

                        }
                    }
                    dgvr.Cells.Add(dgvCell);
                }
                
                dgvCell = new DataGridViewTextBoxCell();
                dgvCell.Value = lStorage[iRow].ToString();
                dgvr.Cells.Add(dgvCell);
                dgv.Rows.Add(dgvr);

                iRow++;
            }
            values = new string[lTable[0].Count + 3];
            int t = 0;
            values[t++] = "Нужно";
            uint iFunctionValue = 0;

            foreach (List<CTransportCell> lCTC in lTable)
            {
                foreach (CTransportCell ctc in lCTC)
                {
                    if (ctc.bZeroValue || ctc.bArtificial || ctc.iActualValue == null) continue;

                    iFunctionValue += (((uint)ctc.iActualValue) * ctc.iTariff);
                }
            }

            values[t++] = "F = " + iFunctionValue.ToString();

            for (int i = 0; i < lNeeds.Count; ++i)
                values[t++] = lNeeds[i].ToString();

            values[t] = "~_~";
            dgv.Rows.Add(values);
            dgv.Rows[dgv.Rows.Count - 1].DividerHeight = 2;

            dgv.Refresh();
        }

        // Test needed!!
        private void FindCorners()
        {
            bool bMode = true; // Mean to start horizontal
            for (int i = 0; i < lTable.Count + lTable[0].Count; ++i)
            {
                if (bMode)
                {
                    // Check all rows
                    foreach (List<CTransportCell> lCTC in lTable)
                    {
                        int iCount = 0;
                        foreach (CTransportCell ctc in lCTC)
                        {
                            if (ctc.bCorner || ctc.bStarter)
                                iCount++;
                        }

                        if (iCount == 1)
                        {
                            foreach (CTransportCell ctc in lCTC)
                            {
                                if (ctc.bCorner)
                                    ctc.bCorner = false;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    // Check all columns
                    for (int j = 0; j < lTable[0].Count; ++j)
                    {
                        int iCount = 0;
                        foreach (List<CTransportCell> lCTC in lTable)
                        {
                            if (lCTC[j].bCorner || lCTC[j].bStarter)
                                iCount++;
                        }

                        if (iCount == 1)
                        {
                            foreach (List<CTransportCell> lCTC in lTable)
                            {
                                if (lCTC[j].bCorner)
                                    lCTC[j].bCorner = false;
                            }
                            break;
                        }
                    }
                }
                bMode = !bMode;
                /*
                foreach (CTransportCell ctc in lCorners)
                {
                    int xPos = ctc.iPosX;
                    int yPos = ctc.iPosY;

                    bool bVerticalExist = false;
                    bool bHorizontalExist = false;

                    for (int j = 0; j < lTable.Count; j++)
                    {
                        if (j == xPos) continue;

                        if (lTable[j][yPos].iActualValue != null || (lTable[j][yPos].iDiff > 0 && lTable[j][yPos].bStarter))
                        {
                            bVerticalExist = true;
                            break;
                        }
                    }

                    for (int j = 0; j < lTable[xPos].Count; j++)
                    {
                        if (j == yPos) continue;
                        
                        if (lTable[xPos][j].iActualValue != null || (lTable[xPos][j].iDiff > 0 && lTable[xPos][j].bStarter))
                        {
                            bHorizontalExist = true;
                            break;
                        }
                    }

                    if (bVerticalExist == false || bHorizontalExist == false)
                    {
                        // Mark it as uncornable
                        ctc.bCorner = false;
                    }
                }*/
                
                
            }

            // Override lCorners with new values of corners
            lCorners = lCorners.Select(x => x).Where(y => y.bCorner == true).ToList();
        }

        //private void ResourceAllocation_Double() // maybe later
        //{

        //}

        private void ResourceAllocation() // метод С-З угла
        {
            List<uint> lStorageTmp = new List<uint>(lStorage);
            for (int i = 0; i < lNeeds.Count; ++i)
            {
                uint iHowManyNeed = lNeeds[i];
                for (int j = 0; j < lStorageTmp.Count; ++j)
                {
                    if (lStorageTmp[j] == 0) continue; // If empty, we can't get from this storage

                    if (lStorageTmp[j] > iHowManyNeed)
                    {
                        lStorageTmp[j] -= iHowManyNeed;
                        lTable[j][i].iActualValue = iHowManyNeed;
                        iHowManyNeed = 0;
                        break;
                    }
                    else if (lStorageTmp[j] < iHowManyNeed)
                    {
                        uint iDiff = iHowManyNeed - lStorageTmp[j];

                        lTable[j][i].iActualValue = lStorageTmp[j];
                        lStorageTmp[j] = 0;
                        iHowManyNeed = iDiff;
                    }
                    else
                    {
                        lTable[j][i].iActualValue = lStorageTmp[j];
                        lStorageTmp[j] = 0;
                        iHowManyNeed = 0;
                        break;
                    }
                }
            }
        }
    }
}
