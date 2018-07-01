using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter)), ExecuteInEditMode]
[AddComponentMenu("Effects/Post Effect Mask Renderer", -1)]
public class PostEffectMaskRenderer : MonoBehaviour {

    static class Uniforms {
        internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");
        internal static readonly int _Color = Shader.PropertyToID("_Color");
    }

    [SerializeField] private PostEffectMask m_mask;

    [Header("Appearance in the mask")]
    [Range(0, 1)] public float opacity = 1;
    public Vector3 scale = Vector3.one;
    [Tooltip("Only alpha channel is used")]
    public Texture texture;
    //public bool depthTest = true;
    //public CullMode cullMode = CullMode.Off;

    private PostEffectMask m_attachedMask;
    private MaterialPropertyBlock m_materialProps;
    private MeshFilter m_meshFilter;
    private Transform m_transform;
    
    public Mesh mesh { get { return m_meshFilter.sharedMesh; } }

    public PostEffectMask mask {
        get { return m_attachedMask; }
        set {
            m_mask = value;
            if (m_attachedMask != value) {
                if (m_attachedMask != null)
                    m_attachedMask.maskRenderers.Remove(this);
                m_attachedMask = value;
                if (m_attachedMask != null)
                    m_attachedMask.maskRenderers.Add(this);
            }
        }
    }

    public Matrix4x4 localToWorldMatrix {
        get { return m_transform.localToWorldMatrix * Matrix4x4.Scale(scale); }
    }

    internal MaterialPropertyBlock materialProps {
        get {
            if (m_materialProps == null) m_materialProps = new MaterialPropertyBlock();

            m_materialProps.Clear();
            m_materialProps.SetColor(Uniforms._Color, new Color(1, 1, 1, opacity));
            // these don't work through the material property block, would have to create a separate material for each object
            //var depthCompare = depthTest ? CompareFunction.LessEqual : CompareFunction.Always;
            //m_materialProps.SetFloat(Uniforms._DepthComp, (float)depthCompare);
            //m_materialProps.SetFloat(Uniforms._CullMode, (float)cullMode);
            if (texture != null)
                m_materialProps.SetTexture(Uniforms._MainTex, texture);

            return m_materialProps;
        }
    }

    private void Reset() {
        m_mask = FindObjectOfType<PostEffectMask>();
        OnValidate();
    }

    private void Awake() {
        m_transform = GetComponent<Transform>();
        m_meshFilter = GetComponent<MeshFilter>();

        if (m_mask != null) {
            m_attachedMask = m_mask;
            m_attachedMask.maskRenderers.Add(this);
        }
    }

    private void OnEnable() { }

    private void OnDestroy() {
        if (m_attachedMask != null)
            m_attachedMask.maskRenderers.Remove(this);
        m_attachedMask = null;
    }

    private void OnValidate() {
        mask = m_mask;
    }

}
