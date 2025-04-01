using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class JournalPhotoGallery : ContentPlayer
{
    [SerializeField] private Image displayImage;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    [SerializeField] private TMP_Text galleryCountCurrent;
    [SerializeField] private TMP_Text galleryCountMax;
    
    private int currentIndex;
    private List<Sprite> pics = new List<Sprite>();
  
    public void SetGallery(JournalEntry entry)
    {
        pics = entry.AssociatedMedia.GalleryPics;
        galleryCountCurrent.text = (currentIndex + 1).ToString();
        galleryCountMax.text = pics.Count.ToString();
        displayImage.sprite = pics[currentIndex];
        nextButton.onClick.RemoveAllListeners();
        previousButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(() =>
        {
            MoveToNext();
        });
        
        previousButton.onClick.AddListener(() =>
        {
            MoveToPrevious();
        });
    }

    public void MoveToNext()
    {
        currentIndex = (currentIndex + 1) % (pics.Count);
        Debug.Log($"Current index is {currentIndex} Incre");
        displayImage.sprite = pics[currentIndex];
        galleryCountCurrent.text = (currentIndex + 1).ToString();
    }

    public void MoveToPrevious()
    {
        currentIndex = (currentIndex - 1 + (pics.Count)) % (pics.Count);
        Debug.Log($"Current index is {currentIndex} Decre");

        displayImage.sprite = pics[currentIndex];
        galleryCountCurrent.text = (currentIndex + 1).ToString();
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