using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AnswerCeil : MonoBehaviour
{
    public Button btnAns;
    [SerializeField] Text txtAnswer;
    [SerializeField] GameObject rightObj;
    [SerializeField] GameObject wrongObj;
    [SerializeField] Image answerBG;
    [SerializeField] Image outline;
    public Answer answer;
    
    Color rightColor;
    Color wrongColor;
    Color normalColor;
    Color rightBgColor;
    Color wrongBgColor;
    Color normalBgColor;
    Color textColor;

    public void SetColors(Color rightClr,Color wrongClr, Color normalClr, Color rightBgClr, Color wrongBgClr, Color normalBgClr, Color textClr)
    {
        rightColor = rightClr;
        wrongColor = wrongClr;
        normalColor = normalClr;
        rightBgColor = rightBgClr;
        wrongBgColor = wrongBgClr;
        normalBgColor = normalBgClr;
        textColor = textClr;
    }

    public void ReSet()
    {
        txtAnswer.text = "";
        txtAnswer.color = textColor;
        rightObj.SetActive(false);
        wrongObj.SetActive(false);
        answerBG.color = normalBgColor;
        outline.color = normalColor;
        rightObj.transform.parent.gameObject.SetActive(false);
    }

    public void SetData(Answer ans)
    {
        answer = ans;
        txtAnswer.text = ans.answer;
    }

    public void SetResult(bool isCorrect)
    {
        answerBG.color = (isCorrect) ? rightBgColor : wrongBgColor;
        outline.color = (isCorrect) ? rightColor : wrongColor;
        txtAnswer.color = (isCorrect) ? rightColor : wrongColor;
        rightObj.transform.parent.gameObject.SetActive(true);
        rightObj.SetActive(isCorrect);
        wrongObj.SetActive(!isCorrect);
    }

    //public void SelectAnswer(Color _color)
    //{
    //    if (IsCorrect())
    //    {
    //        answerBG.color = _color;
    //        rightObj.transform.parent.gameObject.SetActive(false);
    //    }
    //}
    public bool IsCorrect()
    {
        return (answer.is_correct == "1") ? true : false;
    }
}
