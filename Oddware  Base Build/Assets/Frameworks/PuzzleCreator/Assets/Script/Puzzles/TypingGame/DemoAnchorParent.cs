using UnityEngine;

namespace TypingGameKit.Demo
{
    public class DemoAnchorParent : MonoBehaviour
    {
        [SerializeField] private float _angleRotatedPerSecond = 10;

        public float AngleRotatedPerSecond
        {
            get { return _angleRotatedPerSecond; }
            set { _angleRotatedPerSecond = value; }
        }

        private void Update()
        {
            transform.RotateAround(transform.parent.GetChild(2).position, Vector3.up, Time.deltaTime * _angleRotatedPerSecond);
        }
    }
}