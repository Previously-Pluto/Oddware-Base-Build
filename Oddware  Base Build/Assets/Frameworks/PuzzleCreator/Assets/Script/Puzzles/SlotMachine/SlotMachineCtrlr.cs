using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class SlotMachineCtrlr : MonoBehaviour
{
    public static event Action HandlePulled = delegate { };
    public SpriteRenderer m_gotchaImage;
    [SerializeField]
    private Transform _handle;
    private bool _resultsChecked;
    [SerializeField]
    private TextMeshProUGUI _prizeText;
    private int _prizeValue;
    public SlotMachine_PC _slotMachinePc;
    private static System.Random rng = new System.Random();
    private GotchaObject m_currentGotchaObject;
    private bool m_isHandlePulled;

    // Update is called once per frame
    void Update()
    {
        if(m_isHandlePulled)
            CheckResults();
    }

    public void OnMouseDown()
    {
        if(!m_isHandlePulled)
        {
            StartCoroutine("HandleAnim");
            StartCoroutine("PullHandle");
        }
    }

    private IEnumerator PullHandle()
    {
        float speed = 0.05f;
        int numberOfRot = UnityEngine.Random.Range(3, 5);
        HandlePulled();
        for (int j = 0; j < numberOfRot; j++)
        {
            var objects = _slotMachinePc.m_gotchaObjects.OrderBy(a => rng.Next()).ToList();
            for (int i = 0; i < objects.Count; i++)
            {
                m_gotchaImage.sprite = objects[i].m_image;
                m_currentGotchaObject = objects[i];
                yield return new WaitForSeconds(speed);
            }
            speed += 0.05f;
            for (int i = 0; i < objects.Count; i++)
            {
                m_gotchaImage.sprite = objects[i].m_image;
                m_currentGotchaObject = objects[i];
                yield return new WaitForSeconds(speed);
            }
        }
        m_isHandlePulled = true;
    }

    private IEnumerator HandleAnim()
    {
        for (int i = 0; i < 5; i++)
        {
            _handle.Rotate(0f, 0f, i);
            yield return new WaitForSeconds(0.1f);
        }

        HandlePulled();

        for (int i = 0; i < 5; i++)
        {
            _handle.Rotate(0f, 0f, -i);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CheckResults()
    {
        _prizeText.enabled = true;
        //_resultsChecked = false;
        _prizeText.text = "You have won " + m_currentGotchaObject.m_name;
        StartCoroutine(WaitBeforeSolvedPuzzleProcess());
        _resultsChecked = true;
    }

    IEnumerator WaitBeforeSolvedPuzzleProcess()
    {
        yield return new WaitForSeconds(1.5f);
        _slotMachinePc.puzzleSolved();
    }
}
