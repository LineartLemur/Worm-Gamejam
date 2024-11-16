using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Freya;
using UnityEngine.Profiling;

public class GeometryUtil : MonoBehaviour
{
	public static float GetSquareDistanceLineToPoint3D(Ray line, Vector3 point) {
		return (GetClosestPointOnLineToPoint3D(line,point) - point).sqrMagnitude;
	}

	public static float GetSquareDistanceLineToPoint3D(Vector3 lineStart, Vector3 lineDirection, Vector3 point) {
		return (GetClosestPointOnLineToPoint3D(lineStart,lineDirection,point) - point).sqrMagnitude;
	}
	public static Vector3 GetClosestPointOnLineToPoint3D(Ray line, Vector3 point) {
		return GetClosestPointOnLineToPoint3D(line.origin, line.direction, point);
	}
	public static Vector3 GetClosestPointOnLineToPoint3D(Vector3 lineStart, Vector3 lineDirection, Vector3 point) {
		Vector3 pointDirection = point - lineStart;

		// Calculate the projection of the point onto the line
		float projection = Vector3.Dot(pointDirection, lineDirection.normalized);

		// Calculate the closest point on the line to the given point
		return lineStart + lineDirection.normalized * projection;
	}
	
	public static float GetDistanceLineToPoint(Vector2 p1, Vector2 p2, Vector2 p)
	{
		return GetDistanceLineToCircle(p1, p2, p);
	}
	public static float GetDistanceLineToCircle(Vector2 p1, Vector2 p2, Vector2 center)
	{

		// compute the closest u vector to our sphere
		Vector2 p3 = center;
		float u = ((p3.x - p1.x) * (p2.x - p1.x) + (p3.y - p1.y) * (p2.y - p1.y)) / (p2 - p1).sqrMagnitude;

		// clip to [0,1], we only consider the line segment
		if (u < 0.0f) u = 0.0f;
		if (u > 1.0f) u = 1.0f;

		// we calculate the location and distance of the projected point
		Vector2 proj = new Vector2(p1.x + u * (p2.x - p1.x), p1.y + u * (p2.y - p1.y));
		float distance = (proj - center).magnitude;
		return distance;
	}


	// project a point on a line
	public static Vector2 ProjectPointOnLine(Vector2 p1, Vector2 p2, Vector2 p)
	{
		// http://paulbourke.net/geometry/pointline/

		Vector2 p3 = p;
		float u = ((p3.x - p1.x) * (p2.x - p1.x) + (p3.y - p1.y) * (p2.y - p1.y)) / (p2 - p1).sqrMagnitude;
		Vector2 proj = new Vector2(p1.x + u * (p2.x - p1.x), p1.y + u * (p2.y - p1.y));
		return proj;
	}

	// get the t-value of the point on this line (after projection)
	public static float GetLineT(Vector2 p1, Vector2 p2, Vector2 p)
	{
		Vector2 proj = ProjectPointOnLine(p1, p2, p);
		if (Mathf.Abs(p1.x - p2.x) > Mathf.Abs(p1.y - p2.y))
		{
			return (proj.x - p1.x) / (p2.x - p1.x);
		}
		else
		{
			return (proj.y - p1.y) / (p2.y - p1.y);
		}
	}

	// project a point onto a plane
	public static Vector3 ProjectPointOnPlane(Vector3 pointOnPlane, Vector3 planeNormal, Vector3 point)
	{
		// http://stackoverflow.com/questions/8942950/how-do-i-find-the-orthogonal-projection-of-a-point-onto-a-plane
		Vector3 q = point;
		Vector3 p = pointOnPlane;
		Vector3 n = planeNormal;
		Vector3 proj = q - Vector3.Dot(q - p, n) * n;
		return proj;
	}

	public static float GetLineIntersection(Vector2 p0, Vector2 p1, Vector2 q0, Vector2 q1, ref float t0, ref float t1)
	{
		// http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect

		// calculate the direction vectors
		Vector2 r = p1 - p0;
		Vector2 s = q1 - q0;
		//Logger.Log ("Calculate intersection between " + p0.ToString ("F4") + " -> " + p1.ToString ("F4") + " and " + q0.ToString ("F4") + " -> " + q1.ToString ("F4"));

		// calculate the 2D cross product between the dir vectors
		float dirCross = Cross2D(r, s);
		//Logger.Log("Dir cross is " + dirCross);

		// no intersection - they are colinear or parallel
		if (Mathf.Abs(dirCross) < Mathf.Epsilon)
		{
			t0 = float.PositiveInfinity;
			t1 = float.PositiveInfinity;
			return t0;
		}

		// calculate the u-value
		t0 = Cross2D(q0 - p0, s) / dirCross;
		t1 = Cross2D(q0 - p0, r) / dirCross;
		//Logger.Log ("We calculate t0 and t1 as " + t0 + " and " + t1);
		return t0;
	}

	// returns t0 for the line p0 -> p1
	public static bool GetLineSegmentIntersection(Vector2 p0, Vector2 p1, Vector2 q0, Vector2 q1) {
		float t0 =0, t1=0;
		return GetLineSegmentIntersection(p0, p1, q0, q1, ref t0, ref t1);
	}
	public static bool GetLineSegmentIntersection(Vector2 p0, Vector2 p1, Vector2 q0, Vector2 q1, ref float t0, ref float t1)
	{
		GetLineIntersection(p0, p1, q0, q1, ref t0, ref t1);
		if (t0 < 0.0f || t0 > 1.0f || t1 < 0.0f || t1 > 1.0f) return false;
		return true;
	}

	public static bool GetLinePlaneIntersection(Vector3 planePoint, Vector3 planeNormal, Vector3 linePoint0, Vector3 linePoint1, out Vector3 intersectionPoint)
	{
		Vector3 lineDirection = (linePoint1 - linePoint0).normalized;
		intersectionPoint = Vector3.zero;

		// parallel
		if (Vector3.Dot(planeNormal, lineDirection) == 0)
		{
			return false;
		}

		float distance = (Vector3.Dot(planeNormal, planePoint) - Vector3.Dot(planeNormal, linePoint0)) / Vector3.Dot(planeNormal, lineDirection);
		intersectionPoint = linePoint0 + lineDirection * distance;
		return true;
	}

	public static bool GetLineSegmentPlaneIntersection(Vector3 planePoint, Vector3 planeNormal, Vector3 linePoint0, Vector3 linePoint1, out Vector3 intersectionPoint)
	{
		Vector3 lineDirection = (linePoint1 - linePoint0).normalized;
		float lineLength = (linePoint1 - linePoint0).magnitude;
		intersectionPoint = Vector3.zero;

		///Debug.Log("Check " + linePoint0 + " -> " + linePoint1 + " against normal " + planeNormal + " in dir " + lineDirection);

		// parallel
		if (Vector3.Dot(planeNormal, lineDirection) == 0)
		{
			return false;
		}

		float distance = (Vector3.Dot(planeNormal, planePoint) - Vector3.Dot(planeNormal, linePoint0)) / Vector3.Dot(planeNormal, lineDirection);
		//Debug.Log("Distance from " + linePoint0 + " to " + linePoint1 + " is " + distance + " / " + lineLength + " with lineDirection " + lineDirection + " and intersectionPoint " + (linePoint0 + lineDirection * distance));
		if (distance < 0.0f || distance > lineLength) return false;
		intersectionPoint = linePoint0 + lineDirection * distance;
		return true;
	}

	public static bool GetLineSegmentTriangleIntersection(Vector3 linePoint0, Vector3 linePoint1, Vector3 trianglePoint0, Vector3 trianglePoint1, Vector3 trianglePoint2, out Vector3 intersectionPoint)
	{
		//Debug.Log("Match line " + linePoint0 + " -> " + linePoint1 + " with triangle " + trianglePoint0 + ", " + trianglePoint1 + ", " + trianglePoint2);

		intersectionPoint = Vector3.zero;
		const float EPSILON = 0.00001f;
		Vector3 rayVector = (linePoint1 - linePoint0).normalized;
		float rayLength = Vector3.Distance(linePoint0, linePoint1);
		Vector3 vertex0 = trianglePoint0;
		Vector3 vertex1 = trianglePoint1;
		Vector3 vertex2 = trianglePoint2;
		Vector3 edge1, edge2, h, s, q;
		float a, f, u, v;
		edge1 = vertex1 - vertex0;
		edge2 = vertex2 - vertex0;
		h = Vector3.Cross(rayVector, edge2);
		a = Vector3.Dot(edge1, h);
		if (a > -EPSILON && a < EPSILON)
		{
			//Debug.Log("Parallel!");
			return false;    // This ray is parallel to this triangle.
		}

		f = 1.0f / a;
		s = linePoint0 - vertex0;
		u = f * Vector3.Dot(s, h);
		if (u < EPSILON || u > 1.0 - EPSILON)
		{
			//Debug.Log("Out of triangle!");
			return false;
		}

		q = Vector3.Cross(s, edge1);
		v = f * Vector3.Dot(rayVector, q);
		if (v < EPSILON || u + v > 1.0 - EPSILON)
		{
			//Debug.Log("Also out of triangle!");
			return false;
		}

		// At this stage we can compute t to find out where the intersection point is on the line.
		float t = f * Vector3.Dot(edge2, q);

		//Debug.Log("t is " + t);
		if (t > EPSILON && t < rayLength - EPSILON) // ray intersection
		{
			intersectionPoint = linePoint0 + rayVector * t;
			//Debug.Log("Intersection found between line " + linePoint0 + " -> " + linePoint1 + " with triangle " + trianglePoint0 + ", " + trianglePoint1 + ", " + trianglePoint2);
			//Debug.Log("Intersection at " + intersectionPoint + ", t " + t + ", u " + u + ", v " + v + ", u+v " + (u+v) + ", a " + a);
			return true;
		}
		else // This means that there is a line intersection but not a ray intersection.
		{
			//Debug.Log("t is " + t + " so no intersection");
			return false;
		}
	}

	public static bool GetLineSegment3DSquareIntersection(Vector3 squareCenter, Vector3 squareNormal, Vector3 squareUp, Vector2 squareSize, Vector3 linePoint0, Vector3 linePoint1, out Vector3 intersectionPoint)
	{

		// first, make sure we hit the plane that contains the square, and figure out where we hit it
		bool hitFound = GetLineSegmentPlaneIntersection(squareCenter, squareNormal, linePoint0, linePoint1, out intersectionPoint);
		if (!hitFound) return false;

		// now make sure we actually hit the square!

		// transform the point to the [x,y] plane, so that we can just check the x/y coordinates to make sure it fits the square
		Vector3 relativePoint = intersectionPoint - squareCenter;


		Quaternion q = Quaternion.Inverse(Quaternion.LookRotation(squareNormal, squareUp));
		Vector3 flatPoint = q * relativePoint;
		//Debug.Log("Square center is " + squareCenter + ", intersection point is " + intersectionPoint + ", relative point is " + relativePoint + ", so flat point is " + flatPoint + ", size is " + squareSize);

		if (flatPoint.x < -squareSize.x * 0.5f || flatPoint.x > squareSize.x * 0.5f || flatPoint.y < -squareSize.y * 0.5f || flatPoint.y > squareSize.y * 0.5f) return false;
		return true;
	}


	// return whether the line hit the box, and set t0 [0,1] from p0 to p1 and set the normal of the collision
	public static bool GetLineSquareIntersection(Vector2 p0, Vector2 p1, Vector2 bottomLeft, Vector2 topRight, ref float t, ref Vector2 normal)
	{
		float t0 = 0, t1 = 0, t2 = 0, t3 = 0;
		float tx = 0.0f;
		bool t0hit, t1hit, t2hit, t3hit;
		t0hit = GetLineSegmentIntersection(p0, p1, bottomLeft, new Vector2(bottomLeft.x, topRight.y), ref t0, ref tx);
		t1hit = GetLineSegmentIntersection(p0, p1, bottomLeft, new Vector2(topRight.x, bottomLeft.y), ref t1, ref tx);
		t2hit = GetLineSegmentIntersection(p0, p1, new Vector2(bottomLeft.x, topRight.y), topRight, ref t2, ref tx);
		t3hit = GetLineSegmentIntersection(p0, p1, new Vector2(topRight.x, bottomLeft.y), topRight, ref t3, ref tx);
		//Logger.Log ("Line " + p0.ToString ("F4") + " to " + p1.ToString ("F4") + " hitting " + bottomLeft + " to " + topRight + " has hits: " + t0hit + "," + t1hit + "," + t2hit + "," + t3hit + ", with t-values " + t0 + "," + t1 + "," + t2 + "," + t3);

		// now we determine the closest hit
		if (t0hit && (t0 <= t1 || !t1hit) && (t0 <= t2 || !t2hit) && (t0 <= t3 || !t3hit))
		{
			normal = new Vector2(-1.0f, 0.0f);
			t = t0;
			return true;
		}
		else if (t1hit && (t1 <= t0 || !t0hit) && (t1 <= t2 || !t2hit) && (t1 <= t3 || !t3hit))
		{
			normal = new Vector2(0.0f, -1.0f);
			t = t1;
			return true;
		}
		else if (t2hit && (t2 <= t0 || !t0hit) && (t2 <= t1 || !t1hit) && (t2 <= t3 || !t3hit))
		{
			normal = new Vector2(0.0f, 1.0f);
			t = t2;
			return true;
		}
		else if (t3hit && (t3 <= t0 || !t0hit) && (t3 <= t1 || !t1hit) && (t3 <= t2 || !t2hit))
		{
			normal = new Vector2(1.0f, 0.0f);
			t = t3;
			return true;
		}
		else
		{
			return false;
		}
	}

	public static bool IsSquareSquareIntersection(Vector2 bottomLeft0, Vector2 topRight0, Vector2 bottomLeft1, Vector2 topRight1)
	{
		if (topRight0.x < bottomLeft1.x) return false; // a is left of b
		if (bottomLeft0.x > topRight1.x) return false; // a is right of b
		if (topRight0.y < bottomLeft1.y) return false; // a is above b
		if (bottomLeft0.y > topRight1.y) return false; // a is below b
		return true; // boxes overlap
	}

	public static bool IsPointInSquare(float minX, float minY, float maxX, float maxY, float x, float y)
	{
		return (minX <= x && x <= maxX && minY <= y && y <= maxY);
	}

	private static float Cross2D(Vector2 v, Vector2 w)
	{
		return v.x * w.y - v.y * w.x;
	}

	public static bool MatchLineToCircle(Vector2 p1, Vector2 p2, Vector2 center, float radius)
	{
		return GetDistanceLineToCircle(p1, p2, center) <= radius;
	}

	public static List<Vector2Int> GetPixelsInCircle(Vector2 p, float radius)
	{
		List<Vector2Int> pixels = new List<Vector2Int>();
		GetPixelsInCircleNonAlloc(p, radius, ref pixels);
		return pixels;
	}

	public static void GetPixelsInCircleNonAlloc(Vector2 p, float radius, ref List<Vector2> pixels)
	{
		int minX = Mathf.FloorToInt(p.x - radius);
		int maxX = Mathf.CeilToInt(p.x + radius);
		int minY = Mathf.FloorToInt(p.y - radius);
		int maxY = Mathf.CeilToInt(p.y + radius);
		pixels.Clear();
		float radiusSqr = (radius + 0.5f) * (radius + 0.5f);
		for (int x = minX; x <= maxX; ++x)
		{
			for (int y = minY; y <= maxY; ++y)
			{
				float d = (p.x - (x + 0.5f)) * (p.x - (x + 0.5f)) + (p.y - (y + 0.5f)) * (p.y - (y + 0.5f));
				//float d = (p - new Vector2(x+0.5f, y+0.5f)).sqrMagnitude;
				if (d < radiusSqr) pixels.Add(new Vector2(x, y));
				/*if (matchLineToCircle(new Vector2(x, y), new Vector2(x+1, y), p, radius)) pixels.Add(new Vector2(x, y));
				else if (matchLineToCircle(new Vector2(x, y), new Vector2(x, y+1), p, radius)) pixels.Add(new Vector2(x, y));
				else if (matchLineToCircle(new Vector2(x+1, y), new Vector2(x+1, y+1), p, radius)) pixels.Add(new Vector2(x, y));
				else if (matchLineToCircle(new Vector2(x, y+1), new Vector2(x+1, y+1), p, radius)) pixels.Add(new Vector2(x, y));*/
			}
		}
	}

	public static void GetPixelsInCircleNonAlloc(Vector2 p, float radius, ref List<Vector2Int> pixels)
	{
		var minX = Mathf.FloorToInt(p.x - radius);
		var maxX = Mathf.CeilToInt(p.x + radius);
		var minY = Mathf.FloorToInt(p.y - radius);
		var maxY = Mathf.CeilToInt(p.y + radius);
		pixels.Clear();
		for (int x = minX; x <= maxX; ++x)
		{
			for (int y = minY; y <= maxY; ++y)
			{
				if (MatchLineToCircle(new Vector2(x, y), new Vector2(x + 1, y), p, radius)) pixels.Add(new Vector2Int(x, y));
				else if (MatchLineToCircle(new Vector2(x, y), new Vector2(x, y + 1), p, radius)) pixels.Add(new Vector2Int(x, y));
				else if (MatchLineToCircle(new Vector2(x + 1, y), new Vector2(x + 1, y + 1), p, radius)) pixels.Add(new Vector2Int(x, y));
				else if (MatchLineToCircle(new Vector2(x, y + 1), new Vector2(x + 1, y + 1), p, radius)) pixels.Add(new Vector2Int(x, y));
			}
		}
	}

	public static void GetPixelsInPolygonNonAlloc(IList<Vector2> polygon,  List<Vector2Int> pixels,  List<int> cachedI, float scale = 1)
	{
		Profiler.BeginSample("GetPixelsInPolygonNonAlloc");
		
		// Debug.Log($"===========================");
		var minX = Mathf.CeilToInt(polygon.Min(e=>e.x)/scale);
		var maxX = Mathf.FloorToInt(polygon.Max(e=>e.x)/scale);
		var minY = Mathf.CeilToInt(polygon.Min(e=>e.y)/scale);
		var maxY = Mathf.FloorToInt(polygon.Max(e=>e.y)/scale);

		var minYf = (minY-2)*scale;
		
		pixels.Clear();
		int length = polygon.Count;
		for (int x = minX; x <= maxX; ++x) {
			var xf = x * scale;
			cachedI.Clear();
			for (int i = 0; i < polygon.Count; i++) {
				if ((xf - polygon[i].x) * (polygon[(i + 1) % length].x - xf) > 0) {// if x is between line
					cachedI.Add(i);
				}
			}
			
			for (int y = minY; y <= maxY; ++y) {
				var yf = y * scale;
				int n = 0;
				foreach (var i in cachedI) {
					if(GetLineSegmentIntersection(polygon[i], polygon[(i + 1) % length], new Vector2(xf, minYf),new Vector2(xf, yf))) n++;
				}
				
				if(n%2==1) pixels.Add(new Vector2Int(x, y));
			}
		}
		
		Profiler.EndSample();
	}
	public static bool IsPointInPolygon( IReadOnlyCollection<Vector2> polygon, Vector2 point ) {
		return WindingNumber(polygon, point) != 0;
	}

	public static float Determinant /*or Cross*/( Vector2 a, Vector2 b ) => a.x * b.y - a.y * b.x; // 2D "cross product"
	
	public static int SignWithZero( float value, float epsilon = 0.000001f ) => (int)(Mathf.Abs( value ) < epsilon ? 0 : Mathf.Sign( value ));
	private static float IsLeft( Vector2 a, Vector2 b, Vector2 p ) => SignWithZero( Determinant( p-a, b-a) );
	public static int WindingNumber( IReadOnlyCollection<Vector2> polygon, Vector2 point ) {
		
		Profiler.BeginSample("WindingNumber");
		int winding = 0;

		Profiler.BeginSample("sides");
		foreach (var (a,b) in polygon.Sides()) {
			if( a.y <= point.y ) {
				if( b.y > point.y && IsLeft( a, b, point ) > 0 )
					winding--;
			} else {
				if( b.y <= point.y && IsLeft( a, b, point ) < 0 )
					winding++;
			}
		}
		Profiler.EndSample();

		Profiler.EndSample();
		return winding;
	}

	public static float GetDistanceBoxToCircle(Vector2 boxLoc, Vector2 boxSize, Vector2 center, float radius)
	{
		float minDistance = GetDistanceBoxToPoint(boxLoc, boxSize, center);
		return Mathf.Max(minDistance - radius, 0.0f); // we are in the circle radius
	}
	public static float GetDistanceBoxToPoint(Vector2 boxLoc, Vector2 boxSize, Vector2 center)
	{
		// we are INSIDE the box - always min distance!
		if (boxLoc.x <= center.x && center.x <= boxLoc.x + boxSize.x && boxLoc.y <= center.y && center.y <= boxLoc.y + boxSize.y) return 0.0f;
		var minDistance = float.MaxValue;

		/**
		 * WHAT THE FUCK, THIS SHIT WAS ALL WRONG???
		 * */
		/*if (center.y < boxLoc.y) minDistance = Mathf.Min(minDistance, getDistanceLineToCircle(boxLoc, boxLoc + new Vector2(boxSize.x, 0), center));
		if (center.y > boxLoc.y+boxSize.y) minDistance = Mathf.Min(minDistance, getDistanceLineToCircle(boxLoc, boxLoc + new Vector2(0, boxSize.y), center));
		if (center.x < boxLoc.x) minDistance = Mathf.Min(minDistance, getDistanceLineToCircle(boxLoc + new Vector2(boxSize.x, 0), boxLoc + boxSize, center));
		if (center.x > boxLoc.x+boxSize.x) minDistance = Mathf.Min(minDistance, getDistanceLineToCircle(boxLoc + new Vector2(0, boxSize.y), boxLoc + boxSize, center));*/

		// good version???
		if (center.y < boxLoc.y) minDistance = Mathf.Min(minDistance, GetDistanceLineToPoint(boxLoc, boxLoc + new Vector2(boxSize.x, 0), center));
		if (center.y > boxLoc.y + boxSize.y) minDistance = Mathf.Min(minDistance, GetDistanceLineToPoint(boxLoc + new Vector2(0, boxSize.y), boxLoc + boxSize, center));
		if (center.x < boxLoc.x) minDistance = Mathf.Min(minDistance, GetDistanceLineToPoint(boxLoc, boxLoc + new Vector2(0, boxSize.y), center));
		if (center.x > boxLoc.x + boxSize.x) minDistance = Mathf.Min(minDistance, GetDistanceLineToPoint(boxLoc + new Vector2(boxSize.x, 0), boxLoc + boxSize, center));
		return minDistance;
	}

	public static List<Vector2Int> GetPixelsOnLine(Vector2 p1, Vector2 p2)
	{
		List<Vector2Int> pixels = new List<Vector2Int>();
		CalculatePixelsOnLine(p1, p2, pixels);
		return pixels;
	}

	public static int CalculatePixelsOnLine(Vector2 p1, Vector2 p2, List<Vector2Int> pixels)
	{
		// modified version of Bresenham's algorithm
		// http://en.wikipedia.org/wiki/Bresenham%27s_line_algorithm
		// http://www.idav.ucdavis.edu/education/GraphicsNotes/Bresenhams-Algorithm.pdf
		// http://www.idav.ucdavis.edu/education/GraphicsNotes/Bresenhams-Algorithm/Bresenhams-Algorithm.html

		// pixels
		//List<Vector2Int> pixels = new List<Vector2Int>();

		// x,y
		float x1 = p1.x; float y1 = p1.y;
		float x2 = p2.x; float y2 = p2.y;

		// both are too small - just return the original pixel
		int nPixels = 0;
		if (Mathf.Abs(p1.x - p2.x) < Mathf.Epsilon && Mathf.Abs(p1.y - p2.y) < Mathf.Epsilon)
		{
			SetPixelNonAlloc(pixels, nPixels++, Mathf.FloorToInt(x1), Mathf.FloorToInt(y1));
			return nPixels;
		}

		// bresenham implementation
		// only works for left to right, x driving axis, so we swap some stuff up front

		// x must be the driving axis
		bool steep = Mathf.Abs(y2 - y1) > Mathf.Abs(x2 - x1);
		if (steep)
		{
			//swap(x1, y1);
			float tmp = x1;
			x1 = y1;
			y1 = tmp;
			//swap(x2, y2);
			tmp = x2;
			x2 = y2;
			y2 = tmp;
		}

		// from left to right
		if (x2 < x1)
		{
			//swap(x1, x2);
			float tmp = x1;
			x1 = x2;
			x2 = tmp;
			//swap(y1, y2);
			tmp = y1;
			y1 = y2;
			y2 = tmp;
		}

		float dx = (x2 - x1);
		float dy = (y2 - y1);
		float m = dy / dx;
		int y = Mathf.FloorToInt(y1);
		if (!steep)
		{
			SetPixelNonAlloc(pixels, nPixels++, Mathf.FloorToInt(x1), Mathf.FloorToInt(y1));
		}
		else
		{
			SetPixelNonAlloc(pixels, nPixels++, Mathf.FloorToInt(y1), Mathf.FloorToInt(x1));
		}
		//pixels.Add(!steep ? new Vector2Int(Mathf.FloorToInt(x1), Mathf.FloorToInt(y1)) : new Vector2Int(Mathf.FloorToInt(y1), Mathf.FloorToInt(x1)));
		int prevY;
		int cx1 = Mathf.CeilToInt(x1);
		int fx2 = Mathf.FloorToInt(x2);
		for (int x = cx1; x < fx2; ++x)
		{

			// compute the y coordinate at this spot
			prevY = y;
			y = Mathf.FloorToInt(Mathf.FloorToInt(y1 + m * (x - x1)));

			// fill in the pixels to the left and right of this spot
			if (!steep)
			{
				if (prevY != y) SetPixelNonAlloc(pixels, nPixels++, Mathf.FloorToInt(Mathf.FloorToInt(x - 0.5f)), y);
				SetPixelNonAlloc(pixels, nPixels++, Mathf.FloorToInt(Mathf.FloorToInt(x + 0.5f)), y);
				/*if (prevY != y) pixels.Add(new Vector2Int(Mathf.FloorToInt(Mathf.FloorToInt(x-0.5f)), y));
				pixels.Add(new Vector2Int(Mathf.FloorToInt(Mathf.FloorToInt(x+0.5f)), y));*/
			}
			else
			{
				if (prevY != y) SetPixelNonAlloc(pixels, nPixels++, y, Mathf.FloorToInt(Mathf.FloorToInt(x - 0.5f)));
				SetPixelNonAlloc(pixels, nPixels++, y, Mathf.FloorToInt(Mathf.FloorToInt(x + 0.5f)));
				/*if (prevY != y) pixels.Add(new Vector2Int(y, Mathf.FloorToInt(Mathf.FloorToInt(x-0.5f))));
				pixels.Add(new Vector2Int(y, Mathf.FloorToInt(Mathf.FloorToInt(x+0.5f))));*/
			}
		}
		if (pixels[nPixels - 1].x != Mathf.FloorToInt(x2) || pixels[nPixels - 1].y != Mathf.FloorToInt(y2))
		{
			if (!steep) SetPixelNonAlloc(pixels, nPixels++, Mathf.FloorToInt(x2), Mathf.FloorToInt(y2));
			else SetPixelNonAlloc(pixels, nPixels++, Mathf.FloorToInt(y2), Mathf.FloorToInt(x2));
		}
		//if (pixels[pixels.Count-1].x != Mathf.FloorToInt(x2) || pixels[pixels.Count-1].y != Mathf.FloorToInt(y2)) pixels.Add(!steep ? new Vector2Int(Mathf.FloorToInt(x2), Mathf.FloorToInt(y2)) : new Vector2Int(Mathf.FloorToInt(y2), Mathf.FloorToInt(x2)));

		//if (Mathf.FloorToInt(y2) != y) pixels.Add(!steep ? new Vector2Int(Mathf.FloorToInt(x2), Mathf.FloorToInt(y2)) : new Vector2Int(Mathf.FloorToInt(y2), Mathf.FloorToInt(x2)));
		/*string ss = "";
		bool first = true;
		foreach (Vector2Int pixel in pixels) {
			if (first) first = false;
			else ss += " -> ";
			ss += pixel;
		}
		Logger.Log ("Trace: " + ss);*/
		// return pixels
		//return pixels;
		return nPixels;
	}

	private static void SetPixelNonAlloc(List<Vector2Int> pixels, int idx, int x, int y)
	{
		while (idx > pixels.Count - 1) pixels.Add(Vector2Int.zero);
		pixels[idx] = new Vector2Int(x, y);
	}

	public static float ClampAngle(float angleRad, float minAngle, float maxAngle)
	{
		float angle = angleRad;
		while (minAngle > maxAngle) minAngle -= Mathf.PI * 2.0f; // make sure minAngle is lower than maxAngle
		while (minAngle < maxAngle - Mathf.PI * 2.0f) minAngle += Mathf.PI * 2.0f; // minAngle now lies in [maxAngle - 2pi, maxAngle]
		while (angle > maxAngle) angle -= Mathf.PI * 2.0f; // angle now lies in [-inf, maxAngle]
		while (angle <= minAngle) angle += Mathf.PI * 2.0f; // angle now lies in ]minAngle, minAngle + 2pi]
		if (angle > maxAngle)
		{ // this means the angle is actually bigger than maxAngle now - we are not in range
			if (angle - maxAngle <= Mathf.PI) return maxAngle;
			else return minAngle;
		}
		return angle;
	}

	public static float RoundAngle(float angle, float angleIncrements)
	{
		// convert to [0, 2PI]
		angle = ClampAngle(angle, 0.0f, 2 * Mathf.PI);

		// round off
		return Mathf.RoundToInt(angle / angleIncrements) * angleIncrements;
	}

	public static bool IsBetweenAngles(float angle, float minAngle, float maxAngle)
	{
		// not 100% sure this actually works?
		if (Mathf.DeltaAngle(Mathf.Rad2Deg * minAngle, Mathf.Rad2Deg * maxAngle) < 0.0f)
		{
			float swapAngle = minAngle;
			minAngle = maxAngle;
			maxAngle = swapAngle;
		}
		float dMinAngle = Mathf.DeltaAngle(Mathf.Rad2Deg * minAngle, Mathf.Rad2Deg * angle);
		float dMaxAngle = Mathf.DeltaAngle(Mathf.Rad2Deg * angle, Mathf.Rad2Deg * maxAngle);
		return (dMinAngle >= 0.0f) && (dMaxAngle >= 0.0f);
	}

	public static void GenerateTangents(Mesh theMesh)
	{
		int vertexCount = theMesh.vertexCount;
		Vector3[] vertices = theMesh.vertices;
		Vector3[] normals = theMesh.normals;
		Vector2[] texcoords = theMesh.uv;
		int[] triangles = theMesh.triangles;
		int triangleCount = triangles.Length / 3;
		Vector4[] tangents = new Vector4[vertexCount];
		Vector3[] tan1 = new Vector3[vertexCount];
		Vector3[] tan2 = new Vector3[vertexCount];
		int tri = 0;
		for (int i = 0; i < (triangleCount); i++)
		{
			int i1 = triangles[tri];
			int i2 = triangles[tri + 1];
			int i3 = triangles[tri + 2];

			Vector3 v1 = vertices[i1];
			Vector3 v2 = vertices[i2];
			Vector3 v3 = vertices[i3];

			Vector2 w1 = texcoords[i1];
			Vector2 w2 = texcoords[i2];
			Vector2 w3 = texcoords[i3];

			float x1 = v2.x - v1.x;
			float x2 = v3.x - v1.x;
			float y1 = v2.y - v1.y;
			float y2 = v3.y - v1.y;
			float z1 = v2.z - v1.z;
			float z2 = v3.z - v1.z;

			float s1 = w2.x - w1.x;
			float s2 = w3.x - w1.x;
			float t1 = w2.y - w1.y;
			float t2 = w3.y - w1.y;

			float r = 1.0f / (s1 * t2 - s2 * t1);
			Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
			Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

			tan1[i1] += sdir;
			tan1[i2] += sdir;
			tan1[i3] += sdir;

			tan2[i1] += tdir;
			tan2[i2] += tdir;
			tan2[i3] += tdir;

			tri += 3;
		}

		for (int i = 0; i < (vertexCount); i++)
		{
			Vector3 n = normals[i];
			Vector3 t = tan1[i];

			// Gram-Schmidt orthogonalize
			Vector3.OrthoNormalize(ref n, ref t);

			tangents[i].x = t.x;
			tangents[i].y = t.y;
			tangents[i].z = t.z;

			// Calculate handedness
			tangents[i].w = (Vector3.Dot(Vector3.Cross(n, t), tan2[i]) < 0.0) ? -1.0f : 1.0f;
		}
		theMesh.tangents = tangents;
	}


	public static float MoveTowardsAngleButAvoidCrossing(float currentAngle, float targetAngle, float rotateSpeed, float forbiddenAngle)
	{
		float deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);
		float forbiddenDeltaAngle = Mathf.DeltaAngle(currentAngle, forbiddenAngle);
		//Debug.Log("deltaAngle is " + deltaAngle + ", forbiddenDeltaAngle is " + forbiddenDeltaAngle);
		if (deltaAngle * forbiddenDeltaAngle > 0.0f && Mathf.Abs(forbiddenDeltaAngle) < Mathf.Abs(deltaAngle))
		{
			// same sign - which means we are moving over the forbidden angle towards our goal - NOT GOOD!
			deltaAngle = -deltaAngle;
			//Debug.Log("TURN TO THE OTHER SIDE! to " + deltaAngle);
		}
		if (Mathf.Abs(deltaAngle) > rotateSpeed) deltaAngle = Mathf.Sign(deltaAngle) * rotateSpeed;
		return currentAngle + deltaAngle * rotateSpeed;
	}

	public static float MirrorAroundAngle(float currentAngle, float axisAngle)
	{
		float dAngle = Mathf.DeltaAngle(currentAngle * Mathf.Rad2Deg, axisAngle * Mathf.Rad2Deg);
		return (currentAngle + 2 * dAngle) * Mathf.Deg2Rad;
	}

	public static float ForceMinDistanceFromAngle(float currentAngle, float targetAngle, float minDistanceAngle)
	{
		Debug.Log("Compare " + currentAngle + " to " + targetAngle + " with minDistance " + minDistanceAngle);
		float dAngle = Mathf.DeltaAngle(currentAngle, targetAngle);
		Debug.Log("dAngle: " + dAngle);
		if (Mathf.Abs(dAngle) < minDistanceAngle) return targetAngle + Mathf.Sign(-dAngle) * minDistanceAngle;
		else return currentAngle;
	}

	public static int ConvertDirToIndex(Vector2Int dir)
	{

		// this formula converts each dir [-1,0],[1,0],[0,-1],[0,1] to a unique index in range (0,4)
		// the first term converts to [0 2 0 2]
		// the second term converts to [1 1 0 0]
		return (dir.x + dir.y + 1) + (dir.y + 1) % 2;
	}

	private static Vector2Int[] indexedDirs = new Vector2Int[]{
		new Vector2Int(-1, 0),
		new Vector2Int(1, 0),
		new Vector2Int(0, -1),
		new Vector2Int(0, 1)
	};

	public static Vector2Int ConvertIndexToDir(int idx)
	{
		return indexedDirs[idx];
	}

	public static float MoveTowardsAngleEase(float currentAngle, float targetAngle, float maxDelta, float easeDeltaAngle)
	{
		float dAngle = Mathf.DeltaAngle(currentAngle, targetAngle);
		if (Mathf.Abs(dAngle) < easeDeltaAngle)
		{
			float t = Mathf.Abs(dAngle) / easeDeltaAngle;
			maxDelta = Mathf.Lerp(maxDelta * 0.25f, maxDelta, t);
		}
		return Mathf.MoveTowardsAngle(currentAngle, targetAngle, maxDelta);
	}

	public static float InterpolateBilinear(float x, float y, float x1, float y1, float x2, float y2, float f11, float f12, float f21, float f22)
	{
		// https://en.wikipedia.org/wiki/Bilinear_interpolation
		float f = f11 * (x2 - x) * (y2 - y) + f21 * (x - x1) * (y2 - y) + f12 * (x2 - x) * (y - y1) + f22 * (x - x1) * (y - y1);
		f /= (x2 - x1) * (y2 - y1);
		return f;
	}


	public static int DivideFloor(int a, int b)
	{
		return a / b - (a % b < 0 ? 1 : 0);
	}

	public static float DeltaAngle(float current, float target)
	{
		return Mathf.DeltaAngle(current * Mathf.Rad2Deg, target * Mathf.Rad2Deg) * Mathf.Deg2Rad;
	}

	public static float AbsDeltaAngle(float current, float target)
	{
		return Mathf.Abs(DeltaAngle(current, target));
	}

	public static float MoveTowardsAngle(float current, float target, float maxDelta)
	{
		return Mathf.MoveTowardsAngle(current * Mathf.Rad2Deg, target * Mathf.Rad2Deg, maxDelta * Mathf.Rad2Deg) * Mathf.Deg2Rad;
	}

	public static float MoveTowardsAngleInShortestWayPossible(float current, float target, float maxDelta)
	{
		return Mathf.MoveTowardsAngle(current * Mathf.Rad2Deg, target * Mathf.Rad2Deg, maxDelta * Mathf.Rad2Deg) * Mathf.Deg2Rad;
	}

	public static Vector3 GetBarycentricCoordinates(Vector3 p, Vector3 a, Vector3 b, Vector3 c)
	{
		// based on https://gamedev.stackexchange.com/questions/23743/whats-the-most-efficient-way-to-find-barycentric-coordinates
		b = b - a;
		c = c - a;
		p = p - a;
		a = Vector3.zero;
		float dbb = Vector3.Dot(b, b);
		float dbc = Vector3.Dot(b, c);
		float dcc = Vector3.Dot(c, c);
		float dpb = Vector3.Dot(p, b);
		float dpc = Vector3.Dot(p, c);
		float denom = dbb * dcc - dbc * dbc;
		Vector3 result = Vector3.zero;
		result.y = (dcc * dpb - dbc * dpc) / denom;
		result.z = (dbb * dpc - dbc * dpb) / denom;
		result.x = 1.0f - result.y - result.z;
		return result;
	}

  public static void SolveQuadraticEquation(float a, float b, float c, out float[] roots)
  {
    float D = b*b - 4*a*c;
    if (D < 0)
    {
      roots = new float[0];
    }
    else if (D == 0)
    {
      roots = new float[1];
      roots[0] = -b / (2 * a);
    }
    else
    {
      roots = new float[2];
      roots[0] = (-b + Mathf.Sqrt(D)) / (2 * a);
      roots[1] = (-b - Mathf.Sqrt(D)) / (2 * a);
    }
  }
  public static Vector2 InsetCorner( // assumes clockwise
	  Vector2  a,    //  previous point
	  Vector2  b,    //  current point that needs to be inset
	  Vector2  c,    // next point
	  float insetDist) {     //  amount of inset (perpendicular to each line segment)

	  

	  //  Calculate length of line segments.
	  Vector2 offset1 = (b - a).Rotate90CW().normalized*insetDist;
	  Vector2 offset2 = (c - b).Rotate90CW().normalized*insetDist;

	  a += offset1;
	  var b1 = b+ offset1;
	  var b2 = b+ offset2;
	  c += offset2;
	  if (b1 == b2) {
		  return b1;
	  }

	  float t0=0, t1=0;
	  return Vector2.LerpUnclamped(a, b1, GetLineIntersection(a, b1, b2, c, ref t0, ref t1));
	   }

  public static void CutPolygonHole(LinkedList<Vector2> outline, LinkedList<Vector2> hole) {
	  CutPolygonHole(outline, hole, e => e);
  }
  public static void CutPolygonHole<T>(LinkedList<T> outline, LinkedList<T> hole,Func<T,Vector2> select) {
	  //find the closest two nodes and connect them there.
	  //hole needs to turn opposite direction of outline.

	  var avgCenterhole = hole.Select(select).Aggregate((a, b) => a + b) / hole.Count;
	  LinkedListNode<T> closestOuter =  outline.NodeWithMin(node => (select(node) - avgCenterhole).sqrMagnitude);
	  LinkedListNode<T> closestInner = null;
	  int  f=0;
	  while (true) {
		  var tryNode = hole.NodeWithMin(node => (select(node) - select(closestOuter.Value)).sqrMagnitude);
		  if (tryNode == closestInner) break;
		  closestInner = tryNode;
		  tryNode = outline.NodeWithMin(node => (select(node) - select(closestInner.Value)).sqrMagnitude); 
		  if (tryNode == closestOuter) break;
		  closestOuter = tryNode;
		  f++;
	  }
	  Debug.Log($"Cut hole in {f} iterations");


	  outline.AddBefore(closestOuter, closestOuter.Value);
	  hole.AddBefore(closestInner, closestInner.Value);
	  outline.AddBefore(closestOuter, closestInner.Value);

	  for (var element = closestInner.NextWrapped(); element != closestInner; element = element.NextWrapped()) {
		  outline.AddBefore(closestOuter, element.Value);
	  }
  }

  
  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  public static float leftVal(Vector2 a, Vector2 b, Vector2 p) { //rhs is positive, lhs is negative
	  return (b.y-a.y) * (p.x - a.x) + (a.x -b.x) * (p.y - a.y);
  }
  public static void TriangulatePolygon(LinkedList<(Vector2 p, int i)> polygon, ref int indexOffset, int[] tris) {
	  var p = polygon.First;

	  int fuse = 0;
	  
	  // Debug.Log($"Poly {polygon.Count}");
	  while (polygon.Count > 2) {
		  if (fuse++ > polygon.Count) {
			  Debug.LogError("Fuse in triangulatioooon");
			  return; // throw new Exception("Fuse in triangulatioooon");
		  }
		  var a = p;
		  var b = a.NextWrapped();
		  var c = b.NextWrapped();

		  p = b;

		  if (leftVal(a.Value.p, c.Value.p, b.Value.p) > 0) {
			  // Debug.Log($"{fuse}: inverse");
			  continue;
		  }
		  
		  bool breakLoop = false;
		  for (var element = c.NextWrapped(); element != a; element = element.NextWrapped()) { //check points inside
			  if (leftVal(a.Value.p,c.Value.p,element.Value.p) >= 0) continue;
			  if (leftVal(c.Value.p,b.Value.p,element.Value.p) >= 0) continue;
			  if (leftVal(b.Value.p,a.Value.p,element.Value.p) >= 0) continue;
			  breakLoop = true;
			  // Debug.Log($"{fuse}: other point inside");
			  break;
		  }
		  if (breakLoop) continue;

		  // Debug.Log($"{fuse}: <color=green>Poly MADE!</color> {polygon.Count} {a.Value.i} {b.Value.i} {c.Value.i} {indexOffset}");
		  tris[indexOffset++] = a.Value.i;
		  tris[indexOffset++] = b.Value.i;
		  tris[indexOffset++] = c.Value.i;
		  p = a;
		  polygon.Remove(b);
		  fuse = 0;
	  }
  }

  // public void insetCorner(
	 //  double  a, double  b,   //  previous point
	 //  double  c, double  d,   //  current point that needs to be inset
	 //  double  e, double  f,   //  next point
	 //  double *C, double *D,   //  storage location for new, inset point
	 //  double insetDist) {     //  amount of inset (perpendicular to each line segment)
  //
	 //  double  c1=c, d1=d, c2=c, d2=d, dx1, dy1, dist1, dx2, dy2, dist2, insetX, insetY ;
  //
	 //  //  Calculate length of line segments.
	 //  dx1=c-a; dy1=d-b; dist1=sqrt(dx1*dx1+dy1*dy1);
	 //  dx2=e-c; dy2=f-d; dist2=sqrt(dx2*dx2+dy2*dy2);
  //
	 //  //  Exit if either segment is zero-length.
	 //  if (dist1==0. || dist2==0.) return;
  //
	 //  //  Inset each of the two line segments.
	 //  insetX= dy1/dist1*insetDist; a+=insetX; c1+=insetX;
	 //  insetY=-dx1/dist1*insetDist; b+=insetY; d1+=insetY;
	 //  insetX= dy2/dist2*insetDist; e+=insetX; c2+=insetX;
	 //  insetY=-dx2/dist2*insetDist; f+=insetY; d2+=insetY;
  //
	 //  //  If inset segments connect perfectly, return the connection point.
	 //  if (c1==c2 && d1==d2) {
		//   *C=c1; *D=d1; return; }
  //
	 //  //  Return the intersection point of the two inset segments (if any).
	 //  if (lineIntersection(a,b,c1,d1,c2,d2,e,f,&insetX,&insetY)) {
		//   *C=insetX; *D=insetY; }}
}
