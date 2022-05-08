using Systems.Healths;
using Services.Hero;
using UnityEngine;

namespace Hero
{
    public class HeroDeath : MonoBehaviour
    {
        [SerializeField] private HeroStateMachine hero;
        [SerializeField] private Collider heroCollider;
        [SerializeField] private HeroInput input;

        private IHeroDeathService _deathService;
        private IHealth _health;
        private HeroSongs _heroSongs;
        private AudioSource _audioSource;

        public void Construct(IHeroDeathService deathService, IHealth health)
        {
            _deathService = deathService;
            _health = health;
            _health.Dead += Dead;
            _heroSongs = GetComponentInChildren<HeroSongs>();
            _audioSource = GetComponentInChildren<AudioSource>();
        }

        private void Dead()
        {
            _audioSource.clip = _heroSongs.DeathSong;
            _audioSource.Play();

            input.Disable();
            hero.Dead();
            heroCollider.enabled = false;
            _deathService.Dead();
            Cleanup();
        }

        private void Cleanup() =>
            _health.Dead -= Dead;
    }
}