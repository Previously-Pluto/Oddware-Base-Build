using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Naninovel;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Serialization;
using VHierarchy.Libs;

public class JournalManager : MonoBehaviour
{
    public event Action<JournalEntry> OnEntryAdded;

    [SerializeField] private UnityEvent showJournalUIEvent;
    [SerializeField] private List<JournalEntry> journalEntryList;
    [SerializeField] private List<string> testEntryKeys;

    public Dictionary<string, JournalEntry> JournalEntries = new();
    public static JournalManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void InsertNewEntry(JournalEntry entry, bool showJournalUI)
    {
        JournalEntries.Add(entry.Key, entry);
        journalEntryList.Add(entry);

        OnEntryAdded?.Invoke(entry);
        if (showJournalUI)
            showJournalUIEvent?.Invoke();
    }

    [ContextMenu("Insert Test Entries")]
    public async Task InsertTestEntries()
    {
        for (int i = 0; i < testEntryKeys.Count; i++)
        {
            string description = StringEntryProcessor.GetLocalizedString("Journal Entries", testEntryKeys[i]);
            string title = StringEntryProcessor.GetLocalizedString("Journal Titles", testEntryKeys[i]);

            var handle =
                Addressables.LoadAssetAsync<EntryMedia>($"Scriptable Objects/Journal System/{testEntryKeys[i]}.asset");
            EntryMedia media =
                await handle.Task;

            if (media == null)
                Debug.LogError($"{testEntryKeys[i]} media not loaded.");

            JournalEntry newJournalEntry = new JournalEntry(testEntryKeys[i], title, description, media);
            InsertNewEntry(newJournalEntry, false);
        }
    }
}