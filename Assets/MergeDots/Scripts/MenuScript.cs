using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

namespace MergeDots.Scripts
{
    public class MenuScript : MonoBehaviour
    {
        private General generalScript;
        public GameObject levelsArea;
        public GameObject menuArea;
        public GameObject fader;
        public GameObject content;

        public Button btnPlay;
        public Button btnBack;
        public GameObject btnLevel;

        public GameObject scrollBack;

        public bool scrollEnable;

        private GameController gameController;
        
        void Start()
        {
            // fade out at start
            fader.GetComponent<Image>().enabled = true;
            LeanTween.alpha(fader.GetComponent<Image>().rectTransform, 0f, 1).setEase(LeanTweenType.easeOutQuint).setDelay(0.5f);
        
        
            generalScript = FindObjectOfType<General>();
            gameController = FindObjectOfType<GameController>();
        
            btnBack.GetComponent<Button>().enabled = false;
        
            CreateLevelButtons();

            var areaPosition = levelsArea.transform.position;
            areaPosition = new Vector3(areaPosition.x + 1080, areaPosition.y,areaPosition.z);
            levelsArea.transform.position = areaPosition;

            Button btnPlayButton = btnPlay.GetComponent<Button>();
            btnPlayButton.onClick.AddListener(BtnPlayClick);
        
            Button btnBackButton = btnBack.GetComponent<Button>();
            btnBackButton.onClick.AddListener(BtnBackClick);

            LeanTween.alpha(btnBack.image.rectTransform, 0f, 0f);
        }


        // creating level buttons 
        private void CreateLevelButtons()
        {
            for (var i = 1; i <= generalScript.allLevels; i++)
            {
                GameObject btnLevelObject = Instantiate(btnLevel, content.transform.position, content.transform.rotation);
                btnLevelObject.transform.SetParent(content.transform);
                btnLevelObject.transform.localScale = new Vector3(1, 1, 1);
                btnLevelObject.GetComponentInChildren<TMP_Text>().text = i.ToString();
                btnLevelObject.GetComponent<Button>().interactable = true;

                var num = i;
                // create click listener of current level button
                btnLevelObject.GetComponent<Button>().onClick.AddListener(() => { BtnLevelClick(num); });
            
                // disable current level button if its number isn't in list of passedLevels(variable from generalScript.cs)
                if (generalScript.passedLevels.Contains("," + i + ",")) continue;
                btnLevelObject.GetComponent<Button>().interactable = false;
                btnLevelObject.GetComponentInChildren<TMP_Text>().color = new Color(255,255,255,0.5f);
            }
        }


        private void BtnPlayClick()
        {
            // moving level buttons(they are in levelsArea) to the left (screen center)
            var areaPosition = levelsArea.transform.position;
            LeanTween.moveX(levelsArea,areaPosition.x - 1080, 0.5f)
                .setEase(LeanTweenType.easeOutQuint);

            // moving menu to the left
            var menuAreaPosition = menuArea.transform.position;
            LeanTween.moveX(menuArea.gameObject, menuAreaPosition.x - 1080, 0.5f)
                .setEase(LeanTweenType.easeOutQuint);

            // showing BtnBack 
            LeanTween.alpha(btnBack.image.rectTransform, 1f, 0.3f)
                .setDelay(0f)
                .setEase(LeanTweenType.linear)
                .setOnComplete(OnShowingLevelButtons);
            btnBack.GetComponent<Button>().enabled = true;

        }

        private void BtnBackClick()
        {
            // moving level buttons(they are in levelsArea) to the right
            var areaPosition = levelsArea.transform.position;
            LeanTween.moveX(levelsArea,areaPosition.x + 1080, 0.5f)
                .setEase(LeanTweenType.easeOutQuint);

            // moving menu to the right (screen center)
            var menuAreaPosition = menuArea.transform.position;
            LeanTween.moveX(menuArea.gameObject, menuAreaPosition.x + 1080, 0.5f)
                .setEase(LeanTweenType.easeOutQuint);

            // hiding BtnBack 
            LeanTween.alpha(btnBack.image.rectTransform, 0f, 0f)
                .setDelay(0.2f)
                .setEase(LeanTweenType.linear);
            btnBack.GetComponent<Button>().enabled = false;

            OnHidingLevelButtons();
        }
    
        private void OnShowingLevelButtons()
        {
            // activate scrolling when level buttons are showing
            scrollEnable = true;
            scrollBack.GetComponent<ScrollRect>().enabled = true;
        }
    
        private void OnHidingLevelButtons()
        {
            // deactivate scrolling when level buttons aren't showing
            scrollEnable = false;
            scrollBack.GetComponent<ScrollRect>().enabled = false;
        }

        private void BtnLevelClick(int levelNum)
        {
            // going to game, to level of levelNum after clicking BtnLevel
            generalScript.level = levelNum;
        
            LeanTween.alpha(fader.GetComponent<Image>().rectTransform, 1f, 0.5f).setEase(LeanTweenType.easeOutQuint)
                .setOnComplete(GotoGame);

        }

        void GotoGame()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
