using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace OthelloGameProj
{
    /// <summary>
    /// ゲーム状態を表示する処理制御
    /// </summary>
    public class GameStateController : MonoBehaviour
    {
        [SerializeField, Header("表示ラベル")]
        private TextMeshProUGUI gameStateLabel;
        [SerializeField, Header("結果表示")]
        private GameObject resultObj;
        [SerializeField, Header("画面サイズのスケールスピード")]
        private float scaleSpeed = 1.0f;
        [SerializeField, Header("フェード用の画像")]
        private Image fadeImage;
        [SerializeField, Header("フェードのスピード")]
        private float alphaSpeed = 1.0f;
        [SerializeField, Header("ボードの制御処理オブジェクト")]
        private BoardController boardController;

        Sequence sequence;
        private readonly Color InitFadeColor = new Color(0, 0, 0, 0);
        private readonly float IntervalTime = 1.0f;
        bool isSetResult = false;

        void Start()
        {
            gameStateLabel.text = string.Empty;
            resultObj.SetActive(false);
            StartFade();
            isSetResult = false;
        }

        void Update()
        {
            if (OthelloGameManager.Instance.PlayerWinOrLose != GameConst.GameWinOrLoss.None && !isSetResult)
            {
                gameStateLabel.text = string.Empty;
                SetResultObj();
                isSetResult = true;
            }

            switch (FlowManager.Instance.NowGameState) 
            {
                case GameConst.GameState.GameStart:
                    gameStateLabel.text = GameConst.GAMESTART_LABEL;
                    break;
                case GameConst.GameState.PlayerTurn:
                    gameStateLabel.text = GameConst.PLAYER_TURN;
                    break;
                case GameConst.GameState.NpcTurn:
                    gameStateLabel.text = GameConst.NPC_TURN;
                    break;
                default:
                    gameStateLabel.text = string.Empty;
                    break;
            }
        }

        private void SetResultObj()
        {
            resultObj.SetActive(true);
            sequence = DOTween.Sequence();
            sequence.Append(resultObj.GetComponent<RectTransform>().DOScale(Vector3.one, scaleSpeed));
            sequence.Play()
                .OnComplete(() =>
                {
                    FlowManager.Instance.SetState(GameConst.GameState.Result);
                    CustomDebugger.ColorLog("リザルトを表示します", GameConst.LogLevel.Lime);
                });
        }

        private void StartFade()
        {
            OthelloGameManager.Instance.IsGameStart = true;
            // ボードの制御処理オブジェクトの初期化
            boardController.Init();
            // BGMの変更
            AudioController.Instance.Init();

            sequence = DOTween.Sequence();
            sequence.Append(fadeImage.DOColor(InitFadeColor, alphaSpeed));
            sequence.AppendInterval(IntervalTime);
            sequence.Play()
                .OnComplete(() => {
                    CustomDebugger.ColorLog("ゲームメインを開始します", GameConst.LogLevel.Lime);
                    fadeImage.gameObject.SetActive(false);
                });
        }

        private void OnEnable()
        {
            sequence = DOTween.Sequence();
        }

        private void OnDisable()
        {
            sequence?.Kill();
        }
    }
}
