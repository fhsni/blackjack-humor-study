using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int coins ;

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

}
}
