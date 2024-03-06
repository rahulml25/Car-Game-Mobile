using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cars : MonoBehaviour
{

    [SerializeField] private GameObject[] carPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpanCars());
    }

    IEnumerator SpanCars()
    {
        while (!GlobalVariables.Instance.gameOver)
        {
            yield return new WaitForSeconds(Random.Range(1, 5));

            int[] indexChoices = new int[] {
                Random.Range(0, carPrefabs.Length),
                Random.Range(0, carPrefabs.Length),
                Random.Range(0, carPrefabs.Length),
                Random.Range(0, carPrefabs.Length),
                Random.Range(0, carPrefabs.Length),
            };

            int idx = GlobalVariables.RandomChoice(indexChoices);
            GameObject createdCar = Instantiate(carPrefabs[idx]);

            createdCar.transform.parent = transform;

            int sign = GlobalVariables.RandomChoice(new int[] { -1, 1 });
            createdCar.transform.position = new Vector3(1.47F * sign, 7.3F);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
