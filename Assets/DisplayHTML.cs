using UnityEngine;

public class DisplayHTML : MonoBehaviour
{
    private WebViewObject webViewObject;

    void Start()
    {
        webViewObject = new GameObject("WebViewObject").AddComponent<WebViewObject>();
        webViewObject.Init();

        // Load the local HTML file or remote URL
        webViewObject.LoadURL("file://" + Application.dataPath + "/HTML/yourfile.html");

        // Adjust the WebView's position and size
        webViewObject.SetMargins(10, 10, 10, 10);
        webViewObject.SetVisibility(true);
    }

    void OnDestroy()
    {
        if (webViewObject != null)
        {
            webViewObject.SetVisibility(false);
        }
    }
}
