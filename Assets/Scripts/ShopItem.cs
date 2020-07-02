using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
	public Text itemCountText;
	public Text itemPriceText;
	public int itemPrice;
	public string itemName;

	void Start()
	{
		UpdateShopItem();
	}

	void UpdateShopItem()
	{
		itemPriceText.text = (((float)itemPrice) / 100.0f).ToString("0.00");
		itemCountText.text = "x " + PreferencesManager.Instance.GetPowerup(itemName);
	}

	public void BuyPowerup()
	{
		if (PreferencesManager.Instance.GetCoins() >= itemPrice)
		{
			PreferencesManager.Instance.SetCoins(PreferencesManager.Instance.GetCoins() - itemPrice);
			PreferencesManager.Instance.ModifyPowerup(itemName, 1);
			UpdateShopItem();
			UIManager.Instance.UpdateMenuCoinCounts();
			FirebaseEventManager.Instance.SendSpendVirtualCurrency();
		}
	}
}
