using UnityEngine;

public class BuildTurretSlots : MonoBehaviour
{
    [Header("6 Slots")]
    public BuildTurretSlot[] slots =
        new BuildTurretSlot[6];

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (slots == null)
            return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            TurretData turret = null;

            if (InventoryManager.Instance != null)
            {
                turret =
                    InventoryManager.Instance
                        .GetSelectedTurret(i);
            }

            if (turret != null)
            {
                slots[i].Setup(turret);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}