using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.PlayerPrefs;
namespace MergeDots.Scripts
{
    public class GameController : MonoBehaviour
    {
        public Spot lastObj;
        public bool touchStart;
        public bool touching;
        public int curNum = 1;
        private Spot[] allSpots;
        private Lines linesScript;
        public bool getCode;
        [TextArea(5,5)] public string generatedCode = "";
        public string[] levels;

        private int levelNumber
        {
            get => GetInt("MyLevelNumber");
            set => SetInt("MyLevelNumber",value);
        }
        private void Start()
        {
            allSpots = FindObjectsOfType<Spot>();
            linesScript = FindObjectOfType<Lines>();

            if (getCode)
            {
                GenerateCode();
            }
            else
            {
                CreateFromCode(levels[levelNumber]);
            }
        }
        private void GenerateCode()
        {
            generatedCode = "";
            Spot[] spots = FindObjectsOfType<Spot>();
            foreach (var spot in spots)
            {
                var position = spot.transform.position;
                generatedCode += (Mathf.Round(position.x *100) /100) + "," + (Mathf.Round(position.y *100) /100) + "|";
            }
        }
        private void CreateFromCode(string code)
        {
        
            var spots = FindObjectsOfType<Spot>();
            foreach (var spot in spots)
            {
                Destroy(spot.gameObject);
            }
            if (code == "")
            {
                SceneManager.LoadScene("Menu");
            }
            else
            {
                var spotsParent = new GameObject("AllSpots");
                spotsParent.transform.parent = transform;
                var mainToken = code.Split('|');
                for (int i = 0; i < mainToken.Length-1; i++)
                {
                    var token = mainToken[i].Split(',');
                
                    var x = float.Parse(token[0]);
                    var y = float.Parse(token[1]);
                    var pos = new Vector3(x,y,0);
                    var newObj = FindObjectOfType<Spot>();
                    var newSpot = Instantiate(newObj.gameObject, pos, transform.rotation);
                    newSpot.name = "Spot";
                    newSpot.transform.parent = spotsParent.transform;
                    newSpot.GetComponent<Spot>().enabled = true;
                    newSpot.GetComponent<BoxCollider2D>().enabled = true;
    
                }
            }
        }

        public void ClearSpots(int number)
        {
            // deselecting all Spots with num => number
            allSpots = FindObjectsOfType<Spot>();
        
            foreach (var curSpot in allSpots)
            {
                if (curSpot.num >= number)
                {
                    curSpot.num = 0;
                    curSpot.colored = false;
                    curNum -= 1;
                    curSpot.AfterSetColor();
                    linesScript.DrawLines();
                }
            }
            var v = -Mathf.Infinity;
            foreach (var curSpot in allSpots)
            {
                if (!curSpot.colored || !(curSpot.num > v)) continue;
                v = curSpot.num;
                lastObj = curSpot;
            }
        }
        public void CheckLevelDone()
        {
            var allFull = true;
            var spots = FindObjectsOfType<Spot>();
            foreach (var curSpot in spots)
            {
                if (!curSpot.colored)
                {
                    allFull = false;
                }
            }
            if (!allFull) return;
            levelNumber++;
            levelNumber = Mathf.Clamp(levelNumber, 0, levels.Length);
            StartCoroutine(WaitThenLoadScene(1.5f));

        }
        private IEnumerator WaitThenLoadScene(float time)
        {
            yield return new WaitForSeconds(time);
             SceneManager.LoadScene("Game");
        }
    }
}