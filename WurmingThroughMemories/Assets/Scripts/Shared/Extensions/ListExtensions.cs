
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

public static class ListExtensions {
        public static string ToLogString<T>(this IList<T> list){
            string s = "[";
            for (int i = 0; i < list.Count ; i++) {
                s += list[i].ToString() + ", ";
            }

            s += "]";
            return s;
        }
        public static T Random<T>(this IList<T> list){
            return list[UnityEngine.Random.Range(0, list.Count)];
        }
        public static T Random<T>(this IEnumerable<T> list){
	        return list.Skip(UnityEngine.Random.Range(0, list.Count())).First();
        }
        public static T RandomByWeight<T>(this IList<T> list, Func<T,float> GetWeight) {
	        float sum = list.Sum(GetWeight);
	        float s = UnityEngine.Random.value * sum;
	        float acc = 0;
	        for (int i = 0; i < list.Count; i++) {
		        acc += GetWeight(list[i]);
		        if (s < acc) return list[i];
	        }
	        return list[^1];
        }

        public static IEnumerable<(T,T)> Sides<T>(this IList<T> list, bool loop = true) {
          for (int i = 0; i < ((loop)?list.Count : list.Count -1) ; i++) {
            yield return (list[i], list[(i + 1) % list.Count]);
          }
        }
        public static IEnumerable<(T, T)> Sides<T>(this IEnumerable<T> list, bool loop = true) {
	        
	        Profiler.BeginSample("setup");
	        T a = default;
	        bool notFirst = false;
	        T firstElement = default;
	        Profiler.EndSample();
	        
	        Profiler.BeginSample("sides2");
	        foreach (var b in list) {
		        Profiler.BeginSample("sides3");
		        if (notFirst) {
			        yield return (a,b);
		        }
		        else {
			        firstElement = b;
		        }

		        notFirst = true;
		        a = b;
		        Profiler.EndSample();
	        }
	        Profiler.EndSample();

	        if (loop) yield return (a,firstElement);
        }

        public static IList<IDisposable> Dispose(this IList<IDisposable> list) {
            for (int i = 0; i < list.Count; i++) {
                list[i].Dispose();
            }
            list.Clear();
            return list;
        }

        public static void SetActive(this IEnumerable<GameObject> list, bool active) {
	        foreach (var t in list) {
		        t.SetActive(active);
	        }
        }
        public static void Move<T>(this List<T> list, int fromIndex, int toIndex) {
	        var element = list[fromIndex];
	        list.RemoveAt(fromIndex);
	        list.Insert(toIndex,element);
        }
        
        public static T GetElementWithRandomF<T>(this IList<T> list, float f) {
	        if (list.Count == 0) throw new IndexOutOfRangeException();
            
	        return list[Mathf.FloorToInt(Mathf.Repeat(f * list.Count,list.Count))];
            
            
        }
        public static T WithMin<T>(this IEnumerable<T> source, Func<T,float> selector){
	        var min = float.MaxValue;
	        T withMin = default;

	        foreach (var element in source) {
		        float val;
		        if ((val = selector(element)) < min) {
			        withMin = element;
			        min = val;
		        }
	        }

	        return withMin;
        }
        public static T WithMax<T>(this IEnumerable<T> source, Func<T,float> selector){
	        var max = float.MinValue;
	        T withMax = default;

	        foreach (var element in source) {
		        float val;
		        if ((val = selector(element)) > max) {
			        withMax = element;
			        max = val;
		        }
	        }

	        return withMax;
        }
        public static LinkedListNode<T> NodeWithMin<T>(this LinkedList<T> source, Func<T,float> selector){
	        var min = float.MaxValue;
	        LinkedListNode<T> withMin = null;

	        for (var element = source.First; element != null; element = element.Next) {
		        float val;
		        if ((val = selector(element.Value)) < min) {
			        withMin = element;
			        min = val;
		        }
	        }

	        return withMin;
        }
        public static LinkedListNode<T> NodeWithMax<T>(this LinkedList<T> source, Func<T,float> selector){
	        var max = float.MinValue;
	        LinkedListNode<T> withMax = null;

	        for (var element = source.First; element != null; element = element.Next) {
		        float val;
		        if ((val = selector(element.Value)) > max) {
			        withMax = element;
			        max = val;
		        }
	        }

	        return withMax;
        }

        public static Vector2 Sum<TSource>(this System.Collections.Generic.IEnumerable<TSource> source,
	        System.Func<TSource, Vector2> selector) {
	        return source.Select(selector).Aggregate((a, b) => a + b);
        }
        public static Vector3 Sum<TSource>(this System.Collections.Generic.IEnumerable<TSource> source,
	        System.Func<TSource, Vector3> selector) {
	        return source.Select(selector).Aggregate((a, b) => a + b);
        }
        public static Vector2Int Sum<TSource>(this System.Collections.Generic.IEnumerable<TSource> source,
	        System.Func<TSource, Vector2Int> selector) {
	        return source.Select(selector).Aggregate((a, b) => a + b);
        }
        public static Vector3Int Sum<TSource>(this System.Collections.Generic.IEnumerable<TSource> source,
	        System.Func<TSource, Vector3Int> selector) {
	        return source.Select(selector).Aggregate((a, b) => a + b);
        }
        
        public static LinkedListNode<T> NextWrapped<T>(this LinkedListNode<T> node) {
	        return node.Next??node.List.First;
        }
        public static LinkedListNode<T> PreviousWrapped<T>(this LinkedListNode<T> node) {
	        return node.Previous??node.List.Last;
        }
        // public static NativeLinkedList<T>.Enumerator NextWrapped<T>(this NativeLinkedList<T>.Enumerator node) where T : unmanaged {
	       //  return (node.Next.IsValid)? node.Next:node.List.Head;
        // }
        // public static NativeLinkedList<T>.Enumerator PrevWrapped<T>(this NativeLinkedList<T>.Enumerator node)where T : unmanaged {
	       //  return (node.Prev.IsValid)? node.Prev:node.List.Tail;
        // }

    }
