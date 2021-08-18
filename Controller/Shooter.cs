using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// MonoBehaviour class used to load the canon, show next bubble inline and shoot bubble
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class Shooter : MonoBehaviour
{
    public GameObject[] pfBubbles;
    public GameObject firePoint;
    public int ammoSize = 10;
    public float bubbleForce = 20;
    private List<GameObject> bubbles;
    private int randomPick;
    private const float YPos = -3.5f;
    private const float XPos = -1f;
    private Material material;
    private MeshRenderer thisMaterial;
    private int color;
    private const float DelayTime = 0.5f;
    private float counter = 0;
    private GameObject loadedBubble;
    private GameObject nextBubble;
    private void Start()
    {
        thisMaterial = GetComponent<MeshRenderer>();
        bubbles = new List<GameObject>();
        FillRandom(bubbles, ammoSize);
        LoadCanon();
        showNextInLine();
    }
    
    private void Update()
    {
        if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && counter > DelayTime)
        {
            if (bubbles.Count == 5)
            {
                FillRandom(bubbles, ammoSize);
            }
            else
            {
                shoot();
                AudioManager.instance.Play("Shot");
                LoadCanon();
                showNextInLine();
                counter = 0;
            }
        }   
        // Prevent the canon from spamming bubbles
        counter += Time.deltaTime; 
    }

    /// <summary>
    /// Fill a GameObject array with random Prefab bubbles
    /// </summary>
    /// <param name="arr">GameObject array</param>
    private void FillRandom(List<GameObject> arr, int size)
    {
        for (var i = 0; i < size; i++)
        {
            randomPick = Random.Range(0, pfBubbles.Length);
            arr.Add(pfBubbles[randomPick]);
        }
    }

    /// <summary>
    /// Shoot a bubble in the the direction of the firePoint and destroy the next bubble in line to make
    /// room for a new random picked bubble.
    /// </summary>
    private void shoot()
    {
        var name = loadedBubble.name;
        loadedBubble = Instantiate(loadedBubble, new Vector3(0f, YPos, 0f), loadedBubble.transform.rotation);
        loadedBubble.name = name;
        var rb = loadedBubble.GetComponent<Rigidbody>();
        rb.AddForce(firePoint.transform.up * bubbleForce, ForceMode.Impulse);
        bubbles.RemoveAt(0);
        Destroy(nextBubble);
    }

    /// <summary>
    /// Load the canon with a random picked bubble and set this objects material to the picked bubbles material
    /// so the bubble will attach to the canon and follow the rotation.
    /// </summary>
    private void LoadCanon()
    {
        loadedBubble = bubbles[0];
        material = loadedBubble.GetComponent<MeshRenderer>().sharedMaterial;
        thisMaterial.material = material;
    }

    /// <summary>
    /// Instantiate and next bubble in line to be shoot.
    /// </summary>
    private void showNextInLine()
    {
        var loaded = bubbles[1];
        nextBubble = Instantiate(loaded, new Vector3(XPos, YPos, -0.5f), loaded.transform.rotation);
    }
}
