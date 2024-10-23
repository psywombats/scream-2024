using UnityEngine;
using UnityEngine.SceneManagement;

public class MapOverlayUI : MonoBehaviour
{
    private static MapOverlayUI instance;
    private static Scene lastScene;
    public static MapOverlayUI Instance
    {
        get
        {
            Scene scene = SceneManager.GetActiveScene();
            if (lastScene != scene)
            {
                lastScene = scene;
                instance = null;
            }
            if (instance == null)
            {
                instance = FindObjectOfType<MapOverlayUI>();
            }
            return instance;
        }
    }

    [SerializeField] public MapNameBox setting;
    [SerializeField] public NVLComponent nvl;
    [SerializeField] public RadioUIComponent radio;
    [SerializeField] public CutinComponent cutin;
    [SerializeField] public CanvasGroup fader;
    [SerializeField] public CanvasGroup abseilInfo;
    [SerializeField] public CanvasGroup ascendInfo;
    [SerializeField] public CanvasGroup flareInfo;
}
