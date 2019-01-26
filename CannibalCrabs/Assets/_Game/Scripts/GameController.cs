using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Shell> shellPrefabs;
    public Player playerPrefab;
    public int maxShells = 6;
    public int shellsSpawned;
    public float respawnTime;

    private List<Player> players = new List<Player>();
    private List<Vector2> spawnPos = new List<Vector2>() { new Vector2(-4, 4), new Vector2(4, 4), new Vector2(-4, -4), new Vector2(4, -4), };

    private IEnumerator Start()
    {
        for (int i = 0; i < maxShells; i++)
            SpawnShell();

        StartCoroutine(SpawnShells());

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        {
            var player = Instantiate(playerPrefab);
            player.SetPlayer(i);
            player.transform.position = spawnPos[i];
            players.Add(player);
            player.OnDie += OnPlayerDeath;
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

    public void OnPlayerDeath(Player player)
    {
        StartCoroutine(PlayerRespawnRoutine(player));
    }
    IEnumerator PlayerRespawnRoutine(Player player)
    {
        yield return new WaitForSeconds(respawnTime);
        var index = players.IndexOf(player);
        player.transform.position = spawnPos[index];
        player.gameObject.SetActive(true);
    }

}
