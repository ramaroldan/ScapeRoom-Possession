using UnityEngine;

public class BedsideDrawerController : MonoBehaviour
{
    private Animator _bedsideDrawerAnimator; // reference to the animator component
    [SerializeField] private bool isOpen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _bedsideDrawerAnimator= GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDrawer()
    {
        
        
        _bedsideDrawerAnimator.SetTrigger("OpenDrawer");
        
    }

    public void CloseDrawer()
    {
        if (isOpen)
        {
            _bedsideDrawerAnimator.SetTrigger("CloseDrawer");
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Triggered entered by " + other.name);
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("pressed F");
                if (!isOpen)
                {
                    OpenDrawer();
                }
                if (isOpen)
                {
                    //Debug.Log("attempting to close drawer");
                    CloseDrawer();
                }
            }
        }
    }
}
