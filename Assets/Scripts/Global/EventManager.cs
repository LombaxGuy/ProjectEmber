using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void ProjectileLaunched(Vector3 direction, float forceStrength);
    public static event ProjectileLaunched OnProjectileLaunched;

    public delegate void ProjectileUpdated(Vector3 direction, float forceStrength);
    public static event ProjectileUpdated OnProjectileUpdated;

    public delegate void ProjectileDeath();
    public static event ProjectileDeath OnProjectileDeath;

    public delegate void ProjectileIgnite(Flammable flammableObject);
    public static event ProjectileIgnite OnProjectileIgnite;

    public delegate void ProjectileRespawn();
    public static event ProjectileRespawn OnProjectileRespawn;

    public delegate void SetNewSpawnPoint(Vector3 newCheckpoint);
    public static event SetNewSpawnPoint OnSetNewSpawnPoint;

    public delegate void GameWorldReset();
    public static event GameWorldReset OnGameWorldReset;

    public delegate void EndOfTurn();
    public static event EndOfTurn OnEndOfTurn;

    public delegate void LevelLost();
    public static event LevelLost OnLevelLost;

    public delegate void LevelCompleted();
    public static event LevelCompleted OnLevelCompleted;

    public delegate void ShootingStarted();
    public static event ShootingStarted OnShootingStarted;

    public delegate void ShootingEnded();
    public static event ShootingStarted OnShootingEnded;
    
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

    public static void InvokeOnProjectileDeath()
    {
        if (OnProjectileDeath != null)
        {
            OnProjectileDeath.Invoke();
            Debug.Log("EventManager.cs: The event 'OnProjectileDeath' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnProjectileDeath' was not invoked because nothing subscibes to it.");
        }
    }

    public static void InvokeOnProjectileIgnite(Flammable flammableObject)
    {
        if (OnProjectileIgnite != null)
        {
            OnProjectileIgnite.Invoke(flammableObject);
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

    public static void InvokeOnSetNewSpawnPoint(Vector3 spawnPoint)
    {
        if (OnSetNewSpawnPoint != null)
        {
            OnSetNewSpawnPoint.Invoke(spawnPoint);
            Debug.Log("EventManager.cs: The event 'OnSetNewSpawnPoint' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnSetNewSpawnPoint' was not invoked because nothing subscibes to it.");
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

    public static void InvokeOnEndOfTurn()
    {
        if (OnEndOfTurn != null)
        {
            OnEndOfTurn.Invoke();
            Debug.Log("EventManager.cs: The event 'OnEndOfTurn' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnEndOfTurn' was not invoked because nothing subscibes to it.");
        }
    }

    public static void InvokeOnLevelLost()
    {
        if (OnLevelLost != null)
        {
            OnLevelLost.Invoke();
            Debug.Log("EventManager.cs: The event 'OnLevelLost' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnLevelLost' was not invoked because nothing subscibes to it.");
        }
    }

    public static void InvokeOnLevelCompleted()
    {
        if (OnLevelCompleted != null)
        {
            OnLevelCompleted.Invoke();
            Debug.Log("EventManager.cs: The event 'OnLevelCompleted' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnLevelCompleted' was not invoked because nothing subscibes to it.");
        }
    }

    public static void InvokeOnShootingStarted()
    {
        if(OnShootingStarted != null)
        {
            OnShootingStarted.Invoke();
            Debug.Log("EventManager.cs: The event 'OnShootingStarted' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnShootingStarted' was not invoked because nothing subscibes to it.");
        }
    }

    public static void InvokeOnShootingEnded()
    {
        if (OnShootingEnded != null)
        {
            OnShootingEnded.Invoke();
            Debug.Log("EventManager.cs: The event 'OnShootingStarted' was invoked.");
        }
        else
        {
            Debug.Log("EventManager.cs: The event 'OnShootingStarted' was not invoked because nothing subscibes to it.");
        }
    }
}
