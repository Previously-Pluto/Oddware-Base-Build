using UnityEngine;
using UnityEngine.Events;

namespace ChessPuzzleSystem
{
    public class ChessPowerManager : MonoBehaviour
    {
        [SerializeField] private ChessFuseBoxController fuseBox1 = null;
        [SerializeField] private ChessFuseBoxController fuseBox2 = null;
        [SerializeField] private ChessFuseBoxController fuseBox3 = null;
        [SerializeField] private ChessFuseBoxController fuseBox4 = null;
        [SerializeField] private ChessFuseBoxController fuseBox5 = null;
        [SerializeField] private ChessFuseBoxController fuseBox6 = null;

        private bool isFuseBox1;
        private bool isFuseBox2;
        private bool isFuseBox3;
        private bool isFuseBox4;
        private bool isFuseBox5;
        private bool isFuseBox6;

        [Header("Power on - Chess pieces correct")]
        [SerializeField] private UnityEvent powerUp = null;


        public void CheckFuses()
        {
            if (fuseBox1.fuseBoxName == fuseBox1.fuseName)
                isFuseBox1 = true;
            else
                isFuseBox1 = false;

            if (fuseBox2.fuseBoxName == fuseBox2.fuseName)
                isFuseBox2 = true;
            else
                isFuseBox2 = false;

            if (fuseBox3.fuseBoxName == fuseBox3.fuseName)
                isFuseBox3 = true;
            else
                isFuseBox3 = false;

            if (fuseBox4.fuseBoxName == fuseBox4.fuseName)
                isFuseBox4 = true;
            else
                isFuseBox4 = false;

            if (fuseBox5.fuseBoxName == fuseBox5.fuseName)
                isFuseBox5 = true;
            else
                isFuseBox5 = false;

            if (fuseBox6.fuseBoxName == fuseBox6.fuseName)
                isFuseBox6 = true;
            else
                isFuseBox6 = false;

            if (isFuseBox1 && isFuseBox2 && isFuseBox3 && isFuseBox4 && isFuseBox5 && isFuseBox6)
            {
                powerUp.Invoke();
            }
        }
    }
}
