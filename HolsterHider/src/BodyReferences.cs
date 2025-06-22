using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow;

using UnityEngine;

namespace HolsterHider;

public class BodyReferences
{
    private readonly HolsterReference[] _holsterReferences = null;

    public HolsterReference[] HolsterReferences => _holsterReferences;

    public HolsterReference HeadGroup => _holsterReferences[0];
    public HolsterReference RightShoulderGroup => _holsterReferences[1];
    public HolsterReference LeftShoulderGroup => _holsterReferences[2];
    public HolsterReference RightUnderarmGroup => _holsterReferences[3];
    public HolsterReference LeftUnderarmGroup => _holsterReferences[4];
    public HolsterReference BackGroup => _holsterReferences[5];
    public HolsterReference AmmoPouchGroup => _holsterReferences[6];
    public HolsterReference BodyLogGroup => _holsterReferences[7];

    private RigManager _rigManager = null;

    public BodyReferences()
    {
        _holsterReferences = new HolsterReference[] {
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
            new(),
        };
    }

    public void ApplyConfig(BodyConfig bodyConfig)
    {
        if (_rigManager == null)
        {
            return;
        }

        float scale = 1f;

        if (bodyConfig.ScaleHolsters)
        {
            scale = _rigManager.avatar.height / 1.76f;
        }

        for (var i = 0; i < HolsterReferences.Length; i++)
        {
            var reference = HolsterReferences[i];

            reference.SetScale(scale);
            reference.SetVisibility(bodyConfig.HolsterConfigs[i].Visibility);
        }
    }

    public void CacheReferences(RigManager rigManager)
    {
        _rigManager = rigManager;
        var bodySlots = rigManager.inventory.bodySlots;

        CacheHolster(rigManager.physicsRig.m_head.GetComponentInChildren<InventorySlotReceiver>(true), HeadGroup);
        CacheHolster(bodySlots[3], RightShoulderGroup);
        CacheHolster(bodySlots[2], LeftShoulderGroup);

        CacheHolster(bodySlots[5], RightUnderarmGroup);
        CacheHolster(bodySlots[0], LeftUnderarmGroup);

        CacheHolster(bodySlots[4], BackGroup);

        CacheAmmoPouch(rigManager.GetComponentInChildren<InventoryAmmoReceiver>(true), AmmoPouchGroup);
        CachePullCord(rigManager.inventory.specialItems[0].GetComponentInChildren<PullCordDevice>(true), BodyLogGroup);
    }

    private static void CacheHolster(SlotContainer slotContainer, HolsterReference holster)
    {
        var root = slotContainer.transform;
        var slotReceiver = slotContainer.inventorySlotReceiver;

        holster.CacheReferences(root, root.GetComponentsInChildren<MeshRenderer>(true), slotReceiver);
    }

    private static void CacheHolster(InventorySlotReceiver slotReceiver, HolsterReference holster)
    {
        Transform root = slotReceiver.transform.parent;
        holster.CacheReferences(root, root.GetComponentsInChildren<MeshRenderer>(true), slotReceiver);
    }

    private static void CacheAmmoPouch(InventoryAmmoReceiver ammoReceiver, HolsterReference holster)
    {
        Transform root = ammoReceiver.transform.parent;
        holster.CacheReferences(root, root.GetComponentsInChildren<MeshRenderer>(true));
    }

    private static void CachePullCord(PullCordDevice pullCord, HolsterReference holster)
    {
        var root = pullCord.transform;
        holster.CacheReferences(root, root.Find("BodyLog").GetComponentsInChildren<MeshRenderer>(true));
    }
}
