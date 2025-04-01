using UnityEngine;
using UnityEngine.UI;

namespace NoteSystem
{
    public class CustomNoteUIManager : MonoBehaviour
    {
        [Header("Audio Prompt UI")]
        public GameObject audioPromptUI = null;

        [Header("Page Buttons UI")]
        public GameObject pageButtons = null;
        public GameObject nextButton = null;
        public GameObject previousButton = null;

        [Header("Note Page UI's")]
        public GameObject customNoteMainUI = null;
        public Image customNotePageUI = null;

        [Header("Note Text UI's")]
        public Text customNoteTextUI = null;

        [HideInInspector] public NormalCustomNoteController noteController;

        public static CustomNoteUIManager instance;

        private void Awake()
        {
            if (instance == null) { instance = this; }
        }

        public void ShowPageButtons(bool shouldShow)
        {
            if (shouldShow)
            {
                pageButtons.SetActive(true);
            }
            else
            {
                pageButtons.SetActive(false);
            }
        }

        public void ShowAudioPrompt()
        {
            audioPromptUI.SetActive(true);
        }

        public void PlayPauseAudio()
        {
            noteController.NoteReadingAudio();
        }

        public void RepeatAudio()
        {
            noteController.RepeatReadingAudio();
        }

        public void CloseButton()
        {
            noteController.CloseNote();
        }

        public void NextPage()
        {
            noteController.NextPage();
        }

        public void BackPage()
        {
            noteController.BackPage();
        }
    }
}
