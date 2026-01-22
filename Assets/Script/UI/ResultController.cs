using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

namespace OthelloGameProj
{
    /// <summary>
    /// リザルト画面の表示制御処理まとめ
    /// </summary>
    public class ResultController : MonoBehaviour
    {
        [SerializeField, Header("結果表示")]
        private TextMeshProUGUI resultLabel;
        [SerializeField, Header("プレイヤーの石の数")]
        private TextMeshProUGUI playerCntText;
        [SerializeField, Header("NPCの石の数")]
        private TextMeshProUGUI npcCntText;
        [SerializeField, Header("フェード用の画像")]
        private Image fadeImage;
        [SerializeField, Header("フェードのスピード")]
        private float alphaSpeed = 1.0f;

        Sequence sequence;

        void Start()
        {
            resultLabel.text = string.Empty;
            playerCntText.text = string.Empty;
            npcCntText.text = string.Empty;
            fadeImage.gameObject.SetActive(false);
        }

        void Update()
        {
            switch (OthelloGameManager.Instance.PlayerWinOrLose)
            {
                case GameConst.GameWinOrLoss.PlayerWin:

                    resultLabel.text = GameConst.PLAYER_WIN_LABEL;
                    SetStoneCnt();
                    break;
                case GameConst.GameWinOrLoss.NPCWin:

                    resultLabel.text = GameConst.NPC_WIN_LABEL;
                    SetStoneCnt();
                    break;
                default:
                    resultLabel.text = string.Empty;
                    break;
            }
        }

        void SetStoneCnt()
        {
            playerCntText.text = "Player Count: " + OthelloGameManager.Instance.GetPlayerStoneCnt().ToString();
            npcCntText.text = "NPC Count: " + OthelloGameManager.Instance.GetNPCStoneCnt().ToString();
        }

        public void Replay()
        {
            fadeImage.gameObject.SetActive(true);
            sequence = DOTween.Sequence();
            sequence.Append(fadeImage.DOColor(Color.black, alphaSpeed));
            sequence.Play()
                .OnComplete(() =>
                {
                    CustomDebugger.ColorLog("ゲームメインに再遷移します", GameConst.LogLevel.Lime);
                    OthelloGameManager.Instance.Init();
                    FlowManager.Instance.Init(GameConst.GameState.GameStart);
                    SceneManager.LoadScene((int)GameConst.GameState.GameStart);
                });
        }

        public void ChangeToTitle()
        {
            fadeImage.gameObject.SetActive(true);
            sequence = DOTween.Sequence();
            sequence.Append(fadeImage.DOColor(Color.black, alphaSpeed));
            sequence.Play()
                .OnComplete(() => {
                    CustomDebugger.ColorLog("タイトルに遷移します", GameConst.LogLevel.Lime);
                    OthelloGameManager.Instance.Init();
                    FlowManager.Instance.Init(GameConst.GameState.Title);
                    SceneManager.LoadScene((int)GameConst.GameState.Title);
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
