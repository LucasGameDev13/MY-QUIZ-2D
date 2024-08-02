using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerIntro : MonoBehaviour
{
    [SerializeField] private GameObject explanationObj;
    [SerializeField] private bool isExplaning;


    private void Update()
    {
        if(!isExplaning)
        {
            explanationObj.SetActive(false);
        }
        else
        {
            explanationObj.SetActive(true);
        }
    }

    public void GamePlay(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void DisplayExplanation()
    {
        isExplaning = !isExplaning;
    }
    
}
