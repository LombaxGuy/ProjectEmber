using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    #region Flame Management
    private GameObject activeFlame;

    public GameObject ActiveFlame
    {
        get { return activeFlame; }
        set { activeFlame = value; }
    }
    #endregion

    /// <summary>
    /// Subscribes to events.
    /// </summary>
    private void OnEnable()
    {
        EventManager.OnProjectileIgnite += OnIgnite;
    }
    /// <summary>
    /// Unsubscribes from events.
    /// </summary>
    private void OnDisable()
    {
        EventManager.OnProjectileIgnite -= OnIgnite;
    }

    private void OnIgnite(Vector3 newCheckpoint)
    {

    }

    /// <summary>
    /// Contains the kill condition for the whole of the game not made to work yet
    /// </summary>
    void Update()
    {

            // Invokes the on GameWorldReset event.
            EventManager.InvokeOnGameWorldReset();
    }
}
