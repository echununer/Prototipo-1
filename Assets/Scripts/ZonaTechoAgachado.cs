using UnityEngine;

// Adjuntar este script a un GameObject con un Collider marcado como Trigger.
// Colocar el trigger exactamente en el sector del techo bajo donde el jugador
// debe mantenerse agachado. Cuando el jugador entra, se bloquea la posibilidad
// de pararse; cuando sale, se libera.
public class ZonaTechoAgachado : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == GameManager.instance.controller.gameObject)
        {
            GameManager.instance.forzarAgachado = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == GameManager.instance.controller.gameObject)
        {
            GameManager.instance.forzarAgachado = false;
        }
    }
}
