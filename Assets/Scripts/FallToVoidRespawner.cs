using UnityEngine;

public class FallToVoidRespawner : MonoBehaviour
{
    [Header("Spawn Point")]
    [Tooltip("... A GameObject at the desired spawn position?")]
    [SerializeField] private Transform spawnPoint;

    private BoxCollider _collider;

    private void Start()
    {
        _collider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //yield return new WaitForEndOfFrame(); // esperar a todos los Start()
            //yield return new WaitForSeconds(0.05f); // tiny delay opcional

            var player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                var cc = player.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                player.transform.position = spawnPoint.position;
                player.transform.rotation = spawnPoint.rotation;

                var rb = player.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                if (cc != null) cc.enabled = true;

                Debug.Log("🟢 Player TELEPORTADO con delay OK.");
            }
            else
            {
                Debug.LogError("❌ No encontré Player para teleportar.");
            }
        }
    }
}
