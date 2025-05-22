using UnityEngine;
using TMPro;
using OthelloGameProj;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class ResultController : MonoBehaviour
{
    [SerializeField, Header("���ʕ\��")]
    private TextMeshProUGUI resultLabel;
    [SerializeField, Header("�v���C���[�̐΂̐�")]
    private TextMeshProUGUI playerCntText;
    [SerializeField, Header("NPC�̐΂̐�")]
    private TextMeshProUGUI npcCntText;
    [SerializeField, Header("�t�F�[�h�p�̉摜")]
    private Image fadeImage;
    [SerializeField, Header("�t�F�[�h�̃X�s�[�h")]
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
                CustomDebugger.ColorLog("�Q�[�����C���ɑJ�ڂ��܂�", GameConst.LogLevel.Lime);
                OthelloGameManager.Instance.Init();
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
                CustomDebugger.ColorLog("�Q�[�����C���ɑJ�ڂ��܂�", GameConst.LogLevel.Lime);
                SceneManager.LoadScene((int)GameConst.GameState.Title);
            });
    }

    private void OnDisable()
    {
        sequence?.Kill();
    }
}
