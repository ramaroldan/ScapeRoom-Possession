using UnityEngine;
using System.Collections;

public class SceneSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame(); // esperar a todos los Start()
        yield return new WaitForSeconds(0.05f); // tiny delay opcional

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
