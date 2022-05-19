using Environment.DoorObject;
using TMPro;
using UI.Displaying;
using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    [SerializeField] private DoorStateMachine stateMachine;
        
    private readonly string PlayerTag = "Player";
    private InformationBlock _informationBlock;
    private TextMeshProUGUI _text;
    private bool _needToCheck;
    
    private void OnTriggerEnter(Collider other)
    {
        if (_informationBlock == null)
        {
            _informationBlock = FindObjectOfType<InformationBlock>(true); 
            _text = _informationBlock.GetComponentInChildren<TextMeshProUGUI>();
        }

        if (!other.CompareTag(PlayerTag) || stateMachine.DoorIsOpen()) 
            return;
        
        _informationBlock.gameObject.SetActive(true);
        _text.text = "Please click E\nYou need a key";
        _needToCheck = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_needToCheck)
            return;
        
        if (other.CompareTag(PlayerTag))
            _informationBlock.gameObject.SetActive(false);

        _needToCheck = false;
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
