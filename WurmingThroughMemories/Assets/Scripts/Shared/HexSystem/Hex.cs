using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace HexSystem {

	// Inspiration: https://www.redblobgames.com/grids/hexagons/


	public class InvalidNeighbourException : Exception {
		public InvalidNeighbourException() { }
		public InvalidNeighbourException(string msg) : base(msg) { }
	}

	public enum HexDirection {
		TOP_RIGHT = 0,
		TOP_LEFT = 1,
		LEFT = 2,
		BOTTOM_LEFT = 3,
		BOTTOM_RIGHT = 4,
		RIGHT = 5,
	}

	public static class HexDirectionExtensions {

		public static float GetAngleInDegrees(this HexDirection direction) {
			switch (direction) {
				case HexDirection.RIGHT: return 0.0f;
				case HexDirection.TOP_RIGHT: return 60.0f;
				case HexDirection.TOP_LEFT: return 120.0f;
				case HexDirection.LEFT: return 180.0f;
				case HexDirection.BOTTOM_LEFT: return 240.0f;
				case HexDirection.BOTTOM_RIGHT: return 300.0f;
				default: throw new Exception("Invalid direction " + direction + " not supported");
			}
		}
		public static HexDirection MirrorX(this HexDirection direction) {
			switch (direction) {
				case HexDirection.RIGHT: return HexDirection.LEFT;
				case HexDirection.TOP_RIGHT: return HexDirection.TOP_LEFT;
				case HexDirection.TOP_LEFT: return HexDirection.TOP_RIGHT;
				case HexDirection.LEFT: return HexDirection.RIGHT;
				case HexDirection.BOTTOM_LEFT: return HexDirection.BOTTOM_RIGHT;
				case HexDirection.BOTTOM_RIGHT: return HexDirection.BOTTOM_LEFT;
				default: throw new Exception("Invalid direction " + direction + " not supported");
			}
		}
		public static HexDirection MirrorY(this HexDirection direction) {
			switch (direction) {
				case HexDirection.RIGHT: return HexDirection.RIGHT;
				case HexDirection.TOP_RIGHT: return HexDirection.BOTTOM_RIGHT;
				case HexDirection.TOP_LEFT: return HexDirection.BOTTOM_LEFT;
				case HexDirection.LEFT: return HexDirection.LEFT;
				case HexDirection.BOTTOM_LEFT: return HexDirection.TOP_LEFT;
				case HexDirection.BOTTOM_RIGHT: return HexDirection.TOP_RIGHT;
				default: throw new Exception("Invalid direction " + direction + " not supported");
			}
		}
		public static HexDirection RotateClockwise(this HexDirection dir) {
			return Hex.GetNextDirectionClockwise(dir);
		}

		public static HexDirection RotateAntiClockwise(this HexDirection dir) {
			return Hex.GetNextDirectionAntiClockwise(dir);
		}

		public static HexDirection Rotate(this HexDirection dir, HexRotation rotation) {
			if (rotation == HexRotation.LEFT) return dir.RotateAntiClockwise();
			else return dir.RotateClockwise();
		}

		public static HexCorner GetClockwiseCorner(this HexDirection side) {
			return (HexCorner)(((int)side - 1 + 6) % 6);
		}

		public static HexCorner GetAntiClockwiseCorner(this HexDirection side) {
			return (HexCorner)((int)side);
		}

		public static HexCorner GetCorner(this HexDirection side, HexRotation rotation) {
			if (rotation == HexRotation.LEFT) return side.GetAntiClockwiseCorner();
			else return side.GetClockwiseCorner();
		}

		public static HexDirection GetOppositeDirection(this HexDirection dir) {
			return (HexDirection)(((int)dir + 3) % 6);
		}

		public static bool IsLeftFrom(this HexDirection a, HexDirection b) {
			return (b - a + 6) % 6 > 3;
		}
		public static bool IsRightFrom(this HexDirection a, HexDirection b) {
			return (b - a - 1+ 6) % 6 < 2;
		}
	}

	public enum HexCorner {
		TOP = 0,
		TOP_LEFT = 1,
		BOTTOM_LEFT = 2,
		BOTTOM = 3,
		BOTTOM_RIGHT = 4,
		TOP_RIGHT = 5,
	}

	public static class HexCornerExtensions {

		public static float GetAngleInDegrees(this HexCorner corner) {
			switch (corner) {
				case HexCorner.TOP: return 90.0f;
				case HexCorner.TOP_LEFT: return 150.0f;
				case HexCorner.BOTTOM_LEFT: return 210.0f;
				case HexCorner.BOTTOM: return 270.0f;
				case HexCorner.BOTTOM_RIGHT: return 330.0f;
				case HexCorner.TOP_RIGHT: return 30.0f;
				default: throw new Exception("Invalid corner " + corner + " not supported");
			}
		}
		
		public static HexCorner MirrorX(this HexCorner corner) {
			switch (corner) {
				case HexCorner.TOP: return HexCorner.TOP;
				case HexCorner.TOP_LEFT: return HexCorner.TOP_RIGHT;
				case HexCorner.BOTTOM_LEFT: return HexCorner.BOTTOM_RIGHT;
				case HexCorner.BOTTOM: return HexCorner.BOTTOM;
				case HexCorner.BOTTOM_RIGHT: return HexCorner.BOTTOM_LEFT;
				case HexCorner.TOP_RIGHT: return HexCorner.TOP_LEFT;
				default: throw new Exception("Invalid corner " + corner + " not supported");
			}
		}
		public static HexCorner MirrorY(this HexCorner corner) {
			switch (corner) {
				case HexCorner.TOP: return HexCorner.BOTTOM;
				case HexCorner.TOP_LEFT: return HexCorner.BOTTOM_LEFT;
				case HexCorner.BOTTOM_LEFT: return HexCorner.TOP_LEFT;
				case HexCorner.BOTTOM: return HexCorner.BOTTOM;
				case HexCorner.BOTTOM_RIGHT: return HexCorner.TOP_RIGHT;
				case HexCorner.TOP_RIGHT: return HexCorner.BOTTOM_RIGHT;
				default: throw new Exception("Invalid corner " + corner + " not supported");
			}
		}

		public static HexCorner RotateClockwise(this HexCorner corner) {
			return (HexCorner)(((int)corner - 1 + 6) % 6);

		}

		public static HexCorner RotateAntiClockwise(this HexCorner corner) {
			return (HexCorner)(((int)corner + 1) % 6);
		}

		public static HexCorner Rotate(this HexCorner corner, HexRotation rotation) {
			if (rotation == HexRotation.LEFT) return corner.RotateAntiClockwise();
			else return corner.RotateClockwise();
		}

		public static HexDirection GetClockwiseSide(this HexCorner corner) {
			return (HexDirection)((int)corner);
		}

		public static HexDirection GetAntiClockwiseSide(this HexCorner corner) {
			return (HexDirection)(((int)corner + 1) % 6);
		}

		public static HexDirection GetSide(this HexCorner corner, HexRotation rotation) {
			if (rotation == HexRotation.LEFT) return corner.GetAntiClockwiseSide();
			else return corner.GetClockwiseSide();
		}

		public static HexCorner GetOppositeCorner(this HexCorner corner) {
			return (HexCorner)(((int)corner + 3) % 6);
		}

		public static Vector2 GetLoc(this HexCorner corner) {
			switch (corner) {
				case HexCorner.TOP: return new Vector2(0,0.5f);
				case HexCorner.TOP_LEFT: return new Vector2(-Hex.Width/2f,0.25f);
				case HexCorner.BOTTOM_LEFT: return new Vector2(-Hex.Width/2f,-0.25f);
				case HexCorner.BOTTOM: return new Vector2(0,-0.5f);
				case HexCorner.BOTTOM_RIGHT: return new Vector2(Hex.Width/2f,-0.25f);
				case HexCorner.TOP_RIGHT: return new Vector2(Hex.Width/2f,0.25f);
				default: throw new Exception("Invalid corner " + corner + " not supported");
			}
		}
	}

	public enum HexRotation {
		LEFT,
		RIGHT,
	}


	[Serializable]
	public struct Hex : IEquatable<Hex> {
		
		// cube coordinates
		public int x {
			get {
				return q;
			}
		}
		public int y {
			get {
				return -x - z;
			}
		}
		public int z {
			get {
				return r;
			}
		}


		public int q, r; // axial coordinates

		public static Hex Zero {
			get {
				return new Hex(0, 0);
			}
		}

		private static float width = Mathf.Sqrt(3.0f) / 2.0f;
		public static float Width {
			get {
				return width;
			}
		}

		public static float Height {
			get {
				return 1.0f;
			}
		}

		public static float LineLength {
			get {
				return 0.5f;
			}
		}

		// calculated from A² = B² + C² for triangle between center, corner and perpendicular line from center to side
		private static float lineDistanceFromCenter = Mathf.Sqrt(LineLength*LineLength - LineLength*0.5f*LineLength*0.5f);
		public static float LineDistanceFromCenter {
			get {
				return lineDistanceFromCenter;
			}
		}

		public Hex(int x, int y, int z) {
			q = x;
			r = z;
			Assert.IsTrue(x + y + z == 0);
		}

		public Hex(int q, int r) {
			this.q = q;
			this.r = r;
			Assert.IsTrue(x + y + z == 0);
		}

		public Hex(HexDirection dir) {
			Hex generic = genericNeighbours[(int)dir];
			q = generic.q;
			r = generic.r;
		}

		public override int GetHashCode() {
			unchecked { // Overflow is fine, just wrap
				int hash = 17;
				// Suitable nullity checks etc, of course :)
				hash = hash * 23 + r;
				hash = hash * 23 + q;
				return hash;
			}
		}

		public override bool Equals(object obj) {
			if (!(obj is Hex)) return false;
			Hex hex = (Hex)obj;
			return (hex.r == r && hex.q == q);
		}

		public HexDirection GetDirection() {
			return Hex.GetNeighbourDirection(Hex.Zero, this);
		}

		public bool Equals(Hex other) {
			return (other.r == r && other.q == q);
		}

		public static bool operator ==(Hex hex1, Hex hex2) {
			return hex1.Equals(hex2);
		}

		public static bool operator !=(Hex hex1, Hex hex2) {
			return !hex1.Equals(hex2);
		}

		public static Hex operator+(Hex a, Hex b) {
			return new Hex(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static Hex operator -(Hex a, Hex b) {
			return new Hex(a.x - b.x, a.y - b.y, a.z - b.z);
		}
		
		public static Hex operator *(Hex a, Hex b) {
			return new Hex(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		public static Hex operator *(Hex a, int v) {
			return new Hex(a.x * v, a.y * v, a.z * v);
		}

		public static implicit operator Hex(HexDirection dir) {
			return Hex.GetDirection(dir);
		}

		public int magnitude {
			get {
				return (Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z)) / 2;
			}
		}

		public static int Distance(Hex a, Hex b) {
			return (b - a).magnitude;
		}

		public static HexDirection GetNeighbourDirection(Hex hex, Hex neighbour) {
			Hex diff = neighbour - hex;
			for (int i = 0; i < 6; ++i) {
				if (diff == genericNeighbours[i]) {
					return (HexDirection)i;
				}
			}
			throw new InvalidNeighbourException();
		}

		public static HexDirection GetHexDirectionFromVector2(Vector2 vector) {
			// the HexDirection goes clockwise, so we flip x and y to
			// make atan2 work.
			float angle = Mathf.Atan2(vector.x, vector.y);
			angle *= Mathf.Rad2Deg;
			angle = (angle + 360) % 360;
			angle /= 60;
			int hexDirIndex = Mathf.FloorToInt(angle);
			if (hexDirIndex >= 6) hexDirIndex = 5; // not sure how this can happen but apparently it does
			if (hexDirIndex < 0) hexDirIndex = 0;

			// the formula above goes clockwise but we flipped the HexDirection order to
			// anticlockwise, so we perform a flip back to clockwise here because that's
			// simpler than re-figuring out the formula below ;).
			hexDirIndex = (6 - hexDirIndex) % 6;

			return (HexDirection)(hexDirIndex);
        }

		public static HexCorner GetHexCornerFromVector2(Vector2 vector) {
			float angle = Mathf.Atan2(vector.y, vector.x);
			angle *= Mathf.Rad2Deg; // [-180, 180]
			if (angle < 60.0f) angle += 360.0f; // make sure it is in the [60, 420] range
			int idx = Mathf.FloorToInt(angle / 60.0f) - 1; // map into [0,5] range
			return (HexCorner)(idx);
		}

		private static Hex[] genericNeighbours = new Hex[] {
			new Hex(1, 0, -1), new Hex(0, 1, -1), new Hex(-1, 1, 0), new Hex(-1, 0, 1), new Hex(0, -1, 1), new Hex(1, -1, 0)
		};

		public Hex RotateAround(Hex center, HexRotation rotation) {

			// calculate the vector from our hex to the center
			Hex diff = this - center;

			// rotate according to
			// http://www.redblobgames.com/grids/hexagons/
			if (rotation == HexRotation.LEFT) {
				diff = new Hex(-diff.y, -diff.z, -diff.x);
			}
			else {
				diff = new Hex(-diff.z, -diff.x, -diff.y);
			}

			// hand back
			return center + diff;
		}

		public List<Hex> GetNeighbours(int range = 1) {
			List<Hex> neighbours = new List<Hex>();
			GetNeighboursNonAlloc(neighbours, range);
			return neighbours;
		}

		public void GetNeighboursNonAlloc(IList<Hex> neighbours, int range = 1) {

			// clear
			neighbours.Clear();

			// add each ring
			for (int radius = 1; radius <= range; ++radius) {

				// move to the left radius steps
				Hex hex = this + Hex.GetDirection(HexDirection.LEFT) * radius;

				// each ring contains of 6 * radius hexes
				HexDirection dir = HexDirection.TOP_RIGHT;
				for (int i = 0; i < 6; ++i) {
					for (int step = 0; step < radius; ++step) {
						hex += dir;
						neighbours.Add(hex);
					}

					// next dir
					dir = Hex.GetNextDirectionClockwise(dir);
				}
			}
		}

		public static Hex GetDirection(HexDirection dir) {
			return new Hex(dir);
		}

		public static HexDirection GetNextDirectionClockwise(HexDirection dir) {
			return (HexDirection)((((int)dir)-1+6) % 6);
		}

		public static HexDirection GetNextDirectionAntiClockwise(HexDirection dir) {
			return (HexDirection)((((int)dir)+1) % 6);
		}

		public static Hex GetDirection(int dir) {
			return genericNeighbours[dir];
		}

		public bool IsNeighbour(Hex neighbourHex) {
			return Hex.Distance(this, neighbourHex) == 1;
		}

		public HexCorner MapNeighbourCorner(Hex neighbourHex, HexCorner neighbourCorner) {

			// we are not neighbours
			if (!IsNeighbour(neighbourHex)) throw new InvalidNeighbourException();

			// see if this hex lies on one of the two sides lying around this corner
			for (int i = 0; i < 2; ++i) {
				HexRotation rotation = (HexRotation)i;

				// get the side either to the left or right of the corner
				HexDirection side = neighbourCorner.GetSide(rotation);

				// see if the hex lies on the other side of this side
				if (neighbourHex + side == this) {

					// flip the side so that it is now relative to the other hex
					side = side.GetOppositeDirection();

					// get the corner by rotating in the same direction as we previously did,
					// but now from the perspective of the other hex.
					HexCorner corner = side.GetCorner(rotation);
					return corner;
				}
			}
			
			// we are neighbours, but we don't share this corner!
			throw new InvalidNeighbourException("Hex " + neighbourHex + " does not share corner " + neighbourCorner + " with hex " + this);
		}

		public Vector2 GetLoc() {
			float x = Mathf.Sqrt(3.0f) * (q + r * 0.5f);
			float y = -3.0f / 2.0f * r; // IMPORTANT! this whole system assumes y goes downwards
			return new Vector2(x, y) * 0.5f;
		}

		public override string ToString() {
			//return "[" + q + "," + r + "]";
			return "[" + GetLoc().x.ToString("F2") + "," + GetLoc().y.ToString("F2") + "]";
		}

		public static Hex ConvertToHex(Vector2 loc) {
			loc *= 2.0f;
			float q = (loc.x * Mathf.Sqrt(3.0f) / 3.0f - (-loc.y) / 3.0f);
			float r = (-loc.y) * 2.0f/3.0f;
			return RoundHex(q, r);
		}

		public bool Contains(Vector2 loc) {
			return ConvertToHex(loc) == this;
		}

		private static Hex RoundHex(float q, float r) {

			// convert to cube coordinates
			float x = q;
			float y = -q-r;
			float z = r;

			int rx = Mathf.RoundToInt(x);
			int ry = Mathf.RoundToInt(y);
			int rz = Mathf.RoundToInt(z);

			float x_diff = Mathf.Abs(rx - x);
			float y_diff = Mathf.Abs(ry - y);
			float z_diff = Mathf.Abs(rz - z);

			if (x_diff > y_diff && x_diff > z_diff) {
				rx = -ry-rz;
			}
			else if (y_diff > z_diff) {
				ry = -rx-rz;
			}
			else {
				rz = -rx-ry;
			}

			return new Hex(rx, ry, rz);
		}
	}
}