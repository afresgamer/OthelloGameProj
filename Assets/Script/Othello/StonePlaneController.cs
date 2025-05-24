using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace OthelloGameProj
{
    /// <summary>
    /// 置ける場所の可視化の制御まとめ
    /// </summary>
    public class StonePlaneController : MonoBehaviour
    {
        [SerializeField, Header("通常の置ける場所のマテリアル")]
        private Material planeBaseMaterial;
        [SerializeField, Header("置ける場所のマテリアル")]
        private Material planeActiveMaterial;

        // 置ける場所の実体
        StonePlane[,] stonePlaneArray = new StonePlane[GameConst.STONE_PUT_POINT, GameConst.STONE_PUT_POINT];
        public StonePlane[,] StonePlaneArray { get { return stonePlaneArray; } set { stonePlaneArray = value; } }

        // 置けるセルの座標情報リスト
        private List<StonePlane> stonePlaneList = new();

        /// <summary>
        /// 置ける場所の初期化
        /// </summary>
        /// <param name="stonePlane">置ける場所の関連情報</param>
        /// <param name="cell">セル情報</param>
        public void InitPlane(StonePlane stonePlane, Cell cell)
        {
            var x = cell.GetXPos();
            var y = cell.GetYPos();
            var collider = stonePlane.gameObject.GetComponent<MeshCollider>();
            var renderer = stonePlane.gameObject.GetComponent<MeshRenderer>();
            collider.enabled = false;
            renderer.enabled = false;
            stonePlaneArray[x, y] = stonePlane;
        }

        /// <summary>
        /// 置ける場所の表示処理
        /// </summary>
        /// <param name="stoneType">石の色</param>
        public void SetStonePlane(GameConst.StoneType stoneType)
        {
            // 置けるセルの初期化
            stonePlaneList = new List<StonePlane>();

            for (int x = 0; x < GameConst.STONE_PUT_POINT; x++)
            {
                for (int y = 0; y < GameConst.STONE_PUT_POINT; y++)
                {
                    var plane = stonePlaneArray[x, y];
                    var info = plane.StoneInfo;

                    // 空はスキップ
                    if (info.GetStoneType() == GameConst.StoneType.empty) continue;

                    // 石から周りを判定
                    if (info.GetStoneType() == stoneType)
                    {
                        if (CheckPutPoint(x, y, stoneType)) SetPlaneList(true);
                    }
                }
            }
        }

        /// <summary>
        /// 置ける場所の取得処理
        /// </summary>
        /// <param name="stoneType">石の色</param>
        /// <returns>置ける場所のリスト</returns>
        public List<StonePlane> GetStonePlaneList(GameConst.StoneType stoneType)
        {
            // 置けるセルの初期化
            stonePlaneList = new List<StonePlane>();

            for (int x = 0; x < GameConst.STONE_PUT_POINT; x++)
            {
                for (int y = 0; y < GameConst.STONE_PUT_POINT; y++)
                {
                    var plane = stonePlaneArray[x, y];
                    var info = plane.StoneInfo;

                    // 空はスキップ
                    if (info.GetStoneType() == GameConst.StoneType.empty) continue;

                    // 石から周りを判定
                    if (info.GetStoneType() == stoneType) CheckPutPoint(x, y, stoneType);
                }
            }

            return stonePlaneList;
        }

        public void ResetStonePlaneList()
        {
            stonePlaneList.Clear();
        }

        /// <summary>
        /// 空いている場所を全検索して取得処理
        /// </summary>
        /// <returns>空いている場所のリスト</returns>
        public List<StonePlane> GetEmptyPlaneList()
        {
            List<StonePlane> result = new List<StonePlane>();

            for (int x = 0; x < GameConst.STONE_PUT_POINT; x++)
            {
                for (int y = 0; y < GameConst.STONE_PUT_POINT; y++)
                {
                    var plane = stonePlaneArray[x, y];
                    var info = plane.StoneInfo;

                    // 空いている場所を格納
                    if (info.GetStoneType() == GameConst.StoneType.empty) 
                        result.Add(plane);
                }
            }

            return result;
        }

        /// <summary>
        /// 置ける場所の検索判定処理
        /// </summary>
        /// <param name="x">中心位置のx座標</param>
        /// <param name="y">中心位置のy座標</param>
        /// <returns>置けるor置けない</returns>
        private bool CheckPutPoint(int x, int y, GameConst.StoneType stoneType)
        {
            var result = false;

            // 上下左右
            var top = CheckOneWay(x, y, -1, 0, stoneType);
            var bottom = CheckOneWay(x, y, 1, 0, stoneType);
            var right = CheckOneWay(x, y, 0, 1, stoneType);
            var left = CheckOneWay(x, y, 0, -1, stoneType);
            // 斜め
            var rightDiagonalTop = CheckOneWay(x, y, -1, 1, stoneType);   // 右上方向
            var rightDiagonalBottom = CheckOneWay(x, y, 1, 1, stoneType); // 右下方向
            var leftDiagonalTop = CheckOneWay(x, y, -1, -1, stoneType);   // 左上方向
            var leftDiagonalBottom = CheckOneWay(x, y, 1, -1, stoneType); // 左下方向

            // どれか一つでも座標情報があればTRUE
            if (!top.IsCellValid() || !bottom.IsCellValid() || !right.IsCellValid() || !left.IsCellValid() ||
                !rightDiagonalTop.IsCellValid() || !rightDiagonalBottom.IsCellValid() ||
                !leftDiagonalTop.IsCellValid() || !leftDiagonalBottom.IsCellValid()) 
            { 
                result = true;

                // セル設定
                if (!top.IsCellValid())
                    stonePlaneList.Add(stonePlaneArray[top.GetXPos(), top.GetYPos()]);
                if (!bottom.IsCellValid())
                    stonePlaneList.Add(stonePlaneArray[bottom.GetXPos(), bottom.GetYPos()]);
                if (!right.IsCellValid())
                    stonePlaneList.Add(stonePlaneArray[right.GetXPos(), right.GetYPos()]);
                if (!left.IsCellValid())
                    stonePlaneList.Add(stonePlaneArray[left.GetXPos(), left.GetYPos()]);
                if (!rightDiagonalTop.IsCellValid())
                    stonePlaneList.Add(stonePlaneArray[rightDiagonalTop.GetXPos(), rightDiagonalTop.GetYPos()]);
                if (!rightDiagonalBottom.IsCellValid())
                    stonePlaneList.Add(stonePlaneArray[rightDiagonalBottom.GetXPos(), rightDiagonalBottom.GetYPos()]);
                if (!leftDiagonalTop.IsCellValid())
                    stonePlaneList.Add(stonePlaneArray[leftDiagonalTop.GetXPos(), leftDiagonalTop.GetYPos()]);
                if (!leftDiagonalBottom.IsCellValid())
                    stonePlaneList.Add(stonePlaneArray[leftDiagonalBottom.GetXPos(), leftDiagonalBottom.GetYPos()]);
            }

            // 重複削除
            stonePlaneList = stonePlaneList.Distinct().ToList();

            return result;
        }

        /// <summary>
        /// 一方向の置ける場所の判定処理
        /// </summary>
        /// <param name="x">置ける場所のx座標</param>
        /// <param name="y">置ける場所のy座標</param>
        /// <param name="horizontal">水平方向にいくつ移動するのか決める値</param>
        /// <param name="vertical">垂直方向にいくつ移動するのか決める値</param>
        /// <param name="type">自分の石の色</param>
        /// <returns>セル情報</returns>
        private Cell CheckOneWay(int x, int y, int horizontal, int vertical, GameConst.StoneType type)
        {
            var result = new Cell();
            result.SetCell(-1, -1);

            int h = x + horizontal;
            int v = y + vertical;
            StoneInfo beforeStoneInfo = null;

            while (h < GameConst.STONE_PUT_POINT && h >= 0 && v < GameConst.STONE_PUT_POINT && v >= 0)
            {
                var info = stonePlaneArray[h, v].StoneInfo;

                // 自分の石だった場合
                if (info.GetStoneType() == type) 
                {
                    //前の石が相手の石　かつ　前の石が相手の石の色の場合は置けないので処理を終える
                    if (beforeStoneInfo != null && beforeStoneInfo.GetStoneType() != type)
                        break;

                    // 確認座標を次に進める
                    h += horizontal;
                    v += vertical;
                    continue;
                }
                // 空欄だった場合
                else if (info.GetStoneType() == GameConst.StoneType.empty)
                {
                    // 前の石が空ではない かつ 前の石が相手の石の色の場合は置けるので座標指定
                    if (beforeStoneInfo != null && beforeStoneInfo.GetStoneType() != type)
                    {
                        result = new Cell();
                        result.SetCell(h, v);
                        break;
                    }

                    // 前の石が空の場合処理を終える
                    if (beforeStoneInfo == null) break;
                }
                // 相手の石だった場合
                else if (info.GetStoneType() != type)
                {
                    beforeStoneInfo = info;
                }

                // 確認座標を次に進める
                h += horizontal;
                v += vertical;
                continue;
            }

            return result;
        }

        /// <summary>
        /// 置ける場所の更新処理
        /// </summary>
        /// <param name="cell">セル情報</param>
        /// <param name="newPlane">新しい置ける場所情報</param>
        public void UpdatePlane(Cell cell, StonePlane newPlane)
        {
            var plane = StonePlaneArray[cell.GetXPos(), cell.GetYPos()];
            plane = newPlane;
        }

        /// <summary>
        /// 置ける場所の表示更新処理
        /// </summary>
        /// <param name="stonePlane">置ける場所</param>
        /// <param name="isActive">アクティブにするしない</param>
        public void SetPlane(StonePlane stonePlane, bool isActive)
        {
            var collider = stonePlane.gameObject.GetComponent<MeshCollider>();
            var renderer = stonePlane.gameObject.GetComponent<MeshRenderer>();
            collider.enabled = isActive;
            renderer.enabled = isActive;
        }

        /// <summary>
        /// 置ける場所リストの表示更新処理
        /// </summary>
        /// <param name="isActive">アクティブにするしない</param>
        public void SetPlaneList(bool isActive)
        {
            foreach (var stonePlane in stonePlaneList)
            {
                var collider = stonePlane.gameObject.GetComponent<MeshCollider>();
                var renderer = stonePlane.gameObject.GetComponent<MeshRenderer>();
                collider.enabled = isActive;
                renderer.enabled = isActive;
            }
        }

        /// <summary>
        /// 置ける場所マテリアルの更新処理
        /// </summary>
        /// <param name="stonePlane">置ける場所</param>
        /// <param name="isActive">アクティブにするしない</param>
        public void UpdatePlaneMaterial(StonePlane stonePlane, bool isActive)
        {
            if (stonePlane == null) return;
            if (!stonePlane.gameObject.TryGetComponent<MeshRenderer>(out var renderer)) return;
            if (renderer.enabled)
            {
                renderer.material = isActive ? planeActiveMaterial : planeBaseMaterial;
            }
        }

        /// <summary>
        /// 置ける場所マテリアルが光っているかどうか確認処理
        /// </summary>
        /// <param name="stonePlane">置ける場所</param>
        /// <returns></returns>
        public bool CheckPlaneMaterial(StonePlane stonePlane)
        {
            if (stonePlane == null) return false;
            if (!stonePlane.gameObject.TryGetComponent<MeshRenderer>(out var renderer)) return false;

            return renderer.material.name.Contains(planeActiveMaterial.name);
        }

        /// <summary>
        /// 勝敗判定
        /// </summary>
        /// <param name="stoneType">プレイヤーかNPCの石の色</param>
        /// <returns>勝ちがtrue、負けがfalse</returns>
        public bool CheckWinOrLose(GameConst.StoneType stoneType)
        {
            var emptyStoneList = GetEmptyPlaneList();
            int cnt = 0;
            int opponentCnt = 0;
            if (stoneType == GameConst.StoneType.white)
            {
                if (OthelloGameManager.Instance.WhiteStoneList.Count > OthelloGameManager.Instance.BlackStoneList.Count)
                    return true;
                else if (OthelloGameManager.Instance.BlackStoneList.Count < OthelloGameManager.Instance.WhiteStoneList.Count)
                    return false;

                cnt = OthelloGameManager.Instance.WhiteStoneList.Count + emptyStoneList.Count;
                opponentCnt = OthelloGameManager.Instance.BlackStoneList.Count;
            }
            else if(stoneType == GameConst.StoneType.black)
            {
                if (OthelloGameManager.Instance.BlackStoneList.Count > OthelloGameManager.Instance.WhiteStoneList.Count)
                    return true;
                else if (OthelloGameManager.Instance.WhiteStoneList.Count < OthelloGameManager.Instance.BlackStoneList.Count)
                    return false;

                cnt = OthelloGameManager.Instance.BlackStoneList.Count + emptyStoneList.Count;
                opponentCnt = OthelloGameManager.Instance.WhiteStoneList.Count;
            }

            return cnt > opponentCnt;
        }
    }
}
