using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GlobalVariables : MonoBehaviour
{
    // This is where you create the global variable that can be accessed from anywhere.
    private int score = 0, highScore = 0;
    [SerializeField] public int speed = 8;
    [HideInInspector] public bool gameOver = false;

    [SerializeField] private GameObject gameOverObject;
    [SerializeField] private Text highScoreField, currentScoreField;

    public const string CAR_TAG = "car";
    public const string GAMEPLAY_SCENE = "GamePlay";
    private const string HIGH_SCORE_KEY = "HighScore";


    // This part helps ensure there's only one instance of this class.
    private static GlobalVariables instance;

    public static GlobalVariables Instance
    {
        get { return instance; }
    }


    private void Awake()
    {
        // Check if there's already an instance of this class. If yes, destroy this one. If not, set this as the instance.
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else { instance = this; }
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY);
        highScoreField.text = "HighScore: " + highScore;
    }

    private void Update()
    {

    }

    public void IncreaseScore()
    {
        score += 1;
        currentScoreField.text = "Score: " + score;

        if (score > highScore)
        {
            highScore = score;
        }
        highScoreField.text = "HighScore: " + highScore;
    }

    public void GameOver()
    {
        speed = 0;
        gameOver = true;
        SaveGame();

        gameOverObject.SetActive(true);
    }

    void SaveGame()
    {
        if (score >= highScore)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
        }
        PlayerPrefs.Save();
    }


    public static T RandomChoice<T>(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    public static IEnumerator TransitionPositionX(GameObject theObject, float startX, float endX, float duration)
    {
        float elapsedTime = 0f;

        // Positions
        Vector3 startPos = new(startX, theObject.transform.position.y, theObject.transform.position.z);
        Vector3 endPos = new(endX, theObject.transform.position.y, theObject.transform.position.z);

        while (elapsedTime < duration)
        {
            // Calculate the interpolation factor (0 to 1)
            float t = elapsedTime / duration;

            // Smoothly transition the position using Lerp
            theObject.transform.position = Vector3.Lerp(startPos, endPos, t);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure final position is accurate
        theObject.transform.position = new Vector3(endX, theObject.transform.position.y, theObject.transform.position.z);
    }

    public static IEnumerator TransitionPositionY(GameObject theObject, float startY, float endY, float duration)
    {
        float elapsedTime = 0f;

        // Positions
        Vector3 startPos = new(theObject.transform.position.x, startY, theObject.transform.position.z);
        Vector3 endPos = new(theObject.transform.position.x, endY, theObject.transform.position.z);

        while (elapsedTime < duration)
        {
            // Calculate the interpolation factor (0 to 1)
            float t = elapsedTime / duration;

            // Smoothly transition the position using Lerp
            theObject.transform.position = Vector3.Lerp(startPos, endPos, t);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure final position is accurate
        theObject.transform.position = new Vector3(theObject.transform.position.x, endY, theObject.transform.position.z);
    }

    public static IEnumerator TransitionLocalPositionX(GameObject theObject, float startX, float endX, float duration)
    {
        float elapsedTime = 0f;

        // Positions
        Vector3 startPos = new(startX, theObject.transform.localPosition.y, theObject.transform.localPosition.z);
        Vector3 endPos = new(endX, theObject.transform.localPosition.y, theObject.transform.localPosition.z);

        while (elapsedTime < duration)
        {
            // Calculate the interpolation factor (0 to 1)
            float t = elapsedTime / duration;

            // Smoothly transition the position using Lerp
            theObject.transform.localPosition = Vector3.Lerp(startPos, endPos, t);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure final position is accurate
        theObject.transform.localPosition = new Vector3(endX, theObject.transform.localPosition.y, theObject.transform.localPosition.z);
    }

    public static IEnumerator TransitionLocalPositionY(GameObject theObject, float startY, float endY, float duration)
    {
        float elapsedTime = 0f;

        // Positions
        Vector3 startPos = new(theObject.transform.localPosition.x, startY, theObject.transform.localPosition.z);
        Vector3 endPos = new(theObject.transform.localPosition.x, endY, theObject.transform.localPosition.z);

        while (elapsedTime < duration)
        {
            // Calculate the interpolation factor (0 to 1)
            float t = elapsedTime / duration;

            // Smoothly transition the position using Lerp
            theObject.transform.localPosition = Vector3.Lerp(startPos, endPos, t);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure final position is accurate
        theObject.transform.localPosition = new Vector3(theObject.transform.localPosition.x, endY, theObject.transform.localPosition.z);
    }

}
