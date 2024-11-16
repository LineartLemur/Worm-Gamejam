using System;
using System.Collections.Generic;
using UnityEngine;

namespace HexSystem {
    [Serializable]
    public class HexRegion {
        public List<Hex> hexes;

        public void DrawGizmos(Vector3 center) {
            for (int i = 0; i < hexes.Count; i++) {
                HexGizmo.DrawHexGizmo(center, hexes[i]);
            }
        }

        
    }
}