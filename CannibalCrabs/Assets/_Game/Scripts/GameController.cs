using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Shell> shellPrefabs;
    public Player playerPrefab;
    public int maxShells = 6;
    public List<Shell> shellsSpawned;
    public float respawnTime;

    private List<Player> players = new List<Player>();
    private List<Vector2> spawnPos = new List<Vector2>() { new Vector2(-17, 8), new Vector2(17, 8), new Vector2(-17, -8), new Vector2(17, -8), };

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        //for (int i = 0; i < Input.GetJoystickNames().Length; i++)
        for (int i = 0; i < 4; i++)
        {
            var player = Instantiate(playerPrefab);
            player.SetPlayer(i);
            player.transform.position = spawnPos[i];
            players.Add(player);
            player.OnDie += OnPlayerDeath;
        }

        for (int i = 0; i < maxShells; i++)
            SpawnShell();
        StartCoroutine(SpawnShells());
    }

    public void SpawnShell()
    {
        Shell shell = Instantiate(shellPrefabs.GetRandom());
        shell.OnEnterShell += (s) => shellsSpawned.Remove(s);
        shell.transform.position = Random.insideUnitCircle * Random.Range(.5f, 12);
        //Debug.Log($"Min size: {players.Min(player => player.size)}, max size: {players.Max(player => player.size)}");

        shell.size = Random.Range(players.Min(player => player.size), players.Max(player => player.size));
        //shell.size = Random.Range(players.Min(player => player.size), 3);
        shell.transform.localScale = Vector3.one * (1 + (shell.size * 0.25f));
        shellsSpawned.Add(shell);
    }

    IEnumerator SpawnShells()
    {
        while (true)
        {
            if (shellsSpawned.Count < maxShells)
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
