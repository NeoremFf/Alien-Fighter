using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class Purchaser : MonoBehaviour
{
    [Header("Gold")]
    [SerializeField] private int goldAdd_0;
    [SerializeField] private int goldAdd_1;
    [SerializeField] private int goldAdd_2;

    [Header("UI Update")]
    [SerializeField] private GameObject shopGOToUpdateUI; // GO "Skin Controller" on Canvas - lust for update UI
    private void Start()
    {
        PurchaseManager.OnPurchaseConsumable += PurchaseManager_OnPurchaseConsumable;
        PurchaseManager.OnPurchaseNonConsumable += PurchaseManager_OnPurchaseNonConsumable;
    }

    private void PurchaseManager_OnPurchaseConsumable(PurchaseEventArgs args)
    {
        switch (args.purchasedProduct.definition.id)
        {
            case "gold_0":
                ScoreController.AddPurchasedGold(goldAdd_0);
                break;

            case "gold_1":
                ScoreController.AddPurchasedGold(goldAdd_1);
                break;

            case "gold_2":
                ScoreController.AddPurchasedGold(goldAdd_2);
                break;

            default:
                Debug.LogError("You try to purche item that is not in list!");
                break;
        }

        SkinSelected.UpdateGoldUI();
    }

    private void PurchaseManager_OnPurchaseNonConsumable(PurchaseEventArgs args)
    {
        MonetizationSupport.CheckRemoveAd();
    }
}
