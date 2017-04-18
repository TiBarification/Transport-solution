using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_Transport
{
    public class CTransportCell
    {
        public uint? iActualValue;
        public uint iTariff;
        public bool bArtificial;
        public bool bZeroValue;
        public int iPosX;
        public int iPosY;
        public bool bCorner;
        public int iDiff;
        public bool bStarter;
        public bool bIsPathCell;
        public bool? bSign;

        public CTransportCell(uint iTariff, uint? iActualValue, int iPosX, int iPosY, bool bArtificial, bool bZeroValue)
        {
            this.iTariff = iTariff; // тариф
            this.iActualValue = iActualValue; // значение в ячейке
            this.bArtificial = bArtificial; // искусственная ли ячейка с нулевым тарифом
            this.bZeroValue = bZeroValue; // является ли это нулевой ячейкой
            this.iPosX = iPosX;
            this.iPosY = iPosY;
            this.bCorner = false;
        }

        public CTransportCell(uint iTariff, int iPosX, int iPosY)
        {
            this.iTariff = iTariff;
            this.iActualValue = null;
            this.bArtificial = false;
            this.bZeroValue = false;
            this.iPosX = iPosX;
            this.iPosY = iPosY;
            this.bCorner = false;
        }

        public void ClearCustomInfo()
        {
            iDiff = 0;
            bStarter = false;
            bCorner = false;
            bIsPathCell = false;
            bSign = null;

            if (bZeroValue == true)
            {
                iActualValue = null;
                bZeroValue = false;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is CTransportCell == false) return false;
            CTransportCell target = obj as CTransportCell;

            return (iActualValue == target.iActualValue && iTariff == target.iTariff && bArtificial == target.bArtificial && bZeroValue == target.bZeroValue && iPosX == target.iPosX && iPosY == target.iPosY && bCorner == target.bCorner && iDiff == target.iDiff && bStarter == target.bStarter && bIsPathCell == target.bIsPathCell);
        }
    }
}
