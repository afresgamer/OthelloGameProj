using UnityEngine;

namespace OthelloGameProj
{
    public static class GameConst
    {
        // ログの注意レベル
        // 明るい緑：正常(表示された場合は問題ない)
        // 黄色：管理方法が把握不足、必要なデータが用意されていない(改善のみでOK)
        // 赤：エラー、もしくはバグ(表示された場合は修正や改善が必要)
        public enum LogLevel { Lime, Yellow, Red }

        // 石の状態
        public enum StoneType
        {
            empty, // 何もない
            white, // 白
            black  // 黒
        }

        // 1辺の長さ
        public const int STONE_PUT_POINT = 8;
        
        // 石の初期位置
        public const int STONE_INIT_POS = 3;
        public const int STONE_FINISH_POS = 4;

        // 石の色の角度
        public const int STONE_BLACK_ROT = -90;
        public const int STONE_WHITE_ROT = 90;

        // タグ
        public const string PLAYER_TAG = "Player";

        // ゲーム難易度
        public enum GameDifficulty
        {
            Easy = 0,
            Normal,
            Hard
        }

        // ハンデ
        public const int MAX_CORNER_CNT = 4;
        public const int CORNER_POINT = 7;

        // ゲーム状態
        public enum GameState 
        {
            Title = 0,  // タイトル
            GameStart,  // ゲームスタート
            PlayerTurn, // プレイヤーの番
            NpcTurn,    // Npcの番
            Result      // リザルト
        }
        public const string GAMESTART_LABEL = "GameStart";
        public const string PLAYER_TURN = "Player Turn";
        public const string NPC_TURN = "Npc Turn";
        public const int INIT_TURN = 1;

        // ゲーム勝敗
        public enum GameWinOrLoss
        {
            None = 0,
            PlayerWin,
            NPCWin
        }
        public const string PLAYER_WIN_LABEL = "Player Win";
        public const string NPC_WIN_LABEL = "NPC Win";
        // パス最大回数
        public const int PASS_MAX_CNT = 3;
    }
}
