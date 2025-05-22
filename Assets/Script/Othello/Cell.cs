using System;
using UnityEngine;

namespace OthelloGameProj
{
    /// <summary>
    /// 盤の位置情報
    /// </summary>
    public struct Cell
    {
        private int xPos, yPos;

        public int GetXPos()
        {
            return xPos;
        }

        public int GetYPos() 
        {
            return yPos;
        }

        public void SetCell(int x, int y)
        {
            xPos = x; yPos = y;
        }

        public void SetCellX(int x) 
        {
            xPos = x;
        }

        public void SetCellY(int y)
        {
            yPos = y;
        }

        public bool IsCellValid() 
        {
            return xPos == -1 && yPos == -1;
        }
    }
}
