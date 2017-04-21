//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.Advertisements;
//using UnityEngine;
//using System.Net;
//using System.IO;

//public class AdScript : MonoBehaviour {

//    //Cooldown on ads

//    private bool adClicked = false;
//    private bool cdActivated = false;
//    private float timeLeft = 0;
//    [SerializeField] private float timerCD;

//    void Awake()
//    {
//        // Ads getting initialized, android and testmode only
//        if (Advertisement.isInitialized == false)
//        {
//            Advertisement.Initialize("1359445", true);
//            //initialize without testmode
//            //Advertisement.Initialize("1359445", false);

//        }
//    }

//    /// <summary>
//    /// When ad is watched this method will make another ad ready when timeLeft is 0
//    /// </summary>
//    void Update()
//    {
//        if(cdActivated == true)
//        {
//            timeLeft -= Time.deltaTime;
//            if(timeLeft <= 0)
//            {
//                adClicked = false;
//                cdActivated = false;
//            }
//        }
//    }

//    /// <summary>
//    /// Starts a coroutine with ad
//    /// </summary>
//    public void ShowRewardedAd()
//    {
//        if(adClicked == false)
//        {
//            adClicked = true;
            
//            StartCoroutine(IsAddReady());
//        }      
//    }

//    /// <summary>
//    /// Get the result of the ad.
//    /// </summary>
//    /// <param name="result"></param>
//    private void HandleShowResult(ShowResult result)
//    {
//        switch (result)
//        {
//            case ShowResult.Finished:
//                Debug.Log("The ad was successfully shown.");
//                cdActivated = true;
//                timeLeft = timerCD;
//                //Add xx coins
//                break;
//            case ShowResult.Skipped:
//                Debug.Log("The ad was skipped before reaching the end.");
//                adClicked = false;
//                //Message popup?
//                break;
//            case ShowResult.Failed:
//                Debug.LogError("The ad failed to be shown.");
//                adClicked = false;
//                //Message popup?
//                break;
//        }
//    }

//    /// <summary>
//    /// Making a web requst to see if the player have connection to the internet or not.
//    /// Maybe this some changed settings in Unity, player settings.(API compatibility level need to be .net 2.0 instead of the subset) Need testing.
//    /// </summary>
//    /// <param name="resource"></param>
//    /// <returns></returns>
//    public string GetHtmlFromUri(string resource)
//    {
//        string html = string.Empty;
//        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
//        try
//        {
//            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
//            {
//                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
//                if (isSuccess)
//                {
//                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
//                    {
//                        //We are limiting the array to 80 so we don't have
//                        //to parse the entire html document feel free to 
//                        //adjust (probably stay under 300)
//                        char[] cs = new char[80];
//                        reader.Read(cs, 0, cs.Length);
//                        foreach (char ch in cs)
//                        {
//                            html += ch;
//                        }
//                    }
//                }
//            }
//        }
//        catch
//        {
//            return "";
//        }
//        return html;
//    }

//    /// <summary>
//    /// Checks connection from the GetHtmlFromUri method then returning a bool according to the connection status
//    /// </summary>
//    /// <returns></returns>
//    private bool CheckConnection()
//    {
//        bool isConnected = false;

//        string HtmlText = GetHtmlFromUri("http://google.com");

//        if (HtmlText == "")
//        {
//            isConnected = false;
//        }
//        else if (!HtmlText.Contains("schema.org/WebPage"))
//        {
//            isConnected = false;
//        }
//        else
//        {
//            isConnected = true;
//        }

//        Debug.Log("Connection check : " + isConnected);
//        return isConnected;
//    }

//    /// <summary>
//    /// This will show the ad when it is ready. (Add timer if ad fails to show after x seconds?)
//    /// </summary>
//    /// <returns></returns>
//    IEnumerator IsAddReady()
//    {
//        if(CheckConnection() == true)
//        {
//            bool isWaiting = true;
//            while (isWaiting)
//            {
//                if (Advertisement.IsReady("rewardedVideo"))
//                {
//                    isWaiting = false;
//                    var options = new ShowOptions { resultCallback = HandleShowResult };
//                    Advertisement.Show("rewardedVideo", options);
//                }
//                yield return new WaitForSeconds(0.5f);
//            }
//        }
//        else
//        {
//            adClicked = false;
//            //Show connection failed popup?
//        }   

//    }


//}
