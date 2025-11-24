//using System.Reflection;
//using UnityEngine;

//public static class ActivePlayer
//{
//    private const string playerTag = "Player";

//    public static void EnsurePlayerIsActive()
//    {
//        GameObject player = FindInactivePlayerByTag(playerTag);

//        if (player != null)
//        {
//            if (!player.activeSelf)
//            {
//                player.SetActive(true);
//                Debug.Log("[ActivePlayer] Player encontrado y activado.");
//            }
//            else
//            {
//                Debug.Log("[ActivePlayer] Player ya estaba activo.");
//            }
//            SetPlayerControl(player, false);
//        }
//        else
//        {
//            Debug.LogWarning("[ActivePlayer] No se encontró ningún objeto con tag 'Player'.");
//        }
//    }
//    private static void SetPlayerControl(GameObject player, bool isUIActive)
//    {
//        // Buscar cualquier componente con overrideCursorLock
//        var allComponents = player.GetComponentsInChildren<MonoBehaviour>(true);

//        foreach (var component in allComponents)
//        {
//            var type = component.GetType();
//            var property = type.GetProperty("overrideCursorLock");
//            if (property != null && property.PropertyType == typeof(bool))
//            {
//                property.SetValue(component, isUIActive);
//                Debug.Log($"[ActivePlayer] overrideCursorLock set en {type.Name} a {isUIActive}");
//            }
//        }

//        // Controlar el cursor del sistema
//        Cursor.visible = isUIActive;
//        Cursor.lockState = isUIActive ? CursorLockMode.None : CursorLockMode.Locked;
//        Debug.Log($"[ActivePlayer] Cursor {(isUIActive ? "visible" : "oculto")} y {(isUIActive ? "desbloqueado" : "bloqueado")}");
//    }
//    private static GameObject FindInactivePlayerByTag(string tag)
//    {
//        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>(true); // true = incluye inactivos
//        foreach (GameObject obj in allObjects)
//        {
//            if (obj.CompareTag(tag))
//            {
//                return obj;
//            }
//        }
//        return null;
//    }
//}
using System.Collections;
using UnityEngine;

public static class ActivePlayer
{
    private const string playerTag = "Player";

    public static void EnsurePlayerIsActiveAndUnlockCamera(MonoBehaviour context)
    {
        context.StartCoroutine(ActivateAndUnlock());
    }

    private static IEnumerator ActivateAndUnlock()
    {
        GameObject player = FindInactivePlayerByTag(playerTag);

        if (player != null && !player.activeSelf)
        {
            player.SetActive(true);
            Debug.Log("[ActivePlayer] Player activado.");
        }

        // Esperar a que PlayerMovement exista
        while (Object.FindFirstObjectByType<PlayerMovement>() == null)
            yield return null;

        // Esperar a que MainCamera esté disponible
        while (Camera.main == null)
            yield return null;

        // Buscar todos los MouseLook en la escena (activos o no)
        MouseLook[] allLooks = Object.FindObjectsOfType<MouseLook>(true);

        MouseLook mouseLookPlayer = null;
        MouseLook mouseLookCamera = null;

        foreach (var ml in allLooks)
        {
            if (ml.CompareTag("Player"))
                mouseLookPlayer = ml;
            else if (ml.CompareTag("MainCamera"))
                mouseLookCamera = ml;
        }

        // Cambiar overrideCursorLock
        if (mouseLookPlayer != null)
        {
            mouseLookPlayer.overrideCursorLock = false;
            Debug.Log("[ActivePlayer] overrideCursorLock false en Player.");
        }

        if (mouseLookCamera != null)
        {
            mouseLookCamera.overrideCursorLock = false;
            Debug.Log("[ActivePlayer] overrideCursorLock false en MainCamera.");
        }

        // Control del cursor del sistema
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private static GameObject FindInactivePlayerByTag(string tag)
    {
        GameObject[] allObjects = Object.FindObjectsOfType<GameObject>(true);
        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag(tag))
                return obj;
        }
        return null;
    }
}
