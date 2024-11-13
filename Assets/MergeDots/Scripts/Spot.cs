using UnityEngine;

namespace MergeDots.Scripts
{
    public class Spot : MonoBehaviour
    {
        private GameController gc;
        public bool colored;
        private Lines linesScript;
        public int num;
        private Spot[] allSpots;
    
        void Start()
        {
            gc = FindObjectOfType<GameController>();
            linesScript = FindObjectOfType<Lines>();
        }

        // if only one spot selected, we show it (alpha to 1)
        // if more than one spot selected, we hide all selected spots (alpha to 0)
        public void AfterSetColor()
        {
        
            allSpots = FindObjectsOfType<Spot>();
            foreach (var curSpot in allSpots)
            {
            
                if (curSpot.colored)
                {
                    SetColor(curSpot.gameObject, gc.curNum <= 2 ? 1f : 0f);
                }
                else
                {
                    SetColor(curSpot.gameObject , 0.5f);
                }
            }
        }
    
        // setting alpha number of Spots 
        private void SetColor(GameObject go , float value)
        {
            var sr = go.GetComponent<SpriteRenderer>();
            var tempColor = sr.color;
        
            tempColor.a = value;
            sr.color = tempColor;
        }
    
        // start touching
        private void OnMouseDown()
        {
            // click on a Spot to clear it and Spots after on 
            if (gc.touchStart)
            {
                gc.ClearSpots(num+1);
            }
        
            gc.touching = true;
    
            // selecting first Spot if there is no Spots selected
            if (!gc.touchStart)
            {
                num = gc.curNum;
                gc.curNum += 1;
        
                gc.touchStart = true;
                colored = true;
                AfterSetColor();
                gc.lastObj = this;
                linesScript.DrawLines();
            }
        }
    
        // end touching
        private void OnMouseUp()
        {
            gc.touching = false;
        
            //if only one spot selected, deselect it 
            if (gc.curNum <= 2)
            {
                gc.ClearSpots(1);
                gc.lastObj = null;
                gc.touchStart = false;
            }

            gc.CheckLevelDone();
        }
    
        // continue touching/hovering
        private void OnMouseEnter()
        {
            if (!(Time.timeSinceLevelLoad > 1)) return;
            if (!gc.touching) return;
            // selecting another Spot if its near to last selected Spot
            if ((!colored) && (Vector2.Distance(this.transform.position , gc.lastObj.transform.position) <= 0.7f))
            {
                num = gc.curNum;
                gc.curNum += 1;
                
                colored = true;
                AfterSetColor();
                gc.lastObj = this;
                linesScript.DrawLines();

            }
            // cutting line (/connection of Spots) if we touch another selected Spot
            else if (colored)
            {
                if (gc.curNum > 3)
                {
                    if (num > 1)
                    {
                        gc.ClearSpots(num+1);
                    }
                }
                else
                {
                    if (num > 0)
                    {
                        gc.ClearSpots(num+1);
                    }
                }
                    
            }
        }
    
    }
}
