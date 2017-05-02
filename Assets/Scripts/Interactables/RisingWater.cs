using System.Collections;
using UnityEngine;

public class RisingWater : MonoBehaviour
{
    private WorldManager worldManager;

    private Flame flame;
    private float flameSpawnOffset = 0.5f;

    private Coroutine moveCoroutine;

    private Vector3 waterStartPosition;

    [SerializeField]
    private float topOfMap = 20;

    [SerializeField]
    private int roundsBeforeStart = 3;

    [SerializeField]
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
        EventManager.OnProjectileDeath += OnDeath;
        EventManager.OnProjectileIgnite += OnIgnite;
        EventManager.OnGameWorldReset += OnReset;
        EventManager.OnEndOfTurn += OnEndOfTurn;
    }

    private void OnDisable()
    {
        EventManager.OnProjectileLaunched -= OnShoot;
        EventManager.OnProjectileDeath -= OnDeath;
        EventManager.OnProjectileIgnite -= OnIgnite;
        EventManager.OnGameWorldReset -= OnReset;
        EventManager.OnEndOfTurn -= OnEndOfTurn;
    }

    private void OnShoot(Vector3 direction, float force)
    {
        currentRoundsInTotal++;
    }

    private void OnDeath()
    {
        if (currentRoundsInTotal > roundsBeforeStart)
        {
            MoveWater();
        }
        else
        {
            // Invokes the EndOfTurn event.
            EventManager.InvokeOnEndOfTurn();
        }
    }

    private void OnIgnite(Flammable flammableObject)
    {
        if (currentRoundsInTotal > roundsBeforeStart)
        {
            MoveWater();
        }
        else
        {
            // Invokes the EndOfTurn event.
            EventManager.InvokeOnEndOfTurn();
        }
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

    // Use this for initialization
    private void Start()
    {
        worldManager = GameObject.Find("World").GetComponent<WorldManager>();
        flame = worldManager.ActiveFlame.GetComponent<Flame>();

        waterStartPosition = transform.position;

        deltaY = Vector3.Distance(transform.position, new Vector3(transform.position.x, topOfMap, transform.position.z));
        increment = deltaY / roundsAfterStart;
    }

    private void OnEndOfTurn()
    {
        // If the waters y-position is larger than the flames spawnpoints y-position
        if (transform.position.y > flame.SpawnPoint.y - flameSpawnOffset)
        {
            // Invokes the on GameWorldReset event.
            EventManager.InvokeOnGameWorldReset();
        }
    }

    private void MoveWater()
    {
        oldY = transform.position.y;
        currentY = increment * (currentRoundsInTotal - roundsBeforeStart - 1);

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
        
        // Invokes the EndOfTurn event.
        EventManager.InvokeOnEndOfTurn();
    }
}
