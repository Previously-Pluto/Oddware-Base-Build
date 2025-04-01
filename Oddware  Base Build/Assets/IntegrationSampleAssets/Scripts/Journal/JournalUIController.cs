using System;
using System.Collections.Generic;
using System.Linq;
using Naninovel.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class JournalUIController : MonoBehaviour
{
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private int poolSize;
    [SerializeField] private JournalEntryContentUI journalContentUI;
    [SerializeField] private JournalEntryUI journalEntryUI;
    [SerializeField] private GameObject journalEntryListContainer;
    [SerializeField] private Button exitButton;

    private JournalManager journalManager;
    private Queue<JournalEntryUI> journalUIQueue = new Queue<JournalEntryUI>();

    public static JournalUIController Instance;

    private void Awake()
    {
        Instance = this;
        journalManager = FindObjectOfType<JournalManager>();

        for (int i = 0; i < poolSize; i++)
        {
            JournalEntryUI newContentUI = Instantiate(journalEntryUI, journalEntryListContainer.transform);
            journalUIQueue.Enqueue(newContentUI);
            newContentUI.gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        journalManager.OnEntryAdded += HandleDequeueEntry;
        exitButton.onClick.AddListener(() => journalContentUI.Close());
    }

    private void OnDisable()
    {
        journalManager.OnEntryAdded -= HandleDequeueEntry;
        exitButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Updates Journal UI by adding a new entry
    /// to the entry collection.
    /// </summary>
    /// <param name="entry"></param>
    private void HandleDequeueEntry(JournalEntry entry)
    {
        JournalEntryUI newEntryUI = null;

        if (journalUIQueue.Count <= 0)
            newEntryUI = Instantiate(journalEntryUI, journalEntryListContainer.transform);
        else
            newEntryUI = journalUIQueue.Dequeue();

        newEntryUI.SetData(entry);
        newEntryUI.gameObject.SetActive(true);
        newEntryUI.Button.onClick.RemoveAllListeners();
        newEntryUI.Button.onClick.AddListener(() => journalContentUI.UpdateCurrentEntry(entry));
        newEntryUI.OnFinishedUse += HandleEnqueueEntry;

        if (journalContentUI.CurrentEntry == null)
            journalContentUI.UpdateCurrentEntry(entry);
    }

    private void HandleEnqueueEntry(JournalEntryUI entryContent)
    {
        entryContent.OnFinishedUse -= HandleEnqueueEntry;
        journalUIQueue.Enqueue(entryContent);
        entryContent.gameObject.SetActive(false);
    }
}