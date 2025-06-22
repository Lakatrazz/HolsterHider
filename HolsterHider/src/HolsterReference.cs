using Il2CppSLZ.Marrow;

using UnityEngine;

namespace HolsterHider;

public class HolsterReference
{
    private Transform _root;
    private Vector3 _rootLocalScale;

    private MeshRenderer[] _renderers;

    private InventorySlotReceiver _slotReceiver = null;
    private bool _hasSlotReceiver = false;

    public void CacheReferences(Transform root, MeshRenderer[] renderers, InventorySlotReceiver slotReceiver)
    {
        _hasSlotReceiver = true;
        _slotReceiver = slotReceiver;

        CacheReferences(root, renderers);
    }

    public void CacheReferences(Transform root, MeshRenderer[] renderers)
    {
        _root = root;
        _rootLocalScale = root.localScale;

        _renderers = renderers;
    }

    public void SetScale(float scale)
    {
        if (_hasSlotReceiver && HasWeapon())
        {
            ScaleWithoutItem(scale);
        }
        else
        {
            ScaleRoot(scale);
        }
    }

    private bool HasWeapon()
    {
        var slottedWeapon = _slotReceiver._slottedWeapon;

        return slottedWeapon != null && slottedWeapon.interactableHost != null && slottedWeapon.interactableHost.marrowEntity != null;
    }

    private void ScaleWithoutItem(float scale)
    {
        var marrowEntity = _slotReceiver._slottedWeapon.interactableHost.marrowEntity.transform;

        var parent = marrowEntity.parent;
        var localPosition = marrowEntity.localPosition;

        marrowEntity.parent = null;

        ScaleRoot(scale);

        marrowEntity.parent = parent;
        marrowEntity.localPosition = localPosition;
    }

    private void ScaleRoot(float scale)
    {
        _root.localScale = _rootLocalScale * scale;
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
