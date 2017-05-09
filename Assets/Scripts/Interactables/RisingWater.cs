using System.Collections;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    private WorldManager worldManager;

    private Flame flame;
    private float flameSpawnOffset = 0.5f;

    private Flammable[] levelFlammables;

    private Coroutine moveCoroutine;

    private Vector3 waterStartPosition;

    private float topOfMap = 20;

    private int roundsBeforeStart = 3;

    private int roundsAfterStart = 5;

    private int currentRoundsInTotal = 0;

    // This value should always be less than the extingushTime in the flame script.
    private float waterRiseTime = 1.5f;

    private float deltaY = 0;
    private float increment = 0;
    private float currentY = 0;
    private float oldY = 0;

    private void OnEnable()
    {
        EventManager.OnProjectileLaunched += OnShoot;
        //EventManager.OnProjectileDeath += OnDeath;
        //EventManager.OnProjectileIgnite += OnIgnite;
        EventManager.OnWaterMove += OnWaterMove;
        EventManager.OnGameWorldReset += OnReset;
        EventManager.OnEndOfTurn += OnEndOfTurn;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnShoot;
        //EventManager.OnProjectileDeath -= OnDeath;
        //EventManager.OnProjectileIgnite -= OnIgnite;
        EventManager.OnWaterMove -= OnWaterMove;
        EventManager.OnGameWorldReset -= OnReset;
        EventManager.OnEndOfTurn -= OnEndOfTurn;
    }

    private void OnShoot(Vector3 direction, float force)
    {
        currentRoundsInTotal++;
    }

    private void OnDeath()
    {
        MoveWater();

        //if (currentRoundsInTotal > roundsBeforeStart)
        //{
        //    MoveWater();
        //}
        //else
        //{
        //    // Invokes the EndOfTurn event.
        //    EventManager.InvokeOnEndOfTurn();
        //}
    }

    private void OnIgnite(Flammable flammableObject)
    {
        MoveWater();

        //if (currentRoundsInTotal > roundsBeforeStart)
        //{
        //    MoveWater();
        //}
        //else
        //{
        //    // Invokes the EndOfTurn event.
        //    EventManager.InvokeOnEndOfTurn();
        //}
    }

    private void OnWaterMove()
    {
        MoveWater();
    }

    private void OnReset()
    {
        currentRoundsInTotal = 0;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        transform.position = waterStartPosition;
    }

    private void OnEndOfTurn()
    {
        Vector3 highestCheckpoint = Vector3.zero;
        Flammable flammableObject = levelFlammables[levelFlammables.Length - 1];

        for (int i = 0; i < levelFlammables.Length; i++)
        {
            if (levelFlammables[i].transform.position.y > highestCheckpoint.y && levelFlammables[i].OnFire)
            {
                highestCheckpoint = levelFlammables[i].transform.position;
                flammableObject = levelFlammables[i];
            }
        }

        // If the waters y-position is larger than the highest spawnpoint y-position
        if (transform.position.y > highestCheckpoint.y - flameSpawnOffset)
        {
            worldManager.LevelEnded = true;
            
            // Invokes the on GameWorldReset event.
            //EventManager.InvokeOnLevelLost();
        }
        else if (transform.position.y > flame.SpawnPoint.y - flameSpawnOffset)
        {
            flame.SpawnPoint = flammableObject.SpawnPoint;
        }
    }

    // Use this for initialization
    private void Start()
    {
        worldManager = GameObject.Find("World").GetComponent<WorldManager>();
        flame = worldManager.ActiveFlame.GetComponent<Flame>();

        topOfMap = worldManager.TopOfLevelYCoordinate;
        roundsBeforeStart = worldManager.RoundsBeforeWaterRising;
        roundsAfterStart = worldManager.RoundsAfterWaterRising;

        GameObject[] levelFlammableObjects = GameObject.FindGameObjectsWithTag("FlammableObject");
        levelFlammables = new Flammable[levelFlammableObjects.Length];

        currentY = transform.position.y;

        for (int i = 0; i < levelFlammableObjects.Length; i++)
        {
            try
            {
                levelFlammables[i] = levelFlammableObjects[i].GetComponent<Flammable>();
            }
            catch
            {
                Debug.LogError("RisingWater.cs: Object number " + i + " does not have a Flammable component even though it is tagged as a FlammableObject.");
            }
        }

        waterStartPosition = transform.position;

        deltaY = Vector3.Distance(transform.position, new Vector3(transform.position.x, topOfMap, transform.position.z));
        increment = deltaY / roundsAfterStart;
    }


    private void MoveWater()
    {
        oldY = transform.position.y;

        int rounds = currentRoundsInTotal - roundsBeforeStart < 0 ? 0 : currentRoundsInTotal - roundsBeforeStart;

        currentY = waterStartPosition.y + increment * rounds;

        moveCoroutine = StartCoroutine(CoroutineMove());
    }

    private IEnumerator CoroutineMove()
    {
        float t = 0;
        float s = 0;

        while (t < 1)
        {
            t += Time.deltaTime / waterRiseTime;

            s = 0.5f * Mathf.Sin((t - 0.5f) / (1 / Mathf.PI)) + 0.5f;

            transform.position = new Vector3(transform.position.x, Mathf.Lerp(oldY, currentY, s), transform.position.z);

            yield return null;
        }

        // Invokes the OnEndOfTurn event.
        //EventManager.InvokeOnEndOfTurn();

        // Invokes the OnRespawn event.
        //EventManager.InvokeOnProjectileRespawn();

        // Invokes a sequence of events that should be called when a turn ends.
        EventManager.InvokeEndTurnSequence(worldManager);
    }
}
