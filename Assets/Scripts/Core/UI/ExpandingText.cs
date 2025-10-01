using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPExpandOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float expandedSpacing = 30f;
    public float defaultSpacing = 0f;
    public float animationSpeed = 10f;

    private TextMeshProUGUI tmpText;
    private bool isHovered = false;

    private void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        float target = isHovered ? expandedSpacing : defaultSpacing;
        tmpText.characterSpacing = Mathf.Lerp(tmpText.characterSpacing, target, Time.deltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }
}
