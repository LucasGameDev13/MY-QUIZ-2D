using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Quiz quizScript;
    [SerializeField] private GameObject endScreenScript;

    private void Awake()
    {
        quizScript = FindObjectOfType<Quiz>();
    }

    // Start is called before the first frame update
    void Start()
    {
        quizScript.gameObject.SetActive(true);
        endScreenScript.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(quizScript._isCompleted)
        {
            quizScript.gameObject.SetActive(false);
            endScreenScript.SetActive(true);
            endScreenScript.GetComponent<EndScreen>().ShowFinalScore();
        }
    }

    public void OnReplayLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
