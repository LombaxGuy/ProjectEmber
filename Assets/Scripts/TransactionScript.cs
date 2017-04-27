using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class TransactionScript : IStoreListener
{

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    private int skinElements;

    private int powerupElements;

    private int coinElements;

    private static string kSkinElementID = "skin0";
    private static string kPowerupElementID = "powerup0";
    private static string kCoinElementID = "coin0";

    private static string purchasedItemID;

    private ShopItem itemForPurchasing;

    private MenuScript blargh;

    // Use this for initialization

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// This method is basically a Start method but has to be run in MenuScript's start method since this script needs the amount of shop elements 
    /// </summary>
    /// <param name="shopSkinElements">Number of buyable skins</param>
    /// <param name="shopPowerupElements">Number of buyable powerups</param>
    /// <param name="shopCoinElements">Number of buyable coins</param>
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

    /// <summary>
    /// Adds all buyable objects to a thing from the InAppPurchasing package. Gives all objects ID's and a product type
    /// </summary>
    public void InitializePurchasing()
    {
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

    /// <summary>
    /// In case Initializing fails this will pickup the error
    /// </summary>
    /// <param name="error"></param>
    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Initialization Failed. Reason: " + error);
    }

    /// <summary>
    /// If the registrered purchased ID matches the ID of the item intended to be purchased the game gives the user the purchased object
    /// </summary>
    /// <param name="e">The purchase event</param>
    /// <returns></returns>
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

    /// <summary>
    /// If the purchase fails this method prints an error message
    /// </summary>
    /// <param name="i">The Product which failed to be purchased</param>
    /// <param name="p">The reason why it failed</param>
    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.Log("Purchase of Product " + i + "failed because of " + p);
    }

    /// <summary>
    /// This method is run on initialization 
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="extensions"></param>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    /// <summary>
    /// The method which needs to be called in order to initialize a purchase
    /// </summary>
    /// <param name="currentItemShown">The variable of the same name in MenuScript</param>
    /// <param name="currentlyActive">The variable of the same name in MenuScript</param>
    /// <param name="item">The item intended to be bought</param>
    public void BuyShopItem(int currentItemShown, string currentlyActive, ShopItem item)
    {
        itemForPurchasing = item;
        purchasedItemID = GetSelected(currentItemShown, currentlyActive);
        BuyProductID(purchasedItemID);
    }

    /// <summary>
    /// Checks if the object intended to be bought is a coin item or not and then runs the correct methods for buying
    /// </summary>
    /// <param name="productID">The ID of the object intended to be bought</param>
    private void BuyProductID(string productID)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productID);
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

    /// <summary>
    /// Checks if the Initialize method has been run
    /// </summary>
    /// <returns></returns>
    public bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    /// <summary>
    /// Gets the currently selected object based on the information received from the BuyShopItem method
    /// </summary>
    /// <param name="currentItemShown"></param>
    /// <param name="currentlyActive"></param>
    /// <returns></returns>
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

    /// <summary>
    /// A simple method for buying stuff with coins. May or may not be used in final product based on tests after Google implementation
    /// </summary>
    /// <param name="product"></param>
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
