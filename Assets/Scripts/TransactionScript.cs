using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class TransactionScript : MonoBehaviour, IStoreListener
{

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    private static int skinElements;

    private static int powerupElements;

    private static int coinElements;

    private static string kSkinElementID = "skin0";
    private static string kPowerupElementID = "powerup0";
    private static string kCoinElementID = "coin0";

    private static string purchasedItemID;

    private static ShopItem itemForPurchasing;

    private static MenuScript blargh;

    // Use this for initialization

    // Update is called once per frame
    void Update()
    {

    }

    public void OnStartUp(int shopSkinElements, int shopPowerupElements, int shopCoinElements)
    {
        skinElements = shopSkinElements;
        powerupElements = shopPowerupElements;
        coinElements = shopCoinElements;
        blargh = GameObject.Find("MainMenuCanvas").GetComponent<MenuScript>();

        if (m_StoreController == null)
        {
            InitializePurchasing();
        }
    }

    public void InitializePurchasing()
    {
        Debug.Log("Blargh");
        if (IsInitialized())
        {
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        for (int i = 0; i < skinElements; i++)
        {
            builder.AddProduct(kSkinElementID + i, ProductType.NonConsumable);
            Debug.Log(kSkinElementID + i);
        }

        for (int i = 0; i < powerupElements; i++)
        {
            builder.AddProduct(kPowerupElementID + i, ProductType.Consumable);
            Debug.Log(kPowerupElementID + i);
        }

        for (int i = 0; i < coinElements; i++)
        {
            builder.AddProduct(kCoinElementID + i, ProductType.Consumable);
        }

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Initialization Failed. Reason: " + error);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        if (String.Equals(e.purchasedProduct.definition.id, purchasedItemID))
        {
            blargh.Coins += 50;
        }
        else
        {
            Debug.Log("Purchase Failed. PurchasedItemID: " + purchasedItemID + "does not match PurchaseEventID: " + e.purchasedProduct.definition.id);
        }

        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.Log("Purchase of Product " + i + "failed because of " + p);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void BuyShopItem(int currentItemShown, string currentlyActive, ShopItem item)
    {
        itemForPurchasing = item;
        purchasedItemID = GetSelected(currentItemShown, currentlyActive);
        BuyProductID(purchasedItemID);
    }

    private void BuyProductID(string productID)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productID);

            Debug.Log(product);
            if (!productID.Contains("coin"))
            {
                BuyWithCoins(product);
            }
            else if (product != null && product.availableToPurchase == true)
            {
                m_StoreController.InitiatePurchase(product);
            }
        }
    }


    public bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public string GetSelected(int currentItemShown, string currentlyActive)
    {
        if (currentlyActive == "SkinObject")
        {
            return kSkinElementID + currentItemShown;
        }
        else if (currentlyActive == "PowerupObject")
        {
            return kPowerupElementID + currentItemShown;
        }
        else if (currentlyActive == "CoinObject")
        {
            return kCoinElementID + currentItemShown;
        }
        else
        {
            return null;
        }
    }

    //For Apple devices since they don't have the same options that googleplay has
    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("Not Initialized");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();

            apple.RestoreTransactions((result) =>
            {

                Debug.Log("Restore Purchases continuing: " + result + "If no futher messages, no purchase available to restore");
            });
        }
        else
        {
            Debug.Log("Purchase restoration is not avaiable on this platform" + Application.platform);
        }
    }

    public void BuyWithCoins(Product product)
    {
        if (blargh.Coins >= 50)
        {
            if (product.definition.type == ProductType.Consumable)
            {
                blargh.AddToList(product.definition.id);
            }
            else
            {
                itemForPurchasing.Unlocked = true;
            }
            blargh.Coins -= 50;
        }
        else
        {
            Debug.Log("You don't have enough coins");
        }
    }
}
