using UnityEngine;

public class DresserLeftDoorTrigger : MonoBehaviour
{
    [SerializeField] DresserLeftDoorController dresserLeftDoorController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            //Debug.Log("Triggered entered by " + other.name);
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("pressed F");
                if (!dresserLeftDoorController.GetFlag())
                {
                    dresserLeftDoorController.OpenDoor();
                }
                if (dresserLeftDoorController.GetFlag())
                {
                    //Debug.Log("attempting to close door");
                    dresserLeftDoorController.CloseDoor();
                }
            }
        }
    }
}
