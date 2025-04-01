using PixelCrushers;
using PixelCrushers.QuestMachine;

public class UIQuestDialogueJournal : UnityUIQuestDialogueUI, IQuestDialogueUI
{
    protected override void SetControlButtons(bool enableClose, bool enableBack, bool enableAcceptDecline)
    {
        base.SetControlButtons(enableClose, enableBack, enableAcceptDecline);
        if (selectedQuest.labels.Find(label => StringField.Equals(label, "NoDecline")) != null)
        {
            declineButton.gameObject.SetActive(false);
        }
    }
}