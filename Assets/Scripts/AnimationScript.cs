using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private float spd;
    
    void ChangeSpd()
    {
        GetComponent<Animator>().speed = 0;
    }
}
