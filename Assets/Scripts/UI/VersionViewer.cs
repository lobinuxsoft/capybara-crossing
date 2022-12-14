using UnityEngine;
using TMPro;

namespace CapybaraCrossing.Utils
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class VersionViewer : MonoBehaviour
    {
        private TextMeshProUGUI versionTextField;

        void Awake()
        {
            versionTextField = GetComponent<TextMeshProUGUI>();
            versionTextField.text = $"{Application.version}";

#if CHEATS
            versionTextField.text += " (CHEATS ENABLED)";
#endif
        }
    }
}