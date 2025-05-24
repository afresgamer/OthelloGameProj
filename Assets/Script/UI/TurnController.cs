using UnityEngine;
using TMPro;

namespace OthelloGameProj
{
    /// <summary>
    /// ターン数の表示制御のまとめ
    /// </summary>
    public class TurnController : MonoBehaviour
    {
        [SerializeField, Header("ターン数")]
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
