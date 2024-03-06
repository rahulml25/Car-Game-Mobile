using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Road : MonoBehaviour
{

    private bool createdNext = false;

    private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject roadPrefab;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    float topY
    {
        get
        {
            return transform.position.y + spriteRenderer.bounds.size.y / 2;
        }
        set
        {
            var distance = value - topY;
            transform.position += new Vector3(0, distance);
        }
    }

    public float bottomY
    {
        set
        {
            var tempBottomY = transform.position.y - spriteRenderer.bounds.size.y / 2;
            var distance = value - tempBottomY;
            transform.position += new Vector3(0, distance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Moving road
        topY -= GlobalVariables.Instance.speed * Time.deltaTime;

        // Creating a new Read Above
        if (!createdNext && topY <= 6.3)
        {
            GameObject createdRoad = Instantiate(roadPrefab);
            createdRoad.transform.parent = transform.parent;
            createdRoad.GetComponent<Road>().bottomY = topY;

            createdNext = true;
        }

        else if (topY <= -6.3)
        {
            Destroy(gameObject);
        }
    }


}
