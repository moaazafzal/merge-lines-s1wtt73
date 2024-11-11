using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Lines : MonoBehaviour
{
    Vector3 startPos;
    Vector3 endPos;
    public LineRenderer lr;
    int linesCount = 0;
    private Spot[] allSpots;
    
    List<Spot> spotsList = new List<Spot>();
    List<Spot> orderedSpotsList = new List<Spot>();
    
    
    void Start()
    {
        
        lr = GetComponent<LineRenderer>();
    }
    
    public void DrawLines()
    {
        // every time this function calls, spotsList clears, then we add current selected(colored) spots to spotsList
        allSpots = FindObjectsOfType<Spot>();
        spotsList.Clear();
        
        foreach (var spot in allSpots)
        {
            if (spot.colored)
            {
                spotsList.Add(spot);
            }
        }

        lr.positionCount = Mathf.Max(spotsList.Count-1 , 0);
        // ordering spotsList and saving it in orderedSpotsList (for drawing lines by order)
        orderedSpotsList = spotsList.OrderBy(v => v.num).ToList();
        lr.positionCount += 1;
        
        // drawing lines based on orderedSpotsList items
        for (var i = 0; i < orderedSpotsList.Count; i++)
        {
             Vector3 pos = new Vector3(orderedSpotsList[i].transform.position.x , orderedSpotsList[i].transform.position.y , -1f);
             lr.SetPosition(i, pos);
            
        }
    }
    
}
