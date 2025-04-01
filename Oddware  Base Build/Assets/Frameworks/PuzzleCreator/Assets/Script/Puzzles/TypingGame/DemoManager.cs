using System.Collections.Generic;
using UnityEngine;

namespace TypingGameKit.Demo
{
    /// <summary>
    /// Sets up and manages the interactive demo.
    /// </summary>
    public class DemoManager : MonoBehaviour
    {
        private AudioSource _audioSource;

        [SerializeField] private int _initualSequenceCount = 10;
        [SerializeField] private GameObject _anchorObject = null;
        [SerializeField] private SequenceOverlay _sequenceOverlay = null;
        [SerializeField] private OverlaySettings _overlaySettings = null;
        [SerializeField] private TextCollection _texts = null;
        [SerializeField] private float _positionRange = 10;
        [SerializeField] private Transform _anchorParent = null;

        [SerializeField] private AudioClip _inputFailedClip = null;
        [SerializeField] private AudioClip _inputSucceededClip = null;

        private bool _respawn = false;
        private bool _puzzleGameHasStartedForFirstTime = false;
        public GameObject _cameraContainer;

        public List<LockStruct> _locks;
        public LockCtrlr _lockCtrlr;
        public GameObject _endScreen;
        public bool _isEndOfGame;
        public SequenceOverlay SequenceOverlay
        {
            get { return _sequenceOverlay; }
        }

        public OverlaySettings SequenceSettings
        {
            get { return _overlaySettings; }
        }

        public TypingGamePuzzle_Pc _typingPuzzleGame;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            //SpawnInitialSequences();

            InputSequenceManager.OnInputRejected += PlayInputFailed;
            InputSequenceManager.OnInputAccepted += PlayInputSuccessfull;
        }

        private void Update()
        {
            if(_cameraContainer.activeInHierarchy && !_puzzleGameHasStartedForFirstTime)
            {
                SpawnInitialSequences();
                _puzzleGameHasStartedForFirstTime = true;
                _lockCtrlr.ActivateLockUI();//activate lock UI
                _lockCtrlr._timerIsSet = true;//set the timer
            }

            if(_cameraContainer.activeInHierarchy && _lockCtrlr._timerIsSet
                && _anchorParent.childCount == 0 && _lockCtrlr.GetCurrentLockIndex() != _locks.Count)    //verified if the timer is set and the camera container is empty
            {
                NextSequence();
            }
        }

        private void OnDestroy()
        {
            InputSequenceManager.OnInputRejected -= PlayInputFailed;
            InputSequenceManager.OnInputAccepted -= PlayInputSuccessfull;
        }

        private void PlayInputSuccessfull()
        {
            PlayClip(_inputSucceededClip);
        }

        private void PlayInputFailed()
        {
            PlayClip(_inputFailedClip);
        }

        public void SetRespawn(bool value)
        {
            _respawn = value;
        }

        private Vector3 RandomPosition()
        {
            float x = Random.Range(-_positionRange, _positionRange);
            float y = Random.Range(-_positionRange, _positionRange);
            float z = Random.Range(-_positionRange, _positionRange);
            return new Vector3(x, y, z);
        }

        public void SpawnInitialSequences()
        {
            if(_lockCtrlr == null)
            {
                for (int i = 0; i < _initualSequenceCount; i++)
                {
                    AddSequenceObject();
                }
            }
            else
            {
                _lockCtrlr.SetCurrentTimerValue(_lockCtrlr.GetCurrentLock().timer); //set current timer value
                for (int i = 0; i < _lockCtrlr.GetCurrentLock().numberOfWords; i++)
                {
                    AddSequenceObject();
                }
            }
        }

        public void DisplayEndScreen()
        {
            _endScreen.SetActive(true);
            _isEndOfGame = true;
        }

        public void NextSequence()
        {
            _lockCtrlr.DesactivateCurrentLock();//desactivate the current lock
            _lockCtrlr.IncreaseLockIndex();
            if(_lockCtrlr.GetCurrentLockIndex() == _lockCtrlr._locks.Count) //the player has completed all the locks
            {
                GetComponent<LockCtrlr>().DesactivateLockUI();
                _typingPuzzleGame.puzzleSolved();//puzzle is solved
            }
            else
            {
                SpawnInitialSequences();
            }
        }

        public void AddSequenceObject()
        {
            GameObject anchor = Instantiate(_anchorObject, _anchorParent, true);
            anchor.transform.localPosition = RandomPosition();
            AttachSequence(anchor);
        }

        private void AttachSequence(GameObject obj)
        {
            string textSequence = _texts.FindUniquelyTargetableText();
            InputSequence sequence = _sequenceOverlay.CreateSequence(textSequence, obj.transform);
            sequence.OnCompleted += delegate
            {
                Destroy(obj);
                if (_respawn)
                {
                    AddSequenceObject();
                }
            };
            sequence.OnRemoval += delegate
            {
                Destroy(obj);
            };
        }

        private void PlayClip(AudioClip clip)
        {
            _audioSource.PlayOneShot(clip);
        }

        public void SetDemoManagerData(GameObject anchorObject, float positionRange, TextCollection texts, AudioClip failedClip, AudioClip inputSucceededClip, List<LockStruct> locks)
        {
            _anchorObject = anchorObject;
            _positionRange = positionRange;
            _texts = texts;
            _inputSucceededClip = inputSucceededClip;
            _inputFailedClip = failedClip;
            _locks = locks;
        }

        public void ResetDemo()
        {
            _lockCtrlr._endScr.SetActive(false);
            _lockCtrlr.ResetCurrentLockIndex();
            _lockCtrlr.SetLocksInformation(_lockCtrlr._locks);
            DestroyCurrentObjs();//reset the current demo
            SpawnInitialSequences();
            _puzzleGameHasStartedForFirstTime = true;

        }
        public void QuitDemo()
        {
            transform.parent.parent.GetComponent<TypingGamePuzzle_Pc>().puzzleSolved();
        }

        public void DestroyCurrentObjs()
        {
            foreach(Transform t in _anchorParent.transform)
            {
                Destroy(t.gameObject);
            }
            SequenceOverlay.ResetSpawnSequence();
            foreach (Transform t in SequenceOverlay.transform)
            {
                Destroy(t.gameObject);
            }
        }
    }
}