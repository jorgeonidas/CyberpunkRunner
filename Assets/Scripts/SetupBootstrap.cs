    using UnityEngine;

public class SetupBootstrap : MonoBehaviour
{
    [Header("Prefabs to spawn (persist across scenes)")]
    [SerializeField] private ScenesManager _scenesManagerPrefab; 
    [SerializeField] private SfxManager _audioManagerPrefab;
    [SerializeField] private AddressablessSetup _addressablessSetup;
    // ... agrega aquí otros managers/SDKs si usás prefabs (Analytics, etc.)

    private bool _initialized;

    private async void Awake()
    {
        // Evita doble init si por algún motivo volvés a Setup
        if (_initialized)
        {
            Finish(); return;
        }
        
        await _addressablessSetup.Initialize();

        UserDataServiceSO.Instance.Initialize();
        // 1) Instanciar managers persistentes
        if (ScenesManager.Instance == null && _scenesManagerPrefab != null)
        {
            var go = Instantiate(_scenesManagerPrefab);
        }

        if (SfxManager.Instance == null && _audioManagerPrefab != null)
        {
            var go = Instantiate(_audioManagerPrefab);
        }

        // 2) Inicializar servicios/sistemas (orden recomendado)
        // User data / save system

       // StoreCatalog.Instance.Init();

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
