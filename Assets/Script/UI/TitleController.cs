using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using OthelloGameProj;
using UnityEngine.SceneManagement;

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

    Sequence sequence;
    private readonly Color InitFadeColor = new Color(0, 0, 0, 0);
    private readonly float IntervalTime = 1.0f;

    void Start()
    {
        fadeImage.color = InitFadeColor;
        fadeImage.gameObject.SetActive(false);
        optionObj.SetActive(false);
    }

    public void OpenOption()
    {
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

    private void OnDisable()
    {
        sequence?.Kill();
    }
}
