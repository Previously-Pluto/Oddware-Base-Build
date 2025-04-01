using System;
using UnityEngine;
using UnityEngine.Video;

public class JournalVideoPlayer : ContentPlayer
{
    private CanvasGroup mainUI;
    private VideoPlayer player;

    private void Awake()
    {
        mainUI = GetComponentInChildren<CanvasGroup>();
        player = GetComponent<VideoPlayer>();
    }

    public void PlayVideo(JournalEntry entry)
    {
        player.clip = entry.AssociatedMedia.Video;
        player.Play();
    }
   
    public void ToggleCanvasGroup(CanvasGroup group, bool toggle)
    {
        if (toggle)
        {
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;
        }
        else
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }
}