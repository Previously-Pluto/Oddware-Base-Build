using System.Collections.Generic;
using System.Linq;
using System.Text;
using Naninovel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class JournalEntryContentUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button galleryMediaBtn;
    [SerializeField] private Button videoMediaBtn;
    [SerializeField] private Button audioMediaBtn;

    [SerializeField] private JournalVideoPlayer videoPlayerUI;
    [SerializeField] private JournalPhotoGallery photoGalleryUI;

    [SerializeField] private Sprite mysteryImageSprite;

    private AudioSource audioSource;
    private JournalEntry currentEntry;
    private PlayScript playScript;
    private List<EntryTipData> generalTipDatas = new List<EntryTipData>();
    private List<EntryTipData> videoTipDatas = new List<EntryTipData>();
    private List<EntryTipData> audioTipDatas = new();
    private List<EntryTipData> galleryTipDatas = new();

    public JournalEntry CurrentEntry => currentEntry;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playScript = GetComponent<PlayScript>();
    }

    public void SetData(JournalEntry entry)
    {
        currentEntry = entry;
        image.sprite = entry == null ? mysteryImageSprite : entry.Image;
        titleText.text = entry == null ? "???" : entry.Title;
        text.text = entry == null ? "???" : entry.Text;

        videoMediaBtn.gameObject.SetActive(false);
        audioMediaBtn.gameObject.SetActive(false);
        galleryMediaBtn.gameObject.SetActive(false);
        videoMediaBtn.onClick.RemoveAllListeners();
        audioMediaBtn.onClick.RemoveAllListeners();
        galleryMediaBtn.onClick.RemoveAllListeners();

        if (entry == null) return;
        generalTipDatas =
            entry.AssociatedMedia.TipIDs.Where(x => x.TipType == TipType.General).ToList();
        
        SetPlayScript(generalTipDatas);
        playScript.Play();
        
        if (entry.AssociatedMedia.Video != null)
        {
            videoMediaBtn.gameObject.SetActive(true);

            videoTipDatas =
                entry.AssociatedMedia.TipIDs.Where(x => x.TipType == TipType.Video).ToList();

            videoMediaBtn.onClick.AddListener(() =>
            {
                videoPlayerUI.Open();
                SetPlayScript(videoTipDatas);
                videoPlayerUI.PlayVideo(entry);
                playScript.Play();
            });
        }

        if (entry.AssociatedMedia.Audio != null)
        {
            audioMediaBtn.gameObject.SetActive(true);
            audioTipDatas =
                entry.AssociatedMedia.TipIDs.Where(x => x.TipType == TipType.Audio).ToList();

            audioMediaBtn.onClick.AddListener(() =>
            {
                SetPlayScript(audioTipDatas);
                audioSource.PlayOneShot(entry.AssociatedMedia.Audio);
                playScript.Play();
            });
        }

        if (entry.AssociatedMedia.GalleryPics.Count > 0)
        {
            galleryMediaBtn.gameObject.SetActive(true);
            galleryTipDatas = entry.AssociatedMedia.TipIDs.Where(x => x.TipType == TipType.Gallery).ToList();
            galleryMediaBtn.onClick.AddListener(() =>
            {
                SetPlayScript(galleryTipDatas);
                photoGalleryUI.SetGallery(entry);
                photoGalleryUI.Open();
                playScript.Play();
            });
        }
    }

    public void SetPlayScript(List<EntryTipData> tipDatas)
    {
        StringBuilder stringBuilder = new StringBuilder();

        foreach (EntryTipData tipData in tipDatas)
        {
            stringBuilder.AppendLine($"@unlock Tips/{tipData.TipData.name}");
        }

        string newString = stringBuilder.ToString();
        playScript.SetPlayScriptText(newString);
    }

    public void UpdateCurrentEntry(JournalEntry newEntryUI = null)
    {
        if (newEntryUI == null)
        {
            SetData(currentEntry);
        }
        else
        {
            SetData(newEntryUI);
        }
    }

    public void Close()
    {
        videoPlayerUI.Close();
        photoGalleryUI.Close();
    }
}