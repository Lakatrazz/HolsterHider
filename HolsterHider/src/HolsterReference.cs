using System.Collections.Generic;

using UnityEngine;

namespace HolsterHider;

public class HolsterReference
{
    private Transform _root;
    private Vector3 _rootLocalPosition;
    private Vector3 _rootLocalScale;

    private MeshRenderer[] _renderers;
    private Collider[] _colliders;

    private Vector3[] _rendererScales;
    private ColliderScaleHolder[] _colliderScales;

    public void CacheReferences(Transform root, MeshRenderer[] renderers, Collider[] colliders)
    {
        _root = root;
        _rootLocalPosition = root.localPosition;
        _rootLocalScale = root.localScale;

        foreach (var renderer in renderers)
        {
            List<Transform> children = new();
            foreach (var child in renderer.transform)
            {
                children.Add(child.TryCast<Transform>());
            }

            foreach (var child in children)
            {
                child.transform.parent = renderer.transform.parent;
            }
        }

        _renderers = renderers;
        _colliders = colliders;

        _rendererScales = new Vector3[renderers.Length];
        for (var i = 0; i < _rendererScales.Length; i++)
        {
            _rendererScales[i] = renderers[i].transform.localScale;
        }

        _colliderScales = new ColliderScaleHolder[colliders.Length];
        for (var i = 0; i < _colliderScales.Length; i++)
        {
            _colliderScales[i] = new ColliderScaleHolder(_colliders[i]);
        }
    }

    public void ScaleObject(float scale)
    {
        _root.localScale = _rootLocalScale * scale;
    }

    public void ScaleMeshes(float scale)
    {
        _root.localPosition = _rootLocalPosition * scale;

        for (var i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].transform.localScale = _rendererScales[i] * scale;
        }
    }

    public void ScaleColliders(float scale)
    {
        for (var i = 0; i < _colliders.Length; i++)
        {
            _colliderScales[i].SetScale(_colliders[i], scale);
        }
    }

    public void SetVisibility(HolsterVisibility visibility)
    {
        switch (visibility)
        {
            case HolsterVisibility.INVISIBLE:
                _root.gameObject.SetActive(true);

                foreach (var renderer in _renderers)
                {
                    renderer.enabled = false;
                }
                break;
            case HolsterVisibility.DISABLED:
                _root.gameObject.SetActive(false);
                break;
            default:
            case HolsterVisibility.DEFAULT:
                _root.gameObject.SetActive(true);

                foreach (var renderer in _renderers)
                {
                    renderer.enabled = true;
                }
                break;
        }
    }
}
