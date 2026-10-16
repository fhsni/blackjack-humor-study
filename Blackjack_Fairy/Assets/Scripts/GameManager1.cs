using UnityEngine;
using System.IO;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 Instance { get; private set; }
    public int coins ;
    public int lastRating = -1;

    public int currentBet = 0;

    private string filePath;

    void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        // DontDestroyOnLoad(gameObject);

        coins = 2000;
        PlayerPrefs.SetInt("coins", coins);
        PlayerPrefs.Save();

        // ✅ Set default coins only if not set already
        if (PlayerPrefs.HasKey("coins"))
        {
            coins = PlayerPrefs.GetInt("coins");
        }
        else
        {
            PlayerPrefs.DeleteKey("coins");
            coins = 2000;
            PlayerPrefs.SetInt("coins", coins);
        }
    }
    else
    {
        Destroy(gameObject);
    }

}


    public void SaveRating(string sceneName, int rating)
    {
        lastRating = rating;
        string line = $"{sceneName},{rating},{coins}";
        File.AppendAllText(filePath, line + "\n");
    }
}
