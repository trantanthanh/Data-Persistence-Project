using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;


#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [HideInInspector]
    public string playerName = "";
    [HideInInspector]
    public string bestScoreName = "";
    [HideInInspector]
    public int bestScore = 0;

    public Text BestScoreText;

    private string saveFileName = "savefile.json";
    private string pathSave;

    [SerializeField] TMP_InputField nameInput;
    void Awake()
    {
        pathSave = $"{Application.persistentDataPath}/{saveFileName}";
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadScore();
    }

    [Serializable]
    class SaveData
    {
        public string bestScoreName = "";
        public int bestScore;
        public string playerName = "";
    }

    public void SaveScore()
    {
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.bestScoreName = bestScoreName;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(pathSave, json);
    }

    public void LoadScore()
    {
        if (File.Exists(pathSave))
        {
            string json = File.ReadAllText(pathSave);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.playerName;
            bestScoreName = data.bestScoreName;
            bestScore = data.bestScore;
        }

        if (playerName != "")
        {
            SetPlayerName();
        }

        if (bestScoreName != "")
        {
            SetHighScoreName();
        }
    }

    private void SetHighScoreName()
    {
        BestScoreText.text = $"Best Score : {bestScoreName} : {bestScore}";
    }

    void ReadInputName()
    {
        playerName = nameInput.text;
    }

    void SetPlayerName()
    {
        nameInput.text = playerName;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        ReadInputName();
        if (playerName != "")
        {
            SaveScore();
            SceneManager.LoadScene("GamePlay");
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
