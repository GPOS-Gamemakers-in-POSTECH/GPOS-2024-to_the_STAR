using UnityEngine;

namespace Game.Player
{
    public class PlayerRotateTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.tag == "TurningPoint")
            {
                int turningType = other.gameObject.GetComponent<TurningPointSet>().getType();
                Debug.Log(turningType);
            }
        }
    }
}
