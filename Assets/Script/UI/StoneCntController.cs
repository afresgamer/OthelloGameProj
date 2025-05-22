using UnityEngine;
using TMPro;

namespace OthelloGameProj
{
    public class StoneCntController : MonoBehaviour
    {
        [SerializeField, Header("黒石の数")]
        private TextMeshProUGUI blackCntLabel;
        [SerializeField, Header("白石の数")]
        private TextMeshProUGUI whiteCntLabel;
        [SerializeField, Header("ボードの制御処理オブジェクト")]
        private BoardController boardController;

        void Start()
        {
            blackCntLabel.text = "Black : " + OthelloGameManager.Instance.BlackStoneList.Count;
            whiteCntLabel.text = "White : " + OthelloGameManager.Instance.WhiteStoneList.Count;
        }

        void Update()
        {
            blackCntLabel.text = "Black : " + OthelloGameManager.Instance.BlackStoneList.Count;
            whiteCntLabel.text = "White : " + OthelloGameManager.Instance.WhiteStoneList.Count;
        }
    }
}