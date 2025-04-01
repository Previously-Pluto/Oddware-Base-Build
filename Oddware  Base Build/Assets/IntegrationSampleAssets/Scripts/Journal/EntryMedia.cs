using System;
using System.Collections.Generic;
using Naninovel.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;

[CreateAssetMenu(menuName = "Create Entry Media", fileName = "EntryMedia", order = 0)]
public class EntryMedia : ScriptableObject
{
    public Sprite MainImage;
    public VideoClip Video;
    public AudioClip Audio;
    public List<Sprite> GalleryPics;
    public List<EntryTipData> TipIDs;
}

[Serializable]
public class EntryTipData
{
    public TipType TipType = TipType.General;
    public TipData TipData;
}

public enum TipType
{
    None,
    General,
    Video,
    Audio,
    Gallery
}