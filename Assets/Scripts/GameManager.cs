using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string playerName = "";
    public string bestScoreName = "";
    public int bestScore = 0;

    private string saveFileName = "savefile.json";
    private string pathSave;
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
        SceneManager.LoadScene("GamePlay");
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
