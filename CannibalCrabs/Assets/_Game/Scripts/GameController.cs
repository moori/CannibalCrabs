using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [FMODUnity.EventRef]
    public FMOD.Studio.EventInstance bgmMusic;
    private FMOD.Studio.ParameterInstance bgm_victoty;

    public List<Shell> shellPrefabs;
    public Player playerPrefab;
    public int maxShells = 6;
    public List<Shell> shellsSpawned;
    public float respawnTime;
    public Sprite masterShellSprite;

    private List<Player> players = new List<Player>();
    private List<Vector2> spawnPos = new List<Vector2>() { new Vector2(-17, 8), new Vector2(17, 8), new Vector2(-17, -8), new Vector2(17, -8), };
    private FMOD_StudioEventEmitter fmodEmitter;

    public ParticleSystem deathPartPrefab;
    public LayerMask shellSpawnMask;

    public TextMeshProUGUI winnerText;
    private void Awake()
    {

        bgmMusic = FMODUnity.RuntimeManager.CreateInstance("event:/Music/music_gameplay");
        bgmMusic.getParameter("victory", out bgm_victoty); //The first proximidadeEnemigo is the name you gave in your FMOD Project
        bgmMusic.start(); //Start your music
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
        shell.transform.position = ShellSpawnPosition();

        if (!isFuderosao)
        {
            int min = 0, max = 0, forceSize = -1;

            foreach (Player player in players)
            {
                if (!shellsSpawned.Any(ss => ss.size == player.size))
                {
                    forceSize = player.size;
                    break;
                }

                if (player.size < min)
                    min = player.size;

                if (player.size > max)
                    max = player.size;
            }

            if (forceSize >= 0)
            {
                shell.size = forceSize;
            }
            else
            {
                if (max < Shell.maxSize && !shellsSpawned.Any(ss => ss.size == max + 1))
                    max++;

                shell.size = Random.Range(min, max + 1);
            }
            
            shellsSpawned.Add(shell);
        }
        else
        {
            shell.size = Shell.maxSize;
            shell.sprite.sprite = masterShellSprite;
        }
        shell.transform.localScale = Vector3.one * Size.sizeScale[shell.size];
    }

    IEnumerator SpawnShells()
    {
        while (true)
        {
            shellsSpawned.RemoveAll(shell => shell == null);
            int shorterCrabSize = players.Min(player => player.size);
            IEnumerable<Shell> uselessShells = shellsSpawned.Where(ss => ss.size < shorterCrabSize);

            foreach (Shell uselessShell in uselessShells)
                uselessShell.BreakShell();

            if (shellsSpawned.Count < maxShells)
                SpawnShell();
            yield return new WaitForSeconds(2);
        }
    }

    public Vector3 ShellSpawnPosition()
    {
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < 100; i++)
        //while (true)
        {

            if (Physics2D.OverlapCircle(pos, 3, shellSpawnMask) == null)
            {
                break;
            }
            pos = new Vector2(Random.Range(-18f, 18f), Random.Range(-8f, 8f));
        }
        return pos;

    }

    public void OnPlayerDeath(Player player)
    {
        StartCoroutine(PlayerRespawnRoutine(player));
        var part = Instantiate(deathPartPrefab);
        part.transform.position = player.transform.position;
    }

    IEnumerator PlayerRespawnRoutine(Player player)
    {
        yield return new WaitForSeconds(respawnTime);
        var index = players.IndexOf(player);
        player.transform.position = spawnPos[index];
        player.gameObject.SetActive(true);
        player.SetVisibility(true);
    }

    public void EndGame(Player winner)
    {
        bgm_victoty.setValue(1f); //calculateEnemyDistance()is your method that will return the correct distance to be passed to FMOD)
        var cam = FindObjectOfType<Camera>();
        cam.DOOrthoSize(8, 1).SetEase(Ease.InOutQuad);
        cam.transform.SetParent(winner.transform);
        cam.transform.DOLocalMove(new Vector3(0, 0, cam.transform.localPosition.z), 1).SetEase(Ease.InOutQuad);

        winnerText.gameObject.SetActive(true);
        winnerText.color = winner.color;
    }
}

public static class Size
{
    public static float[] sizeScale = new float[] { .8f, 1.1f, 1.5f, 2, 2.5f };
}
