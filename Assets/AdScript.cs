using System.Collections;
using System.Collections.Generic;
#if UNITY_ADS
using UnityEngine.Advertisements;
#endif
using UnityEngine;

public class AdScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowRewardedAd()
    {
#if UNITY_ADS
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
#endif
    }

#if UNITY_ADS

   
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
	    {
        case ShowResult.Finished:
        Debug.Log("The ad was successfully shown.");
        //
        // YOUR CODE TO REWARD THE GAMER
        // Give coins etc.
        break;
        case ShowResult.Skipped:
        Debug.Log("The ad was skipped before reaching the end.");
        break;
        case ShowResult.Failed:
        Debug.LogError("The ad failed to be shown.");
        break;
	    }
    }

#endif

}
