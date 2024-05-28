using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer vp;
    public VideoClip[] clips;
    private int currentVideo = 0;

    void Start()
    {
        if (clips != null && clips.Length > 0)
        {
            vp.clip = clips[currentVideo];
        }
        else
        {
            Debug.LogError("No video clips assigned.");
        }
    }

    void Update()
    {
        // Update method currently not needed
    }

    public void PlayVideo()
    {
        if (vp.clip != null)
        {
            vp.Play();
            Debug.Log("Playing video: " + vp.clip.name);
        }
        else
        {
            Debug.LogError("No video clip assigned to VideoPlayer.");
        }
    }

    public void StopVideo()
    {
        if (vp.clip != null)
        {
            vp.Stop();
            Debug.Log("Stopping video: " + vp.clip.name);
        }
        else
        {
            Debug.LogError("No video clip assigned to VideoPlayer.");
        }
    }

    public void NextVideo()
    {
        if (clips != null && clips.Length > 0)
        {
            currentVideo++;
            if (currentVideo >= clips.Length)
            {
                currentVideo = 0;
            }
            vp.clip = clips[currentVideo];
            vp.Play();
            Debug.Log("Playing next video: " + vp.clip.name);
        }
        else
        {
            Debug.LogError("No video clips assigned.");
        }
    }
}
