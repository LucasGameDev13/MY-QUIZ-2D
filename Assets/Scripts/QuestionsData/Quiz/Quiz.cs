using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{

    private Timer timerScript;

    [Header("Questions Settings")]
    [SerializeField] private List<QuestionsSO> questionsSO = new List<QuestionsSO>();
    private QuestionsSO currentQuestionSO;
    [SerializeField] private TextMeshProUGUI questionText;

    [Header("Answers Settings")]
    [SerializeField] private GameObject[] answersButtons = new GameObject[4];
    private int correctAnswerIndex;
    private bool hasAnsweredEarly = true;

    [Header("Buttons Settings")]
    [SerializeField] private Sprite defaultAnswerSprite;
    [SerializeField] private Sprite correctAnswerSprite;

    [Header("Timer")]
    [SerializeField] private Image timerImage;

    [Header("Scoring")]
    [SerializeField] private TextMeshProUGUI scoreText;
    private ScoreKeeper scoreKeeper;

    [Header("Bar Settings")]
    [SerializeField] private Slider progressBar;
    [SerializeField] private bool isCompleted;

    public bool _isCompleted
    {
        get { return isCompleted; }
        private set { isCompleted = value; }
    }

    void Awake()
    {
        timerScript = FindObjectOfType<Timer>();
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        progressBar.maxValue = questionsSO.Count;
        progressBar.value = 0;
    }

    void Update()
    {
        //Updating the timer image according with the time fraction
        timerImage.fillAmount = timerScript._fillFraction;

        //Checking out the conditions to load the next question
        if (timerScript._loadNextQuestion)
        {
            //Checking if the game has already achieved the total of questions
            if (progressBar.value == progressBar.maxValue)
            {
                //If yes, get out.
                isCompleted = true;
                return;
            }


            hasAnsweredEarly = false;
            GetNextQuestion();
            timerScript._loadNextQuestion = false;
        }
        //Checking out a new condition, where if the time to answer the question has gone and the player didn't answer
        //So show up the right answer
        else if(!hasAnsweredEarly && !timerScript._isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    //Function to call the next question
    private void GetNextQuestion()
    {
        //It works only if I have questions to be asked
        if (questionsSO.Count > 0)
        {
            SetButtonState(true);
            SetDefaultButtonSprites();
            GetRandomQuestion();
            DisplayQuestion();
            progressBar.value++;
            scoreKeeper.IncrementQuestionsSeen();
        }
    }

    //Function to make the questions random
    private void GetRandomQuestion()
    {
        int index = Random.Range(0, questionsSO.Count);
        currentQuestionSO = questionsSO[index];

        //As soon as the question to be released .. take it off from the questions list
        if (questionsSO.Contains(currentQuestionSO))
        {
            questionsSO.Remove(currentQuestionSO);
        }
    }

    //Method to show up the question ans the answers will be selected on the game
    private void DisplayQuestion()
    {
        questionText.text = currentQuestionSO.GetQuestion();

        for (int i = 0; i < answersButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answersButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestionSO.GetAnswer(i);
        }
    }

    //Method to be called on the game buttons
    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timerScript.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculeteScore() + "%";
    }

    //Method to check out the right and the wrong answers
    private void DisplayAnswer(int index)
    {
        Image buttonImage;

        //If right, display the correct message and change the button sprite
        if (index == currentQuestionSO.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!!!";
            buttonImage = answersButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else // If wrong, display the wrong message and change the button sprite, indicating the right answer
        {
            correctAnswerIndex = currentQuestionSO.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestionSO.GetAnswer(correctAnswerIndex);
            questionText.text = "Sorry, the correct answer was; \n" + correctAnswer;
            buttonImage = answersButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    //Method to call off the buttons' interactivity
    private void SetButtonState(bool state)
    {
        for(int i = 0; i < answersButtons.Length; i++)
        {
            Button button = answersButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

    //Method to turn back the buttons' sprite to normal
    private void SetDefaultButtonSprites()
    {
        Image buttonImage;

        for(int i = 0; i < answersButtons.Length; i++)
        {
            buttonImage = answersButtons[i].GetComponent<Image>();
            buttonImage.sprite = defaultAnswerSprite;
        }
    }
}
