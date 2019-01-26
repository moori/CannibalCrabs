using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Meat meatPrefab;
    public Player playerPrefab;

    private List<Player> players = new List<Player>();


    private IEnumerator Start()
    {
        //StartCoroutine(SpawnMeat());


        List<Vector2> pos = new List<Vector2>() { new Vector2(-4, 4), new Vector2(4, 4), new Vector2(-4, -4), new Vector2(4, -4), };
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
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
