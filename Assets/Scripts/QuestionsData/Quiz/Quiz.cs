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
        timerImage.fillAmount = timerScript._fillFraction;
        if (timerScript._loadNextQuestion)
        {
            if (progressBar.value == progressBar.maxValue)
            {
                isCompleted = true;
                return;
            }


            hasAnsweredEarly = false;
            GetNextQuestion();
            timerScript._loadNextQuestion = false;
        }
        else if(!hasAnsweredEarly && !timerScript._isAnsweringQuestion)
        {
            DisplayAnswer(-1);
            SetButtonState(false);
        }
    }

    private void GetNextQuestion()
    {
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

    private void GetRandomQuestion()
    {
        int index = Random.Range(0, questionsSO.Count);
        currentQuestionSO = questionsSO[index];

        if (questionsSO.Contains(currentQuestionSO))
        {
            questionsSO.Remove(currentQuestionSO);
        }
    }

    private void DisplayQuestion()
    {
        questionText.text = currentQuestionSO.GetQuestion();

        for (int i = 0; i < answersButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = answersButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = currentQuestionSO.GetAnswer(i);
        }
    }

    public void OnAnswerSelected(int index)
    {
        hasAnsweredEarly = true;
        DisplayAnswer(index);
        SetButtonState(false);
        timerScript.CancelTimer();
        scoreText.text = "Score: " + scoreKeeper.CalculeteScore() + "%";


    }

    private void DisplayAnswer(int index)
    {
        Image buttonImage;

        if (index == currentQuestionSO.GetCorrectAnswerIndex())
        {
            questionText.text = "Correct!!!";
            buttonImage = answersButtons[index].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
            scoreKeeper.IncrementCorrectAnswers();
        }
        else
        {
            correctAnswerIndex = currentQuestionSO.GetCorrectAnswerIndex();
            string correctAnswer = currentQuestionSO.GetAnswer(correctAnswerIndex);
            questionText.text = "Sorry, the correct answer was; \n" + correctAnswer;
            buttonImage = answersButtons[correctAnswerIndex].GetComponent<Image>();
            buttonImage.sprite = correctAnswerSprite;
        }
    }

    private void SetButtonState(bool state)
    {
        for(int i = 0; i < answersButtons.Length; i++)
        {
            Button button = answersButtons[i].GetComponent<Button>();
            button.interactable = state;
        }
    }

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
