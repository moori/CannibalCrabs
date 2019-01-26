using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameController : MonoBehaviour
{
    public Meat meatPrefab;
    public Player playerPrefab;

    private List<Player> players = new List<Player>();


    private void Start()
    {
        StartCoroutine(SpawnMeat());


        List<Vector2> pos = new List<Vector2>() { new Vector2(-4, 4), new Vector2(4, 4), new Vector2(-4, -4), new Vector2(4, -4), };
        for (int i = 0; i < 4; i++)
        {
            var p = Instantiate(playerPrefab);
            p.SetPlayer(i);
            p.transform.position = pos[i];
            players.Add(p);
        }
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
