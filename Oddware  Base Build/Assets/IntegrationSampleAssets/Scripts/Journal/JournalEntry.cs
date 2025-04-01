using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class JournalEntry
{
    public Sprite Image;
    public string Key;
    public string Title;
    public string Text;
    public EntryMedia AssociatedMedia;
    public JournalEntry(string key, string title, string text, EntryMedia media)
    {
        Key = key;
        Title = title;
        Text = text;
        AssociatedMedia = media;
        Image = media.MainImage;
    }
}