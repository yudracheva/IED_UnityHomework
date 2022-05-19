using System;
using ConstantsValue;
using GameStates;
using GameStates.States;
using Hero;
using UnityEngine;

namespace Environment.DoorObject
{
    public class NextLevel : MonoBehaviour
    {
        private IGameStateMachine _gameStateMachine;

        public void Construct(IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine ?? throw  new ArgumentNullException(nameof(gameStateMachine));
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<HeroPlayer>() != null)
            {
                _gameStateMachine.Enter<LoadGameLevel2State, string>(Constants.Game2Scene);
            }
        }
    }
}
