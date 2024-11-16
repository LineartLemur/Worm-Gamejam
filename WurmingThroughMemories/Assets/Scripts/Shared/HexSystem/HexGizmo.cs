using UnityEngine;

namespace HexSystem {
    public static class HexGizmo {
        public static void DrawHexGizmo(Vector3 origin, Hex hex) {
            Gizmos.DrawLine(origin + (Vector3) (hex.GetLoc() + HexCorner.TOP.GetLoc()),
                origin + (Vector3) (hex.GetLoc() + HexCorner.TOP_RIGHT.GetLoc()));
            Gizmos.DrawLine(origin + (Vector3) (hex.GetLoc() + HexCorner.TOP_RIGHT.GetLoc()),
                origin + (Vector3) (hex.GetLoc() + HexCorner.BOTTOM_RIGHT.GetLoc()));
            Gizmos.DrawLine(origin + (Vector3) (hex.GetLoc() + HexCorner.BOTTOM_RIGHT.GetLoc()),
                origin + (Vector3) (hex.GetLoc() + HexCorner.BOTTOM.GetLoc()));
            Gizmos.DrawLine(origin + (Vector3) (hex.GetLoc() + HexCorner.BOTTOM.GetLoc()),
                origin + (Vector3) (hex.GetLoc() + HexCorner.BOTTOM_LEFT.GetLoc()));
            Gizmos.DrawLine(origin + (Vector3) (hex.GetLoc() + HexCorner.BOTTOM_LEFT.GetLoc()),
                origin + (Vector3) (hex.GetLoc() + HexCorner.TOP_LEFT.GetLoc()));
            Gizmos.DrawLine(origin + (Vector3) (hex.GetLoc() + HexCorner.TOP_LEFT.GetLoc()),
                origin + (Vector3) (hex.GetLoc() + HexCorner.TOP.GetLoc()));
        }

        public delegate Vector3 LocalToWorld(Vector3 point);
        public static void DrawHexGizmo( LocalToWorld tranform, Hex hex) {
            Gizmos.DrawLine(tranform( (Vector3) (hex.GetLoc() + HexCorner.TOP.GetLoc())),
                tranform( (Vector3) (hex.GetLoc() + HexCorner.TOP_RIGHT.GetLoc())));
            Gizmos.DrawLine(tranform( (Vector3) (hex.GetLoc() + HexCorner.TOP_RIGHT.GetLoc())),
                tranform( (Vector3) (hex.GetLoc() + HexCorner.BOTTOM_RIGHT.GetLoc())));
            Gizmos.DrawLine(tranform( (Vector3) (hex.GetLoc() + HexCorner.BOTTOM_RIGHT.GetLoc())),
                tranform( (Vector3) (hex.GetLoc() + HexCorner.BOTTOM.GetLoc())));
            Gizmos.DrawLine(tranform( (Vector3) (hex.GetLoc() + HexCorner.BOTTOM.GetLoc())),
                tranform( (Vector3) (hex.GetLoc() + HexCorner.BOTTOM_LEFT.GetLoc())));
            Gizmos.DrawLine(tranform( (Vector3) (hex.GetLoc() + HexCorner.BOTTOM_LEFT.GetLoc())),
                tranform( (Vector3) (hex.GetLoc() + HexCorner.TOP_LEFT.GetLoc())));
            Gizmos.DrawLine(tranform( (Vector3) (hex.GetLoc() + HexCorner.TOP_LEFT.GetLoc())),
                tranform( (Vector3) (hex.GetLoc() + HexCorner.TOP.GetLoc())));
        }
    }
}