using System;
using UnityEngine;

// [ExecuteInEditMode]
public class VerticalBillboardController : MonoBehaviour {

    private int _scaleID;
    private Material _material;
    private MeshFilter _meshFilter;
    private Renderer _renderer;

    private void Awake() {
        _material = GetComponent<Renderer>().material;
        _scaleID = Shader.PropertyToID("_ScaleXY");
        _meshFilter = GetComponent<MeshFilter>();
        _renderer = GetComponent<Renderer>();
        InitBoundingBox();
    }
    
    public void InitBoundingBox() {
        Mesh mesh = _meshFilter.mesh;
        Bounds bounds = _renderer.bounds;
        Vector3 extents = bounds.extents;
        float maxLength = Mathf.Sqrt(extents.x * extents.x + extents.z * extents.z);
        extents.x = maxLength;
        extents.z = maxLength;
        bounds.extents = transform.InverseTransformVector(extents);
        bounds.center = Vector3.zero;
        mesh.bounds = bounds;
    }

    private void Update() {
        var scale = transform.lossyScale;
        _material.SetVector(_scaleID, scale);
    }
}