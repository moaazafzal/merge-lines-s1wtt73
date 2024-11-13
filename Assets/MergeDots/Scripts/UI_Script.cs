using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MergeDots.Scripts
{
    public class UI_Script : MonoBehaviour
    {
        private General generalScript;
        public TMP_Text txtLevel;
        public GameObject btnPause;
        public GameObject pauseBox;
        public GameObject black;
        public GameObject fader;
    
        public GameObject btnClose;
        public GameObject btnReset;
        public GameObject btnHome;
    

        void Start()
        {
            generalScript = FindObjectOfType<General>();
            SetLevelText();

            CreateButtonsListener();

            fader.GetComponent<Image>().enabled = true;
            LeanTween.alpha(fader.GetComponent<Image>().rectTransform, 0f, 1).setEase(LeanTweenType.easeOutQuint).setDelay(0.5f);
        }
    

        // creating click listeners of bellow buttons
        public void CreateButtonsListener()
        {
            var btnPauseButton = btnPause.GetComponent<Button>();
            btnPauseButton.onClick.AddListener(BtnPauseClick);

            var btnCloseButton = btnClose.GetComponent<Button>();
            btnCloseButton.onClick.AddListener(BtnCloseClick);

            var btnResetButton = btnReset.GetComponent<Button>();
            btnResetButton.onClick.AddListener(BtnResetClick);

            var btnHomeButton = btnHome.GetComponent<Button>();
            btnHomeButton.onClick.AddListener(BtnHomeClick);
        }
    

        public void SetLevelText()
        {
            //showing level number
            if (generalScript!=null)
            {
                txtLevel.text = "Lv. " + generalScript.level;
            }
        }

        // showing pause popup
        private void BtnPauseClick()
        {
            LeanTween.moveLocalY(pauseBox, 0, 0.3f).setEase(LeanTweenType.easeOutQuint);
            LeanTween.alpha(black.GetComponent<Image>().rectTransform, 0.5f, 0.3f).setEase(LeanTweenType.easeOutQuint);
        }
    
        // closing pause popup
        private void BtnCloseClick()
        {
            LeanTween.moveLocalY(pauseBox, 1700, 0.3f).setEase(LeanTweenType.easeOutQuint);
            LeanTween.alpha(black.GetComponent<Image>().rectTransform, 0f, 0.3f).setEase(LeanTweenType.easeOutQuint);
        }


        private void BtnResetClick()
        {
            LeanTween.alpha(fader.GetComponent<Image>().rectTransform, 1f, 0.5f).setEase(LeanTweenType.easeOutQuint).setOnComplete(ResetGame);
        }

        private void BtnHomeClick()
        {
            LeanTween.alpha(fader.GetComponent<Image>().rectTransform, 1f, 0.5f).setEase(LeanTweenType.easeOutQuint).setOnComplete(GoHome);
        }

        private void GoHome()
        {
            SceneManager.LoadScene("Menu");
        }

        private void ResetGame()
        {
            SceneManager.LoadScene("Game");
        }

    }
}
