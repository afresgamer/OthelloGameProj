using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace OthelloGameProj
{
    /// <summary>
    /// BGM、SEの制御処理のまとめ
    /// </summary>
    public class AudioController : SingletonMonoBehaviour<AudioController>
    {
        [SerializeField, Header("BGMを設定、調整するオブジェクト")]
        private GameObject bgmObj;
        [SerializeField, Header("SEを設定、調整するオブジェクト")]
        private GameObject seObj;
        [SerializeField, Header("Audioミキサー")] 
        private AudioMixer audioMixer;
        [SerializeField, Header("タイトルのBGM音源")]
        private AudioClip titleClip;
        [SerializeField, Header("ゲームメインのBGM音源")]
        private AudioClip mainClip;

        private GameObject currentBgm = null;
        private GameObject currentSe = null;
        private const string BGM_AUDIO_MIXER_STR = "BgmVolume";
        private const string SE_AUDIO_MIXER_STR = "SeVolume";

        public void Init()
        {
            var bgm = GameObject.Find(bgmObj.name);
            if (bgm == null)
            {
                var newBgmObj = Instantiate(bgmObj, Vector3.zero, Quaternion.identity);
                currentBgm = newBgmObj;
            }
            else currentBgm = bgm;
            var se = GameObject.Find(seObj.name);
            if (se == null)
            {
                var newSeObj = Instantiate(seObj, Vector3.zero, Quaternion.identity);
                currentSe = newSeObj;
            }
            else currentSe = se;

            // BGMの設定を行う
            var bgmSource = currentBgm.GetComponent<AudioSource>();
            if (bgmSource != null)
            {
                bgmSource.clip = FlowManager.Instance.NowGameState == GameConst.GameState.Title ? titleClip : mainClip;
                bgmSource.loop = true;
                bgmSource.Play();
            }
        }

        public void InitBGMVolume(Slider BGMSlider)
        {
            //BGM
            audioMixer.GetFloat(BGM_AUDIO_MIXER_STR, out float bgmVolume);
            BGMSlider.value = bgmVolume;
        }

        public void InitSEVolume(Slider SESlider)
        {
            //SE
            audioMixer.GetFloat(SE_AUDIO_MIXER_STR, out float seVolume);
            SESlider.value = seVolume;
        }

        public void SetBGM(float volume)
        {
            audioMixer.SetFloat(BGM_AUDIO_MIXER_STR, volume);
        }

        public void SetSE(float volume)
        {
            audioMixer.SetFloat(SE_AUDIO_MIXER_STR, volume);
        }

        public void PlaySE(AudioClip seClip)
        {
            currentSe.GetComponent<AudioSource>().PlayOneShot(seClip);
        }
    }
}
