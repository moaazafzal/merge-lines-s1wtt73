using UnityEngine;

namespace MergeDots.Scripts
{
    public class AnimationScript : MonoBehaviour
    {
        private float spd;
    
        void ChangeSpd()
        {
            GetComponent<Animator>().speed = 0;
        }
    }
}
