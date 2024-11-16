using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using HexSystem;
using System.Collections.Generic;

public class HexTest {

	[Test]
	public static void TestDirectionAngles() {
		Assert.IsTrue(IsSameAngle(HexDirection.TOP_RIGHT.GetAngleInDegrees(), 60.0f));
		Assert.IsTrue(IsSameAngle(HexDirection.TOP_LEFT.GetAngleInDegrees(), 120.0f));
		Assert.IsTrue(IsSameAngle(HexDirection.LEFT.GetAngleInDegrees(), 180.0f));
		Assert.IsTrue(IsSameAngle(HexDirection.BOTTOM_LEFT.GetAngleInDegrees(), 240.0f));
		Assert.IsTrue(IsSameAngle(HexDirection.BOTTOM_RIGHT.GetAngleInDegrees(), 300.0f));
		Assert.IsTrue(IsSameAngle(HexDirection.RIGHT.GetAngleInDegrees(), 0.0f));
	}

	[Test]
	public static void TestCornerAngles() {
		Assert.IsTrue(IsSameAngle(HexCorner.TOP.GetAngleInDegrees(), 90.0f));
		Assert.IsTrue(IsSameAngle(HexCorner.TOP_LEFT.GetAngleInDegrees(), 150.0f));
		Assert.IsTrue(IsSameAngle(HexCorner.BOTTOM_LEFT.GetAngleInDegrees(), 210.0f));
		Assert.IsTrue(IsSameAngle(HexCorner.BOTTOM.GetAngleInDegrees(), 270.0f));
		Assert.IsTrue(IsSameAngle(HexCorner.BOTTOM_RIGHT.GetAngleInDegrees(), 330.0f));
		Assert.IsTrue(IsSameAngle(HexCorner.TOP_RIGHT.GetAngleInDegrees(), 30.0f));
	}

	[Test]
	public static void TestCoordinateSystems() {
		Assert.IsTrue(Hex.Zero == new Hex(0, 0, 0));

		Assert.IsTrue(new Hex(1, 0, -1) == new Hex(1, -1));
		Assert.IsTrue(new Hex(0, 1, -1) == new Hex(0, -1));
		Assert.IsTrue(new Hex(-1, 1, 0) == new Hex(-1, 0));
		Assert.IsTrue(new Hex(-1, 0, 1) == new Hex(-1, 1));
		Assert.IsTrue(new Hex(0, -1, 1) == new Hex(0, 1));
		Assert.IsTrue(new Hex(1, -1, 0) == new Hex(1, 0));
	}

	[Test]
	public static void TestDirectionsAdditions() {
		Assert.IsTrue(Hex.Zero + HexDirection.TOP_RIGHT == new Hex(1, -1));
		Assert.IsTrue(Hex.Zero + HexDirection.TOP_LEFT == new Hex(0, -1));
		Assert.IsTrue(Hex.Zero + HexDirection.LEFT == new Hex(-1, 0));
		Assert.IsTrue(Hex.Zero + HexDirection.BOTTOM_LEFT == new Hex(-1, 1));
		Assert.IsTrue(Hex.Zero + HexDirection.BOTTOM_RIGHT == new Hex(0, 1));
		Assert.IsTrue(Hex.Zero + HexDirection.RIGHT == new Hex(1, 0));

		Assert.IsTrue(new Hex(HexDirection.TOP_RIGHT) == new Hex(1, -1));
		Assert.IsTrue(new Hex(HexDirection.TOP_LEFT) == new Hex(0, -1));
		Assert.IsTrue(new Hex(HexDirection.LEFT) == new Hex(-1, 0));
		Assert.IsTrue(new Hex(HexDirection.BOTTOM_LEFT) == new Hex(-1, 1));
		Assert.IsTrue(new Hex(HexDirection.BOTTOM_RIGHT) == new Hex(0, 1));
		Assert.IsTrue(new Hex(HexDirection.RIGHT) == new Hex(1, 0));
	}

	[Test]
	public static void TestGetDirection() {
		Assert.IsTrue(new Hex(1, -1).GetDirection() == HexDirection.TOP_RIGHT);
		Assert.IsTrue(new Hex(0, -1).GetDirection() == HexDirection.TOP_LEFT);
		Assert.IsTrue(new Hex(-1, 0).GetDirection() == HexDirection.LEFT);
		Assert.IsTrue(new Hex(-1, 1).GetDirection() == HexDirection.BOTTOM_LEFT);
		Assert.IsTrue(new Hex(0, 1).GetDirection() == HexDirection.BOTTOM_RIGHT);
		Assert.IsTrue(new Hex(1, 0).GetDirection() == HexDirection.RIGHT);
	}

	[Test]
	public static void TestAddition() {
		Assert.IsTrue(new Hex(1, -1) + new Hex(1, -1) == new Hex(2, -2));
		Assert.IsTrue(new Hex(1, -1) + new Hex(0, -1) == new Hex(1, -2));
		Assert.IsTrue(new Hex(1, -1) + new Hex(-1, 0) == new Hex(0, -1));
		Assert.IsTrue(new Hex(1, -1) + new Hex(-1, 1) == Hex.Zero);
		Assert.IsTrue(new Hex(1, -1) + new Hex(0, 1) == new Hex(1, 0));
		Assert.IsTrue(new Hex(1, -1) + new Hex(1, 0) == new Hex(2, -1));
	}

	[Test]
	public static void TestDistance() {
		Assert.IsTrue(Hex.Distance(Hex.Zero, Hex.Zero) == 0);
		Assert.IsTrue(Hex.Distance(new Hex(2, -2), new Hex(2, -2)) == 0);
		Assert.IsTrue(Hex.Distance(new Hex(2, -2), Hex.Zero) == 2);
		Assert.IsTrue(Hex.Distance(new Hex(2, -2), new Hex(-2, 1)) == 4);
	}

	[Test]
	public static void TestNeighbourDirection() {
		Assert.IsTrue(Hex.GetNeighbourDirection(Hex.Zero, new Hex(HexDirection.TOP_RIGHT)) == HexDirection.TOP_RIGHT);
		Assert.IsTrue(Hex.GetNeighbourDirection(Hex.Zero, new Hex(HexDirection.TOP_LEFT)) == HexDirection.TOP_LEFT);
		Assert.IsTrue(Hex.GetNeighbourDirection(Hex.Zero, new Hex(HexDirection.LEFT)) == HexDirection.LEFT);
		Assert.IsTrue(Hex.GetNeighbourDirection(Hex.Zero, new Hex(HexDirection.BOTTOM_LEFT)) == HexDirection.BOTTOM_LEFT);
		Assert.IsTrue(Hex.GetNeighbourDirection(Hex.Zero, new Hex(HexDirection.BOTTOM_RIGHT)) == HexDirection.BOTTOM_RIGHT);
		Assert.IsTrue(Hex.GetNeighbourDirection(Hex.Zero, new Hex(HexDirection.RIGHT)) == HexDirection.RIGHT);
	}

	[Test]
	public static void TestGetHexDirectionFromVector2() {
		Assert.IsTrue(Hex.GetHexDirectionFromVector2(new Vector2(1, 1)) == HexDirection.TOP_RIGHT);
		Assert.IsTrue(Hex.GetHexDirectionFromVector2(new Vector2(-1, 1)) == HexDirection.TOP_LEFT);
		Assert.IsTrue(Hex.GetHexDirectionFromVector2(new Vector2(-1, 0)) == HexDirection.LEFT);
		Assert.IsTrue(Hex.GetHexDirectionFromVector2(new Vector2(-1, -1)) == HexDirection.BOTTOM_LEFT);
		Assert.IsTrue(Hex.GetHexDirectionFromVector2(new Vector2(1, -1)) == HexDirection.BOTTOM_RIGHT);
		Assert.IsTrue(Hex.GetHexDirectionFromVector2(new Vector2(1, 0)) == HexDirection.RIGHT);


		Assert.IsTrue(Hex.GetHexDirectionFromVector2(new Vector2(1, 0.1f)) == HexDirection.RIGHT);
		Assert.IsTrue(Hex.GetHexDirectionFromVector2(new Vector2(1, -0.1f)) == HexDirection.RIGHT);
	}

	[Test]
	public static void TestGetHexCornerFromVector2() {
		Assert.IsTrue(Hex.GetHexCornerFromVector2(new Vector2(0, 1)) == HexCorner.TOP);
		Assert.IsTrue(Hex.GetHexCornerFromVector2(new Vector2(-1, 1)) == HexCorner.TOP_LEFT);
		Assert.IsTrue(Hex.GetHexCornerFromVector2(new Vector2(-1, -1)) == HexCorner.BOTTOM_LEFT);
		Assert.IsTrue(Hex.GetHexCornerFromVector2(new Vector2(0, -1)) == HexCorner.BOTTOM);
		Assert.IsTrue(Hex.GetHexCornerFromVector2(new Vector2(1, -1)) == HexCorner.BOTTOM_RIGHT);
		Assert.IsTrue(Hex.GetHexCornerFromVector2(new Vector2(1, 1)) == HexCorner.TOP_RIGHT);

		Assert.IsTrue(Hex.GetHexCornerFromVector2(new Vector2(0.1f, 1)) == HexCorner.TOP);
		Assert.IsTrue(Hex.GetHexCornerFromVector2(new Vector2(-0.1f, 1)) == HexCorner.TOP);
	}

	[Test]
	public static void TestRotation() {
		Assert.IsTrue(new Hex(1, -1).RotateAround(Hex.Zero, HexRotation.LEFT) == new Hex(0, -1));
		Assert.IsTrue(new Hex(1, -1).RotateAround(Hex.Zero, HexRotation.RIGHT) == new Hex(1, 0));

		Assert.IsTrue(new Hex(1, -2).RotateAround(Hex.Zero, HexRotation.LEFT) == new Hex(-1, -1));
		Assert.IsTrue(new Hex(1, -2).RotateAround(Hex.Zero, HexRotation.RIGHT) == new Hex(2, -1));
	}

	[Test]
	public static void TestNeighbours() {
		Hex hex = new Hex(1, -1);
		List<Hex> neighbours = hex.GetNeighbours(1);
		Assert.IsTrue(neighbours.Count == 6);
		for (int i = 0; i < 6; ++i) {
			Hex neighbour = hex + (HexDirection)i;
			Assert.IsTrue(neighbours.Contains(neighbour));
			neighbours.Remove(neighbour);
		}
		Assert.IsTrue(neighbours.Count == 0);
	}

	[Test]
	public static void TestRotateDirection() {
		Assert.IsTrue(Hex.GetNextDirectionClockwise(HexDirection.TOP_RIGHT) == HexDirection.RIGHT);
		Assert.IsTrue(Hex.GetNextDirectionAntiClockwise(HexDirection.TOP_RIGHT) == HexDirection.TOP_LEFT);

		Assert.IsTrue(HexDirection.TOP_RIGHT.RotateClockwise() == HexDirection.RIGHT);
		Assert.IsTrue(HexDirection.TOP_RIGHT.RotateAntiClockwise() == HexDirection.TOP_LEFT);

		Assert.IsTrue(HexCorner.TOP.RotateClockwise() == HexCorner.TOP_RIGHT);
		Assert.IsTrue(HexCorner.TOP.RotateAntiClockwise() == HexCorner.TOP_LEFT);
	}

	[Test]
	public static void TestCornerSides() {
		Assert.IsTrue(HexCorner.TOP.GetClockwiseSide() == HexDirection.TOP_RIGHT);
		Assert.IsTrue(HexCorner.TOP.GetAntiClockwiseSide() == HexDirection.TOP_LEFT);

		Assert.IsTrue(HexCorner.BOTTOM_RIGHT.GetClockwiseSide() == HexDirection.BOTTOM_RIGHT);
		Assert.IsTrue(HexCorner.BOTTOM_RIGHT.GetAntiClockwiseSide() == HexDirection.RIGHT);

		Assert.IsTrue(HexDirection.TOP_RIGHT.GetClockwiseCorner() == HexCorner.TOP_RIGHT);
		Assert.IsTrue(HexDirection.TOP_RIGHT.GetAntiClockwiseCorner() == HexCorner.TOP);

		Assert.IsTrue(HexDirection.RIGHT.GetClockwiseCorner() == HexCorner.BOTTOM_RIGHT);
		Assert.IsTrue(HexDirection.RIGHT.GetAntiClockwiseCorner() == HexCorner.TOP_RIGHT);
	}

	[Test]
	public static void TestOpposites() {
		Assert.IsTrue(HexDirection.TOP_RIGHT.GetOppositeDirection() == HexDirection.BOTTOM_LEFT);
		Assert.IsTrue(HexDirection.RIGHT.GetOppositeDirection() == HexDirection.LEFT);
		Assert.IsTrue(HexDirection.BOTTOM_RIGHT.GetOppositeDirection() == HexDirection.TOP_LEFT);

		Assert.IsTrue(HexCorner.TOP.GetOppositeCorner() == HexCorner.BOTTOM);
		Assert.IsTrue(HexCorner.TOP_RIGHT.GetOppositeCorner() == HexCorner.BOTTOM_LEFT);
		Assert.IsTrue(HexCorner.BOTTOM_RIGHT.GetOppositeCorner() == HexCorner.TOP_LEFT);
	}

	[Test]
	public static void TestSideCornerRotation() {
		Assert.IsTrue(HexDirection.TOP_RIGHT.Rotate(HexRotation.LEFT) == HexDirection.TOP_LEFT);
		Assert.IsTrue(HexDirection.TOP_RIGHT.Rotate(HexRotation.RIGHT) == HexDirection.RIGHT);

		Assert.IsTrue(HexDirection.RIGHT.GetCorner(HexRotation.LEFT) == HexCorner.TOP_RIGHT);
		Assert.IsTrue(HexDirection.RIGHT.GetCorner(HexRotation.RIGHT) == HexCorner.BOTTOM_RIGHT);

		Assert.IsTrue(HexCorner.TOP_RIGHT.Rotate(HexRotation.LEFT) == HexCorner.TOP);
		Assert.IsTrue(HexCorner.TOP_RIGHT.Rotate(HexRotation.RIGHT) == HexCorner.BOTTOM_RIGHT);

		Assert.IsTrue(HexCorner.BOTTOM_LEFT.GetSide(HexRotation.LEFT) == HexDirection.BOTTOM_LEFT);
		Assert.IsTrue(HexCorner.BOTTOM_LEFT.GetSide(HexRotation.RIGHT) == HexDirection.LEFT);
	}

	private static bool IsSameAngle(float angle1, float angle2) {
		return Mathf.Approximately(0.0f, Mathf.DeltaAngle(angle1, angle2));
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator HexTestWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
