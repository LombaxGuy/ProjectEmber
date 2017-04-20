// #############################################################################
// Look at Material.Lerp
// #############################################################################

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour
{

    [SerializeField]
    private int health;

    [SerializeField]
    private Texture burntTexture;

    private Color B_color;
    private Color W_color;

    private Material myMaterial;

    public int Health
    {
        get
        {
            return health;
        }

    }

    private void OnEnable()
    {

        EventManager.OnGameWorldReset += OnWorldReset;
        EventManager.OnProjectileIgnite += OnIgnite;
    }

    private void OnDisable()
    {

        EventManager.OnGameWorldReset -= OnWorldReset;
        EventManager.OnProjectileIgnite -= OnIgnite;
    }

    private void OnWorldReset()
    {
        Reset();
    }

    private void OnIgnite(int amount, Vector3 newCheckpoint)
    {
        
    }

    // Use this for initialization
    void Start()
    {
        myMaterial = gameObject.GetComponent<Renderer>().material;
        B_color = new Color(0,0,0,1);
        W_color = new Color(255,255,255,1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Reset()
    {
        gameObject.GetComponent<Collider>().enabled = true;
    }

    public void FlameHitTransition()
    {
        StartCoroutine(Burn());
        
        gameObject.GetComponent<Collider>().enabled = false;
    }


    private IEnumerator Burn()
    {
        float t = 0;

        while (myMaterial.color != B_color)
        {
            t += Time.deltaTime / 4f;
            myMaterial.color = Color.Lerp(myMaterial.color, Color.black, t);

            yield return null;
        }
            
            yield return new WaitForSeconds(0.1f);
            myMaterial.mainTexture = burntTexture;
            t = 0;

        while (myMaterial.color != W_color)
        {
            t += Time.deltaTime / 4f;
            myMaterial.color = Color.Lerp(myMaterial.color, Color.white, t);

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        Debug.Log("Done");
    }

}
