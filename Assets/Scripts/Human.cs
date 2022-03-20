using NaughtyAttributes;
using UnityEngine;

    public class Human : MonoBehaviour
    {
        [Button()]
        public void Die()
        {
            ParticleManager.main.play(0, transform.position);
            AudioManager.main.Play("blood");
            gameObject.SetActive(false);
        }
    }