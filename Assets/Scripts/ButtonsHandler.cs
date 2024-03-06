using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsHandler : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        KeyboardInput();
    }

    void KeyboardInput()
    {
        if (GlobalVariables.Instance.gameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(GlobalVariables.GAMEPLAY_SCENE);
    }

}
