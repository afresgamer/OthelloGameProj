using OthelloGameProj;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnController : MonoBehaviour
{
    [SerializeField, Header("�p�X�{�^��")]
    private Button passBtn;
    [SerializeField, Header("�~�Q�{�^��")]
    private Button surrenderBtn;
    
    void Start()
    {
        passBtn.gameObject.SetActive(true);
        surrenderBtn.gameObject.SetActive(true);
    }

    void Update()
    {
        if (OthelloGameManager.Instance.PlayerWinOrLose != GameConst.GameWinOrLoss.None)
        {
            passBtn.interactable = false;
            surrenderBtn.interactable = false;
        }

        if (OthelloGameManager.Instance.PassCnt >= GameConst.PASS_MAX_CNT)
            passBtn.interactable = false;
    }

    public void Surrender()
    {
        OthelloGameManager.Instance.PlayerWinOrLose = GameConst.GameWinOrLoss.NPCWin;
    }

    public void Pass()
    {
        OthelloGameManager.Instance.PassCnt++;
        // ���Ԃ̍X�V
        FlowManager.Instance.SetState(GameConst.GameState.NpcTurn);
        FlowManager.Instance.SetTurn(1);
    }
}
