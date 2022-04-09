﻿using System;
using System.Collections.Generic;
using Services.PlayerData;
using StaticData.Loot.Items;
using UnityEngine;

namespace UI.Windows.Inventories
{
  public class UIInventoryArea : MonoBehaviour
  {
    [SerializeField] private UIInventorySlot[] slots;
    public ItemStaticData LastClickedItem { get; private set; }
    
    public event Action<ItemStaticData> InventoryClicked;

    public void UpdateView(IEnumerable<InventorySlot> inventorySlots)
    {
      var index = 0;
      foreach (var inventorySlot in inventorySlots)
      {
        slots[index].SetItem(inventorySlot.Item, inventorySlot.Count);
        index++;
      }
    }

    public void Subscribe()
    {
      for (var i = 0; i < slots.Length; i++)
      {
        slots[i].Clicked += NotifyAboutEquipClick;
      } 
    }

    public void Cleanup()
    {
      for (var i = 0; i < slots.Length; i++)
      {
        slots[i].Clicked -= NotifyAboutEquipClick;
      } 
    }

    public void RemoveLastClickedItem() => 
      LastClickedItem = null;

    private void NotifyAboutEquipClick(ItemStaticData item)
    {
      if (item != null)
      {
        InventoryClicked?.Invoke(item);
        LastClickedItem = item;
      }
    }
  }
}