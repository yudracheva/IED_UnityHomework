using Environment.DoorObject;
using Hero;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private DoorStateMachine stateMachine;
        
    private bool _needToCheck;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HeroDoorInteraction>(out var heroDoorInteraction) && !stateMachine.DoorIsOpen())
        {
            heroDoorInteraction.ShowMessage("Please click E\nYou need a key");
            _needToCheck = true;   
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_needToCheck)
            return;
        
        if (other.TryGetComponent<HeroDoorInteraction>(out var heroDoorInteraction))
        {
            heroDoorInteraction.HideMessage();
            _needToCheck = false;
        }
    }

    private void Update()
    {
        if (!_needToCheck || stateMachine.DoorIsOpen())
            return;

        if (UnityEngine.Input.GetKeyDown(KeyCode.E))
        {
            stateMachine.SetOpeningState();
        }
    }
}
