using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Master.UI
{
    public class ScreenQuizUI : UIScreenView
    {
        [SerializeField] Text txtQuestions;
        [SerializeField] AnswerCeil[] answersCeils;

        [SerializeField] ScaleUpDownAnimate objResult;

        [SerializeField] Text txtResultTitle;
        [SerializeField] Text txtResultMsg;
        //[SerializeField] ParticleSystem winEffect;
        [SerializeField] CanvasGroup loadQaTilt;

        //[SerializeField] Color rightColor;
        //[SerializeField] Color wrongColor;
        //[SerializeField] Color normalColor;
        //[SerializeField] Color rightBgColor;
        //[SerializeField] Color wrongBgColor;
        //[SerializeField] Color normalBgColor;
        //[SerializeField] Color textColor;

        [SerializeField] Text txtContinueBtnText;
        
        [SerializeField] ContentSizeFitter[] contentSizeFitters;

        string winTitle = "Well Done!";
        string loseTitle= "Oh no!";

        string winMsg = "That is the correct answer. Keep going and see how many more question to can answer correctly.";
        string lossMsg = "You got the answer wrong. But, don’t worry keep going and see how many of the other questions you can get correct.";


        [SerializeField] Image resultImage;

        [SerializeField] Sprite winSprite;
        [SerializeField] Sprite loseSprite;

        Questions currentQa;
        int qaCount = 0;
        int totalNumberOfQa = 0;

        public override void OnScreenShowCalled()
        {
            loadQaTilt.alpha = 0;
            totalNumberOfQa = TrailsHandler.instance.CurrentTrailPoi.questions.Count;
            //foreach (var answerCeil in answersCeils)
            //{
            //    answerCeil.SetColors(rightColor, wrongColor, normalColor, rightBgColor, wrongBgColor, normalBgColor, textColor);
            //}
            LoadNextQuestion();
            base.OnScreenShowCalled();
        }
        public override void OnScreenHideCalled()
        {
            base.OnScreenHideCalled();
            qaCount = 0;
            totalNumberOfQa = 0;
        }
        public override void OnBack()
        {
            base.OnBack();
            BackToTrailPoisDetails();
        }
        public async void BackToTrailPoisDetails()
        {
            if (objResult.transform.parent.gameObject.activeInHierarchy)
            {
                objResult.HideAnimation();
                Helper.Execute(this, () => objResult.transform.parent.gameObject.SetActive(false), 0.5f);
                await Task.Delay(TimeSpan.FromSeconds(0.4f));
            }
            if (Result != null) StopCoroutine(Result);
            UIController.instance.ShowNextScreen(ScreenType.PoiDetails);
        }

        void LoadNextQuestion()
        {
            if (qaCount < totalNumberOfQa)
            {
                currentQa = TrailsHandler.instance.CurrentTrailPoi.questions[qaCount];
                LoadQuestions();
                qaCount++;
            }
            else
            {
                StartCoroutine(LoadQa = ShowLoading(0));
                Debug.LogError("Quiz Completed.");
                BackToTrailPoisDetails();
            }
            Refresh();
        }

        public async void OnClickAnswer(int index)
        {
            SetInteraction(false);
            txtContinueBtnText.text = (qaCount < totalNumberOfQa) ? LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "CONTINUE") : LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "COMPLETED_QUIZ");
            StartCoroutine(Result = ShowResult(index));
        }
        IEnumerator Result;
        IEnumerator ShowResult(int index)
        {
            if (answersCeils[index].IsCorrect())
            {
                txtResultTitle.text = winTitle;
                txtResultMsg.text = winMsg;
                resultImage.sprite = winSprite;
                //answersCeils[index].SetResult(true);
                //answersCeils[index].SelectAnswer(ansCorrectColor);
                yield return new WaitForSeconds(2);
                //txtResultMsg.text = LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "Correct");// winMsg;
                //objResult.transform.parent.gameObject.SetActive(true);
                //objResult.ShowAnimation();
                //winEffect.Play();
                //AudioManager.inst.Play(Sounds.QaCorrect);
            }
            else
            {
                txtResultTitle.text = loseTitle;
                txtResultMsg.text = lossMsg;
                resultImage.sprite = loseSprite;
                //answersCeils[index].SetResult(false);
                //foreach (var ans in answersCeils)
                //{
                //    if (ans.IsCorrect())
                //    {
                //        ans.SetResult(true);
                //        break;
                //    }
                //}
                //AudioManager.inst.Play(Sounds.QaIncorrect);
                yield return new WaitForSeconds(2);
                //txtResultMsg.text = LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "Wrong"); //lossMsg;
                //objResult.transform.parent.gameObject.SetActive(true);
                //objResult.ShowAnimation();
            }
            Result = null;
        }
        public void OnClickNextQuesions()
        {
            objResult.HideAnimation();
            Helper.Execute(this, () => objResult.transform.parent.gameObject.SetActive(false), 0.5f);
            LoadNextQuestion();
        }
        void SetInteraction(bool isEnable)
        {
            foreach (var item in answersCeils)
            {
                item.btnAns.interactable = isEnable;
            }
        }
        [EasyButtons.Button]
        async void LoadQuestions()
        {
            if (LoadQa != null)
                StopCoroutine(LoadQa);
            StartCoroutine(LoadQa = ShowLoading(0));
            await Task.Delay(TimeSpan.FromSeconds(0.4f));
            SetInteraction(true);
            foreach (var item in answersCeils)
            {
                item.ReSet();
                item.gameObject.SetActive(false);
            }
            txtQuestions.text = currentQa.question;
            for (int i = 0; i < currentQa.answers.Count; i++)
            {
                answersCeils[i].gameObject.SetActive(true);
                answersCeils[i].SetData(currentQa.answers[i]);
            }

            //QA Loaded
            if (LoadQa != null)
                StopCoroutine(LoadQa);
            StartCoroutine(LoadQa = ShowLoading(1));

        }

        IEnumerator LoadQa;
        IEnumerator ShowLoading(int newAmount)
        {
            if (loadQaTilt.alpha == newAmount) yield break;
            float animationTime = 0.4f;
            float elapsed = 0;
            float perc;
            float oldAmount = (newAmount == 1) ? 0 : 1;//  loadQaTilt.color.a;
            while (elapsed < animationTime)
            {
                perc = elapsed / animationTime;
                loadQaTilt.alpha = Mathf.Lerp(oldAmount, newAmount, perc);
                elapsed += Time.deltaTime;
                yield return 0f;
            }
            loadQaTilt.alpha = newAmount;
        }

        void Refresh()
        {
            contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
            Array.Reverse(contentSizeFitters);
            UIController.instance.RefreshContent(contentSizeFitters);
        }
    }
}