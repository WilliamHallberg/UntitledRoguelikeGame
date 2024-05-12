using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkPassiveItem : PassiveItem
{
    protected override void applyModifier()
    {
        player.CurrentRecovery *= 1 + passiveItemData.Multiplier / 100f;
    }
}
