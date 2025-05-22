using UnityEngine;
using TMPro;

namespace OthelloGameProj
{
    public class TurnController : MonoBehaviour
    {
        [SerializeField, Header("�^�[����")]
        private TextMeshProUGUI turnLabel;

        void Start()
        {
            turnLabel.text = "Turn : " + FlowManager.Instance.Turn.ToString();
        }

        void Update()
        {
            turnLabel.text = "Turn : " + FlowManager.Instance.Turn.ToString();
        }
    }
}
