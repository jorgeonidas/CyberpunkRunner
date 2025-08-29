    using UnityEngine;

public class SetupBootstrap : MonoBehaviour
{
    [Header("Prefabs to spawn (persist across scenes)")]
    [SerializeField] private ScenesManager scenesManagerPrefab; // contiene ScenesManager + CanvasFader
    [SerializeField] private SfxManager audioManagerPrefab;  // opcional
    // ... agrega aquí otros managers/SDKs si usás prefabs (Analytics, etc.)

    private bool _initialized;

    private async void Awake()
    {
        // Evita doble init si por algún motivo volvés a Setup
        if (_initialized)
        {
            Finish(); return;
        }

        // 1) Instanciar managers persistentes
        if (ScenesManager.Instance == null && scenesManagerPrefab != null)
        {
            var go = Instantiate(scenesManagerPrefab);
        }

        if (SfxManager.Instance == null && audioManagerPrefab != null)
        {
            var go = Instantiate(audioManagerPrefab);
        }

        // 2) Inicializar servicios/sistemas (orden recomendado)
        // User data / save system
        // UserDataServiceSO.Instance.Initialize();

        // Audio (volúmenes desde user data)
        // AudioManager.Instance.Initialize();

        // Addressables / Remote Config / Analytics (si aplica)
        // await Addressables.InitializeAsync().Task;

        // 3) Ir a Main Menu (usa tu fader si está listo)
        ScenesManager.ToMainMenu();

        // 4) (Opcional) destruir Setup para dejar la escena limpia
        _initialized = true;
        Finish();
    }

    private void Finish()
    {
        // Si querés descargar la escena Setup, hacelo desde otro script
        // que sepa el nombre exacto, o dejala cargada si no molesta.
        Destroy(gameObject);
    }
}
