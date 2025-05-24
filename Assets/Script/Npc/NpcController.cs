using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OthelloGameProj
{
    /// <summary>
    /// NPCの管理まとめ
    /// </summary>
    public class NpcController : MonoBehaviour
    {
        [SerializeField, Header("ボードの制御処理オブジェクト")]
        private BoardController boardController;
        [SerializeField, Header("置ける場所の可視化の制御オブジェクト")]
        private StonePlaneController stonePlaneController;
        [SerializeField, Header("石置く時のSE音源")]
        private AudioClip putStoneClip;

        // 置けるセルの座標情報リスト
        public List<StonePlane> StonePlaneList { get; private set; }
        bool isPut = false;

        void Start()
        {
            isPut = false;
        }

        void Update()
        {
            // ゲームスタートしてなかったらプレイヤー操作不可
            if (!OthelloGameManager.Instance.IsGameStart) return;
            // オプション画面が開いていたらプレイヤー操作不可
            if (OthelloGameManager.Instance.IsOpenOption) return;
            //　勝敗が決定したら操作不可
            if (OthelloGameManager.Instance.PlayerWinOrLose != GameConst.GameWinOrLoss.None) return;

            if (!IsSetController()) return;

            // Npcの番だったら場所表示をして、思考フェーズを行う
            // 思考フェーズで置き場所確定後にアクションしてからプレイヤーに手番を移行
            if (!UpdateNpcTurn()) CustomDebugger.ColorLog("NPC動き中", GameConst.LogLevel.Lime);
            
        }

        private bool IsSetController()
        {
            if (boardController == null)
            {
                CustomDebugger.Warninglog("ボードの制御処理オブジェクトが設定されていません。");
                return false;
            }

            if (stonePlaneController == null)
            {
                CustomDebugger.Warninglog("置ける場所の可視化の制御オブジェクトが設定されていません。");
                return false;
            }

            return true;
        }

        /// <summary>
        /// NPCの現状管理
        /// </summary>
        /// <returns>現状特になることがない場合はTRUE、それ以外はfalse</returns>
        private bool UpdateNpcTurn()
        {
            if (FlowManager.Instance.NowGameState == GameConst.GameState.NpcTurn)
            {
                StonePlaneList = stonePlaneController.GetStonePlaneList(OthelloGameManager.Instance.NpcType);
                // 置き場所が0箇所　または　置いている最中は何もしない
                if (StonePlaneList.Count == 0)
                {
                    // 勝敗判定
                    if (stonePlaneController.CheckWinOrLose(OthelloGameManager.Instance.NpcType))
                        OthelloGameManager.Instance.PlayerWinOrLose = GameConst.GameWinOrLoss.NPCWin;
                    else
                        OthelloGameManager.Instance.PlayerWinOrLose = GameConst.GameWinOrLoss.PlayerWin;

                    return true;
                }
                if (isPut) return true;

                StonePlane stonePlane = new StonePlane();
                switch (OthelloGameManager.Instance.GameSettingInfo.GetGameDifficulty())
                {
                    case GameConst.GameDifficulty.Easy:
                        stonePlane = ConsiderPointEasy(StonePlaneList);
                        PutStone(stonePlane);
                        break;
                    case GameConst.GameDifficulty.Normal:
                        stonePlane = ConsiderPointNormal(StonePlaneList);
                        PutStone(stonePlane);
                        break;
                    case GameConst.GameDifficulty.Hard:
                        stonePlane = ConsiderPointHard(StonePlaneList);
                        PutStone(stonePlane);
                        break;
                }

                return false;
            }

            return true;
        }

        /// <summary>
        /// ゲーム難易度が簡単の場合の場所決定処理
        /// </summary>
        /// <param name="stonePlaneList">置ける場所のリスト</param>
        /// <returns>置く場所</returns>
        private StonePlane ConsiderPointEasy(List<StonePlane> stonePlaneList)
        {
            if (stonePlaneList == null ||  stonePlaneList.Count == 0) { return null; }

            int cnt = stonePlaneList.Count;
            var randomNum = Random.Range(0, cnt);
            return stonePlaneList[randomNum];
        }

        /// <summary>
        /// ゲーム難易度が普通の場合の場所決定処理
        /// </summary>
        /// <param name="stonePlaneList">置ける場所のリスト</param>
        /// <returns>置く場所</returns>
        private StonePlane ConsiderPointNormal(List<StonePlane> stonePlaneList)
        {
            if (stonePlaneList == null || stonePlaneList.Count == 0) { return null; }

            var result = boardController.GetMaxScorePlane(OthelloGameManager.Instance.NpcType, stonePlaneList);
            return result;
        }

        /// <summary>
        /// ゲーム難易度が難しいの場合の場所決定処理
        /// </summary>
        /// <param name="stonePlaneList">置ける場所のリスト</param>
        /// <returns>置く場所</returns>
        private StonePlane ConsiderPointHard(List<StonePlane> stonePlaneList)
        {
            if (stonePlaneList == null || stonePlaneList.Count == 0) { return null; }

            var result = boardController.SearchNegaAlphaStone(stonePlaneList, 3, true);
            return result;
        }

        /// <summary>
        /// 石を置く処理
        /// </summary>
        /// <param name="stonePlane">配置する位置情報</param>
        private void PutStone(StonePlane stonePlane)
        {
            if (stonePlane == null) { return; }
            if (!FlowManager.Instance.IsNpcTurn()) { return; }
            isPut = true;

            // 置くandひっくり返す
            boardController.AddStone(stonePlane.StoneInfo, OthelloGameManager.Instance.NpcType);
            stonePlaneController.SetPlane(stonePlane, false);
            boardController.TurnoverStone(stonePlane.StoneInfo, OthelloGameManager.Instance.NpcType, () =>
            {
                // 順番の更新
                FlowManager.Instance.SetState(GameConst.GameState.PlayerTurn);
                FlowManager.Instance.SetTurn(1);
                
                // 置き場所の更新
                stonePlaneController.SetStonePlane(OthelloGameManager.Instance.PlayerType);

                isPut = false;
            });

            AudioController.Instance.PlaySE(putStoneClip);
        }
    }
}

