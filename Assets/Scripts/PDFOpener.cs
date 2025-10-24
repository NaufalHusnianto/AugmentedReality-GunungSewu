using UnityEngine;
using System.Collections;

public class PDFLinkOpener : MonoBehaviour
{
    [Header("PDF Link Settings")]
    public string pdfURL = "https://example.com/document.pdf";
    
    public void OpenPDFLink()
    {
        StartCoroutine(OpenPDFCoroutine());
    }
    
    private IEnumerator OpenPDFCoroutine()
    {
        // Cek jika URL valid
        if (string.IsNullOrEmpty(pdfURL))
        {
            Debug.LogError("PDF URL is empty!");
            yield break;
        }
        
        // Tambahkan http:// jika tidak ada
        string finalURL = pdfURL;
        if (!pdfURL.StartsWith("http://") && !pdfURL.StartsWith("https://"))
        {
            finalURL = "https://" + pdfURL;
        }
        
        Debug.Log("Opening PDF URL: " + finalURL);
        
        #if UNITY_WEBGL && !UNITY_EDITOR
        // Untuk WebGL - buka di tab baru
        Application.ExternalEval($"window.open('{finalURL}','_blank')");
        #elif UNITY_ANDROID
        OpenPDFAndroid(finalURL);
        #elif UNITY_IOS
        OpenPDFIOS(finalURL);
        #else
        // Untuk PC/Desktop
        Application.OpenURL(finalURL);
        #endif
        
        yield return null;
    }
    
    #if UNITY_ANDROID
    private void OpenPDFAndroid(string url)
    {
        try
        {
            using (AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent"))
            using (AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent"))
            {
                // Set action untuk view
                intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_VIEW"));
                
                // Set URL
                using (AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri"))
                using (AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", url))
                {
                    intentObject.Call<AndroidJavaObject>("setData", uriObject);
                    
                    // Get current activity dan start intent
                    using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                    using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        currentActivity.Call("startActivity", intentObject);
                    }
                }
            }
            Debug.Log("PDF link opened successfully on Android");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open PDF link on Android: " + e.Message);
            // Fallback ke method biasa
            Application.OpenURL(url);
        }
    }
    #endif
    
    #if UNITY_IOS
    private void OpenPDFIOS(string url)
    {
        // Untuk iOS, gunakan Application.OpenURL
        Application.OpenURL(url);
    }
    #endif
    
    // Method untuk testing di Editor
    #if UNITY_EDITOR
    [ContextMenu("Test Open PDF Link")]
    private void TestOpenPDFLink()
    {
        OpenPDFLink();
    }
    #endif
}