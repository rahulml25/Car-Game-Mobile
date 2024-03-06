using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherCars : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, GlobalVariables.Instance.speed * Time.deltaTime);

        if (transform.position.y <= -6.3)
        {
            GlobalVariables.Instance.IncreaseScore();
            Destroy(gameObject);
        }
    }
}
