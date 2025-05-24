using DG.Tweening;
using OthelloGameProj;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OthelloGameProj
{
    /// <summary>
    /// ゲームメインのボタンの制御処理まとめ
    /// </summary>
    public class BtnController : MonoBehaviour
    {
        [SerializeField, Header("パスボタン")]
        private Button passBtn;
        [SerializeField, Header("降参ボタン")]
        private Button surrenderBtn;
        [SerializeField, Header("オプションボタン")]
        private Button optionBtn;
        [SerializeField, Header("ボタンのSE音源")]
        private AudioClip btnSeClip;
        [SerializeField, Header("Option画面")]
        private GameObject optionObj;
        [SerializeField, Header("画面サイズのスケールスピード")]
        private float scaleSpeed = 1.0f;

        Sequence sequence;

        void Start()
        {
            passBtn.gameObject.SetActive(true);
            surrenderBtn.gameObject.SetActive(true);
            optionBtn.gameObject.SetActive(true);
        }

        void Update()
        {
            if (OthelloGameManager.Instance.PlayerWinOrLose != GameConst.GameWinOrLoss.None)
            {
                passBtn.interactable = false;
                surrenderBtn.interactable = false;
                optionBtn.interactable = false;
            }

            if (OthelloGameManager.Instance.PassCnt >= GameConst.PASS_MAX_CNT)
                passBtn.interactable = false;
        }

        public void Surrender()
        {
            AudioController.Instance.PlaySE(btnSeClip);
            OthelloGameManager.Instance.PlayerWinOrLose = GameConst.GameWinOrLoss.NPCWin;
        }

        public void Pass()
        {
            AudioController.Instance.PlaySE(btnSeClip);
            OthelloGameManager.Instance.PassCnt++;
            // 順番の更新
            FlowManager.Instance.SetState(GameConst.GameState.NpcTurn);
            FlowManager.Instance.SetTurn(1);
        }

        public void OpenOption()
        {
            AudioController.Instance.PlaySE(btnSeClip);
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
            AudioController.Instance.PlaySE(btnSeClip);
            sequence = DOTween.Sequence();
            sequence.Append(optionObj.GetComponent<RectTransform>().DOScale(Vector3.zero, scaleSpeed))
                .SetLink(optionObj);
            sequence.Play()
                .OnComplete(() => {
                    optionObj.SetActive(false);
                    CustomDebugger.ColorLog("オプション画面が閉じました", GameConst.LogLevel.Lime);
                });
        }

        public void OnPointerEnter()
        {
            OthelloGameManager.Instance.IsOpenOption = true;
        }

        public void OnPointerExit()
        {
            OthelloGameManager.Instance.IsOpenOption = false;
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