using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using Unity.Services.Core;
using Unity.Services.Core.Environments;

public class IAPManager : MonoBehaviour, IStoreListener
{
    IStoreController m_StoreController;
    IAppleExtensions m_AppleExtensions;

    public string allCars;
    public string removeAds;
    public GameObject carsButton;
    public GameObject adsButton;
    public GameObject restoreButton;
    private PlayerData data;

    void Awake()
    {
        Initialize(OnSuccess, OnError);
    }

    void Initialize(Action onSuccess, Action<string> onError)
    {
        try
        {
            var options = new InitializationOptions().SetEnvironmentName("production");

            UnityServices.InitializeAsync(options).ContinueWith(task => onSuccess());
        }
        catch (Exception exception)
        {
            onError(exception.Message);
        }
    }


    void OnSuccess()
    {
        var text = "Congratulations!\nUnity Gaming Services has been successfully initialized.";
        Debug.Log(text);
    }

    void OnError(string message)
    {
        var text = $"Unity Gaming Services failed to initialize with error: {message}.";
        Debug.LogError(text);
    }

    void Start()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
        {
            Debug.LogError("services weren't initialized");
        }
        //if file doesn't exist set to knew else get the data
        if ((data = SaveTheData.loadFromFile()) == null)
        {
            data = new PlayerData();
            data.selectedCar = "Default";
            data.ownedCars.Add("Default");
            data.numCoins = 0;
            data.highScore = 0;
            data.soundOn = true;
            data.musicOn = true;
        }
        data.savePlayer();
        InitializePurchasing();
    }

    void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Add products that will be purchasable and indicate its type.
        builder.AddProduct(removeAds, ProductType.NonConsumable);
        builder.AddProduct(allCars, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        if (!data.ownsAllCars)
        {
            carsButton.SetActive(true);
            restoreButton.SetActive(true);
        }
        if (data.adsEnabled)
        {
            adsButton.SetActive(true);
            restoreButton.SetActive(true);
        }

        m_StoreController = controller;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
        Debug.Log("In-App Purchasing successfully initialized");
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log($"In-App Purchasing initialize failed: {error}");
    }

    public void RestorePurchases()
    {
        m_AppleExtensions.RestoreTransactions(OnRestore);
    }

    void OnRestore(bool success)
    {
        if (success)
        {
            // This does not mean anything was restored,
            // merely that the restoration process succeeded.
            Debug.Log("succeed restore");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            // Restoration failed.
            Debug.Log("fail restore");
        }

    }

    public void BuyAllCars()
    {
        m_StoreController.InitiatePurchase(allCars);
    }

    public void RemoveAds()
    {
        m_StoreController.InitiatePurchase(removeAds);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        Debug.Log("processing");
        //Retrieve the purchased product
        var product = args.purchasedProduct;

        //Add the purchased product to the players inventory
        if (product.definition.id == allCars)
        {
            addIfDontHave("Truck");
            addIfDontHave("Cheese");
            addIfDontHave("Hover");
            addIfDontHave("Mushroom");
            addIfDontHave("Sports");
            addIfDontHave("Mini");
            addIfDontHave("Boat");
            addIfDontHave("Beetle");
            data.ownsAllCars = true;
            data.savePlayer();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (product.definition.id == removeAds)
        {
            data.adsEnabled = false;
            data.savePlayer();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        Debug.Log($"Purchase Complete - Product: {product.definition.id}");

        //We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
    }

    private void addIfDontHave(string car)
    {
        if (!data.ownedCars.Contains(car))
            data.ownedCars.Add(car);
    }
}
