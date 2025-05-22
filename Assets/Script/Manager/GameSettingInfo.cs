using UnityEngine;

namespace OthelloGameProj
{
    /// <summary>
    /// ゲーム設定用の情報まとめ
    /// </summary>
    public class GameSettingInfo
    {
        // ゲーム難易度
        private GameConst.GameDifficulty gameDifficulty = GameConst.GameDifficulty.Easy;
        // 石の色
        private GameConst.StoneType stoneType = GameConst.StoneType.black;
        // ハンデ
        private int handicap = 0;

        public GameSettingInfo() 
        {
            gameDifficulty = GameConst.GameDifficulty.Easy;
            stoneType = GameConst.StoneType.black;
            handicap = 0;
        }

        public GameSettingInfo(GameConst.GameDifficulty difficulty, GameConst.StoneType type, int handi)
        {
            gameDifficulty = difficulty;
            stoneType = type;
            handicap = handi;
        }

        public void SetGameDifficulty(GameConst.GameDifficulty difficulty)
        {
            gameDifficulty = difficulty;
        }

        public void SetStoneType(GameConst.StoneType type)
        {
            stoneType = type;
        }

        public void SetHandicap(int handi)
        {
            handicap = handi;
        }

        public int GetHandicap() { return handicap; }

        public GameConst.GameDifficulty GetGameDifficulty() { return gameDifficulty; }

        public GameConst.StoneType GetStoneType() { return stoneType; }
    }
}

