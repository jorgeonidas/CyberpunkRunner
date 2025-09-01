using System;
using CrazyGames;
using UnityEngine;

public interface ICrazyGamesService
{
    void GameplayStart();   // Call when actual play begins/resumes
    void GameplayStop();    // Call when entering menus / pauses
    void HappyTime();       // Optional: celebrate big achievements
    void RequestMidgameAd(Action onFinished = null, Action<string> onError = null);
    void RequestRewardedAd(Action onReward = null, Action<string> onError = null);
}

public class CrazyGamesService : MonoBehaviour, ICrazyGamesService
{
    public static ICrazyGamesService Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log($"CrazySDK.IsAvailable? {CrazySDK.IsAvailable}");
        if (CrazySDK.IsAvailable)
        {
            CrazySDK.Init(() =>
            {
                Debug.Log("CrazySDK initialized in service");
            });
        }
    }

    public void GameplayStart()
    {
        // Inform CrazyGames that gameplay is active (affects site behavior and analytics)
        CrazySDK.Game.GameplayStart(); // Official Unity call
        // Optional: unmute audio or resume game time if you centralized it here.
        // AudioListener.pause = false; Time.timeScale = 1f;
    }

    public void GameplayStop()
    {
        // Inform CrazyGames that gameplay is paused/broken
        CrazySDK.Game.GameplayStop();
        // Optional: centralize pause here.
        // AudioListener.pause = true; Time.timeScale = 0f;
    }

    public void HappyTime()
    {
        // Optional confetti/celebration on site
        CrazySDK.Game.HappyTime();
    }

    public void RequestMidgameAd(Action onFinished = null, Action<string> onError = null)
    {
        // // Pause/mute before ad starts; restore on error/finish
        // CrazySDK.Ad.RequestAd(
        //     CrazyAdType.Midgame,
        //     onAdStarted: () =>
        //     {
        //         // Pause game & mute audio while the ad is showing
        //         Time.timeScale = 0f;
        //         AudioListener.pause = true;
        //     },
        //     onAdError: (error) =>
        //     {
        //         // Restore state even if no fill/error
        //         AudioListener.pause = false;
        //         Time.timeScale = 1f;
        //         onError?.Invoke(error?.Message ?? "Unknown ad error");
        //     },
        //     onAdFinished: () =>
        //     {
        //         // Restore state after ad finishes
        //         AudioListener.pause = false;
        //         Time.timeScale = 1f;
        //         onFinished?.Invoke();
        //     }
        // );
    }

    public void RequestRewardedAd(Action onReward = null, Action<string> onError = null)
    {
        // CrazySDK.Ad.RequestAd(
        //     CrazyAdType.Rewarded,
        //     onAdStarted: () =>
        //     {
        //         Time.timeScale = 0f;
        //         AudioListener.pause = true;
        //     },
        //     onAdError: (error) =>
        //     {
        //         AudioListener.pause = false;
        //         Time.timeScale = 1f;
        //         onError?.Invoke(error?.Message ?? "Unknown ad error");
        //     },
        //     onAdFinished: () =>
        //     {
        //         AudioListener.pause = false;
        //         Time.timeScale = 1f;
        //         // For rewarded ads, grant reward on finished
        //         onReward?.Invoke();
        //     }
        // );
    }
}
