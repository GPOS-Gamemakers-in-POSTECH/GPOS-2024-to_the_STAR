using UnityEngine;

namespace Game.Player
{
    public class PlayerRotateTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Triggered");
        }
    }
}
