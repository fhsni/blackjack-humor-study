using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;


public class BlackjackSceneController : MonoBehaviour
{
    [Header("Player Cards")]
    public GameObject playerCard3;
    public GameObject playerCard4;

    [Header("Dealer Cards")]
    public SpriteRenderer dealerCard2Renderer;  // The SpriteRenderer on the back-facing card
    public Sprite dealerCard2Face;              // The correct face (like 2 of spades)
    public Button hit;
    public Button Stay; 


    public GameObject dealerCard3;

    public Animator robotAnimator;
    public AudioSource robotAudioSource;
    public AudioClip robotVoiceClip;
    
    private bool isVoicePlaying = false;
    private bool overrideAnimations = false;

    public GameObject winImage;
    public GameObject loseImage;
    public GameObject pushImage;

    public TextMeshProUGUI coinText; 
    public TextMeshProUGUI CurrentBet;
    public TextMeshProUGUI playerSumText;
    
    public GameResult currentResult = GameResult.Win;  // Default value

    [Header("Player Card Values")]
    public int playerCard1Value = 10;
    public int playerCard2Value = 5;
    public int playerCard3Value = 4;
    public int playerCard4Value = 2;

    [Header("Dealer Final Sum")]
    public int dealerFinalSum = 20;


    
    public int betAmount = 100; // Current round bet

    public float displayTime = 10f; // time to show result

    private int playerSum = 0;
    private int hitCount = 0;

    void Start()
    {

        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
        // Debug.Log("Cursor: " + Cursor.lockState);

        coinText.text = GameManager.Instance.coins.ToString();
        betAmount = GameManager.Instance.currentBet;
        CurrentBet.text = betAmount.ToString();
        
        Debug.Log("bet amount " + betAmount.ToString());

        playerSum = playerCard1Value + playerCard2Value;
        playerSumText.text = "Your Total: " + playerSum.ToString();
        Debug.Log(playerSum);


        // Re-link the rating panel in the new scene
        // 
        
        
    }
    public void InitializeGame(int bet)
    {
        betAmount = bet;
        coinText.text = GameManager.Instance.coins.ToString();
        CurrentBet.text = betAmount.ToString();
        Debug.Log("Initialized with bet: " + betAmount);
    }   
    public void OnHitClicked()
    {
        hitCount++;

        if (hitCount == 1 && playerCard3 != null)
        {
            playerCard3.SetActive(true);
            playerCard3.GetComponent<Animator>().Play("cardFlip");
            playerSum += playerCard3Value;
            playerSumText.text = "Your Total: " + playerSum.ToString();
            Debug.Log(playerSum);
        }
        else if (hitCount == 2 && playerCard4 != null)
        {
        playerCard4.SetActive(true);
        playerCard4.GetComponent<Animator>().Play("cardFlip2");
        playerSum += playerCard4Value;
        playerSumText.text = "Your Total: " + playerSum.ToString();
        Debug.Log(playerSum);
        }

        // if (hitCount == 2 || (hitCount == 1 && playerCard4 == null))
        // {
        // Debug.Log("NO");
        // }

        if (playerSum >= 21)
        {
            
            DisableButtons();
            Invoke(nameof(HandleDelayedGameOver), 1f);
            //GameOver();

            var ratingManager = FindObjectOfType<RatingManager>();
        if (ratingManager != null)
        {
            ratingManager.ShowRatingPanelWithDelay();
        }
        else
        {
            Debug.LogError("⚠️ RatingManager not found in scene!");
        }    
        
        }
    }

     
    public void OnStayClicked()
    {
        DisableButtons();

        // if (dealerFinalSum > 21)
        //     currentResult = GameResult.Win;
        // else if (playerSum > dealerFinalSum)
        //     currentResult = GameResult.Win;
        // else if (playerSum == dealerFinalSum)
        //     currentResult = GameResult.Push;
        // else
        //     currentResult = GameResult.Lose;

        Invoke(nameof(HandleDelayedGameOver), 1f);
        // GameOver();
        var ratingManager = FindObjectOfType<RatingManager>();
        if (ratingManager != null)
        {
            ratingManager.ShowRatingPanelWithDelay();
        }
        else
        {
            Debug.LogError("⚠️ RatingManager not found in scene!");
        }    
    }


        public void DisableButtons()
        {
            hit.interactable = false;
            Stay.interactable = false;
            //hitbuttonText.color = hit.interactable ? color.white : color.gray;
            hit.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
            Stay.GetComponentInChildren<TextMeshProUGUI>().color = Color.gray;
        }   

    void GameOver()
    {
    
        dealerCard2Renderer.sprite = dealerCard2Face;  // Flip the card by changing the sprite

        if ( dealerCard3 != null)
        {
            dealerCard3.SetActive(true);
            dealerCard3.GetComponent<Animator>().Play("cardFlip3");
        }

        Invoke(nameof(HandleDelayedGameResult), 1f);



        // Show the appropriate one

       //HandleGameResult(currentResult); 


        //  // 1. Trigger the animation
        // robotAnimator.Play("IdleTalk");

        // // 2. Play the voice audio
        // robotAudioSource.clip = robotVoiceClip;
        // robotAudioSource.Play();
        // robotAnimator.SetTrigger("IdleBlendTrigger");
        // isVoicePlaying = true;
        // overrideAnimations = true;



    }

    public enum GameResult { Win, Lose, Push }

void HandleGameResult(GameResult result)
{
    
    switch (result)
    {
        case GameResult.Win:
        
            GameManager.Instance.coins += betAmount * 2;
            winImage.SetActive(true);
            break;
        case GameResult.Lose:
            // Bet is already deducted before playing (optional)
            loseImage.SetActive(true);
            break;
        case GameResult.Push:
            GameManager.Instance.coins += betAmount;
            pushImage.SetActive(true);
            break;
    }
    
     // 1. Trigger the animation
        robotAnimator.Play("IdleTalk");

        // 2. Play the voice audio
        robotAudioSource.clip = robotVoiceClip;
        robotAudioSource.Play();
        robotAnimator.SetTrigger("IdleBlendTrigger");
        isVoicePlaying = true;
        overrideAnimations = true;

    coinText.text = GameManager.Instance.coins.ToString();
    CurrentBet.text = 0.ToString();


}


    void Update()
    {
        Debug.Log("Cursor: " + Cursor.lockState);
        // Check if the audio has finished playing
        if (isVoicePlaying && robotAudioSource.time >= robotAudioSource.clip.length)
        {
            robotAnimator.SetTrigger("IdleBlendTrigger");
            isVoicePlaying = false;
            overrideAnimations = false;
        }

        // Force IdleTalk state by locking parameters
        
    }

    void HideAll()
    {
        winImage.SetActive(false);
        loseImage.SetActive(false);
        pushImage.SetActive(false);
    }

    private void HandleDelayedGameResult()
{
    HandleGameResult(currentResult); // Make sure 'currentResult' is accessible at class level
}

 private void HandleDelayedGameOver()
{
    GameOver(); // Make sure 'currentResult' is accessible at class level
}

}
