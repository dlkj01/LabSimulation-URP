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
        [SerializeField] Button loopModelButton;
        [SerializeField] Sprite playSprite;
        [SerializeField] Sprite pauseSprite;
        [SerializeField] Sprite loopSprite;
        [SerializeField] Text loopText;
        [SerializeField] Slider slider;

        private bool play = false;
        float totalFrame = 1.0f;

        // Start is called before the first frame update
        void Start()
        {
            loopText.text = "";
            slider.value = 0;
            loopModelButton.image.sprite = loopSprite;
            rawImage.texture = videoPlayer.texture;
            playOrPauseButton.onClick.AddListener(delegate { PlayOrPause(); });
            loopModelButton.onClick.AddListener(delegate { Loop(); });
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
            videoPlayer.Play();
            play = true;
            rawImage.texture = videoPlayer.texture;
        }

        public void Reset() {
            videoPlayer.Stop();
        }

        void Loop()
        {
            if (videoPlayer.isLooping)
            {
                loopText.text = "";
                videoPlayer.isLooping = false;
            }
            else
            {
                loopText.text = "1";
                videoPlayer.isLooping = true;
            }
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