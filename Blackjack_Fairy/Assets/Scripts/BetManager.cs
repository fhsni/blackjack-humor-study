using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class BetManager : MonoBehaviour
{
    public GameObject bettingScreen;
    public GameObject gameScreen;


    
    public TextMeshProUGUI coinText; // 🧾 Text UI to show current coins
    public TextMeshProUGUI CurrentBet;
    public int currentBet = 0; // 🎯 The total amount the player is betting

    public Button betButton;
    public Image[] chipImages; // Array to hold chip buttons
    //public Button btn1;
    private List<Image> selectedChips = new List<Image>();  // Track selected chip
    
    private Dictionary<Button, int> chipValues = new Dictionary<Button, int>();


    void Start()
    {

        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
        // Debug.Log("Cursor: " + Cursor.lockState);

        //playerCoins = GameManager.Instance.coins;

        bettingScreen.SetActive(true);
        gameScreen.SetActive(false);

        UpdateCoinDisplay();

        //btn1.GetComponent<Button>().onClick.AddListener(() => Debug.Log("Screen clicked") );
        
        // Add listeners to each chip
       foreach (Image chip in chipImages)
        {
            chip.GetComponent<Button>().onClick.AddListener(() => SelectChip(chip));
        } 
        // Add listener to the bet button
        betButton.onClick.AddListener(OnBetConfirmed);

        
    }

    void Update()
    {
        Debug.Log("Cursor: " + Cursor.lockState);
    }

     void UpdateCoinDisplay()
{
    coinText.text = GameManager.Instance.coins.ToString();
    CurrentBet.text = currentBet.ToString();
    // here it is correct the amount of current bet
}



public void SelectChip(Image chip)
{
    Color grey = new Color32(0xBA, 0xBA, 0xBA, 0xFF);
    if (selectedChips.Contains(chip))
    {
        // Deselect if already selected
        chip.color = Color.white;
        selectedChips.Remove(chip);
    }
    else
    {
        // Select new chip
        chip.color = grey;
        selectedChips.Add(chip);
    }

    Debug.Log("Chip clicked: " + GetChipValue(chip) + " coins");
}

int GetChipValue(Image chip)
{
    Text chipText = chip.GetComponentInChildren<Text>(); // <-- Legacy Text here

    if (chipText != null)
    {
        int value;
        if (int.TryParse(chipText.text, out value))
        {
            return value;
        }
        else
        {
            Debug.LogWarning("Could not parse chip value: " + chipText.text);
            return 0;
        }
    }
    else
    {
        Debug.LogWarning("No Text found on chip: " + chip.name);
        return 0;
    }
}


   public void OnBetConfirmed()
{
    if (selectedChips.Count == 0)
    {
        Debug.Log("No chips selected! Cannot place bet.");
        return;
    }

    // Reset current bet
    currentBet = 0;

    // Sum all selected chips
    foreach (Image chip in selectedChips)
    {
        currentBet += GetChipValue(chip);
    }

    // Deduct the bet from GameManager coins
    GameManager.Instance.coins -= currentBet;
    UpdateCoinDisplay();

    // Hide betting screen and start game
    bettingScreen.SetActive(false);
    gameScreen.SetActive(true);

    Debug.Log("Bet placed: " + currentBet + " coins");

    UpdateCoinDisplay();
    GameManager.Instance.currentBet = currentBet;

    FindObjectOfType<BlackjackSceneController>().InitializeGame(currentBet);

}



}
