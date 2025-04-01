using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JournalEntryUI : MonoBehaviour
{
    public event Action<JournalEntryUI> OnFinishedUse;

    [SerializeField] private Image image;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private Button button;

    public Button Button => button;
    private JournalEntry assignedEntry;

    public JournalEntry AssignedEntry => assignedEntry;

    public void SetData(JournalEntry entry)
    {
        image.sprite = entry.Image;
        titleText.text = entry.Title;
    }
}