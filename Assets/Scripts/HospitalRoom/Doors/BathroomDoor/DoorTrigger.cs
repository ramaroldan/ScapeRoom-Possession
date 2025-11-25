using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] DoorController doorController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Triggered entered by " + other.name);
            if (Input.GetKeyDown(KeyCode.F))
            {
                //Debug.Log("pressed F");
                if (!doorController.GetFlag())
                {
                    doorController.OpenDoor();
                }
                if (doorController.GetFlag())
                {
                    //Debug.Log("attempting to close door");
                    doorController.CloseDoor();
                }
            }
        }
    }
}
