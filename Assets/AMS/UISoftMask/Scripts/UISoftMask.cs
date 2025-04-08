using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.Rendering;

namespace AMS.UI.SoftMask
{
    using static UISoftMaskUtils;

    [HelpURL("https://ams.sorialexandre.tech/ui-soft-mask/")]
    public class UISoftMask : RectUV
    {
        [SerializeField] private Sprite m_Mask;

        public Sprite mask
        {
            get => m_Mask;
            set
            {
                if (m_Mask == value)
                    return;

                m_Mask = value;
                ComputeFinalMaskForRendering();
            }
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        [SerializeField,
         Tooltip("Preview mask output.")]
        private bool m_MaskPreview;

        /// <summary>
        /// Enable/disable mask preview for debugging purposes. (Editor or Development Build only)
        /// </summary>
        public bool maskPreview
        {
            get => m_MaskPreview || m_MaskData.preview;
            set
            {
                m_MaskData.preview = value; //We want to isolate it for child mask preview purpose
                UpdateMaterials();
            }
        }
#endif

        [SerializeField, Tooltip("The mask output size.\n\nNote: Keep it low to save memory allocation.")]
        private MaskSize m_MaskSize = MaskSize._128;

        public MaskSize maskSize
        {
            get => m_MaskSize;

            set
            {
                if (m_MaskSize == value)
                    return;

                m_MaskSize = value;
                ComputeFinalMaskForRendering();
            }
        }

        [SerializeField, Tooltip("Select between a Simple or a Sliced (9-slicing) uv coordinate.")]
        private MaskUV m_MaskUV = MaskUV.Simple;

        public MaskUV maskUV => m_MaskUV;

        [SerializeField, Min(0.01f)] private float m_PixelsPerUnitMultiplier = 1;

        public float pixelsPerUnitMultiplier
        {
            get => m_PixelsPerUnitMultiplier;
            set
            {
                if (Mathf.Approximately(m_PixelsPerUnitMultiplier, value))
                    return;

                m_PixelsPerUnitMultiplier = value;
                ComputeFinalMaskForRendering();
            }
        }

        private readonly MaskData m_MaskData = new MaskData();

        [SerializeField, Range(0, 1)] private float m_FallOff = 1;

        public float fallOff
        {
            get => m_FallOff;
            set
            {
                m_FallOff = value;
                ComputeFinalMaskForRendering();
            }
        }

        [SerializeField, Range(0, 1)] private float m_Opacity = 1;

        public float opacity
        {
            get => m_Opacity;
            set
            {
                m_Opacity = value;
                ComputeFinalMaskForRendering();
            }
        }

        [SerializeField,
         Tooltip(
             "Use this to override the temporary mask material with a material asset from your project. Note: It requires a unique material per mask and the shader must be compatible with AMS UI Soft Mask.")]
        private Material m_OverrideMaskMaterial;

        /// <summary>
        /// Use this to override the temporary mask material with a material asset from your project. Note: It requires a unique material per mask and the shader must be compatible with AMS UI Soft Mask. 
        /// </summary>
        public Material overrideMaterial
        {
            get => m_OverrideMaskMaterial;
            set
            {
                m_OverrideMaskMaterial = value;
                UpdateMaskSetup();
            }
        }

        [SerializeField, Tooltip("Override transform to decouple mask size, position and rotation.")]
        private RectTransform m_OverrideTransform;

        /// <summary>
        /// Override transform to decouple mask size, position and rotation.
        /// </summary>
        public RectTransform overrideTransform
        {
            get => m_OverrideTransform ? m_OverrideTransform : rectTransform;

            set
            {
                if (m_OverrideTransform == value)
                    return;

                m_OverrideTransform = value;
                UpdateMaskSetup();
            }
        }

        private Texture2D m_MaskForRenderingTex;

        [Space(5)] private HashSet<MaskableGraphic> m_MaskableObjects = new();

        /// <summary>
        /// Return mask maskable objects.
        /// </summary>
        public HashSet<MaskableGraphic> maskableObjects => m_MaskableObjects;

        private readonly List<Material> m_ExternalMaterials = new();

        [SerializeField] private Material m_SoftMaskBlitMaterial;

        private Material m_TempMaterial;

        private readonly List<FontMaterialData> m_TMPFontMaterialData = new();

        public List<FontMaterialData> TMPFontMaterialData => m_TMPFontMaterialData;

        private bool m_Started;

        private void Reset() => ResetSetup();

        private new void OnEnable()
        {
            base.OnEnable();

            m_OnBeginContextRendering = OnBeginFrameRendering;
            m_DuringCameraPreRender = DuringCameraPreRender;

            //Initial setup
            m_Started = true;
            CheckType();
            CheckTargetMaterial();
            UpdateMaskSetup();
        }

        private new void OnDisable()
        {
            base.OnDisable();

            //Make sure to reset external materials
            m_MaskData.settings = Vector2.zero;
            m_ExternalMaterials.ForEach(externalMaterial =>
                externalMaterial.SetVector(s_MaskDataSettingsID, m_MaskData.settings));

            var childMasks = GetComponentsInChildren<UISoftMask>().ToList();
            childMasks.Remove(this);
            childMasks.ForEach(c => c.UpdateMaskSetup());

            ResetSetup();
            m_Started = false;
        }

        private void OnDestroy()
        {
            if (m_MaskForRenderingTex)
                DestroyImmediate(m_MaskForRenderingTex);

            DestroyImmediate(m_TempMaterial);
            DestroyImmediate(m_SoftMaskBlitMaterial);
        }

        private void OnValidate()
        {
            if (enabled)
                UpdateMaskSetup();
        }

        private void LateUpdate()
        {
            if (HasChangedRectUV(overrideTransform) || CheckType())
            {
                ComputeFinalMaskForRendering();
            }

            CheckMaskableObjects();
            UpdateMaterials();
        }

        private void UpdateMaterials()
        {
            CheckTargetMaterial();

            m_MaskData.settings.x = enabled ? 1 : 0;
            m_MaskData.settings.y = m_RectProperties.gamma2linear ? 1 : 0;

            UpdateMaterial(m_TempMaterial);

            foreach (var fontData in m_TMPFontMaterialData)
                fontData.UpdateInstances(UpdateMaterial);

            foreach (var externalMaterial in m_ExternalMaterials)
                UpdateMaterial(externalMaterial);
        }

        private void SetWorldCanvasMaterials() => SetWorldCanvasProperty(1);

        private void SetOverlayCanvasMaterials() => SetWorldCanvasProperty(0);

        private void SetWorldCanvasProperty(int value)
        {
            if (m_TempMaterial)
                m_TempMaterial.SetInt(s_WORLDCANVAS, value);

            foreach (var fontMaterialKey in m_TMPFontMaterialData)
                if (fontMaterialKey is { Instances: { } instances })
                    foreach (var fontMaterial in instances)
                        fontMaterial.Value.SetInt(s_WORLDCANVAS, value);

            foreach (var externalMaterial in m_ExternalMaterials)
                externalMaterial?.SetInt(s_WORLDCANVAS, value);
        }

        private void OnBeginFrameRendering(List<Camera> cameras)
        {
            if (canvas && (canvas.renderMode == RenderMode.ScreenSpaceOverlay ||
                           (canvas.renderMode == RenderMode.ScreenSpaceCamera &&
                            !canvas.worldCamera)))
            {
                SetOverlayCanvasMaterials();
#if UNITY_EDITOR
                foreach (var cam in cameras)
                    if (cam.cameraType != CameraType.Game)
                        SetWorldCanvasMaterials();
#endif
            }
            else
                SetWorldCanvasMaterials();
        }

        private void DuringCameraPreRender(Camera targetCamera)
        {
            if (canvas && (canvas.renderMode == RenderMode.ScreenSpaceOverlay ||
                           (canvas.renderMode == RenderMode.ScreenSpaceCamera &&
                            !canvas.worldCamera)))
            {
#if UNITY_EDITOR
                if (targetCamera.cameraType == CameraType.SceneView)
                    SetWorldCanvasMaterials();
                else
#endif
                    SetOverlayCanvasMaterials();
            }
            else
                SetWorldCanvasMaterials();
        }

        private void UpdateMaterial(Material material)
        {
            if (!material)
                return;

            material.SetTexture(s_SoftMaskID, m_MaskForRenderingTex);

            SetMaterialRectParams(material);
            m_MaskData.SetMaterialDataSettings(material);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (maskPreview)
                material.EnableKeyword(k_DEBUG_MASK);
            else
                material.DisableKeyword(k_DEBUG_MASK);
#endif
        }

        /// <summary>
        /// Force update mask setup.
        /// </summary>
        public void UpdateMaskSetup()
        {
#if UNITY_EDITOR
            if (!m_Started)
                return;
#endif
            CheckMaskableObjects();
            ComputeFinalMaskForRendering();
        }

        private bool CheckType()
        {
            var update = false;
            switch (m_MaskUV)
            {
                case MaskUV.Simple:
                    if (m_MaskData.uvType != m_MaskUV)
                    {
                        m_MaskData.uvType = m_MaskUV;
                        update = true;
                    }

                    break;

                case MaskUV.Sliced:
                    if (m_MaskData.uvType != m_MaskUV ||
                        m_Mask && (!m_MaskData.sprite || m_MaskData.sprite != m_Mask) ||
                        !Mathf.Approximately(m_MaskData.pixelsPerUnitMultiplier, m_PixelsPerUnitMultiplier))
                    {
                        m_MaskData.uvType = m_MaskUV;

                        var sprite = m_MaskData.sprite = m_Mask;

                        var size = new Vector2(sprite.rect.width, sprite.rect.height);
                        var borders = sprite.border;
                        borders.x /= size.x;
                        borders.y /= size.y;
                        borders.z /= size.x;
                        borders.w /= size.y;

                        m_MaskData.slicedBorder = borders;
                        m_MaskData.pixelsPerUnitMultiplier = m_PixelsPerUnitMultiplier;

                        update = true;
                    }

                    break;
            }

            return update;
        }

        private Vector2 GetSliceScale(Vector2 textureSize) =>
            rectTransform.rect.size / textureSize * m_MaskData.pixelsPerUnitMultiplier;

        private void CheckRenderingMaskSetup()
        {
            if (!m_SoftMaskBlitMaterial)
            {
                if (!s_SoftMaskBlitShader)
                    s_SoftMaskBlitShader = Shader.Find(k_SoftMaskBlitShader);

                m_SoftMaskBlitMaterial = new Material(s_SoftMaskBlitShader);
                m_SoftMaskBlitMaterial.name = $"SoftMaskBlit [{m_SoftMaskBlitMaterial.GetInstanceID()}]";
                m_SoftMaskBlitMaterial.hideFlags =
                    HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.NotEditable;
            }

            var selectedSize = (int)m_MaskSize;

            if (!m_MaskForRenderingTex)
            {
                m_MaskForRenderingTex = new Texture2D(selectedSize, selectedSize, TextureFormat.R16, false);
                m_MaskForRenderingTex.name =
                    $"SoftMask [{m_MaskForRenderingTex.GetInstanceID()}]"; //R8 is unsupported for some platforms
                m_MaskForRenderingTex.hideFlags =
                    HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.NotEditable;
            }
            else if (m_MaskForRenderingTex && m_MaskForRenderingTex.width != selectedSize)
                m_MaskForRenderingTex.Reinitialize(selectedSize, selectedSize);
        }

        private void CheckMaskableObjects()
        {
            var childMasks = GetComponentsInChildren<UISoftMask>(true).ToList();
            childMasks.Remove(this);

            var newMaskableObjects = GetComponentsInChildren<MaskableGraphic>(true)
                .Where(obj =>
                {
                    return obj.maskable &&
                           childMasks.All(cm => !cm.enabled || !obj.transform.IsChildOf(cm.transform));
                })
                .ToHashSet();

            if (newMaskableObjects.SetEquals(m_MaskableObjects))
            {
                CheckMaskableObjectsMaterial();
                return;
            }

            // Make sure to reset and remove unlisted objects
            foreach (var obj in m_MaskableObjects.Where(obj => obj && !newMaskableObjects.Contains(obj)))
            {
                if (obj is TMP_Text textMeshToRemove && FontMaterialData.FindData(m_TMPFontMaterialData,
                        textMeshToRemove.font, out var materialData))
                {
                    textMeshToRemove.fontSharedMaterial =
                        materialData.GetRelativeKeyMaterial(textMeshToRemove.fontSharedMaterial);
                    continue;
                }

                if (obj.material == m_TempMaterial)
                    obj.material = null;
            }

            m_MaskableObjects = newMaskableObjects;
            CheckMaskableObjectsMaterial();
        }

        private void CheckMaskableObjectsMaterial()
        {
            m_ExternalMaterials.Clear();

            foreach (var maskableObj in m_MaskableObjects)
            {
                if (maskableObj is TMP_Text textMesh)
                    CheckTMPObject(textMesh);
                else if (maskableObj.material is { } material)
                {
                    if (material == maskableObj.defaultMaterial) // Unity default UI
                        maskableObj.material = m_TempMaterial;
                    else if (material != m_TempMaterial &&
                             material.name.StartsWith(k_SoftMaskMatTag)) // Persistant prefab material
                        maskableObj.material = m_TempMaterial;
                    else if (material != m_TempMaterial &&
                             MaterialHasSoftMask(material)) // Register external material
                        m_ExternalMaterials.Add(material);
                }
            }
        }

        private void CheckTMPObject(TMP_Text textMesh)
        {
            if (!textMesh ||
                textMesh.fontSharedMaterial is not { } fontSharedMaterial)
                return;

            var textSoftMask = CheckTMPSoftMask(textMesh);

            var fontAsset = textMesh.font;
            if (!FontMaterialData.FindData(m_TMPFontMaterialData, fontAsset, out var softMaskFontData))
            {
                if (!MaterialHasSoftMask(fontSharedMaterial))
                    return;

                var newFontData = new FontMaterialData(fontAsset);
                textMesh.fontSharedMaterial = newFontData.TryRegisterInstanceMaterial(fontSharedMaterial);
                m_TMPFontMaterialData.Add(newFontData);
            }

            if (textSoftMask && softMaskFontData?.TryRegisterInstanceMaterial(fontSharedMaterial) is
                    { } fontMatInstance)
                textSoftMask.softMaskMaterial = fontMatInstance;
        }

        private static TMPTextForUISoftMask CheckTMPSoftMask(TMP_Text textMesh)
        {
            if (textMesh as TMPTextForUISoftMask is { } isSoftMaskText)
                return isSoftMaskText;

            var fields = textMesh.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var fieldDictionary = fields.ToDictionary(field => field, field => field.GetValue(textMesh));

            var targetObject = textMesh.gameObject;
            DestroyImmediate(textMesh);

            if (textMesh)
                return null;

            var softMaskText = targetObject.AddComponent<TMPTextForUISoftMask>();
            foreach (var fieldPair in fieldDictionary)
                fieldPair.Key.SetValue(softMaskText, fieldPair.Value);

            softMaskText.Awake(); //Make sure to reconstruct mesh

            return softMaskText;
        }

        private void ComputeFinalMaskForRendering()
        {
            CheckRenderingMaskSetup();

            m_SoftMaskBlitMaterial.SetFloat(s_OpacityID, m_Opacity);
            m_SoftMaskBlitMaterial.SetFloat(s_FalloffID, m_FallOff);
            var textureMask = Texture2D.whiteTexture;

            var selectedSize = (int)m_MaskSize;

            RenderTexture parentTex = null;
            var parentMatrix = Matrix4x4.identity;
            //Get parent mask
            var parentMasks = GetComponentsInParent<UISoftMask>().ToList();
            parentMasks.Remove(this);
            if (parentMasks.Count > 0)
                foreach (var parentMask in parentMasks)
                    if (parentMask.enabled)
                    {
                        GetTemporaryParentMask(parentMask, selectedSize, out parentTex, out parentMatrix);
                        break;
                    }

            if (m_Mask)
            {
                var sourceTexRect = m_Mask.rect;
                var texRect = m_Mask.textureRect;
                var rectOffset = m_Mask.textureRectOffset;
                textureMask = m_Mask.texture;
                var textureAtlasFactor = Vector2.one / new Vector2(textureMask.width, textureMask.height);
                var spriteOffset = (texRect.min - rectOffset) * textureAtlasFactor;
                Vector4 atlasData = sourceTexRect.size * textureAtlasFactor;
                atlasData.z = spriteOffset.x;
                atlasData.w = spriteOffset.y;
                m_SoftMaskBlitMaterial.SetVector(s_AtlasDataID, atlasData);

                if (maskUV == MaskUV.Sliced)
                {
                    m_SoftMaskBlitMaterial.EnableKeyword(k_SLICED);
                    m_SoftMaskBlitMaterial.SetVector(s_SliceScaleID, GetSliceScale(sourceTexRect.size));
                    m_SoftMaskBlitMaterial.SetVector(s_SliceBorderID, m_MaskData.slicedBorder);
                }
                else
                    m_SoftMaskBlitMaterial.DisableKeyword(k_SLICED);
            }

            m_SoftMaskBlitMaterial.SetMatrix(s_ParentMaskMatrixID, parentMatrix);
            m_SoftMaskBlitMaterial.SetTexture(s_ParentMaskID, parentTex ? parentTex : Texture2D.whiteTexture);

            var tempMaskForRendering =
                RenderTexture.GetTemporary(selectedSize, selectedSize, 0, RenderTextureFormat.R16);
            Graphics.Blit(textureMask, tempMaskForRendering, m_SoftMaskBlitMaterial);
            if (parentTex)
                RenderTexture.ReleaseTemporary(parentTex);

            RenderTexture.active = tempMaskForRendering;
            m_MaskForRenderingTex.ReadPixels(new Rect(0, 0, tempMaskForRendering.width, tempMaskForRendering.height), 0,
                0);
            m_MaskForRenderingTex.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(tempMaskForRendering);

            // Compute children mask
            var childMasks = GetComponentsInChildren<UISoftMask>().ToList();
            childMasks.Remove(this);
            if (childMasks.Count <= 0)
                return;

            foreach (var c in childMasks.Where(c =>
                         !childMasks.Any(x => x != c && c.transform.IsChildOf(x.transform))))
            {
                c.UpdateMaskSetup();
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                c.maskPreview = maskPreview;
#endif
            }
        }

        private void GetTemporaryParentMask(UISoftMask parentMask, int size, out RenderTexture parentTex,
            out Matrix4x4 overrideMatrix)
        {
            parentTex = RenderTexture.GetTemporary(size, size, 0, RenderTextureFormat.R16);
            parentTex.name = $"ParentSoftMask [{name}]";

            var parentTransform = parentMask.overrideTransform ?? parentMask.rectTransform;
            var childTransform = m_OverrideTransform ? m_OverrideTransform : rectTransform;

            var parentCenterWorld = parentTransform.TransformPoint(parentTransform.rect.center);
            var childCenterWorld = childTransform.TransformPoint(childTransform.rect.center);
            var offsetWorld = childCenterWorld - parentCenterWorld;

            var parentRight = parentTransform.right;
            var parentUp = parentTransform.up;

            var offset = new Vector2(
                Vector3.Dot(offsetWorld, parentRight),
                Vector3.Dot(offsetWorld, parentUp)
            );

            var parentSize = parentTransform.rect.size * parentTransform.lossyScale;
            var maxSize = Mathf.Max(parentSize.x, parentSize.y);
            var squareSize = new Vector2(maxSize, maxSize);
            offset /= squareSize;

            parentSize = squareSize / parentSize;
            var childSize = childTransform.rect.size * childTransform.lossyScale;
            var scale = childSize / squareSize;

            m_SoftMaskBlitMaterial.DisableKeyword(k_SLICED);
            m_SoftMaskBlitMaterial.SetMatrix(s_ParentMaskMatrixID, Matrix4x4.Scale(parentSize));
            m_SoftMaskBlitMaterial.SetTexture(s_ParentMaskID, parentMask.m_MaskForRenderingTex);
            Graphics.Blit(Texture2D.whiteTexture, parentTex, m_SoftMaskBlitMaterial);

            var rotationDelta = Quaternion.Inverse(parentTransform.rotation) * childTransform.rotation;
            overrideMatrix = Matrix4x4.TRS(offset, rotationDelta, scale);
        }

        private void CheckTargetMaterial()
        {
            switch (m_OverrideMaskMaterial && MaterialHasSoftMask(m_OverrideMaskMaterial))
            {
                case true when m_TempMaterial != m_OverrideMaskMaterial:
                    m_TempMaterial = m_OverrideMaskMaterial;
                    return;
                case false when !m_TempMaterial:
                case false when m_TempMaterial && !m_TempMaterial.name.Contains(k_SoftMaskMatTag):
                    m_TempMaterial = new Material(s_SoftMaskShader);
                    m_TempMaterial.name = $"{k_SoftMaskMatTag}{m_TempMaterial.GetInstanceID()}]";
                    m_TempMaterial.hideFlags =
                        HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.NotEditable;
                    m_MaskableObjects.ToList().ForEach(maskableObj =>
                    {
                        if (maskableObj is not TMP_Text && maskableObj.material is var mat &&
                            mat != m_TempMaterial) //Return default material
                            maskableObj.material = m_TempMaterial;
                    });
                    break;
            }
        }

        private void ResetSetup()
        {
            foreach (var maskableObj in m_MaskableObjects)
            {
                if (maskableObj is TMP_Text textMesh && textMesh.font &&
                    FontMaterialData.FindData(m_TMPFontMaterialData, textMesh.font,
                        out var foundData)) //Return source font material
                {
                    if (foundData.GetRelativeKeyMaterial(textMesh.fontSharedMaterial) is var keyMaterial)
                        textMesh.fontSharedMaterial = keyMaterial;

                    textMesh.fontSharedMaterial = textMesh.font.material;
                }
                else if (maskableObj.material is var mat &&
                         mat.name.StartsWith(k_SoftMaskMatTag) ||
                         m_OverrideMaskMaterial &&
                         mat.name.StartsWith(m_OverrideMaskMaterial.name)) //Return default material
                    maskableObj.material = null;
            }

            m_TempMaterial = null;
            m_MaskForRenderingTex = null;

            m_MaskableObjects.Clear();
            m_TMPFontMaterialData.Clear();
        }
    }
}