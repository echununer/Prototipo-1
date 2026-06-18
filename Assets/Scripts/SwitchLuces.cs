using UnityEngine;

// Adjuntar este script al cubo "Switch".
// El cubo necesita un Collider marcado como "Is Trigger".
// Funciona como un interruptor de luz (toggle): cada vez que el FPS Controller
// (o su InteractionArea) entra en contacto con el cubo, alterna el estado.
//  - Estado "camino": se apagan las luces de la escena y se encienden las del piso.
//  - Estado "normal": se vuelven a encender las luces de la escena y se apaga el camino.
//
// Configuracion en el Inspector:
//  - Luces Escena: arrastrar aqui las luces (o GameObjects) que iluminan la escena.
//  - Luces Camino: arrastrar aqui las luces del piso que marcan el camino.
public class SwitchLuces : MonoBehaviour
{
    [Header("Luces principales de la escena")]
    public GameObject[] lucesEscena;

    [Header("Luces del camino en el piso")]
    public GameObject[] lucesCamino;

    [Header("Opciones")]
    [Tooltip("Si esta activado, tambien apaga/restaura la luz ambiental global de la escena.")]
    public bool apagarLuzAmbiental = true;

    [Tooltip("Tiempo minimo (en segundos) entre activaciones, para evitar que se prenda/apague varias veces de golpe.")]
    public float cooldown = 1f;

    // true  = modo camino (escena apagada, camino encendido)
    // false = modo normal (escena encendida, camino apagado)
    private bool modoCamino = false;
    private Color ambientalOriginal;
    private float ultimoUso = -999f;

    private void Start()
    {
        // Guardamos el color ambiental original para poder restaurarlo al apagar.
        ambientalOriginal = RenderSettings.ambientLight;

        // Estado inicial: luces de escena encendidas, camino apagado.
        SetActivos(lucesEscena, true);
        SetActivos(lucesCamino, false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!EsJugador(other)) return;

        // Cooldown: ignora contactos repetidos demasiado seguidos para que no
        // se prenda/apague varias veces de golpe.
        if (Time.time - ultimoUso < cooldown) return;
        ultimoUso = Time.time;

        // Alterna el estado en cada contacto, como un interruptor de luz.
        modoCamino = !modoCamino;
        AplicarEstado();
    }

    private void AplicarEstado()
    {
        if (modoCamino)
        {
            // Apaga la escena y enciende el camino.
            SetActivos(lucesEscena, false);
            SetActivos(lucesCamino, true);
            if (apagarLuzAmbiental)
                RenderSettings.ambientLight = Color.black;
        }
        else
        {
            // Vuelve a encender la escena y apaga el camino.
            SetActivos(lucesEscena, true);
            SetActivos(lucesCamino, false);
            if (apagarLuzAmbiental)
                RenderSettings.ambientLight = ambientalOriginal;
        }

        Debug.Log("[SwitchLuces] Switch presionado. Modo camino: " + modoCamino);
    }

    // Detecta al jugador aunque el collider que entre sea un hijo del FPS Controller
    // (la capsula, la InteractionArea, etc.). Sube por toda la jerarquia buscando
    // el CharacterController o un InteractionArea.
    private bool EsJugador(Collider other)
    {
        GameObject controllerGO = GameManager.instance != null && GameManager.instance.controller != null
            ? GameManager.instance.controller.gameObject
            : null;

        Transform t = other.transform;
        while (t != null)
        {
            if (controllerGO != null && t.gameObject == controllerGO) return true;
            if (t.GetComponent<CharacterController>() != null) return true;
            if (t.GetComponent<InteractionArea>() != null) return true;
            if (t.CompareTag("Player")) return true;
            t = t.parent;
        }
        return false;
    }

    private void SetActivos(GameObject[] objetos, bool estado)
    {
        if (objetos == null) return;

        foreach (GameObject obj in objetos)
        {
            if (obj != null)
                obj.SetActive(estado);
        }
    }
}
