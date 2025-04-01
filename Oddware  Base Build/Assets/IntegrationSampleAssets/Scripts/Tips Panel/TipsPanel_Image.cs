using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Naninovel.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TipsPanel_Image : TipsPanel
    {
        [SerializeField] private Image tipsImage;
        [SerializeField] private GameObject tipsImageContainer;
        [SerializeField] private UnityEvent onImageChanged;

        protected override void FillListItems()
        {
            var records = TextManager.GetAllRecords(ManagedTextCategory);
            foreach (var record in records)
            {
                var unlockableId = $"{UnlockableIdPrefix}/{record.Key}";
                var value = string.IsNullOrEmpty(record.Value)
                    ? TextManager.GetRecordValueWithFallback(record.Key, ManagedTextCategory)
                    : record.Value;
                var title = value.GetBefore(SeparatorLiteral) ?? value;
                var selectedOnce = WasItemSelectedOnce(unlockableId);
                var item = TipsListItem.Instantiate(ItemPrefab, unlockableId, title, selectedOnce, SelectItem);

                TipsListItem_Image item_Image = item as TipsListItem_Image;

                if (item_Image != null)
                {
                    TipData tipData = Resources.Load<TipData>($"Scriptable Objects/{UnlockableIdPrefix}/{record.Key}");
                    item_Image.Sprite = tipData.Sprite;
                }
                else
                    Debug.LogError($"{record.Key} Tip Data was not loaded");

                item.transform.SetParent(ItemsContainer, false);
                ListItemsProperty.Add(item);
            }

            foreach (var item in ListItemsProperty)
                item.SetUnlocked(UnlockableManager.ItemUnlocked(item.UnlockableId));

            TipsCount = ListItems.Count;
        }

        protected override void SelectItem(TipsListItem item)
        {
            base.SelectItem(item);
            SetImage(item);
        }

        protected virtual void SetImage(TipsListItem item)
        {
            TipsListItem_Image itemImage = item as TipsListItem_Image;

            tipsImageContainer.gameObject.SetActive(true);

            if (itemImage != null)
            {
                if (itemImage.Sprite != null)
                    tipsImage.sprite = itemImage.Sprite;
                else
                    tipsImageContainer.gameObject.SetActive(false);
            }
            else
                Debug.LogError($"{item.name} Tips Image was not loaded");
        }
    }
}