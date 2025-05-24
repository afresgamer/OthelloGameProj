using UnityEngine;
using UnityEngine.UI;

namespace OthelloGameProj
{
    /// <summary>
    /// BGMとSEの音量調整用のスライダー制御処理まとめ
    /// </summary>
    public class SliderController : MonoBehaviour
    {
        [SerializeField, Header("Bgmかどうか")]
        private bool isBgm;
        [SerializeField, Header("Seかどうか")]
        private bool isSe;

        private Slider slider;

        void Start()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(delegate { SetVolume(); });

            if (isBgm) AudioController.Instance.InitBGMVolume(slider);
            else if (isSe) AudioController.Instance.InitSEVolume(slider);
        }

        private void SetVolume()
        {
            if (isBgm) AudioController.Instance.SetBGM(slider.value);
            else if (isSe) AudioController.Instance.SetSE(slider.value);
        }
    }
}

