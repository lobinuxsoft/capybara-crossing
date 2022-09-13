using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class VersionViewer : MonoBehaviour
{
    private TextMeshProUGUI versionTextField;

    void Awake()
    {
        versionTextField = GetComponent<TextMeshProUGUI>();
        versionTextField.text = $"version - {Application.version}";

#if CHEATS
            versionTextField.text += " (CHEATS ENABLED)";
#endif
    }
}