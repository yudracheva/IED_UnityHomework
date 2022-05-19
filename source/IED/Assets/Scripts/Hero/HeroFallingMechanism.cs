using UnityEngine;

namespace Hero
{
    public class HeroFallingMechanism : MonoBehaviour
    {
        private const string GroundTag = "Ground";
        
        private GameObject _hero;
        private bool _isFalling; 
            
        protected void Awake()
        {
            _hero = transform.parent.gameObject;
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GroundTag))
                _isFalling = false;
        }

        protected void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(GroundTag))
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