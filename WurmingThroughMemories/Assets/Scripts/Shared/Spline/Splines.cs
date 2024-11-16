using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Curiosmos.Shared.CollectionCaching;
using UnityEngine;
namespace PepijnWillekens.Splines {


#region CatmullRomSpline
    public static class CatmullRomSpline {

        private static readonly Matrix4x4 MyMatrix = new Matrix4x4(
            new Vector4(0f, -0.5f, 1f, -0.5f ),
            new Vector4(1f, 0f, -2.5f, 1.5f ),
            new Vector4(0f, 0.5f, 2f, -1.5f ),
            new Vector4(0f, 0f, -0.5f, 0.5f ));
        private static readonly Vector4 t6_1 = new(1, 0.1666667f, 0.02777778f, 0.00462963f);
        private static readonly Vector4 t6_2 = new(1, 0.3333333f, 0.1111111f, 0.03703704f);
        private static readonly Vector4 t6_3 = new(1, 0.5f, 0.25f, 0.125f);
        private static readonly Vector4 t6_4 = new(1, 0.6666667f, 0.4444445f, 0.2962963f);
        private static readonly Vector4 t6_5 = new(1, 0.8333334f, 0.6944445f, 0.5787038f);
        private static readonly Vector4 t6_6 = new(1, 1f, 1f, 1f);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float DoTransform(Vector4 component, Vector4 powers) {
            var v = Vector4.Scale(MyMatrix * component, powers);
            return v.x + v.y + v.z + v.w;
        }

        public static void TransformPoints(LinkedList<Vector2> controlpoints, LinkedList<Vector2> result) {
            using var pointsB = CollectionPool<LinkedList<Vector2>, Vector2>.Get();

            pointsB.c.AddFirst(controlpoints.First.Value);
            for (var el = controlpoints.First.Next; el is {Next: { }}; el = el.Next) {
                pointsB.c.AddLast((el.Previous.Value + el.Value * 2 + el.Next.Value) / 4);
            }

            pointsB.c.AddLast(controlpoints.Last.Value);
            result.AddFirst(controlpoints.First.Value);

            for (var el = pointsB.c.First; el is {Next: { }}; el = el.Next) {
                var p1 = el.Previous?.Value ?? el.Value;
                var p2 = el.Value;
                var p3 = el.Next.Value;
                var p4 = el.Next.Next?.Value ?? el.Next.Value;
                foreach (var p in Get6Points(p1, p2, p3, p4)) {
                    result.AddLast(p);
                }
            }

            result.AddLast(controlpoints.Last.Value);
        }

        public static IEnumerable<Vector2> GetNPoints(int n, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);

            for (int i = 1; i <= n; i++) {
                float t = (float) i / n;
                var powers = new Vector4(1, 0);
                powers.y = powers.x * t;
                powers.z = powers.y * t;
                powers.w = powers.z * t;

                yield return new Vector2(DoTransform(pointsX, powers), DoTransform(pointsY, powers));
            }
        }


        
        public static Vector2 GetPoint(float t, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            var powers = new Vector4(1, 0);
            powers.y = powers.x * t;
            powers.z = powers.y * t;
            powers.w = powers.z * t;

            return new Vector2(DoTransform(pointsX, powers), DoTransform(pointsY, powers));
        }
        
        public static IEnumerable<Vector2> Get6Points(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            yield return new Vector2(DoTransform(pointsX, t6_1), DoTransform(pointsY, t6_1));
            yield return new Vector2(DoTransform(pointsX, t6_2), DoTransform(pointsY, t6_2));
            yield return new Vector2(DoTransform(pointsX, t6_3), DoTransform(pointsY, t6_3));
            yield return new Vector2(DoTransform(pointsX, t6_4), DoTransform(pointsY, t6_4));
            yield return new Vector2(DoTransform(pointsX, t6_5), DoTransform(pointsY, t6_5));
            yield return new Vector2(DoTransform(pointsX, t6_6), DoTransform(pointsY, t6_6));
        }

        public static void TransformPoints(LinkedList<Vector3> controlpoints, LinkedList<Vector3> result) {
            using var pointsB = CollectionPool<LinkedList<Vector3>, Vector3>.Get();

            pointsB.c.AddFirst(controlpoints.First.Value);
            for (var el = controlpoints.First.Next; el is {Next: { }}; el = el.Next) {
                pointsB.c.AddLast((el.Previous.Value + el.Value * 2 + el.Next.Value) / 4);
            }

            pointsB.c.AddLast(controlpoints.Last.Value);
            result.AddFirst(controlpoints.First.Value);

            for (var el = pointsB.c.First; el is {Next: { }}; el = el.Next) {
                var p1 = el.Previous?.Value ?? el.Value;
                var p2 = el.Value;
                var p3 = el.Next.Value;
                var p4 = el.Next.Next?.Value ?? el.Next.Value;
                foreach (var p in Get6Points(p1, p2, p3, p4)) {
                    result.AddLast(p);
                }
            }

            result.AddLast(controlpoints.Last.Value);
        }

        public static IEnumerable<Vector3> GetNPoints(int n, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);

            for (int i = 1; i <= n; i++) {
                float t = (float) i / n;
                var powers = new Vector4(1, 0);
                powers.y = powers.x * t;
                powers.z = powers.y * t;
                powers.w = powers.z * t;

                yield return new Vector3(DoTransform(pointsX, powers), DoTransform(pointsY, powers), DoTransform(pointsZ, powers));
            }
        }


        
        public static Vector3 GetPoint(float t, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            var powers = new Vector4(1, 0);
            powers.y = powers.x * t;
            powers.z = powers.y * t;
            powers.w = powers.z * t;

            return new Vector3(DoTransform(pointsX, powers), DoTransform(pointsY, powers), DoTransform(pointsZ, powers));
        }
        
        public static IEnumerable<Vector3> Get6Points(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            yield return new Vector3(DoTransform(pointsX, t6_1), DoTransform(pointsY, t6_1), DoTransform(pointsZ, t6_1));
            yield return new Vector3(DoTransform(pointsX, t6_2), DoTransform(pointsY, t6_2), DoTransform(pointsZ, t6_2));
            yield return new Vector3(DoTransform(pointsX, t6_3), DoTransform(pointsY, t6_3), DoTransform(pointsZ, t6_3));
            yield return new Vector3(DoTransform(pointsX, t6_4), DoTransform(pointsY, t6_4), DoTransform(pointsZ, t6_4));
            yield return new Vector3(DoTransform(pointsX, t6_5), DoTransform(pointsY, t6_5), DoTransform(pointsZ, t6_5));
            yield return new Vector3(DoTransform(pointsX, t6_6), DoTransform(pointsY, t6_6), DoTransform(pointsZ, t6_6));
        }

        public static void TransformPoints(LinkedList<Vector4> controlpoints, LinkedList<Vector4> result) {
            using var pointsB = CollectionPool<LinkedList<Vector4>, Vector4>.Get();

            pointsB.c.AddFirst(controlpoints.First.Value);
            for (var el = controlpoints.First.Next; el is {Next: { }}; el = el.Next) {
                pointsB.c.AddLast((el.Previous.Value + el.Value * 2 + el.Next.Value) / 4);
            }

            pointsB.c.AddLast(controlpoints.Last.Value);
            result.AddFirst(controlpoints.First.Value);

            for (var el = pointsB.c.First; el is {Next: { }}; el = el.Next) {
                var p1 = el.Previous?.Value ?? el.Value;
                var p2 = el.Value;
                var p3 = el.Next.Value;
                var p4 = el.Next.Next?.Value ?? el.Next.Value;
                foreach (var p in Get6Points(p1, p2, p3, p4)) {
                    result.AddLast(p);
                }
            }

            result.AddLast(controlpoints.Last.Value);
        }

        public static IEnumerable<Vector4> GetNPoints(int n, Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            Vector4 pointsW = new Vector4(p1.w, p2.w, p3.w, p4.w);

            for (int i = 1; i <= n; i++) {
                float t = (float) i / n;
                var powers = new Vector4(1, 0);
                powers.y = powers.x * t;
                powers.z = powers.y * t;
                powers.w = powers.z * t;

                yield return new Vector4(DoTransform(pointsX, powers), DoTransform(pointsY, powers), DoTransform(pointsZ, powers), DoTransform(pointsW, powers));
            }
        }


        
        public static Vector4 GetPoint(float t, Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            Vector4 pointsW = new Vector4(p1.w, p2.w, p3.w, p4.w);
            var powers = new Vector4(1, 0);
            powers.y = powers.x * t;
            powers.z = powers.y * t;
            powers.w = powers.z * t;

            return new Vector4(DoTransform(pointsX, powers), DoTransform(pointsY, powers), DoTransform(pointsZ, powers), DoTransform(pointsW, powers));
        }
        
        public static IEnumerable<Vector4> Get6Points(Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            Vector4 pointsW = new Vector4(p1.w, p2.w, p3.w, p4.w);
            yield return new Vector4(DoTransform(pointsX, t6_1), DoTransform(pointsY, t6_1), DoTransform(pointsZ, t6_1), DoTransform(pointsW, t6_1));
            yield return new Vector4(DoTransform(pointsX, t6_2), DoTransform(pointsY, t6_2), DoTransform(pointsZ, t6_2), DoTransform(pointsW, t6_2));
            yield return new Vector4(DoTransform(pointsX, t6_3), DoTransform(pointsY, t6_3), DoTransform(pointsZ, t6_3), DoTransform(pointsW, t6_3));
            yield return new Vector4(DoTransform(pointsX, t6_4), DoTransform(pointsY, t6_4), DoTransform(pointsZ, t6_4), DoTransform(pointsW, t6_4));
            yield return new Vector4(DoTransform(pointsX, t6_5), DoTransform(pointsY, t6_5), DoTransform(pointsZ, t6_5), DoTransform(pointsW, t6_5));
            yield return new Vector4(DoTransform(pointsX, t6_6), DoTransform(pointsY, t6_6), DoTransform(pointsZ, t6_6), DoTransform(pointsW, t6_6));
        }

    }
#endregion

#region CardinalSpline8
    public static class CardinalSpline8 {

        private static readonly Matrix4x4 MyMatrix = new Matrix4x4(
            new Vector4(0f, -0.8f, 1.6f, -0.8f ),
            new Vector4(1f, 0f, -2.2f, 1.2f ),
            new Vector4(0f, 0.8f, 1.4f, -1.2f ),
            new Vector4(0f, 0f, -0.8f, 0.8f ));
        private static readonly Vector4 t6_1 = new(1, 0.1666667f, 0.02777778f, 0.00462963f);
        private static readonly Vector4 t6_2 = new(1, 0.3333333f, 0.1111111f, 0.03703704f);
        private static readonly Vector4 t6_3 = new(1, 0.5f, 0.25f, 0.125f);
        private static readonly Vector4 t6_4 = new(1, 0.6666667f, 0.4444445f, 0.2962963f);
        private static readonly Vector4 t6_5 = new(1, 0.8333334f, 0.6944445f, 0.5787038f);
        private static readonly Vector4 t6_6 = new(1, 1f, 1f, 1f);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float DoTransform(Vector4 component, Vector4 powers) {
            var v = Vector4.Scale(MyMatrix * component, powers);
            return v.x + v.y + v.z + v.w;
        }

        public static void TransformPoints(LinkedList<Vector2> controlpoints, LinkedList<Vector2> result) {
            using var pointsB = CollectionPool<LinkedList<Vector2>, Vector2>.Get();

            pointsB.c.AddFirst(controlpoints.First.Value);
            for (var el = controlpoints.First.Next; el is {Next: { }}; el = el.Next) {
                pointsB.c.AddLast((el.Previous.Value + el.Value * 2 + el.Next.Value) / 4);
            }

            pointsB.c.AddLast(controlpoints.Last.Value);
            result.AddFirst(controlpoints.First.Value);

            for (var el = pointsB.c.First; el is {Next: { }}; el = el.Next) {
                var p1 = el.Previous?.Value ?? el.Value;
                var p2 = el.Value;
                var p3 = el.Next.Value;
                var p4 = el.Next.Next?.Value ?? el.Next.Value;
                foreach (var p in Get6Points(p1, p2, p3, p4)) {
                    result.AddLast(p);
                }
            }

            result.AddLast(controlpoints.Last.Value);
        }

        public static IEnumerable<Vector2> GetNPoints(int n, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);

            for (int i = 1; i <= n; i++) {
                float t = (float) i / n;
                var powers = new Vector4(1, 0);
                powers.y = powers.x * t;
                powers.z = powers.y * t;
                powers.w = powers.z * t;

                yield return new Vector2(DoTransform(pointsX, powers), DoTransform(pointsY, powers));
            }
        }


        
        public static Vector2 GetPoint(float t, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            var powers = new Vector4(1, 0);
            powers.y = powers.x * t;
            powers.z = powers.y * t;
            powers.w = powers.z * t;

            return new Vector2(DoTransform(pointsX, powers), DoTransform(pointsY, powers));
        }
        
        public static IEnumerable<Vector2> Get6Points(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            yield return new Vector2(DoTransform(pointsX, t6_1), DoTransform(pointsY, t6_1));
            yield return new Vector2(DoTransform(pointsX, t6_2), DoTransform(pointsY, t6_2));
            yield return new Vector2(DoTransform(pointsX, t6_3), DoTransform(pointsY, t6_3));
            yield return new Vector2(DoTransform(pointsX, t6_4), DoTransform(pointsY, t6_4));
            yield return new Vector2(DoTransform(pointsX, t6_5), DoTransform(pointsY, t6_5));
            yield return new Vector2(DoTransform(pointsX, t6_6), DoTransform(pointsY, t6_6));
        }

        public static void TransformPoints(LinkedList<Vector3> controlpoints, LinkedList<Vector3> result) {
            using var pointsB = CollectionPool<LinkedList<Vector3>, Vector3>.Get();

            pointsB.c.AddFirst(controlpoints.First.Value);
            for (var el = controlpoints.First.Next; el is {Next: { }}; el = el.Next) {
                pointsB.c.AddLast((el.Previous.Value + el.Value * 2 + el.Next.Value) / 4);
            }

            pointsB.c.AddLast(controlpoints.Last.Value);
            result.AddFirst(controlpoints.First.Value);

            for (var el = pointsB.c.First; el is {Next: { }}; el = el.Next) {
                var p1 = el.Previous?.Value ?? el.Value;
                var p2 = el.Value;
                var p3 = el.Next.Value;
                var p4 = el.Next.Next?.Value ?? el.Next.Value;
                foreach (var p in Get6Points(p1, p2, p3, p4)) {
                    result.AddLast(p);
                }
            }

            result.AddLast(controlpoints.Last.Value);
        }

        public static IEnumerable<Vector3> GetNPoints(int n, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);

            for (int i = 1; i <= n; i++) {
                float t = (float) i / n;
                var powers = new Vector4(1, 0);
                powers.y = powers.x * t;
                powers.z = powers.y * t;
                powers.w = powers.z * t;

                yield return new Vector3(DoTransform(pointsX, powers), DoTransform(pointsY, powers), DoTransform(pointsZ, powers));
            }
        }


        
        public static Vector3 GetPoint(float t, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            var powers = new Vector4(1, 0);
            powers.y = powers.x * t;
            powers.z = powers.y * t;
            powers.w = powers.z * t;

            return new Vector3(DoTransform(pointsX, powers), DoTransform(pointsY, powers), DoTransform(pointsZ, powers));
        }
        
        public static IEnumerable<Vector3> Get6Points(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            yield return new Vector3(DoTransform(pointsX, t6_1), DoTransform(pointsY, t6_1), DoTransform(pointsZ, t6_1));
            yield return new Vector3(DoTransform(pointsX, t6_2), DoTransform(pointsY, t6_2), DoTransform(pointsZ, t6_2));
            yield return new Vector3(DoTransform(pointsX, t6_3), DoTransform(pointsY, t6_3), DoTransform(pointsZ, t6_3));
            yield return new Vector3(DoTransform(pointsX, t6_4), DoTransform(pointsY, t6_4), DoTransform(pointsZ, t6_4));
            yield return new Vector3(DoTransform(pointsX, t6_5), DoTransform(pointsY, t6_5), DoTransform(pointsZ, t6_5));
            yield return new Vector3(DoTransform(pointsX, t6_6), DoTransform(pointsY, t6_6), DoTransform(pointsZ, t6_6));
        }

        public static void TransformPoints(LinkedList<Vector4> controlpoints, LinkedList<Vector4> result) {
            using var pointsB = CollectionPool<LinkedList<Vector4>, Vector4>.Get();

            pointsB.c.AddFirst(controlpoints.First.Value);
            for (var el = controlpoints.First.Next; el is {Next: { }}; el = el.Next) {
                pointsB.c.AddLast((el.Previous.Value + el.Value * 2 + el.Next.Value) / 4);
            }

            pointsB.c.AddLast(controlpoints.Last.Value);
            result.AddFirst(controlpoints.First.Value);

            for (var el = pointsB.c.First; el is {Next: { }}; el = el.Next) {
                var p1 = el.Previous?.Value ?? el.Value;
                var p2 = el.Value;
                var p3 = el.Next.Value;
                var p4 = el.Next.Next?.Value ?? el.Next.Value;
                foreach (var p in Get6Points(p1, p2, p3, p4)) {
                    result.AddLast(p);
                }
            }

            result.AddLast(controlpoints.Last.Value);
        }

        public static IEnumerable<Vector4> GetNPoints(int n, Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            Vector4 pointsW = new Vector4(p1.w, p2.w, p3.w, p4.w);

            for (int i = 1; i <= n; i++) {
                float t = (float) i / n;
                var powers = new Vector4(1, 0);
                powers.y = powers.x * t;
                powers.z = powers.y * t;
                powers.w = powers.z * t;

                yield return new Vector4(DoTransform(pointsX, powers), DoTransform(pointsY, powers), DoTransform(pointsZ, powers), DoTransform(pointsW, powers));
            }
        }


        
        public static Vector4 GetPoint(float t, Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            Vector4 pointsW = new Vector4(p1.w, p2.w, p3.w, p4.w);
            var powers = new Vector4(1, 0);
            powers.y = powers.x * t;
            powers.z = powers.y * t;
            powers.w = powers.z * t;

            return new Vector4(DoTransform(pointsX, powers), DoTransform(pointsY, powers), DoTransform(pointsZ, powers), DoTransform(pointsW, powers));
        }
        
        public static IEnumerable<Vector4> Get6Points(Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            Vector4 pointsW = new Vector4(p1.w, p2.w, p3.w, p4.w);
            yield return new Vector4(DoTransform(pointsX, t6_1), DoTransform(pointsY, t6_1), DoTransform(pointsZ, t6_1), DoTransform(pointsW, t6_1));
            yield return new Vector4(DoTransform(pointsX, t6_2), DoTransform(pointsY, t6_2), DoTransform(pointsZ, t6_2), DoTransform(pointsW, t6_2));
            yield return new Vector4(DoTransform(pointsX, t6_3), DoTransform(pointsY, t6_3), DoTransform(pointsZ, t6_3), DoTransform(pointsW, t6_3));
            yield return new Vector4(DoTransform(pointsX, t6_4), DoTransform(pointsY, t6_4), DoTransform(pointsZ, t6_4), DoTransform(pointsW, t6_4));
            yield return new Vector4(DoTransform(pointsX, t6_5), DoTransform(pointsY, t6_5), DoTransform(pointsZ, t6_5), DoTransform(pointsW, t6_5));
            yield return new Vector4(DoTransform(pointsX, t6_6), DoTransform(pointsY, t6_6), DoTransform(pointsZ, t6_6), DoTransform(pointsW, t6_6));
        }

    }
#endregion

#region Bspline
    public static class Bspline {

        private static readonly Matrix4x4 MyMatrix = new Matrix4x4(
            new Vector4(0.1666667f, -0.5f, 0.5f, -0.1666667f ),
            new Vector4(0.6666667f, 0f, -1f, 0.5f ),
            new Vector4(0.1666667f, 0.5f, 0.5f, -0.5f ),
            new Vector4(0f, 0f, 0f, 0.1666667f ));
        private static readonly Vector4 t4_1 = new(1, 0.25f, 0.0625f, 0.015625f);
        private static readonly Vector4 t4_2 = new(1, 0.5f, 0.25f, 0.125f);
        private static readonly Vector4 t4_3 = new(1, 0.75f, 0.5625f, 0.421875f);
        private static readonly Vector4 t4_4 = new(1, 1f, 1f, 1f);
        private static readonly Vector4 t6_1 = new(1, 0.1666667f, 0.02777778f, 0.00462963f);
        private static readonly Vector4 t6_2 = new(1, 0.3333333f, 0.1111111f, 0.03703704f);
        private static readonly Vector4 t6_3 = new(1, 0.5f, 0.25f, 0.125f);
        private static readonly Vector4 t6_4 = new(1, 0.6666667f, 0.4444445f, 0.2962963f);
        private static readonly Vector4 t6_5 = new(1, 0.8333334f, 0.6944445f, 0.5787038f);
        private static readonly Vector4 t6_6 = new(1, 1f, 1f, 1f);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float DoTransform(Vector4 component, Vector4 powers) {
            var v = Vector4.Scale(MyMatrix * component, powers);
            return v.x + v.y + v.z + v.w;
        }

        public static void TransformPoints(LinkedList<Vector2> controlpoints, LinkedList<Vector2> result) {
            using var pointsB = CollectionPool<LinkedList<Vector2>, Vector2>.Get();

            pointsB.c.AddFirst(controlpoints.First.Value);
            for (var el = controlpoints.First.Next; el is {Next: { }}; el = el.Next) {
                pointsB.c.AddLast((el.Previous.Value + el.Value * 2 + el.Next.Value) / 4);
            }

            pointsB.c.AddLast(controlpoints.Last.Value);
            result.AddFirst(controlpoints.First.Value);

            for (var el = pointsB.c.First; el is {Next: { }}; el = el.Next) {
                var p1 = el.Previous?.Value ?? el.Value;
                var p2 = el.Value;
                var p3 = el.Next.Value;
                var p4 = el.Next.Next?.Value ?? el.Next.Value;
                foreach (var p in Get4Points(p1, p2, p3, p4)) {
                    result.AddLast(p);
                }
            }

            result.AddLast(controlpoints.Last.Value);
        }

        public static IEnumerable<Vector2> GetNPoints(int n, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);

            for (int i = 1; i <= n; i++) {
                float t = (float) i / n;
                var powers = new Vector4(1, 0);
                powers.y = powers.x * t;
                powers.z = powers.y * t;
                powers.w = powers.z * t;

                yield return new Vector2(DoTransform(pointsX, powers), DoTransform(pointsY, powers));
            }
        }


        
        public static Vector2 GetPoint(float t, Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            var powers = new Vector4(1, 0);
            powers.y = powers.x * t;
            powers.z = powers.y * t;
            powers.w = powers.z * t;

            return new Vector2(DoTransform(pointsX, powers), DoTransform(pointsY, powers));
        }
        
        public static IEnumerable<Vector2> Get4Points(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            yield return new Vector2(DoTransform(pointsX, t4_1), DoTransform(pointsY, t4_1));
            yield return new Vector2(DoTransform(pointsX, t4_2), DoTransform(pointsY, t4_2));
            yield return new Vector2(DoTransform(pointsX, t4_3), DoTransform(pointsY, t4_3));
            yield return new Vector2(DoTransform(pointsX, t4_4), DoTransform(pointsY, t4_4));
        }
        
        public static IEnumerable<Vector2> Get6Points(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            yield return new Vector2(DoTransform(pointsX, t6_1), DoTransform(pointsY, t6_1));
            yield return new Vector2(DoTransform(pointsX, t6_2), DoTransform(pointsY, t6_2));
            yield return new Vector2(DoTransform(pointsX, t6_3), DoTransform(pointsY, t6_3));
            yield return new Vector2(DoTransform(pointsX, t6_4), DoTransform(pointsY, t6_4));
            yield return new Vector2(DoTransform(pointsX, t6_5), DoTransform(pointsY, t6_5));
            yield return new Vector2(DoTransform(pointsX, t6_6), DoTransform(pointsY, t6_6));
        }

        public static void TransformPoints(LinkedList<Vector3> controlpoints, LinkedList<Vector3> result) {
            using var pointsB = CollectionPool<LinkedList<Vector3>, Vector3>.Get();

            pointsB.c.AddFirst(controlpoints.First.Value);
            for (var el = controlpoints.First.Next; el is {Next: { }}; el = el.Next) {
                pointsB.c.AddLast((el.Previous.Value + el.Value * 2 + el.Next.Value) / 4);
            }

            pointsB.c.AddLast(controlpoints.Last.Value);
            result.AddFirst(controlpoints.First.Value);

            for (var el = pointsB.c.First; el is {Next: { }}; el = el.Next) {
                var p1 = el.Previous?.Value ?? el.Value;
                var p2 = el.Value;
                var p3 = el.Next.Value;
                var p4 = el.Next.Next?.Value ?? el.Next.Value;
                foreach (var p in Get4Points(p1, p2, p3, p4)) {
                    result.AddLast(p);
                }
            }

            result.AddLast(controlpoints.Last.Value);
        }

        public static IEnumerable<Vector3> GetNPoints(int n, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);

            for (int i = 1; i <= n; i++) {
                float t = (float) i / n;
                var powers = new Vector4(1, 0);
                powers.y = powers.x * t;
                powers.z = powers.y * t;
                powers.w = powers.z * t;

                yield return new Vector3(DoTransform(pointsX, powers), DoTransform(pointsY, powers), DoTransform(pointsZ, powers));
            }
        }


        
        public static Vector3 GetPoint(float t, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            var powers = new Vector4(1, 0);
            powers.y = powers.x * t;
            powers.z = powers.y * t;
            powers.w = powers.z * t;

            return new Vector3(DoTransform(pointsX, powers), DoTransform(pointsY, powers), DoTransform(pointsZ, powers));
        }
        
        public static IEnumerable<Vector3> Get4Points(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            yield return new Vector3(DoTransform(pointsX, t4_1), DoTransform(pointsY, t4_1), DoTransform(pointsZ, t4_1));
            yield return new Vector3(DoTransform(pointsX, t4_2), DoTransform(pointsY, t4_2), DoTransform(pointsZ, t4_2));
            yield return new Vector3(DoTransform(pointsX, t4_3), DoTransform(pointsY, t4_3), DoTransform(pointsZ, t4_3));
            yield return new Vector3(DoTransform(pointsX, t4_4), DoTransform(pointsY, t4_4), DoTransform(pointsZ, t4_4));
        }
        
        public static IEnumerable<Vector3> Get6Points(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            yield return new Vector3(DoTransform(pointsX, t6_1), DoTransform(pointsY, t6_1), DoTransform(pointsZ, t6_1));
            yield return new Vector3(DoTransform(pointsX, t6_2), DoTransform(pointsY, t6_2), DoTransform(pointsZ, t6_2));
            yield return new Vector3(DoTransform(pointsX, t6_3), DoTransform(pointsY, t6_3), DoTransform(pointsZ, t6_3));
            yield return new Vector3(DoTransform(pointsX, t6_4), DoTransform(pointsY, t6_4), DoTransform(pointsZ, t6_4));
            yield return new Vector3(DoTransform(pointsX, t6_5), DoTransform(pointsY, t6_5), DoTransform(pointsZ, t6_5));
            yield return new Vector3(DoTransform(pointsX, t6_6), DoTransform(pointsY, t6_6), DoTransform(pointsZ, t6_6));
        }

        public static void TransformPoints(LinkedList<Vector4> controlpoints, LinkedList<Vector4> result) {
            using var pointsB = CollectionPool<LinkedList<Vector4>, Vector4>.Get();

            pointsB.c.AddFirst(controlpoints.First.Value);
            for (var el = controlpoints.First.Next; el is {Next: { }}; el = el.Next) {
                pointsB.c.AddLast((el.Previous.Value + el.Value * 2 + el.Next.Value) / 4);
            }

            pointsB.c.AddLast(controlpoints.Last.Value);
            result.AddFirst(controlpoints.First.Value);

            for (var el = pointsB.c.First; el is {Next: { }}; el = el.Next) {
                var p1 = el.Previous?.Value ?? el.Value;
                var p2 = el.Value;
                var p3 = el.Next.Value;
                var p4 = el.Next.Next?.Value ?? el.Next.Value;
                foreach (var p in Get4Points(p1, p2, p3, p4)) {
                    result.AddLast(p);
                }
            }

            result.AddLast(controlpoints.Last.Value);
        }

        public static IEnumerable<Vector4> GetNPoints(int n, Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            Vector4 pointsW = new Vector4(p1.w, p2.w, p3.w, p4.w);

            for (int i = 1; i <= n; i++) {
                float t = (float) i / n;
                var powers = new Vector4(1, 0);
                powers.y = powers.x * t;
                powers.z = powers.y * t;
                powers.w = powers.z * t;

                yield return new Vector4(DoTransform(pointsX, powers), DoTransform(pointsY, powers), DoTransform(pointsZ, powers), DoTransform(pointsW, powers));
            }
        }


        
        public static Vector4 GetPoint(float t, Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            Vector4 pointsW = new Vector4(p1.w, p2.w, p3.w, p4.w);
            var powers = new Vector4(1, 0);
            powers.y = powers.x * t;
            powers.z = powers.y * t;
            powers.w = powers.z * t;

            return new Vector4(DoTransform(pointsX, powers), DoTransform(pointsY, powers), DoTransform(pointsZ, powers), DoTransform(pointsW, powers));
        }
        
        public static IEnumerable<Vector4> Get4Points(Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            Vector4 pointsW = new Vector4(p1.w, p2.w, p3.w, p4.w);
            yield return new Vector4(DoTransform(pointsX, t4_1), DoTransform(pointsY, t4_1), DoTransform(pointsZ, t4_1), DoTransform(pointsW, t4_1));
            yield return new Vector4(DoTransform(pointsX, t4_2), DoTransform(pointsY, t4_2), DoTransform(pointsZ, t4_2), DoTransform(pointsW, t4_2));
            yield return new Vector4(DoTransform(pointsX, t4_3), DoTransform(pointsY, t4_3), DoTransform(pointsZ, t4_3), DoTransform(pointsW, t4_3));
            yield return new Vector4(DoTransform(pointsX, t4_4), DoTransform(pointsY, t4_4), DoTransform(pointsZ, t4_4), DoTransform(pointsW, t4_4));
        }
        
        public static IEnumerable<Vector4> Get6Points(Vector4 p1, Vector4 p2, Vector4 p3, Vector4 p4) {
            Vector4 pointsX = new Vector4(p1.x, p2.x, p3.x, p4.x);
            Vector4 pointsY = new Vector4(p1.y, p2.y, p3.y, p4.y);
            Vector4 pointsZ = new Vector4(p1.z, p2.z, p3.z, p4.z);
            Vector4 pointsW = new Vector4(p1.w, p2.w, p3.w, p4.w);
            yield return new Vector4(DoTransform(pointsX, t6_1), DoTransform(pointsY, t6_1), DoTransform(pointsZ, t6_1), DoTransform(pointsW, t6_1));
            yield return new Vector4(DoTransform(pointsX, t6_2), DoTransform(pointsY, t6_2), DoTransform(pointsZ, t6_2), DoTransform(pointsW, t6_2));
            yield return new Vector4(DoTransform(pointsX, t6_3), DoTransform(pointsY, t6_3), DoTransform(pointsZ, t6_3), DoTransform(pointsW, t6_3));
            yield return new Vector4(DoTransform(pointsX, t6_4), DoTransform(pointsY, t6_4), DoTransform(pointsZ, t6_4), DoTransform(pointsW, t6_4));
            yield return new Vector4(DoTransform(pointsX, t6_5), DoTransform(pointsY, t6_5), DoTransform(pointsZ, t6_5), DoTransform(pointsW, t6_5));
            yield return new Vector4(DoTransform(pointsX, t6_6), DoTransform(pointsY, t6_6), DoTransform(pointsZ, t6_6), DoTransform(pointsW, t6_6));
        }

    }
#endregion
}