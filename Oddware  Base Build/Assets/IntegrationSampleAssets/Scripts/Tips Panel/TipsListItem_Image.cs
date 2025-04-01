using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Naninovel
{
    public class TipsListItem_Image : TipsListItem
    {
        private Sprite sprite;

        public Sprite Sprite
        {
            get => sprite;
            set => sprite = value;
        }
    }
}
