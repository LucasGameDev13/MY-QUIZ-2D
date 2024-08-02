using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField] private float timerValue;
    [SerializeField] private float timertoCompleteQuestion;
    [SerializeField] private float timertoShowCorrectAnswer;

    [SerializeField] private bool loadNextQuestion;
    [SerializeField] private bool isAnsweringQuestion;
    [SerializeField] private float fillFraction;

    public float _fillFraction
    {
        get { return fillFraction; }
        private set { fillFraction = value; }
    }

    public bool _loadNextQuestion
    {
        get { return loadNextQuestion; }
        set { loadNextQuestion = value; }
    }

    public bool _isAnsweringQuestion
    {
        get { return isAnsweringQuestion; }
        set { isAnsweringQuestion = value; }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }

    public void CancelTimer()
    {
        timerValue = 0;
    }

    private void UpdateTimer()
    {
        timerValue -= Time.deltaTime;

        if(isAnsweringQuestion)
        {
            if(timerValue > 0)
            {
                fillFraction = timerValue / timertoCompleteQuestion;
            }
            else
            {
                isAnsweringQuestion = false;
                timerValue = timertoShowCorrectAnswer;
            }
        }
        else
        {
            if(timerValue > 0)
            {
                fillFraction = timerValue / timertoShowCorrectAnswer;
            }
            else
            {
                isAnsweringQuestion = true;
                timerValue = timertoCompleteQuestion;
                loadNextQuestion = true;
            }
        }
    }
}
