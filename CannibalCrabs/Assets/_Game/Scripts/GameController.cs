using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Shell> shellPrefabs;
    public Player playerPrefab;
    public int maxShells = 6;
    public int shellsSpawned;

    private List<Player> players = new List<Player>();

    private IEnumerator Start()
    {
        Debug.Log("hm " + shellPrefabs.Count);
        for (int i = 0; i < maxShells; i++)
            SpawnShell();

        StartCoroutine(SpawnShells());

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

    public void SpawnShell()
    {
        Shell shell = Instantiate(shellPrefabs.GetRandom());
        shell.transform.position = Random.insideUnitCircle * Random.Range(.5f, 6);
        shellsSpawned++;
    }

    IEnumerator SpawnShells()
    {
        while (shellsSpawned < maxShells)
        {
            SpawnShell();
            yield return new WaitForSeconds(2);
        }
    }
}
