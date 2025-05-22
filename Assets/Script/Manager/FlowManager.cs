namespace OthelloGameProj
{
    /// <summary>
    /// ゲーム状態の管理制御
    /// </summary>
    public class FlowManager : SingletonMonoBehaviour<FlowManager>
    {
        /// <summary>
        /// ターン数
        /// </summary>
        public int Turn {  get; private set; }
        /// <summary>
        /// ゲーム状態
        /// </summary>
        public GameConst.GameState NowGameState { get; private set; }

        /// <summary>
        /// ターン数を設定
        /// </summary>
        /// <param name="turn">追加するターン数</param>
        public void SetTurn(int turn) { Turn += turn; }

        /// <summary>
        /// ゲーム状態を設定
        /// </summary>
        /// <param name="state">ゲーム状態</param>
        public void SetState(GameConst.GameState state) { NowGameState = state; }

        /// <summary>
        /// プレイヤー番かどうか
        /// </summary>
        /// <param name="playerTurn">プレイヤーのターン数</param>
        /// <returns></returns>
        public bool IsPlayerTurn() { return Turn % 2 == 1; }

        /// <summary>
        /// NPC番かどうか
        /// </summary>
        /// <param name="npcTurn">NPCのターン数</param>
        /// <returns></returns>
        public bool IsNpcTurn() { return Turn % 2 == 0; }

        protected override void Awake()
        {
            base.Awake();
        }

        public void Init(GameConst.GameState gameState)
        {
            Turn = GameConst.INIT_TURN;
            NowGameState = gameState;
        }
    }
}
