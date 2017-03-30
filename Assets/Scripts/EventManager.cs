using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void ProjectileLaunched(Vector3 direction, float forceStrength);
    public static event ProjectileLaunched OnProjectileLaunched;

    public delegate void ProjectileUpdated(Vector3 direction, float forceStrength);
    public static event ProjectileUpdated OnProjectileUpdated;

    public delegate void ProjectileDeath(int amount);
    public static event ProjectileDeath OnProjectileDeath;

    public delegate void ProjectileIgnite(int amount, Vector3 newCheckpoint);
    public static event ProjectileIgnite OnProjectileIgnite;

    public delegate void ProjectileRespawn();
    public static event ProjectileRespawn OnProjectileRespawn;

    public delegate void GameWorldReset();
    public static event GameWorldReset OnGameWorldReset;

    public delegate void MenuWellSelected(int wellIndex);
    public static event MenuWellSelected OnMenuWellSelected;

    public delegate void MenuLevelSelected(int wellIndex, int levelIndex);
    public static event MenuLevelSelected OnMenuLevelSelected;

    public static void InvokeOnProjectileLaunched(Vector3 direction, float forceStrength)
    {
        if (OnProjectileLaunched != null)
        {
            OnProjectileLaunched.Invoke(direction, forceStrength);
            Debug.Log("EventManager.cs: The event 'OnProjectileLaunched' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnProjectileLaunched' was not invoked because nothing subscibes to it.");
        }
    }

    public static void InvokeOnProjectileUpdated(Vector3 direction, float forceStrength)
    {
        if (OnProjectileUpdated != null)
        {
            OnProjectileUpdated.Invoke(direction, forceStrength);
            Debug.Log("EventManager.cs: The event 'OnProjectileUpdated' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnProjectileUpdated' was not invoked because nothing subscibes to it.");
        }
    }

    public static void InvokeOnProjectileDeath(int amount)
    {
        if (OnProjectileDeath != null)
        {
            OnProjectileDeath.Invoke(amount);
            Debug.Log("EventManager.cs: The event 'OnProjectileDeath' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnProjectileDeath' was not invoked because nothing subscibes to it.");
        }
    }

    public static void InvokeOnProjectileIgnite(int amount, Vector3 newCheckpoint)
    {
        if (OnProjectileIgnite != null)
        {
            OnProjectileIgnite.Invoke(amount,newCheckpoint);
            Debug.Log("EventManager.cs: The event 'OnProjectileIgnite' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnProjectileIgnite' was not invoked because nothing subscibes to it.");
        }
    }

    public static void InvokeOnProjectileRespawn()
    {
        if (OnProjectileRespawn != null)
        {
            OnProjectileRespawn.Invoke();
            Debug.Log("EventManager.cs: The event 'OnProjectileRespawn' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnProjectileRespawn' was not invoked because nothing subscibes to it.");
        }
    }

    public static void InvokeOnGameWorldReset()
    {
        if (OnGameWorldReset != null)
        {
            OnGameWorldReset.Invoke();
            Debug.Log("EventManager.cs: The event 'OnGameWorldReset' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnGameWorldReset' was not invoked because nothing subscibes to it.");
        }
    }

    public static void InvokeOnMenuWellSelected(int wellIndex)
    {
        if (OnMenuWellSelected != null)
        {
            OnMenuWellSelected.Invoke(wellIndex);
            Debug.Log("EventManager.cs: The event 'OnMenuWellSelected' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnMenuWellSelected' was not invoked because nothing subscibes to it.");
        }
    }

    public static void InvokeOnMenuLevelSelected(int wellIndex, int levelIndex)
    {
        if (OnMenuLevelSelected != null)
        {
            OnMenuLevelSelected.Invoke(wellIndex, levelIndex);
            Debug.Log("EventManager.cs: The event 'OnMenuLevelSelected' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnMenuLevelSelected' was not invoked because nothing subscibes to it.");
        }
    }
}
