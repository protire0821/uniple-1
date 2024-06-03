using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject pacman;
    public GameObject startPanel;
    public GameObject gamePanel;
    public GameObject introPanel;
    public GameObject startCountDownPrefab;
    public GameObject gameoverPrefab;
    public GameObject winPrefab;
    public AudioClip startClip;
    public Text remainText;
    public Text nowText;
    public Text scoreText;

    public List<int> usingIndex = new List<int>();
    public List<int> rawIndex = new List<int> { 0, 1, 2, 3 };
    private List<GameObject> pacdotGos = new List<GameObject>();
    private int pacdotNum = 0;
    private int nowEat = 0;
    public int score = 0;
    public int totalLevel = 2;
    private int currentLevel = 1; // 1 表示 Level 1，2 表示 Level 2
    private bool nextLevel = false;

    private void Awake()
    {
        _instance = this;
        Screen.SetResolution(1024, 768, false);
        int tempCount = rawIndex.Count;
        for (int i = 0; i < tempCount; i++)
        {
            int tempIndex = Random.Range(0, rawIndex.Count);
            usingIndex.Add(rawIndex[tempIndex]);
            rawIndex.RemoveAt(tempIndex);
        }

        LoadLevel(currentLevel);
    }

    private void Start()
    {
        SetGameState(false);
    }

    private void Update()
    {
        if (gamePanel.activeInHierarchy)
        {
            remainText.text = "Remain:\n\n" + (pacdotNum - nowEat);
            nowText.text = "Eaten:\n\n" + nowEat;
            scoreText.text = "Score:\n\n" + score;
        }

        if (nowEat == pacdotNum)
        {
            SetGameState(false);
            introPanel.SetActive(true);
            DisplayIntro(currentLevel);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                nextLevel = true;
                introPanel.SetActive(false);
            }
        }

        if (nextLevel)
        {
            if (currentLevel == totalLevel)
            {
                gamePanel.SetActive(false);
                introPanel.SetActive(false);
                Instantiate(winPrefab);
                StopAllCoroutines();
                SetGameState(false);
                if (Input.anyKeyDown)
                {
                    SceneManager.LoadScene("SampleScene");
                }
            }
            else
            {
                nextLevel = false;
                currentLevel++;
                LoadLevel(currentLevel);
                StartCoroutine(PlayStartCountDown());
            }
        }
    }

    public void OnStartButton()
    {
        StartCoroutine(PlayStartCountDown());
        AudioSource.PlayClipAtPoint(startClip, new Vector3(0, 0, -5));
        startPanel.SetActive(false);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    IEnumerator PlayStartCountDown()
    {
        GameObject go = Instantiate(startCountDownPrefab);
        yield return new WaitForSeconds(4f);
        Destroy(go);
        SetGameState(true);
        gamePanel.SetActive(true);
        GetComponent<AudioSource>().Play();
    }

    public void OnEatPacdot(GameObject go)
    {
        nowEat++;
        score += 100;
        pacdotGos.Remove(go);
        Destroy(go);
    }

    private void SetGameState(bool state)
    {
        pacman.GetComponent<PacmanMove>().enabled = state;
    }

    private void LoadLevel(int level)
    {
        pacdotGos.Clear();
        nowEat = 0;

        GameObject maze = GameObject.Find("Maze");
        if (maze != null)
        {
            // Hide all levels
            for (int i = 1; i <= totalLevel; i++)
            {
                Transform levelTransform = maze.transform.Find("Level " + i);
                if (levelTransform != null)
                {
                    levelTransform.gameObject.SetActive(false);
                }
            }

            // Show current level
            Transform currentLevelTransform = maze.transform.Find("Level " + level);
            if (currentLevelTransform != null)
            {
                currentLevelTransform.gameObject.SetActive(true);
                foreach (Transform t in currentLevelTransform)
                {
                    pacdotGos.Add(t.gameObject);
                }
            }
        }

        pacdotNum = pacdotGos.Count;
    }

    private void DisplayIntro(int level)
    {
        GameObject IntroPanel = GameObject.Find("IntroPanel");
        
        if (IntroPanel != null)
        {
            // Hide all levels
            for (int i = 1; i <= totalLevel; i++)
            {
                Transform levelTransform = IntroPanel.transform.Find("Level " + i);
                if (levelTransform != null)
                {
                    levelTransform.gameObject.SetActive(false);
                }
            }

            // Show current level
            Transform currentLevelTransform = IntroPanel.transform.Find("Level " + level);
            if (currentLevelTransform != null)
            {
                currentLevelTransform.gameObject.SetActive(true);
            }
        }
    }
}
