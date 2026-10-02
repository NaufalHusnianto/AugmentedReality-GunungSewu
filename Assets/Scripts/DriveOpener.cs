using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class DriveOpener : MonoBehaviour
{
    [Header("Paste Google Link Here")]
    [TextArea(2, 4)]
    [SerializeField] private string googleLink = "https://docs.google.com/forms/d/e/1FAIpQLSf_xP2uKB-2CSEJ35R5RLHMA8_U_GSO1SXE-ZSWUimKP6EtPw/viewform?usp=dialog";
    
    [Header("Validation Settings")]
    [Tooltip("Nonaktifkan jika ingin buka semua link")]
    [SerializeField] private bool validateLink = false; // Matikan validasi ketat
    
    [Header("Options")]
    [SerializeField] private bool debugLog = true;
    
    private Button button;
    
    void Start()
    {
        SetupButtonAutomatically();
    }
    
    void SetupButtonAutomatically()
    {
        button = GetComponent<Button>();
        
        if (button == null)
        {
            Debug.LogError("Button component tidak ditemukan!");
            return;
        }
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OpenGoogleLink);
        
        if (debugLog)
        {
            Debug.Log($"✅ Button '{gameObject.name}' siap digunakan");
        }
    }
    
    void OpenGoogleLink()
    {
        if (string.IsNullOrWhiteSpace(googleLink))
        {
            Debug.LogError("❌ Link kosong!");
            return;
        }
        
        string cleanedLink = CleanLink(googleLink);
        
        if (validateLink)
        {
            if (!IsValidGoogleLink(cleanedLink))
            {
                // Tampilkan warning tapi tetap buka
                Debug.LogWarning($"⚠ Link mungkin tidak standar, tetap mencoba membuka: {cleanedLink}");
            }
        }
        
        // Selalu coba buka link
        Application.OpenURL(cleanedLink);
        
        if (debugLog)
        {
            Debug.Log($"🌐 Membuka: {cleanedLink}");
            Debug.Log($"📊 Tipe link: {GetLinkType(cleanedLink)}");
        }
    }
    
    string CleanLink(string url)
    {
        url = url.Trim();
        
        // Hapus karakter tidak perlu
        url = url.Replace("\n", "").Replace("\r", "");
        
        // Tambahkan https:// jika tidak ada protokol
        if (!url.StartsWith("http://") && !url.StartsWith("https://"))
        {
            url = "https://" + url;
        }
        
        return url;
    }
    
    // Validasi lebih luas untuk semua layanan Google
    bool IsValidGoogleLink(string url)
    {
        // Semua domain Google yang umum
        string[] googleDomains = {
            "docs.google.com",
            "drive.google.com",
            "forms.google.com",
            "sheets.google.com",
            "docs.google.com",
            "slides.google.com",
            "script.google.com",
            "sites.google.com",
            "classroom.google.com",
            "mail.google.com",
            "maps.google.com",
            "photos.google.com",
            "keep.google.com",
            "calendar.google.com"
        };
        
        foreach (string domain in googleDomains)
        {
            if (url.Contains(domain))
            {
                return true;
            }
        }
        
        // Juga terima link yang mengandung "google.com"
        return url.Contains("google.com");
    }
    
    // Deteksi tipe link Google
    string GetLinkType(string url)
    {
        if (url.Contains("docs.google.com/document"))
            return "Google Docs";
        else if (url.Contains("docs.google.com/spreadsheets"))
            return "Google Sheets";
        else if (url.Contains("docs.google.com/presentation"))
            return "Google Slides";
        else if (url.Contains("docs.google.com/forms"))
            return "Google Forms";
        else if (url.Contains("drive.google.com"))
            return "Google Drive";
        else if (url.Contains("mail.google.com"))
            return "Gmail";
        else if (url.Contains("calendar.google.com"))
            return "Google Calendar";
        else if (url.Contains("photos.google.com"))
            return "Google Photos";
        else if (url.Contains("maps.google.com"))
            return "Google Maps";
        else
            return "Link Google Lainnya";
    }
    
    public void SetLink(string newLink)
    {
        googleLink = newLink;
        if (debugLog)
        {
            Debug.Log($"📝 Link diperbarui: {newLink}");
        }
    }
    
    // Untuk testing
    #if UNITY_EDITOR
    [ContextMenu("Test Link")]
    void TestLink()
    {
        OpenGoogleLink();
    }
    
    [ContextMenu("Check Link Type")]
    void CheckLinkType()
    {
        string cleaned = CleanLink(googleLink);
        Debug.Log($"Link Type: {GetLinkType(cleaned)}");
        Debug.Log($"Is Valid: {IsValidGoogleLink(cleaned)}");
    }
    #endif
    
    void OnEnable()
    {
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OpenGoogleLink);
        }
    }
}