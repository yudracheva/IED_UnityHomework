using Services.PlayerData;
using Services.Shop;
using StaticData.Loot.Items;
using UI.Base;
using UI.Windows.Inventories;
using UnityEngine;
using UnityEngine.UI;
using UI.Displaying;

namespace UI.Windows
{
    public class ShopWindow : BaseWindow
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private Button buyButton;
        [SerializeField] private UIInventorySlot[] slots;
        [SerializeField] private ShopItemTip tip;
        [SerializeField] private HeroMoneyDisplayer moneyDisplayer;
        
        private ItemStaticData _lastClickedItem;
        private IShopService _shopService;

        public void Construct(IShopService shopService, PlayerMoney money)
        {
            _shopService = shopService;
            _shopService.Changed += UpdateSlots;

            moneyDisplayer.Construct(money);

            foreach (var t in slots)
            {
                t.Clicked += OnSlotClicked;
            }
        }

        public override void Open()
        {
            UpdateSlots();
            RemoveTip();
            ResetLastClickedItem();
            Time.timeScale = 0;
            base.Open();
        }
        
        public override void Close()
        {
            Time.timeScale = 1;
            base.Close();
        }

        protected override void Subscribe()
        {
            base.Subscribe();
            buyButton.onClick.AddListener(TryBuyItem);
            closeButton.onClick.AddListener(Close);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            buyButton.onClick.RemoveListener(TryBuyItem);
            closeButton.onClick.RemoveListener(Close);
            _shopService.Changed -= UpdateSlots;
            foreach (var t in slots)
            {
                t.Clicked -= OnSlotClicked;   
            }
        }

        private void TryBuyItem()
        {
            if (_lastClickedItem == null || !_shopService.IsCanBuyItem(_lastClickedItem)) 
                return;
            
            _shopService.BuyItem(_lastClickedItem);
            ChangeBuyButtonActiveState(false);
            ResetLastClickedItem();
            RemoveTip();
        }

        private void UpdateSlots()
        {
            ResetSlots();
            var index = 0;
            foreach (var shopSlot in _shopService.Slots)
            {
                slots[index].SetItem(shopSlot.Item, shopSlot.Count);
                index++;
            }
        }

        private void OnSlotClicked(ItemStaticData item)
        {
            if (item == null) 
                return;
            
            _lastClickedItem = item;
            DisplayTip(item);
            ChangeBuyButtonActiveState(true);
        }

        private void DisplayTip(ItemStaticData item)
        {
            tip.DisplayTip(item.Name, item.Characteristics, item.BuyCost);
        }

        private void RemoveTip()
        {
            tip.RemoveTip();
        }

        private void ResetSlots()
        {
            foreach (var t in slots)
            {
                t.SetItem(null, 0);   
            }
        }

        private void ChangeBuyButtonActiveState(bool isActive)
        {
            if (buyButton.gameObject.activeSelf != isActive)
                buyButton.gameObject.SetActive(isActive);
        }

        private void ResetLastClickedItem()
        {
            _lastClickedItem = null;
        }
    }
}