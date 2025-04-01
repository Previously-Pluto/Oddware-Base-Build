using Naninovel;
using UnityEditor;
using UnityEngine;

[CommandAlias("activateJournalEntry")]
public class ActivateJournalEntry : Command
{
    [ParameterAlias("key")]
    public StringParameter JournalEntryKey;
    
    [ParameterAlias("showJournal")]
    public bool ShowJournal = false;
    
    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        string description = StringEntryProcessor.GetLocalizedString("Journal Entries", JournalEntryKey);
        string title = StringEntryProcessor.GetLocalizedString("Journal Titles", JournalEntryKey);

        EntryMedia media =
            AssetDatabase.LoadAssetAtPath<EntryMedia>(
                $"Assets/IntegrationSampleAssets/Scriptable Objects/Journal System/{JournalEntryKey}.asset");

        if (media == null)
            Debug.LogError($"{JournalEntryKey} media not loaded.");

        JournalEntry newJournalEntry = new JournalEntry(JournalEntryKey, title, description, media);
        JournalManager.Instance.InsertNewEntry(newJournalEntry, ShowJournal);
    }
}