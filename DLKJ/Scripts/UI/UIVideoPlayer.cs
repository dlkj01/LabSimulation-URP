using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

namespace DLKJ
{
    public class UIVideoPlayer : MonoBehaviour
    {
        [SerializeField] RawImage rawImage;
        [SerializeField] VideoPlayer videoPlayer;
        [SerializeField] Button playOrPauseButton;
        [SerializeField] Sprite playSprite;
        [SerializeField] Sprite pauseSprite;
        [SerializeField] Slider slider;

        private bool play = false;
        float totalFrame = 1.0f;

        // Start is called before the first frame update
        void Start()
        {
            slider.value = 0;
            rawImage.texture = videoPlayer.texture;
            playOrPauseButton.onClick.AddListener(delegate { PlayOrPause(); });
            totalFrame = (float)videoPlayer.clip.frameCount;
        }

        private void Update()
        {
            if (play)
            {
                if (slider.value < 1)
                {
                    slider.value = (videoPlayer.frame / totalFrame);
                }
                rawImage.texture = videoPlayer.texture;
            }
        }

        public void Play()
        {
            videoPlayer.clip = SceneManager.GetInstance().currentLab.currentStep.videoClip;
            videoPlayer.Play();
            play = true;
            rawImage.texture = videoPlayer.texture;
        }

        public void Reset() {
            videoPlayer.Stop();
        }

        void PlayOrPause()
        {
            if (play)
            {
                play = false;
                videoPlayer.Pause();
                playOrPauseButton.image.sprite = pauseSprite;
            }
            else
            {
                play = true;
                videoPlayer.Play();
                playOrPauseButton.image.sprite = playSprite;
            }
        }
    }
}