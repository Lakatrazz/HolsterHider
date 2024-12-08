using UnityEngine;

namespace HolsterHider;

public class ColliderScaleHolder
{
    private Vector3 _initialSize = Vector3.one;
    private Vector3 _initialCenter = Vector3.zero;

    public ColliderScaleHolder(Collider collider)
    {
        _initialSize = GetSize(collider);
        _initialCenter = GetCenter(collider);
    }

    public void SetScale(Collider collider, float scale)
    {
        SetSize(collider, _initialSize * scale);
        SetCenter(collider, _initialCenter * scale);
    }

    public Vector3 GetCenter(Collider collider)
    {
        var boxCollider = collider.TryCast<BoxCollider>();
        if (boxCollider != null)
        {
            return boxCollider.center;
        }

        var capsuleCollider = collider.TryCast<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            return capsuleCollider.center;
        }

        var sphereCollider = collider.TryCast<SphereCollider>();
        if (sphereCollider != null)
        {
            return sphereCollider.center;
        }

        return Vector3.zero;
    }

    public void SetCenter(Collider collider, Vector3 center)
    {
        var boxCollider = collider.TryCast<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.center = center;
            return;
        }

        var capsuleCollider = collider.TryCast<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            capsuleCollider.center = center;
            return;
        }

        var sphereCollider = collider.TryCast<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.center = center;
            return;
        }
    }

    public Vector3 GetSize(Collider collider)
    {
        var boxCollider = collider.TryCast<BoxCollider>();
        if (boxCollider != null)
        {
            return boxCollider.size;
        }

        var capsuleCollider = collider.TryCast<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            return new Vector3(capsuleCollider.radius, capsuleCollider.height, capsuleCollider.radius);
        }

        var sphereCollider = collider.TryCast<SphereCollider>();
        if (sphereCollider != null)
        {
            return Vector3.one * sphereCollider.radius;
        }

        return Vector3.one;
    }

    public void SetSize(Collider collider, Vector3 size)
    {
        var boxCollider = collider.TryCast<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.size = size;
            return;
        }

        var capsuleCollider = collider.TryCast<CapsuleCollider>();
        if (capsuleCollider != null)
        {
            capsuleCollider.radius = size.x;
            capsuleCollider.height = size.y;
            return;
        }

        var sphereCollider = collider.TryCast<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.radius = size.x;
            return;
        }
    }
}
