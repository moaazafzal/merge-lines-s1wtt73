using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCenter : MonoBehaviour
{
    private Bounds bounds;
    void Start()
    {
        LeanTween.move(this.gameObject, transform.position, 0f).setDelay(0.1f).setOnComplete(GoCenter);
    }
    
    void GoCenter()
    {
        //moving camera to center of the screen
        var gamePlayObjects = FindObjectsOfType<Spot>();
        transform.position = new Vector3(FindCenterPosition(gamePlayObjects).x,FindCenterPosition(gamePlayObjects).y , -3f);
        
        //changing scale of the camera(orthographicSize) based on the board size
        for (int i = 0; i < 100; i++)
        {
            if ((Camera.main.WorldToScreenPoint(bounds.max).x+30) > Screen.width || ((Camera.main.WorldToScreenPoint(bounds.max).y+80) > Screen.height) )
            {
                Camera.main.orthographicSize += 0.1f;
            }
            else
            {
                break;
            }
        }
        
        
    }
    
    //finding center position of the spots
    Vector3 FindCenterPosition(Spot[] gos)  
    {
        if (gos.Length == 0)
            return Vector3.zero;
        if (gos.Length == 1)
            return gos[0].transform.position;
        
        bounds = new Bounds(gos[0].transform.position, Vector3.zero);
        for (var i = 1; i < gos.Length; i++)
        {
            bounds.Encapsulate(gos[i].transform.position);

        }
        return bounds.center;
    }
    
}
