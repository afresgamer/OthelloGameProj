using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

namespace OthelloGameProj
{
    /// <summary>
    /// 盤上の関連情報と制御まとめ
    /// </summary>
    public class BoardController : MonoBehaviour
    {
        // オセロの位置情報一覧
        [SerializeField, Header("オセロの位置情報一覧")]
        private BoardInfo[] boardInfoArray;

        [SerializeField, Header("オセロの石オブジェクト")]
        private StoneMovement stoneObj;

        [SerializeField, Header("置ける場所の可視化の制御オブジェクト")]
        private StonePlaneController planeController;

        private int stoneCnt = 1;
        // ひっくり返す石のリスト
        private List<StoneInfo> turnoverStoneList = new List<StoneInfo>();

        /// <summary>
        /// 盤上の関連情報と制御の初期化
        /// </summary>
        public void Init()
        {
            InitBoard(GameConst.StoneType.empty);

            GameStartBoard(OthelloGameManager.Instance.PlayerType);
            planeController.SetStonePlane(OthelloGameManager.Instance.PlayerType);
            FlowManager.Instance.SetState(GameConst.GameState.PlayerTurn);
        }

        /// <summary>
        /// ボードの初期化
        /// </summary>
        /// <param name="type">石の色</param>
        private void InitBoard(GameConst.StoneType type)
        {
            for (int x = 0; x < GameConst.STONE_PUT_POINT; x++)
            {
                for (int y = 0; y < GameConst.STONE_PUT_POINT; y++)
                {
                    Cell cell = new();
                    cell.SetCell(x, y);
                    var pos = boardInfoArray[x].attachPointArray[y].position;
                    var plane = boardInfoArray[x].attachPointArray[y].gameObject.GetComponent<StonePlane>();

                    // 内部データの初期化
                    OthelloGameManager.Instance.NowCellArray[x, y] = new StoneInfo();
                    OthelloGameManager.Instance.NowCellArray[x, y].SetCell(cell);
                    OthelloGameManager.Instance.NowCellArray[x, y].SetStoneType(type);
                    OthelloGameManager.Instance.NowCellArray[x, y].SetPos(pos);
                    plane.StoneInfo = OthelloGameManager.Instance.NowCellArray[x, y];
                    planeController.InitPlane(plane, cell);
                }
            }
        }

        /// <summary>
        /// 石の色を変更する
        /// </summary>
        /// <param name="stoneInfo">石の情報</param>
        /// <param name="stoneType">設定する石の色</param>
        /// <param name="action">ひっくり返し後に行うコールバック処理</param>
        private void ChangeStoneType(StoneInfo stoneInfo, GameConst.StoneType stoneType, TweenCallback action)
        {
            var x = stoneInfo.GetCell().GetXPos();
            var y = stoneInfo.GetCell().GetYPos();
            var info = OthelloGameManager.Instance.NowCellArray[x, y];
            var type = info.GetStoneType();
            var movement = info.GetStone().GetComponent<StoneMovement>();

            if (type == GameConst.StoneType.white)
            {
                OthelloGameManager.Instance.WhiteStoneList.Remove(info.GetStone());
                OthelloGameManager.Instance.BlackStoneList.Add(info.GetStone());
            }
            else if (type == GameConst.StoneType.black)
            {
                OthelloGameManager.Instance.BlackStoneList.Remove(info.GetStone());
                OthelloGameManager.Instance.WhiteStoneList.Add(info.GetStone());
            }

            info.SetStoneType(stoneType);
            planeController.StonePlaneArray[x, y].StoneInfo = info;
            // ひっくり返す
            movement.Movement(null, action, null);
        }

        
        /// <summary>
        /// ゲームスタート時の盤の設定処理
        /// </summary>
        /// <param name="player">playerの石の色</param>
        private void GameStartBoard(GameConst.StoneType player)
        {
            var colorNum = (int)player;

            // 2パターン　黒白白黒(2112)と白黒黒白(1221)
            var stoneInfo001 = OthelloGameManager.Instance.NowCellArray[GameConst.STONE_INIT_POS, GameConst.STONE_INIT_POS];
            var plane001 = planeController.StonePlaneArray[GameConst.STONE_INIT_POS, GameConst.STONE_INIT_POS];
            var stoneInfo002 = OthelloGameManager.Instance.NowCellArray[GameConst.STONE_INIT_POS, GameConst.STONE_FINISH_POS];
            var plane002 = planeController.StonePlaneArray[GameConst.STONE_INIT_POS, GameConst.STONE_FINISH_POS];
            var stoneInfo003 = OthelloGameManager.Instance.NowCellArray[GameConst.STONE_FINISH_POS, GameConst.STONE_INIT_POS];
            var plane003 = planeController.StonePlaneArray[GameConst.STONE_FINISH_POS, GameConst.STONE_INIT_POS];
            var stoneInfo004 = OthelloGameManager.Instance.NowCellArray[GameConst.STONE_FINISH_POS, GameConst.STONE_FINISH_POS];
            var plane004 = planeController.StonePlaneArray[GameConst.STONE_FINISH_POS, GameConst.STONE_FINISH_POS];
            
            stoneInfo001.SetStoneType(player);
            CreateStone(stoneInfo001);

            // 白黒黒白(1221)
            if (stoneInfo001.GetStoneType() == GameConst.StoneType.white)
            {
                colorNum++;
                stoneInfo002.SetStoneType((GameConst.StoneType)colorNum);
                CreateStone(stoneInfo002);

                stoneInfo003.SetStoneType((GameConst.StoneType)colorNum);
                CreateStone(stoneInfo003);

                colorNum--;
                stoneInfo004.SetStoneType((GameConst.StoneType)colorNum);
                CreateStone(stoneInfo004);
            }
            // 黒白白黒(2112)
            else if (stoneInfo001.GetStoneType() == GameConst.StoneType.black)
            {
                colorNum--;
                stoneInfo002.SetStoneType((GameConst.StoneType)colorNum);
                CreateStone(stoneInfo002);

                stoneInfo003.SetStoneType((GameConst.StoneType)colorNum);
                CreateStone(stoneInfo003);

                colorNum++;
                stoneInfo004.SetStoneType((GameConst.StoneType)colorNum);
                CreateStone(stoneInfo004);
            }

            plane001.StoneInfo = stoneInfo001;
            plane002.StoneInfo = stoneInfo002;
            plane003.StoneInfo = stoneInfo003;
            plane004.StoneInfo = stoneInfo004;
        }

        /// <summary>
        /// 石の作成処理
        /// </summary>
        /// <param name="stoneInfo">石の情報</param>
        private void CreateStone(StoneInfo stoneInfo)
        {
            var type = stoneInfo.GetStoneType();
            var pos = stoneInfo.GetPos();
            var rot = type == GameConst.StoneType.white ? 
                Quaternion.Euler(GameConst.STONE_WHITE_ROT, 0, 0) : Quaternion.Euler(GameConst.STONE_BLACK_ROT, 0, 0);

            GameObject stone = Instantiate(stoneObj.gameObject, pos, rot);
            stone.name = stoneObj.gameObject.name + stoneCnt.ToString();
            if (type == GameConst.StoneType.white) OthelloGameManager.Instance.WhiteStoneList.Add(stone);
            else if (type == GameConst.StoneType.black) OthelloGameManager.Instance.BlackStoneList.Add(stone);

            stoneCnt++;
            stoneInfo.SetStone(stone);
        }

        /// <summary>
        /// 石を作成して追加する
        /// </summary>
        /// <param name="stoneInfo">石の情報</param>
        public void AddStone(StoneInfo info, GameConst.StoneType type)
        {
            var x = info.GetCell().GetXPos();
            var y = info.GetCell().GetYPos();
            var rot = type == GameConst.StoneType.white ?
                                Quaternion.Euler(GameConst.STONE_WHITE_ROT, 0, 0) :
                                Quaternion.Euler(GameConst.STONE_BLACK_ROT, 0, 0);
            GameObject stone = Instantiate(stoneObj.gameObject, info.GetPos(), rot);
            stone.name = stoneObj.gameObject.name + stoneCnt.ToString();
            stoneCnt++;

            OthelloGameManager.Instance.NowCellArray[x, y].SetStoneType(type);
            OthelloGameManager.Instance.NowCellArray[x, y].SetStone(stone);
            planeController.StonePlaneArray[x, y].StoneInfo = OthelloGameManager.Instance.NowCellArray[x, y];

            if (type == GameConst.StoneType.white) OthelloGameManager.Instance.WhiteStoneList.Add(stone);
            else if (type == GameConst.StoneType.black) OthelloGameManager.Instance.BlackStoneList.Add(stone);
        }

        /// <summary>
        /// 石をひっくり返す
        /// </summary>
        /// <param name="stoneInfo">置いた場所の石情報</param>
        /// <param name="stoneType">設定する石の色</param>
        /// <param name="action">ひっくり返し後に行うコールバック処理</param>
        public void TurnoverStone(StoneInfo stoneInfo, GameConst.StoneType stoneType, DG.Tweening.TweenCallback action)
        {
            if (stoneInfo == null) return;

            int x = stoneInfo.GetCell().GetXPos();
            int y = stoneInfo.GetCell().GetYPos();
            if (CheckPutPoint(x, y, stoneType))
            {
                // 最後の要素のみコールバック処理を発生する
                var num = 1;
                var cnt = turnoverStoneList.Count;
                turnoverStoneList.ForEach(n => 
                {
                    if (num == cnt) ChangeStoneType(n, stoneType, action);
                    else ChangeStoneType(n, stoneType, null);
                    num++;
                });
            }

            // ひっくり返すリストの初期化
            turnoverStoneList.Clear();
        }

        /// <summary>
        /// ひっくり返す場所の検索判定処理
        /// </summary>
        /// <param name="x">置いた場所のx座標</param>
        /// <param name="y">置いた場所のy座標</param>
        /// <returns></returns>
        public bool CheckPutPoint(int x, int y, GameConst.StoneType stoneType)
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

            if (top.Count > 0 || bottom.Count > 0 || right.Count > 0 || left.Count > 0 ||
                rightDiagonalTop.Count > 0 || rightDiagonalBottom.Count > 0 ||
                leftDiagonalTop.Count > 0 || leftDiagonalBottom.Count > 0) 
            { 
                result = true;

                var data = top.Concat(bottom).Concat(right).Concat(left)
                    .Concat(rightDiagonalTop).Concat(rightDiagonalBottom)
                    .Concat(leftDiagonalTop).Concat(leftDiagonalBottom).ToList();
                // 重複削除
                data = data.Distinct().ToList();
                var stoneArray = planeController.StonePlaneArray;

                if (data.Count > 0)
                {
                    // ひっくり返す石を設定
                    foreach (var item in data)
                    {
                        var info = OthelloGameManager.Instance.NowCellArray[item.GetXPos(), item.GetYPos()];
                        turnoverStoneList.Add(info);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 一方向のひっくり返す場所の取得処理
        /// </summary>
        /// <param name="x">置いた場所のx座標</param>
        /// <param name="y">置いた場所のy座標</param>
        /// <param name="horizontal">水平方向にいくつ移動するのか決める値</param>
        /// <param name="vertical">垂直方向にいくつ移動するのか決める値</param>
        /// <returns></returns>
        public List<Cell> CheckOneWay(int x, int y, int horizontal, int vertical, GameConst.StoneType stoneType)
        {
            var result = new List<Cell>();

            int h = x + horizontal;
            int v = y + vertical;
            var initStoneInfo = OthelloGameManager.Instance.NowCellArray[x, y];
            List<Cell> list = new List<Cell>();

            while (h < GameConst.STONE_PUT_POINT && h >= 0 && v < GameConst.STONE_PUT_POINT && v >= 0)
            {
                var info = OthelloGameManager.Instance.NowCellArray[h, v];

                // 最初の石が自分の色と同じか確認する(違う場合何も取得しない)
                if (initStoneInfo.GetStoneType() != stoneType) break;

                // 自分の駒だった場合
                if (info.GetStoneType() == stoneType)
                {
                    //　終点なので一時格納リストにデータがある場合結果に反映する
                    if (list.Count > 0) result.AddRange(list);

                    // 終点なので処理を終える
                    break;
                }
                // 空欄だった場合
                else if (info.GetStoneType() == GameConst.StoneType.empty)
                {
                    // 挟んでいないので処理を終える
                    break;
                }
                // 相手の駒だった場合
                else if (info.GetStoneType() != stoneType)
                {
                    var data = new Cell();
                    data.SetCell(h, v);
                    list.Add(data);
                }

                // 確認座標を次に進める
                h += horizontal;
                v += vertical;
            }

            return result;
        }
    }
}
