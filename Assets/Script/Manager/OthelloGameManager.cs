using System.Collections.Generic;
using UnityEngine;

namespace OthelloGameProj
{
    /// <summary>
    /// ゲーム管理制御処理まとめ
    /// </summary>
    public class OthelloGameManager : SingletonMonoBehaviour<OthelloGameManager>
    {
        /// <summary>
        /// ゲームスタートフラグ
        /// </summary>
        public bool IsGameStart { get; set; }
        /// <summary>
        /// オプション画面を開いているかどうか
        /// </summary>
        public bool IsOpenOption { get; set; }
        /// <summary>
        /// プレイヤーの石の色
        /// </summary>
        public GameConst.StoneType PlayerType { get; set; } 
        /// <summary>
        /// NPCの石の色
        /// </summary>
        public GameConst.StoneType NpcType { get; set; }
        /// <summary>
        /// ゲーム設定
        /// </summary>
        public GameSettingInfo GameSettingInfo { get; set; }

        /// <summary>
        /// 黒のオセロ一覧
        /// </summary>
        public List<GameObject> BlackStoneList { get; set; } = new List<GameObject>();
        /// <summary>
        /// 白のオセロ一覧
        /// </summary>
        public List<GameObject> WhiteStoneList { get; set; } = new List<GameObject>();
        /// <summary>
        /// プレイヤーの勝敗
        /// </summary>
        public GameConst.GameWinOrLoss PlayerWinOrLose { get; set; }
        /// <summary>
        /// パス回数
        /// </summary>
        public int PassCnt { get; set; }

        // オセロの盤情報
        StoneInfo[,] nowCellArray = new StoneInfo[GameConst.STONE_PUT_POINT, GameConst.STONE_PUT_POINT];
        public StoneInfo[,] NowCellArray { get{ return nowCellArray; } set{ nowCellArray = value; } }

        protected override void Awake()
        {
            base.Awake();
            GameSettingInfo = new GameSettingInfo();
        }

        /// <summary>
        /// ゲームの初期化
        /// </summary>
        public void Init()
        {
            // ゲーム難易度
            BlackStoneList = new List<GameObject>();
            WhiteStoneList = new List<GameObject>();
            PlayerType = GameSettingInfo.GetStoneType();
            if (PlayerType == GameConst.StoneType.white) { NpcType = GameConst.StoneType.black; }
            else if (PlayerType == GameConst.StoneType.black) { NpcType |= GameConst.StoneType.white; }
            PlayerWinOrLose = GameConst.GameWinOrLoss.None;
            PassCnt = 0;
        }

        public void SetGameSettingInfo(GameConst.GameDifficulty difficulty, GameConst.StoneType stoneType, int handi)
        {
            GameSettingInfo.SetGameDifficulty(difficulty);
            GameSettingInfo.SetStoneType(stoneType);
            GameSettingInfo.SetHandicap(handi);
        }

        /// <summary>
        /// プレイヤーの石の数を取得
        /// </summary>
        /// <returns>プレイヤーの石の数</returns>
        public int GetPlayerStoneCnt()
        {
            var result = 0;
            if (PlayerType == GameConst.StoneType.white)
                result = WhiteStoneList.Count;
            else if (PlayerType == GameConst.StoneType.black)
                result = BlackStoneList.Count;

            return result;
        }

        /// <summary>
        /// NPCの石の数を取得
        /// </summary>
        /// <returns>NPCの石の数</returns>
        public int GetNPCStoneCnt()
        {
            var result = 0;
            if (NpcType == GameConst.StoneType.white)
                result = WhiteStoneList.Count;
            else if (NpcType == GameConst.StoneType.black)
                result = BlackStoneList.Count;

            return result;
        }
    }
}
