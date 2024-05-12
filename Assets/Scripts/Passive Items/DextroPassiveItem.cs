using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DextroPassiveItem : PassiveItem
{
    protected override void applyModifier()
    {
        player.CurrentMight *= 1 + passiveItemData.Multiplier / 100f;
    }
}
