using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Meat meatPrefab;

    private void Start()
    {
        StartCoroutine(SpawnMeat());
    }

    IEnumerator SpawnMeat()
    {
        while (true)
        {
            var meat = Instantiate(meatPrefab);
            meat.transform.position = Random.insideUnitCircle * Random.Range(.5f, 6);
            yield return new WaitForSeconds(2);
        }
    }
}
