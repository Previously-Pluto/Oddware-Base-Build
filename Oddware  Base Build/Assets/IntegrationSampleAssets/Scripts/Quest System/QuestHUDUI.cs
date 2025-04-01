using PixelCrushers.QuestMachine;
using UnityEngine;

public class QuestHUDUI : MonoBehaviour
{
    public static QuestHUDUI Instance;
    private QuestJournalButton _button;
    private QuestManager _questManager;
    
    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        _button = GetComponentInChildren<QuestJournalButton>();
        _questManager = QuestManager.Instance;
    }

    public void OpenQuestJournal()
    {
        _button.ShowJournalUI();
    }
    
    public void CloseQuestJournal()
    {
        _button.HideJournalUI();
    }

    public void ToggleQuestJournal()
    {
        _button.ToggleJournalUI();
    }
}