using UnityEngine;
using UnityEngine.UI;

public class PanelToggleWithAnimation : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Animator panelAnimator;
    [SerializeField] private string showAnimation = "ShowPanel";
    [SerializeField] private string hideAnimation = "HidePanel";
    
    [Header("Button Appearance Settings")]
    [SerializeField] private Button toggleButton;
    [SerializeField] private Color activeColor = new Color(0.2f, 0.8f, 0.2f); // Hijau
    [SerializeField] private Color inactiveColor = Color.white;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;
    [SerializeField] private bool changeColor = true;
    [SerializeField] private bool changeSprite = false;
    
    private bool isPanelShowing = false;
    private Image buttonImage;
    
    void Start()
    {
        // Get button image component
        if (toggleButton != null)
        {
            buttonImage = toggleButton.GetComponent<Image>();
            // Set initial appearance
            UpdateButtonAppearance();
        }
        
        // Set initial panel state
        if (panel != null)
        {
            isPanelShowing = panel.activeSelf;
            UpdateButtonAppearance();
        }
    }
    
    public void TogglePanel()
    {
        if (panelAnimator != null)
        {
            if (!isPanelShowing)
            {
                panel.SetActive(true);
                panelAnimator.Play(showAnimation);
            }
            else
            {
                panelAnimator.Play(hideAnimation);
                Invoke("HidePanel", GetAnimationDuration(hideAnimation));
            }
            isPanelShowing = !isPanelShowing;
        }
        else
        {
            // Fallback tanpa animasi
            if (panel != null)
                panel.SetActive(!panel.activeSelf);
            isPanelShowing = panel.activeSelf;
        }
        
        // Update button appearance
        UpdateButtonAppearance();
    }
    
    private void HidePanel()
    {
        if (panel != null)
            panel.SetActive(false);
    }
    
    private void UpdateButtonAppearance()
    {
        if (buttonImage != null)
        {
            if (changeColor)
            {
                buttonImage.color = isPanelShowing ? activeColor : inactiveColor;
            }
            
            if (changeSprite && activeSprite != null && inactiveSprite != null)
            {
                buttonImage.sprite = isPanelShowing ? activeSprite : inactiveSprite;
            }
        }
    }
    
    private float GetAnimationDuration(string animationName)
    {
        // Default duration, sesuaikan dengan durasi animasi sebenarnya
        return 0.5f;
    }
    
    // Method untuk manual update jika diperlukan dari luar
    public void ForceUpdateButtonAppearance()
    {
        UpdateButtonAppearance();
    }
}