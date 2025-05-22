using UnityEngine;
using TMPro;

namespace OthelloGameProj
{
    public class StoneCntController : MonoBehaviour
    {
        [SerializeField, Header("���΂̐�")]
        private TextMeshProUGUI blackCntLabel;
        [SerializeField, Header("���΂̐�")]
        private TextMeshProUGUI whiteCntLabel;
        [SerializeField, Header("�{�[�h�̐��䏈���I�u�W�F�N�g")]
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