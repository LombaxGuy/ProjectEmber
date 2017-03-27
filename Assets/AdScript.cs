using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using UnityEngine;

public class AdScript : MonoBehaviour {

    //Cooldown on ads

    private bool adClicked = false;
    private bool cdActivated = false;
    [SerializeField]
    private float timeLeft = 0;
    [SerializeField]
    private float timerCD;

    void Awake()
    {
        // Ads getting initialized, android and testmode only
        if (Advertisement.isInitialized == false)
        {
            Advertisement.Initialize("1359445", true);
        }
    }

    void Update()
    {
        if(cdActivated == true)
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft <= 0)
            {
                adClicked = false;
                cdActivated = false;
            }
        }
    }

    public void ShowRewardedAd()
    {
        if(adClicked == false)
        {
            adClicked = true;
            
            StartCoroutine(IsAddReady());
        }      
    }

    //Method to get the ads state
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                cdActivated = true;
                timeLeft = timerCD;
                //Add xx coins
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                adClicked = false;
                //Message popup?
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                adClicked = false;
                //Message popup?
                break;
        }
    }

    IEnumerator IsAddReady()
    {
        bool isWaiting = true;
        while (isWaiting)
        {
            if(Advertisement.IsReady("rewardedVideo"))
            {
                isWaiting = false;
                var options = new ShowOptions { resultCallback = HandleShowResult };
                Advertisement.Show("rewardedVideo", options);               
            }
            yield return null;
        }

    }


}
