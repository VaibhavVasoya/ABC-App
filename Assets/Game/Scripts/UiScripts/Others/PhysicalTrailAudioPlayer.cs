using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PhysicalTrailAudioPlayer : MonoBehaviour
{
    [SerializeField] AudioSource audioPlayer;

    //[SerializeField] string shortAudioUrl;

    //[SerializeField] string longAudioUrl;



    bool isPlaying;

    [SerializeField] Sprite playSprite;
    [SerializeField] Sprite pauseSprite;

    [SerializeField] Image playPauseButton;

    [SerializeField] List<SoundWaveAnimate> soundWaveAnimateList;

    [SerializeField] Text audioNameText;
    [SerializeField] CanvasGroup audioPlayerCanvasGroup;

    public async void TogglePlayPauseState()
    {
        if (isPlaying)
        {

            playPauseButton.sprite = playSprite;
            audioPlayer.Pause();
            ToggleSoundWaveAnimation(false);
        }
        else
        {
            if (audioPlayer.clip == null) return;
            playPauseButton.sprite = pauseSprite;
            Debug.Log("pre play");
            await Task.Delay(500);
            audioPlayer.Play();
            Debug.Log("post play");
            ToggleSoundWaveAnimation(true);
        }

        isPlaying = !isPlaying;
    }

    IEnumerator temp;

    public void PlayAudio()
    {
        //AudioClip clip;

        //audioPlayer.PlayOneShot(isLongAudio ? longClip : shortClip);

        StopAudio();
        //await Task.Delay(1000);
        //        StopCoroutine(nameof(AudioPlayerFadeInOut));

        //if (temp == null)
        //{
        //    StartCoroutine(temp = AudioPlayerFadeInOut(true));
        //}



        //if (isLongAudio)
        //{
        //    //await Services.DownloadAudio(TrailsHandler.instance.CurrentTrailPoi.long_audio, (x) => audioPlayer.clip = x);
        //    //audioPlayer.clip = longClip;

        //}
        //else
        //{
        //    //await Services.DownloadAudio(TrailsHandler.instance.CurrentTrailPoi.short_audio, (x) => audioPlayer.clip = x);
        //    //audioPlayer.clip = shortClip;
        //}

        TogglePlayPauseState();
    }

    private void Update()
    {
        if( isPlaying && !audioPlayer.isPlaying)
        {
            StopAudio();
        }
    }


    public void StopAudio()
    {
        audioPlayer.Stop();
        //audioPlayer.clip = null;
        ToggleSoundWaveAnimation(false);
        playPauseButton.sprite = playSprite;
        isPlaying = false;


        //StopCoroutine(nameof(AudioPlayerFadeInOut));
        //StartCoroutine(nameof(AudioPlayerFadeInOut), false);
    }

    //public void StopCoroutineFadeAudio()
    //{
    //    if (temp != null)
    //        StartCoroutine(AudioPlayerFadeInOut(false));
    //    temp = null;
    //    //if (temp==null)
    //    //{

    //    //    //StopCoroutine(temp);
    //    //    //temp = null;
    //    //}
    //}
    public void Alpha0()
    {
        audioPlayerCanvasGroup.alpha = 0;
    }
    //public IEnumerator AudioPlayerFadeInOut(bool fadeIn)
    //{
    //    if (!fadeIn)
    //    {
    //        float timeCounter = 0.5f;
    //        while (timeCounter > 0f)
    //        {
    //            timeCounter -= Time.deltaTime;
    //            audioPlayerCanvasGroup.alpha = timeCounter * 2;
    //            yield return null;
    //        }
    //        audioPlayerCanvasGroup.alpha = 0;
    //    }
    //    else
    //    {
    //        float timeCounter = 0f;
    //        while (timeCounter < 0.5f)
    //        {
    //            timeCounter += Time.deltaTime;
    //            audioPlayerCanvasGroup.alpha = timeCounter * 2;
    //            yield return null;
    //        }
    //        audioPlayerCanvasGroup.alpha = 1;
    //    }
    //}


    public void AssignAudioClip()
    {
//        Debug.LogError("num : " + TrailsHandler.instance.CurrentTrailPoi.num + " || File : " + TrailsHandler.instance.CurrentTrailPoi.audio_file);
        Services.DownloadAudio(TrailsHandler.instance.CurrentTrailPoi.num, TrailsHandler.instance.CurrentTrailPoi.audio_file, (x) => { audioPlayer.clip = x;});
    }
    //public async void SetPlayData(bool isLongAudio)
    //{
    //    isPlaying = false;

    //    if(isLongAudio)
    //    {
    //        //here download and assign the long clip
    //        await Services.DownloadAudio(longAudioUrl,(x)=>audioPlayer.clip = x);

    //    }
    //    else
    //    {
    //        //here download and assign the short clip
    //        await Services.DownloadAudio(shortAudioUrl, (x) => audioPlayer.clip = x);
    //    }

    //    if(string.IsNullOrEmpty(audioPlayer.clip.name)==false && audioPlayer.clip.name.Length>10)
    //    {
    //        audioNameText.text = audioPlayer.clip.name.Substring(0, 10) + "...";
    //    }
    //    else
    //    {
    //        Debug.Log("No name found");
    //    }

    //    //audioPlayer.clip = new AudioClip();
    //    //here download the audio clip from audio url given
    //}

    //public void SetClips(string longClipUrl, string shortClipUrl)
    //{
    //    Services.DownloadAudio(longClipUrl, (x) => longClip = x);
    //    Services.DownloadAudio(shortClipUrl, (x) => shortClip = x);
    //}

    public void ToggleSoundWaveAnimation(bool animationOn)
    {
        if (animationOn)
        {
            foreach (SoundWaveAnimate s in soundWaveAnimateList)
            {
                s.AnimateStart();
            }
        }
        else
        {
            foreach (SoundWaveAnimate s in soundWaveAnimateList)
            {
                s.AnimateStop();
            }
        }


    }
}
