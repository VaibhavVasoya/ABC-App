using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;

using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Master.UI
{
    public class ScreenQuizUI : UIScreenView
    {
        [FormerlySerializedAs("txtQuestions")]
        [SerializeField] Text txtQuestionText;
        [SerializeField] Text txtQuestionNumber;
        [SerializeField] AnswerCeil[] answersCeils;
        ContentSizeFitter[] contentSizeFitters;
        //[SerializeField] ScaleUpDownAnimate objResult;

        [SerializeField] Text txtResultTitle;
        [SerializeField] Text txtResultMsg;
        //[SerializeField] ParticleSystem winEffect;
        [SerializeField] CanvasGroup loadQaTilt;

        [SerializeField] MovePanelAnimate resultPanel;
        [SerializeField] Text txtContinueBtnText;
        

        string winTitle = "Well Done!";
        string loseTitle= "Oh no!";

        string winMsg = "That is the correct answer. Keep going and see how many more question to can answer correctly.";
        string lossMsg = "You got the answer wrong. But, don’t worry keep going and see how many of the other questions you can get correct.";


        [SerializeField] Image resultImage;

        [SerializeField] Sprite winSprite;
        [SerializeField] Sprite loseSprite;


        [SerializeField] AudioSource quizAudioSource;

        Questions currentQa;
        int qaCount = 0;
        int totalNumberOfQa = 0;

        public override void OnScreenShowCalled()
        {
            base.OnScreenShowCalled();
            loadQaTilt.alpha = 0;
            totalNumberOfQa = TrailsHandler.instance.CurrentTrailPoi.questions.Count;
            //foreach (var answerCeil in answersCeils)
            //{
            //    answerCeil.SetColors(rightColor, wrongColor, normalColor, rightBgColor, wrongBgColor, normalBgColor, textColor);
            //}
            LoadNextQuestion();
        }
        public override void OnScreenShowAnimationCompleted()
        {
            base.OnScreenShowAnimationCompleted();
            Refresh();
        }
        public override void OnScreenHideCalled()
        {
            base.OnScreenHideCalled();
            qaCount = 0;
            totalNumberOfQa = 0;
            resultPanel.HideAnimation();
        }
        public override void OnBack()
        {
            base.OnBack();
            BackToTrailPoisDetails();
        }
        public async void BackToTrailPoisDetails()
        {
            //if (objResult.transform.parent.gameObject.activeInHierarchy)
            //{
            //    //objResult.HideAnimation();
            //    //Helper.Execute(this, () => objResult.transform.parent.gameObject.SetActive(false), 0.5f);
            //    await Task.Delay(TimeSpan.FromSeconds(0.4f));
            //}
            if (Result != null) StopCoroutine(Result);
            UIController.instance.ShowNextScreen(ScreenType.PoiDetails);
        }

        void LoadNextQuestion()
        {
            resultPanel.HideAnimation();
            if (qaCount < totalNumberOfQa)
            {
                currentQa = TrailsHandler.instance.CurrentTrailPoi.questions[qaCount];
                txtQuestionNumber.text = "Question "+(qaCount + 1).ToString();
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
            BackKeyActive = false;
            SetInteraction(false);
            //txtContinueBtnText.text = (qaCount < totalNumberOfQa) ? LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "CONTINUE") : LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "COMPLETED_QUIZ");
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


                answersCeils[index].SetResult(true);

                quizAudioSource.Play();

            }
            else
            {
                txtResultTitle.text = loseTitle;
                txtResultMsg.text = lossMsg;
                resultImage.sprite = loseSprite;


                answersCeils[index].SetResult(false);
                foreach (var ans in answersCeils)
                {
                    if (ans.IsCorrect())
                    {
                        ans.SetResult(true);
                        break;
                    }
                }
            }
            Result = null;
            yield return new WaitForSeconds(2);
            resultPanel.ShowAnimation();
            
        }
        public void OnClickNextQuesions()
        {
            ToggleInteraction(true);
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
            txtQuestionText.text = currentQa.question;
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