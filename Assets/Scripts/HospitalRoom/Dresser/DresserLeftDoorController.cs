using UnityEngine;

public class DresserLeftDoorController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator _dresserLeftDoorAnimator; // reference to the animator component
    [SerializeField] private bool isOpen = false;


    
    void Start()
    {
        _dresserLeftDoorAnimator = GetComponent<Animator>();
    }



    public void OpenDoor()
    {
        if (!isOpen)
        {
            _dresserLeftDoorAnimator.SetTrigger("Open");
        }
    }

    public void CloseDoor()
    {
        if (isOpen)
        {
            _dresserLeftDoorAnimator.SetTrigger("Close");
        }
    }

    public void FlagChange() // its being called in animation events!!!
    {
        if (isOpen)
        {
            isOpen = false;
        }
        else { isOpen = true; }
    }

    public bool GetFlag()
    {
        return isOpen;
    }
}
