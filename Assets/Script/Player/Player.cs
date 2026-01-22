using UnityEngine;

namespace OthelloGameProj
{
    /// <summary>
    /// プレイヤーの管理
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField, Header("置き場所レイヤー")]
        private LayerMask layerMask;
        [SerializeField, Header("ボードの制御処理オブジェクト")]
        private BoardController boardController;
        [SerializeField, Header("置ける場所の可視化の制御オブジェクト")]
        private StonePlaneController stonePlaneController;
        [SerializeField, Header("石置く時のSE音源")]
        private AudioClip putStoneClip;

        PlayerInput playerInput;
        readonly float drawRayDistance = 300f;
        StonePlane currentPlane;

        void Start()
        {
            playerInput = GetComponent<PlayerInput>();
            tag = GameConst.PLAYER_TAG;
            currentPlane = null;
        }

        void Update()
        {
            // ゲームスタートしてなかったらプレイヤー操作不可
            if (!OthelloGameManager.Instance.IsGameStart) return;
            // オプション画面が開いていたらプレイヤー操作不可
            if (OthelloGameManager.Instance.IsOpenOption) return;
            // プレイヤーのターンではない場合操作不可
            if (!FlowManager.Instance.IsPlayerTurn()) return;
            //　勝敗が決定したら操作不可
            if (OthelloGameManager.Instance.PlayerWinOrLose != GameConst.GameWinOrLoss.None) return;

            // 置ける場所を表示
            if (CheckStonePlane())
                stonePlaneController.UpdatePlaneMaterial(currentPlane, true);
            else stonePlaneController.UpdatePlaneMaterial(currentPlane, false);

            // 置ける場所を選択済み かつ クリック時
            if (stonePlaneController.CheckPlaneMaterial(currentPlane) && playerInput.IsClick)
            {
                // オプション画面が開いていたら未選択状態にして処理を終える
                if (OthelloGameManager.Instance.IsOpenOption)
                {
                    stonePlaneController.UpdatePlaneMaterial(currentPlane, false);
                    currentPlane = null;
                    return;
                }

                // 置ける場所を初期化
                stonePlaneController.UpdatePlaneMaterial(currentPlane, false);
                
                // 置くandひっくり返す
                boardController.AddStone(currentPlane.StoneInfo, OthelloGameManager.Instance.PlayerType);
                stonePlaneController.SetPlane(currentPlane, false);
                boardController.TurnoverStone(currentPlane.StoneInfo, OthelloGameManager.Instance.PlayerType, () =>
                {
                    // 順番の更新
                    FlowManager.Instance.SetState(GameConst.GameState.NpcTurn);
                    FlowManager.Instance.SetTurn(1);
                });

                // 置き場所の更新
                currentPlane = null;
                stonePlaneController.SetPlaneList(false);
                AudioController.Instance.PlaySE(putStoneClip);
            }
        }

        /// <summary>
        /// 置ける場所にレイを飛ばして置ける場所情報が取得出来るか確認する
        /// </summary>
        /// <returns>置ける場所情報が取得出来たかどうか</returns>
        private bool CheckStonePlane()
        {
            Ray ray = Camera.main.ScreenPointToRay(playerInput.MousePos);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, drawRayDistance, layerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.red, drawRayDistance);
                if (hitInfo.collider.gameObject.TryGetComponent<StonePlane>(out var plane))
                {
                    var cell = plane.StoneInfo.GetCell();
                    currentPlane = plane;
                    return true;
                }
                else currentPlane = null;
            }
            return false;
        }
    }
}
