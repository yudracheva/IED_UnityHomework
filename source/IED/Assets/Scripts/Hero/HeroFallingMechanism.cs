using Environment;
using UnityEngine;

namespace Hero
{
    public class HeroFallingMechanism : MonoBehaviour
    {
        private GameObject _hero;
        private bool _isFalling; 
            
        protected void Awake()
        {
            _hero = transform.parent.gameObject;
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Ground>() != null)
                _isFalling = false;
        }

        protected void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Ground>() != null)
                _isFalling = true;
        }

        protected void Update()
        {
            if (!_isFalling)
                return;
            
            var position = _hero.transform.position;
            position = new Vector3(
                position.x, 
                position.y - 0.025f,
                position.z);
            _hero.transform.position = position;
        }
    }
}