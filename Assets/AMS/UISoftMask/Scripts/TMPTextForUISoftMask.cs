using TMPro;
using UnityEngine;

namespace AMS.UI.SoftMask
{
    [AddComponentMenu("UI/TextMeshPro - Text (UI) + AMS UI Soft Mask")]
    public class TMPTextForUISoftMask : TextMeshProUGUI
    {
        private Material m_SoftMaskMaterial = null;

        public Material softMaskMaterial
        {
            get => m_SoftMaskMaterial;
            set => m_SoftMaskMaterial = value;
        }

        public override Material materialForRendering =>
            maskable && m_SoftMaskMaterial
                ? m_SoftMaskMaterial
                : TMP_MaterialManager.GetMaterialForRendering(this, m_sharedMaterial);

        internal new void Awake() => base.Awake();
    }
}