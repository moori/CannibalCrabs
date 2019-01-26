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
    private FMOD_StudioEventEmitter fmodEmitter;
    private FMOD.Studio.EventInstance bgmMusic;
    private FMOD.Studio.ParameterInstance bgm_victoty;

    private void Awake()
    {
        //bgmMusic = FMOD_StudioSystem
    }

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

        SpawnShell(true);

        for (int i = 0; i < maxShells; i++)
            SpawnShell();

        StartCoroutine(SpawnShells());
    }

    public void SpawnShell(bool isFuderosao = false)
    {
        Shell shell = Instantiate(shellPrefabs.GetRandom());
        shell.OnEnterShell += (s, p) =>
        {
            if (s.isFuderosao)
                EndGame(p);
            else
                shellsSpawned.Remove(s);
        };
        shell.transform.position = new Vector2(Random.Range(-18f, 18f), Random.Range(-8f, 8f));

        if (!isFuderosao)
        {
            int min = players.Min(player => player.size), max = players.Max(player => player.size);
            if (max < Shell.maxSize)
                max++;
            shell.size = Random.Range(min, max + 1);
            shellsSpawned.Add(shell);
        }
        else
        {
            shell.size = Shell.maxSize;
        }

        shell.transform.localScale = Vector3.one * (1 + (shell.size * 0.25f));
    }

    IEnumerator SpawnShells()
    {
        while (true)
        {
            shellsSpawned.RemoveAll(shell => shell == null);

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

    public void EndGame(Player winner)
    {
        Debug.Log("temos um grande vencedor " + winner.name);
    }
}
