using UnityEngine;
using DG.Tweening;

namespace OthelloGameProj
{
    /// <summary>
    /// オセロの石関連情報
    /// </summary>
    public class StoneInfo
    {
        GameConst.StoneType stoneType;
        Cell cell;
        Vector3 pos;
        GameObject stone;
        
        public GameConst.StoneType GetStoneType() { return stoneType; }

        public Cell GetCell() { return cell; }

        public Vector3 GetPos() { return pos; }

        public GameObject GetStone() { return stone; }

        public void SetStoneType(GameConst.StoneType color)
        {
            stoneType = color;
        }

        public void SetCell(Cell currentCell)
        {
            cell = currentCell;
        }

        public void SetPos(Vector3 currentPos) 
        {
            pos = currentPos;
        }

        public void SetStone(GameObject stoneObj) { stone = stoneObj; }
    }
}