using System.Collections.Generic;
using UnityEngine;

public class MobPartAnchorManager : MonoBehaviour
{
    public List<MobPartAnchor> anchors;

    public Transform GetAnchor(string partName)
    {
        foreach (var anchor in anchors)
        {
            if (anchor.partName == partName) return anchor.anchor;
        }
        return transform; // fallback
    }
}
