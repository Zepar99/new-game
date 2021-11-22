using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;


public class script : MonoBehaviour
{
    public float holeProbability;
    public int widith, height, x, y;
    public bool[,] horizontalWalls, verticalWalls;
    public Transform Level, Player, Goal;
    public GameObject Floor, Wall;
    private float level = 0;
    public TextMeshProUGUI textLevels;
    public TextMeshProUGUI textLevels2;
    public TextMeshProUGUI High;
    public Stopwatch stop;

    public void LevelChange()
    {
        level++;
        textLevels.text = "Level:" + level.ToString("00");

    }

    void Start()
    {
        foreach (Transform child in Level)
            Destroy(child.gameObject);
        LevelChange();
        textLevels2.text = "S C O R E " + level.ToString("00");
        High.text = "H I G H E S T  S C O R E " + PlayerPrefs.GetFloat("HighScore", 00).ToString("00");


        horizontalWalls = new bool[widith + 1, height];
        verticalWalls = new bool[widith, height + 1];
        var st = new int[widith, height];


        void dfs(int x, int y)
        {
            st[x, y] = 1;
            Instantiate(Floor, new Vector3(x, y), Quaternion.identity, Level);
            var dirs = new[]
           {
                (x - 1, y, horizontalWalls, x, y, Vector3.right, 90, KeyCode.A),
                (x + 1, y, horizontalWalls, x + 1, y, Vector3.right, 90, KeyCode.D),
                (x, y - 1, verticalWalls, x, y, Vector3.up, 0, KeyCode.S),
                (x, y + 1, verticalWalls, x, y + 1, Vector3.up, 0, KeyCode.W),
            };
            foreach (var (nx, ny, wall, wx, wy, sh, ang, k) in dirs.OrderBy(d => Random.value))
                if (!(0 <= nx && nx < widith && 0 <= ny && ny < height) || (st[nx, ny] == 2 && Random.value > holeProbability))
                {
                    wall[wx, wy] = true;
                    Instantiate(Wall, new Vector3(wx, wy) - sh / 2, Quaternion.Euler(0, 0, ang), Level);
                }
                else if (st[nx, ny] == 0) dfs(nx, ny);
            st[x, y] = 2;
        }
        dfs(0, 0);
        x = Random.Range(0, widith);
        y = Random.Range(0, height);
        Player.position = new Vector3(x, y);
        do Goal.position = new Vector3(Random.Range(0, widith), Random.Range(0, height));
        while (Vector3.Distance(Player.position, Goal.position) < (widith + height) / 4);
    }
    void Update()
    {
        var dirs = new[]
        {
            (x - 1, y, horizontalWalls, x, y, Vector3.right, 90, KeyCode.A),
            (x + 1, y, horizontalWalls, x + 1, y, Vector3.right, 90, KeyCode.D),
            (x, y - 1, verticalWalls, x, y, Vector3.up, 0, KeyCode.S),
            (x, y + 1, verticalWalls, x, y + 1, Vector3.up, 0, KeyCode.W),
        };
        foreach (var (nx, ny, wall, wx, wy, sh, ang, k) in dirs.OrderBy(d => Random.value))
            if (Input.GetKeyDown(k))
                if (wall[wx, wy])
                    Player.position = Vector3.Lerp(Player.position, new Vector3(nx, ny), 0.1f);
                else (x, y) = (nx, ny);

        Player.position = Vector3.Lerp(Player.position, new Vector3(x, y), Time.deltaTime * 12);
        if (Vector3.Distance(Player.position, Goal.position) < 0.12f)
        {
            if (Random.Range(0, 3) < 3) widith++;
            else height++;
            Start();
        }
        if (level > PlayerPrefs.GetFloat("HighScore", 00))
        {
            PlayerPrefs.SetFloat("HighScore", level);
        }
        if (stop.stopWatchActive == false)
        {
            Destroy(gameObject);
        }
    }

}