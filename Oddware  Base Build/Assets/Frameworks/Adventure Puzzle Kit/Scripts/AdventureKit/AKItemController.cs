using ExamineSystem;
using FlashlightSystem;
using GasMaskSystem;
using GeneratorSystem;
using KeypadSystem;
using PadlockSystem;
using PhoneInputSystem;
using ThemedKeySystem;
using ChessPuzzleSystem;
using SafeUnlockSystem;
using FuseboxSystem;
using NoteSystem;
using LeverPuzzleSystem;
using UnityEngine;

namespace AdventurePuzzleKit
{
    public class AKItemController : MonoBehaviour
    {
        [Header("Primary System Type")]
        [SerializeField] private SystemType _systemType = SystemType.None;

        [Header("Secondary System Type - Only for Generator")]
        [SerializeField] private SecondarySystemType _secondarySystemType = SecondarySystemType.None;

        private FlashlightItemController _flashlightItemController;
        private ExamineItemController _examineItemController;
        private GeneratorItemController _generatorItemController;
        private NoteController _noteController;
        private GasMaskItemController _gasMaskItemController;
        private KeypadItemController _keypadItemController;
        private ThemedKeyItemController _themedKeyItemController;
        private PhonePadItemController _phonepadItemController;
        private PadlockItemController _padlockItemController;
        private ChessItemController _chessItemController;
        private SafeItemController _safeItemController;
        private ButtonDoorController _buttonDoorController;
        private FuseItemController _fuseboxItemController;
        private LeverItemController _leverItemController;
        private SingleObjectHighlight _singleObjectHighlight;

        private enum SystemType { None, FlashlightSys, GeneratorSys, ExamineSys, NoteSys, GasMaskSys, KeypadSys, ThemedKeySys, PhoneSys, PadlockSys, ChessSys, SafeSys, buttonDoorSys, FuseBoxSys, LeverSys, ObjectHighlight }
        private enum SecondarySystemType { None, GeneratorSys }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            switch (_systemType)
            {
                case SystemType.FlashlightSys: _flashlightItemController = GetComponent<FlashlightItemController>(); break;
                case SystemType.GeneratorSys: _generatorItemController = GetComponent<GeneratorItemController>(); break;
                case SystemType.ExamineSys: _examineItemController = GetComponent<ExamineItemController>(); break;
                case SystemType.NoteSys: _noteController = GetComponent<NoteController>(); break;
                case SystemType.GasMaskSys: _gasMaskItemController = GetComponent<GasMaskItemController>(); break;
                case SystemType.KeypadSys: _keypadItemController = GetComponent<KeypadItemController>(); break;
                case SystemType.ThemedKeySys: _themedKeyItemController = GetComponent<ThemedKeyItemController>(); break;
                case SystemType.PhoneSys: _phonepadItemController = GetComponent<PhonePadItemController>(); break;
                case SystemType.PadlockSys: _padlockItemController = GetComponent<PadlockItemController>(); break;
                case SystemType.ChessSys: _chessItemController = GetComponent<ChessItemController>(); break;
                case SystemType.SafeSys: _safeItemController = GetComponent<SafeItemController>(); break;
                case SystemType.buttonDoorSys: _buttonDoorController = GetComponent<ButtonDoorController>(); break;
                case SystemType.FuseBoxSys: _fuseboxItemController = GetComponent<FuseItemController>(); break;
                case SystemType.LeverSys: _leverItemController = GetComponent<LeverItemController>(); break;
                case SystemType.ObjectHighlight: _singleObjectHighlight = GetComponent<SingleObjectHighlight>(); break;
            }
            switch (_secondarySystemType)
            {
                case SecondarySystemType.GeneratorSys: _generatorItemController = GetComponent<GeneratorItemController>(); break;
            }
        }

        public void Highlight(bool highlight)
        {
            switch (_systemType)
            {
                case SystemType.ExamineSys:
                    if (highlight)
                    {
                        _examineItemController.MainHighlight(true);
                        switch (_secondarySystemType)
                        {
                            case SecondarySystemType.GeneratorSys: _generatorItemController.ShowObjectStats(); break;
                        }
                    }
                    else
                    {
                        _examineItemController.MainHighlight(false);
                        switch (_secondarySystemType)
                        {
                            case SecondarySystemType.GeneratorSys: _generatorItemController.HideObjectStats(); break;
                        }
                    }
                    break;
                case SystemType.ObjectHighlight:
                    if (highlight)
                    {
                        _singleObjectHighlight.MainHighlight(true);
                        switch (_secondarySystemType)
                        {
                            case SecondarySystemType.GeneratorSys: _generatorItemController.ShowObjectStats(); break;
                        }
                    }
                    else
                    {
                        _singleObjectHighlight.MainHighlight(false);
                        switch (_secondarySystemType)
                        {
                            case SecondarySystemType.GeneratorSys: _generatorItemController.HideObjectStats(); break;
                        }
                    }
                    break;
                case SystemType.GeneratorSys:
                    if (highlight)
                    {
                        _generatorItemController.ShowObjectStats();
                    }
                    else
                    {
                        _generatorItemController.HideObjectStats();
                    }
                    break;
            }
        }

        public void InteractionType()
        {
            switch (_systemType)
            {
                case SystemType.FlashlightSys: _flashlightItemController.ObjectInteract(); break;
                case SystemType.GeneratorSys: _generatorItemController.ObjectInteract(); break;
                case SystemType.ExamineSys: _examineItemController.ExamineObject(); break;
                case SystemType.NoteSys: _noteController.DisplayNotes(); break;
                case SystemType.GasMaskSys: _gasMaskItemController.ObjectInteract(); break;
                case SystemType.KeypadSys: _keypadItemController.ShowKeypad(); break;
                case SystemType.ThemedKeySys: _themedKeyItemController.ObjectInteract(); break;
                case SystemType.PhoneSys: _phonepadItemController.ShowKeypad(); break;
                case SystemType.PadlockSys: _padlockItemController.ObjectInteract(); break;
                case SystemType.ChessSys: _chessItemController.ObjectInteract(); break;
                case SystemType.SafeSys: _safeItemController.ShowSafeLock(); break;
                case SystemType.buttonDoorSys: _buttonDoorController.PlayAnimation(); break;
                case SystemType.FuseBoxSys: _fuseboxItemController.ObjectInteract(); break;
                case SystemType.LeverSys: _leverItemController.ObjectInteract(); break;
                case SystemType.ObjectHighlight: _singleObjectHighlight.InteractionType(); break;
            }
        }
    }
}
