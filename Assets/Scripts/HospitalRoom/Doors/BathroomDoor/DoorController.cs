using UnityEngine;

public class DoorController : MonoBehaviour
{
    private Animator _doorAnimator; // reference to the animator component
    [SerializeField] private bool isOpen = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _doorAnimator= GetComponent<Animator>();
    }

    

    public void OpenDoor()
    {
        if(!isOpen)
        {
            _doorAnimator.SetTrigger("OpenDoor");
        }
    }

    public void CloseDoor()
    {
        if(isOpen)
        {
            _doorAnimator.SetTrigger("CloseDoor");
        }
    }

    public void FlagChange() // its being called in animation events!!!
    {
        if (isOpen)
        {
            isOpen = false;
        } else { isOpen= true; }
    }
    
    public bool GetFlag()
    {
        return isOpen;
    }

}
