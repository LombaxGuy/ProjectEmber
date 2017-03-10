using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void ProjectileLaunched(Vector3 direction, float forceStrength);
    public static event ProjectileLaunched OnProjectileLaunched;

    public delegate void ProjectileUpdated(Vector3 direction, float forceStrength);
    public static event ProjectileUpdated OnProjectileUpdated;

    public delegate void ProjectileDead(int amount);
    public static event ProjectileDead OnProjectileDead;

    public delegate void ProjectileIgnite(int amount);
    public static event ProjectileIgnite OnProjectileIgnite;

    public delegate void ProjectileRespawn();
    public static event ProjectileRespawn OnProjectileRespawn;

    public delegate void GameWorldReset();
    public static event GameWorldReset OnGameWorldReset;

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

    public static void InvokeOnProjectileDead(int amount)
    {
        if (OnProjectileDead != null)
        {
            OnProjectileDead.Invoke(amount);
            Debug.Log("EventManager.cs: The event 'OnProjectileDead' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnProjectileDead' was not invoked because nothing subscibes to it.");
        }
    }

    public static void InvokeOnProjectileIgnite(int amount)
    {
        if (OnProjectileIgnite != null)
        {
            OnProjectileIgnite.Invoke(amount);
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
}
