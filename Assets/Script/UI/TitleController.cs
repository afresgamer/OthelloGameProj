using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using OthelloGameProj;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

namespace OthelloGameProj
{
    /// <summary>
    /// タイトルの制御処理のまとめ
    /// </summary>
    public class TitleController : MonoBehaviour
    {
        [SerializeField, Header("フェード用の画像")]
        private Image fadeImage;
        [SerializeField, Header("Option画面")]
        private GameObject optionObj;
        [SerializeField, Header("GameSetting画面")]
        private GameObject gameSettingObj;
        [SerializeField, Header("画面サイズのスケールスピード")]
        private float scaleSpeed = 1.0f;
        [SerializeField, Header("フェードのスピード")]
        private float alphaSpeed = 1.0f;
        [SerializeField, Header("ゲーム難易度のドロップダウン")]
        private TMP_Dropdown gameDiffDropdown;
        [SerializeField, Header("プレイヤーの石の色")]
        private ToggleGroup toggleGroup;
        [SerializeField, Header("ハンデのスライダー")]
        private Slider handicap;
        [SerializeField, Header("ハンデの数値")]
        private TextMeshProUGUI handText;
        [SerializeField, Header("SEの音源")]
        private AudioClip seClip;

        Sequence sequence;
        private readonly Color InitFadeColor = new Color(0, 0, 0, 0);
        private readonly float IntervalTime = 1.0f;
        private readonly string BlackStoneStr = "Black";
        private readonly string WhiteStoneStr = "White";

        void Start()
        {
            fadeImage.color = InitFadeColor;
            fadeImage.gameObject.SetActive(false);
            optionObj.SetActive(false);
            handicap.onValueChanged.AddListener(delegate { OnValueChange(); });
            AudioController.Instance.Init();
        }

        public void OpenOption()
        {
            AudioController.Instance.PlaySE(seClip);
            optionObj.SetActive(true);
            sequence = DOTween.Sequence();
            sequence.Append(optionObj.GetComponent<RectTransform>().DOScale(Vector3.one, scaleSpeed))
                .SetLink(optionObj);
            sequence.Play()
                .OnComplete(() => {
                    CustomDebugger.ColorLog("オプション画面が開きました", GameConst.LogLevel.Lime);
                });
        }

        public void CloseOption()
        {
            AudioController.Instance.PlaySE(seClip);
            sequence = DOTween.Sequence();
            sequence.Append(optionObj.GetComponent<RectTransform>().DOScale(Vector3.zero, scaleSpeed))
                .SetLink(optionObj);
            sequence.Play()
                .OnComplete(() => {
                    optionObj.SetActive(false);
                    CustomDebugger.ColorLog("オプション画面が閉じました", GameConst.LogLevel.Lime);
                });
        }

        public void OpenGameSetting()
        {
            AudioController.Instance.PlaySE(seClip);
            gameSettingObj.SetActive(true);
            sequence = DOTween.Sequence();
            sequence.Append(gameSettingObj.GetComponent<RectTransform>().DOScale(Vector3.one, scaleSpeed))
                .SetLink(gameSettingObj);
            sequence.Play()
                .OnComplete(() => {
                    CustomDebugger.ColorLog("ゲーム設定が開きました", GameConst.LogLevel.Lime);
                });
        }

        public void CloseGameSetting()
        {
            AudioController.Instance.PlaySE(seClip);
            sequence = DOTween.Sequence();
            sequence.Append(gameSettingObj.GetComponent<RectTransform>().DOScale(Vector3.zero, scaleSpeed))
                .SetLink(gameSettingObj);
            sequence.Play()
                .OnComplete(() => {
                    gameSettingObj.SetActive(false);
                    CustomDebugger.ColorLog("ゲーム設定が閉じました", GameConst.LogLevel.Lime);
                });
        }

        public void MainChange()
        {
            AudioController.Instance.PlaySE(seClip);
            var stoneType = GameConst.StoneType.empty;
            var toggles = GameObject.FindObjectsByType<Toggle>(FindObjectsSortMode.None);
            var list = toggles.Where(x => x.group == toggleGroup).ToList();
            var toggle = list.Where(x => x.isOn).FirstOrDefault();
            if (toggle != null)
            {
                if (toggle.name.Contains(BlackStoneStr)) stoneType = GameConst.StoneType.black;
                else if (toggle.name.Contains(WhiteStoneStr)) stoneType = GameConst.StoneType.white;
            }

            OthelloGameManager.Instance.SetGameSettingInfo((GameConst.GameDifficulty)gameDiffDropdown.value, stoneType, (int)handicap.value);

            fadeImage.gameObject.SetActive(true);
            sequence = DOTween.Sequence();
            sequence.Append(fadeImage.DOColor(Color.black, alphaSpeed));
            sequence.AppendInterval(IntervalTime);
            sequence.Play()
                .OnComplete(() => {
                    CustomDebugger.ColorLog("ゲームメインに遷移します", GameConst.LogLevel.Lime);
                    FlowManager.Instance.Init(GameConst.GameState.GameStart);
                    OthelloGameManager.Instance.Init();
                    SceneManager.LoadScene((int)GameConst.GameState.GameStart);
                });
        }

        public void OnValueChange()
        {
            handText.text = handicap.value.ToString();
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
