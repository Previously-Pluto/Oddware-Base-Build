using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUIButtonController : MonoBehaviour
{
    public CustomUIPuzzle_PC m_customUIPuzzle_PC;
    public CUSTOM_UI_COLORS m_selectedColor;
    public CUSTOM_UI_NUMBERS m_selectedNumber;

    public void PressedButton()
    {
        m_customUIPuzzle_PC.VerifiedIfTheClickedButtonIsCorrect(m_selectedColor, m_selectedNumber);
    }
}
