using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using System;
using UnityEngine.SceneManagement;


public class RatingManager : MonoBehaviour
{
    public GameObject ratingPanel;
    public Button[] starButtons; // Assign your 5 star buttons
    public Button submitButton;  // Assign this in the inspector
    public Color offColor = Color.black;
    public Color onColor = Color.white;

    private int currentRating = -1; // -1 means no rating yet
    private string filePath;

    public string nextSceneName;

    public GameObject thankYouCanvas;

    void Start()
    {
        submitButton.interactable = false ;
        // filePath = Path.Combine(Application.persistentDataPath, "ratings.csv");
        // filePath = "/Users/fairy/blackjack-kyle/ratings.csv";
        // Debug.Log(filePath);


        // Create file with header if doesn't exist
        // if (!File.Exists(filePath))
        // {
        //     File.WriteAllText(filePath, "Timestamp,Scene,Rating\n");
        // }

        // Assign listener to Submit button
        submitButton.onClick.AddListener(SaveRating);
    }

    // void Awake()
    // {
    //     DontDestroyOnLoad(gameObject);
    // }

        public void ShowRatingPanelWithDelay()
    {
        StartCoroutine(ShowPanelAfterDelay());
    }


    private IEnumerator ShowPanelAfterDelay()
    {
        yield return new WaitForSeconds(7f);
        ratingPanel.SetActive(true);
    }

    public void OnStarSelected(int rating)
    {
        currentRating = rating + 1; // So clicking 0 means 1 star, 4 means 5 stars

        submitButton.interactable = true;
        
        Debug.Log("Robot rated: " + currentRating + " stars");

        for (int i = 0; i < starButtons.Length; i++)
        {
            Image starImage = starButtons[i].GetComponent<Image>();
            starImage.color = (i <= rating) ? onColor : offColor;
        }

            
    }
    //     IEnumerator GoToNextScene()
    // {
    //     yield return new WaitForSeconds(0f); // Let the user see rating
    //     SceneManager.LoadScene("Robot_win2"); // Change this to actual name of next scene
    //     //FindObjectOfType<RatingManager>().ratingPanel = GameObject.Find("Rating panel 2");
    // }

        IEnumerator GoToNextScene()
    {
        yield return new WaitForSeconds(0f);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.Log("🎉 Study complete! No next scene to load.");
            ratingPanel.SetActive(false); // Hide rating panel
            thankYouCanvas.SetActive(true);
            // Optional: show a thank-you screen or exit
            // Application.Quit(); // If you want to quit the app
            // ratingPanel.SetActive(false); // Hide the panel
        }
    }

    // public void SaveRating()
    // {
    //     if (currentRating == -1)
    //     {
    //         Debug.LogWarning("No rating selected.");
    //         return;
    //     }
       
    //    //OnStarSelected(currentRating);

    //     string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    //     string scene = SceneManager.GetActiveScene().name;

    //     string line = $"{timestamp},{scene},{currentRating}\n";
    //     File.AppendAllText(filePath, line);

    //     Debug.Log("Rating saved to CSV: " + line);
    //     // Optionally hide panel here: ratingPanel.SetActive(false);
       
    //    // save coins before switching scenes
    //     PlayerPrefs.SetInt("coins", GameManager.Instance.coins);
    //     PlayerPrefs.Save();

    //     // Go to next scene after short delay
    //     StartCoroutine(GoToNextScene());
    //     ratingPanel.SetActive(false); 
    // }

        public void SaveRating()
    {
        if (currentRating == -1)
        {
            Debug.LogWarning("No rating selected.");
            return;
        }

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string scene = SceneManager.GetActiveScene().name;
        
        StartCoroutine(PostToGoogle(timestamp, scene, currentRating.ToString()));

        // Save coins before switching scenes
        PlayerPrefs.SetInt("coins", GameManager.Instance.coins);
        PlayerPrefs.Save();

        StartCoroutine(GoToNextScene());
        ratingPanel.SetActive(false); 
    }


            private IEnumerator PostToGoogle(string timestamp, string scene, string rating)
    {
        string url = "https://docs.google.com/forms/d/e/1FAIpQLSfOfRXVpGEqgWMYfGH3bI0td4rc-ZatNbLhQ_43dB2SChxOkA/formResponse";

        WWWForm form = new WWWForm();
        form.AddField("entry.813599091", timestamp); // field 1
        form.AddField("entry.1653432649", scene);     // field 2
        form.AddField("entry.1913495270", rating);    // field 3

        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                Debug.LogError("❌ Error submitting rating: " + www.error);
            }
            else
            {
                Debug.Log("✅ Rating submitted successfully.");
            }
        }
    }

    


    
}
