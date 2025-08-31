// 31/08/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using UnityEditor;
using UnityEngine;

public class PrefabThumbnailGenerator : EditorWindow
{
    // ====== Inputs / Settings ======
    private GameObject prefabToRender;
    private int thumbnailWidth = 256;
    private int thumbnailHeight = 256;
    private string savePath = "Assets/Thumbnails/";

    // View / camera controls
    private enum ViewPreset { Front_Z, Back_Z, Right_X, Left_X, Top_Y, Bottom_Y, Iso_30, Iso_45, Custom }
    private ViewPreset view = ViewPreset.Right_X; // Default fits your bike case (local +Z aligns with world +X)
    private bool useOrthographic = false;
    private float orthoPadding = 1.15f; // >1 adds margin in orthographic
    private float fov = 35f;            // Perspective FOV
    // Custom yaw/pitch/roll (applied around prefab's local axes)
    private Vector3 customEuler = new Vector3(0f, 0f, 0f);

    private Camera renderCamera;

    [MenuItem("Tools/Prefab Thumbnail Generator")]
    public static void ShowWindow()
    {
        GetWindow<PrefabThumbnailGenerator>("Prefab Thumbnail Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Thumbnail Generator", EditorStyles.boldLabel);

        prefabToRender = (GameObject)EditorGUILayout.ObjectField("Prefab", prefabToRender, typeof(GameObject), false);
        thumbnailWidth = EditorGUILayout.IntField("Thumbnail Width", thumbnailWidth);
        thumbnailHeight = EditorGUILayout.IntField("Thumbnail Height", thumbnailHeight);
        savePath = EditorGUILayout.TextField("Save Path", savePath);

        EditorGUILayout.Space();
        GUILayout.Label("View & Camera", EditorStyles.boldLabel);
        view = (ViewPreset)EditorGUILayout.EnumPopup("View Preset", view);
        useOrthographic = EditorGUILayout.Toggle("Orthographic", useOrthographic);

        if (!useOrthographic)
            fov = EditorGUILayout.Slider("FOV (persp)", fov, 10f, 70f);
        else
            orthoPadding = EditorGUILayout.Slider("Ortho Padding", orthoPadding, 1.0f, 2.0f);

        if (view == ViewPreset.Custom)
            customEuler = EditorGUILayout.Vector3Field("Custom (Yaw/Pitch/Roll)", customEuler);

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Thumbnail"))
        {
            if (prefabToRender != null)
            {
                GenerateThumbnail();
            }
            else
            {
                Debug.LogError("Please assign a prefab to render.");
            }
        }
    }

    private void GenerateThumbnail()
    {
        // Create a temporary camera
        GameObject cameraObject = new GameObject("ThumbnailCamera");
        renderCamera = cameraObject.AddComponent<Camera>();
        SetupCamera(renderCamera, useOrthographic, fov);

        // Instantiate the prefab temporarily (as a prefab instance, not a copy)
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabToRender);
        instance.transform.position = Vector3.zero;

        // Calculate bounds (world space) and build view rotation aligned to prefab's local axes
        Bounds bounds = CalculateBounds(instance);
        Quaternion viewRot = GetViewRotation(instance.transform, view, customEuler);

        // Match camera aspect to thumbnail resolution so fitting math is correct
        float aspect = Mathf.Max(1e-3f, (float)thumbnailWidth / Mathf.Max(1, thumbnailHeight));
        renderCamera.aspect = aspect;

        // Position / orient camera to fit bounds
        PositionCameraToFitBounds(renderCamera, bounds, viewRot, useOrthographic, orthoPadding);

        // Create a RenderTexture and render
        RenderTexture rt = new RenderTexture(thumbnailWidth, thumbnailHeight, 24, RenderTextureFormat.ARGB32);
        var prevActive = RenderTexture.active;
        var prevTarget = renderCamera.targetTexture;

        renderCamera.targetTexture = rt;
        renderCamera.Render();

        // Readback to Texture2D (PNG)
        RenderTexture.active = rt;
        Texture2D thumbnail = new Texture2D(thumbnailWidth, thumbnailHeight, TextureFormat.RGBA32, false);
        thumbnail.ReadPixels(new Rect(0, 0, thumbnailWidth, thumbnailHeight), 0, 0);
        thumbnail.Apply();

        // Save to disk
        if (!System.IO.Directory.Exists(savePath))
            System.IO.Directory.CreateDirectory(savePath);

        string filePath = $"{savePath}{prefabToRender.name}_Thumbnail.png";
        System.IO.File.WriteAllBytes(filePath, thumbnail.EncodeToPNG());
        Debug.Log($"Thumbnail saved to: {filePath}");

        // Cleanup
        RenderTexture.active = prevActive;
        renderCamera.targetTexture = prevTarget;

        DestroyImmediate(rt);
        DestroyImmediate(cameraObject);
        DestroyImmediate(instance);

        // Refresh the Asset Database to show the new file
        AssetDatabase.Refresh();
    }

    // ===== Helpers =====

    // Calculates world-space bounds that encapsulate all renderers in the prefab instance.
    private Bounds CalculateBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            return new Bounds(obj.transform.position, Vector3.zero);
        }

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer renderer in renderers)
            bounds.Encapsulate(renderer.bounds);

        return bounds;
    }

    // Configure camera shared settings.
    private static void SetupCamera(Camera cam, bool orthographic, float fovDeg)
    {
        cam.backgroundColor = Color.clear;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.orthographic = orthographic;
        if (!orthographic) cam.fieldOfView = fovDeg;

        cam.nearClipPlane = 0.01f;
        cam.farClipPlane = 1000f;
    }

    // Build a rotation so that the camera looks along a chosen LOCAL axis of the prefab.
    // e.g., Right_X => cam.forward == prefab.right (local +X).
    private static Quaternion GetViewRotation(Transform t, ViewPreset preset, Vector3 customEuler)
    {
        // Local basis in world space
        Vector3 f = t.forward; // local +Z
        Vector3 r = t.right;   // local +X
        Vector3 u = t.up;      // local +Y

        Vector3 forward, up;

        switch (preset)
        {
            case ViewPreset.Front_Z:  forward = f;      up = u; break;
            case ViewPreset.Back_Z:   forward = -f;     up = u; break;
            case ViewPreset.Right_X:  forward = r;      up = u; break;
            case ViewPreset.Left_X:   forward = -r;     up = u; break;
            case ViewPreset.Top_Y:    forward = u;      up = -f; break; // choose up to avoid gimbal
            case ViewPreset.Bottom_Y: forward = -u;     up = f; break;
            case ViewPreset.Iso_30:
                // 30° isometric-ish from top-right of the prefab
                forward = (r + f + u * 0.57735f).normalized; // tan(30°)=~0.577
                up = u;
                break;
            case ViewPreset.Iso_45:
                forward = (r + f + u).normalized;
                up = u;
                break;
            case ViewPreset.Custom:
            default:
                // Apply yaw/pitch/roll around prefab local axes, then map to world basis.
                // Yaw about local Y, Pitch about local X, Roll about local Z (common convention).
                Quaternion localRot =
                    Quaternion.AngleAxis(customEuler.x, Vector3.up) *     // yaw
                    Quaternion.AngleAxis(customEuler.y, Vector3.right) *  // pitch
                    Quaternion.AngleAxis(customEuler.z, Vector3.forward); // roll

                // Convert local vectors to world using the prefab's basis matrix.
                Matrix4x4 basis = new Matrix4x4(
                    new Vector4(r.x, r.y, r.z, 0f),
                    new Vector4(u.x, u.y, u.z, 0f),
                    new Vector4(f.x, f.y, f.z, 0f),
                    new Vector4(0f, 0f, 0f, 1f)
                );

                Vector3 camFwdLocal = (localRot * Vector3.forward);
                Vector3 camUpLocal  = (localRot * Vector3.up);
                forward = (basis.MultiplyVector(camFwdLocal)).normalized;
                up      = (basis.MultiplyVector(camUpLocal)).normalized;
                break;
        }

        return Quaternion.LookRotation(forward, up);
    }

    // Position the camera so the bounds fit completely in frame for either orthographic or perspective.
    private static void PositionCameraToFitBounds(Camera cam, Bounds b, Quaternion rot, bool orthographic, float orthoPad)
    {
        cam.transform.rotation = rot;
        Vector3 center = b.center;

        if (orthographic)
        {
            // Compute half-width and half-height of the bounds projected on camera axes
            Vector3 ext = b.extents;

            float halfW =
                Mathf.Abs(Vector3.Dot(cam.transform.right,   new Vector3(ext.x, 0, 0))) +
                Mathf.Abs(Vector3.Dot(cam.transform.right,   new Vector3(0, ext.y, 0))) +
                Mathf.Abs(Vector3.Dot(cam.transform.right,   new Vector3(0, 0, ext.z)));

            float halfH =
                Mathf.Abs(Vector3.Dot(cam.transform.up,      new Vector3(ext.x, 0, 0))) +
                Mathf.Abs(Vector3.Dot(cam.transform.up,      new Vector3(0, ext.y, 0))) +
                Mathf.Abs(Vector3.Dot(cam.transform.up,      new Vector3(0, 0, ext.z)));

            // Fit by height, also ensure width fits considering camera aspect
            float aspect = Mathf.Max(1e-3f, cam.aspect);
            float sizeByHeight = halfH;
            float sizeByWidth  = halfW / aspect;

            cam.orthographicSize = Mathf.Max(sizeByHeight, sizeByWidth) * orthoPad;

            // Place camera at any positive distance along -forward so the near plane doesn't clip the object.
            float dist = b.extents.magnitude + 1f;
            cam.transform.position = center - cam.transform.forward * dist;
        }
        else
        {
            // Perspective: derive distance from FOV to fit the largest dimension.
            // Fit by height primarily; also ensure width fits by taking the max.
            Vector3 ext = b.extents;

            // Project extents like we did for ortho to get halfH/halfW in view space
            float halfW =
                Mathf.Abs(Vector3.Dot(cam.transform.right,   new Vector3(ext.x, 0, 0))) +
                Mathf.Abs(Vector3.Dot(cam.transform.right,   new Vector3(0, ext.y, 0))) +
                Mathf.Abs(Vector3.Dot(cam.transform.right,   new Vector3(0, 0, ext.z)));

            float halfH =
                Mathf.Abs(Vector3.Dot(cam.transform.up,      new Vector3(ext.x, 0, 0))) +
                Mathf.Abs(Vector3.Dot(cam.transform.up,      new Vector3(0, ext.y, 0))) +
                Mathf.Abs(Vector3.Dot(cam.transform.up,      new Vector3(0, 0, ext.z)));

            float halfFovRad = Mathf.Max(1e-3f, cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float distH = halfH / Mathf.Tan(halfFovRad);
            // Convert vertical FOV to horizontal FOV for width fit
            float halfHFovRad = Mathf.Atan(Mathf.Tan(halfFovRad) * cam.aspect);
            float distW = halfW / Mathf.Tan(Mathf.Max(1e-3f, halfHFovRad));

            float dist = Mathf.Max(distH, distW) + 0.1f; // small margin
            cam.transform.position = center - cam.transform.forward * (dist + ext.magnitude * 0.1f);
        }

        cam.transform.LookAt(center, cam.transform.up);
    }
}
