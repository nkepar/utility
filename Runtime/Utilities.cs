using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;
using Random = System.Random;

namespace Project.Utilities
{
	public class RangeFloat
	{
		[SuffixLabel("Min", true)]
		[HideLabel]
		[HorizontalGroup("RangeFloat")]
		public float min;

		[SuffixLabel("Max", true)]
		[HideLabel]
		[HorizontalGroup("RangeFloat")]
		public float max;
	}

	public class RangeInt
	{
		[SuffixLabel("Min", true)]
		[HideLabel]
		[HorizontalGroup("RangeInt")]
		public int min;

		[SuffixLabel("Max", true)]
		[HideLabel]
		[HorizontalGroup("RangeInt")]
		public int max;
	}

	public struct Vector3Int : IComparable<Vector3Int>
	{
		public int x;
		public int y;
		public int z;

		public static Vector3Int size0x0x0
		{
			get { return new Vector3Int(0, 0, 0); }
		}

		public static Vector3Int size1x1x1
		{
			get { return new Vector3Int(1, 1, 1); }
		}

		public static Vector3Int size1x1x1Neg
		{
			get { return new Vector3Int(-1, -1, -1); }
		}

		public static Vector3Int size2x2x2
		{
			get { return new Vector3Int(2, 2, 2); }
		}

		public static Vector3Int size2x2x2Neg
		{
			get { return new Vector3Int(-2, -2, -2); }
		}

		public static Vector3Int size3x3x3
		{
			get { return new Vector3Int(3, 3, 3); }
		}

		public static Vector3Int size3x1x3
		{
			get { return new Vector3Int(3, 1, 3); }
		}

		public Vector3Int(int x, int y, int z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public Vector3Int(Vector3 vector3Float)
		{
			x = Mathf.RoundToInt(vector3Float.x);
			y = Mathf.RoundToInt(vector3Float.y);
			z = Mathf.RoundToInt(vector3Float.z);
		}

		public override string ToString()
		{
			var result = "(" + x + ", " + y + ", " + z + ")";
			return result;
		}

		public Vector3Int Invert()
		{
			return new Vector3Int(-x, -y, -z);
		}

		public Vector3 ToVector3()
		{
			return new Vector3(x, y, z);
		}

		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj is Vector3Int && this == (Vector3Int)obj;
		}

		public static bool operator ==(Vector3Int a, Vector3Int b)
		{
			return a.x == b.x && a.y == b.y && a.z == b.z;
		}

		public static bool operator !=(Vector3Int a, Vector3Int b)
		{
			return a.x != b.x || a.y != b.y || a.z != b.z;
		}

		public static Vector3Int operator *(Vector3Int a, Vector3Int b)
		{
			return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
		}

		public static Vector3Int operator *(Vector3Int a, int b)
		{
			return new Vector3Int(a.x * b, a.y * b, a.z * b);
		}

		public static Vector3Int operator /(Vector3Int a, int b)
		{
			return new Vector3Int(a.x / b, a.y / b, a.z / b);
		}

		public static Vector3Int operator +(Vector3Int a, Vector3Int b)
		{
			return new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
		}

		public static Vector3Int operator -(Vector3Int a, Vector3Int b)
		{
			return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
		}

		public int CompareTo(Vector3Int other)
		{
			return x.CompareTo(other.x) + y.CompareTo(other.y) + z.CompareTo(other.z);
		}
	}

	public static class UMath
	{
		public static bool RoughlyEqual(this float a, float b, float threshold = 0.01f)
		{
			return Mathf.Abs(a - b) <= threshold;
		}

		public static bool IsValueEven(int value)
		{
			return value % 2 == 0;
		}

		public static float Truncate(this float value, int digits)
		{
			var mult = Math.Pow(10.0, digits);
			if (mult == 0)
			{
				return value;
			}

			var result = Math.Truncate(mult * value) / mult;
			return (float)result;
		}

		public static float InverseLerpUnclamped(float a, float b, float value)
		{
			return (value - a) / (b - a);
		}

		public const float TAU = 6.28318530717959f;

		public static float Frac(float x)
		{
			return x - Mathf.Floor(x);
		}

		public static float Smooth01(float x)
		{
			return x * x * (3 - 2 * x);
		}

		public static class UVector
		{
			// Vector2
			public static Vector2 AngToDir(float aRad)
			{
				return new Vector2(Mathf.Cos(aRad), Mathf.Sin(aRad));
			}

			public static float DirToAng(Vector2 dir)
			{
				return Mathf.Atan2(dir.y, dir.x);
			}

			public static float Determinant /*or Cross*/(Vector2 a, Vector2 b)
			{
				return a.x * b.y - a.y * b.x;
				// 2D "cross product"
			}

			public static Vector2 Rotate90CW(Vector2 v)
			{
				return new Vector2(v.y, -v.x);
			}

			public static Vector2 Rotate90CCW(Vector2 v)
			{
				return new Vector2(-v.y, v.x);
			}

			public static Vector2 Rotate(Vector2 v, float angRad)
			{
				var ca = Mathf.Cos(angRad);
				var sa = Mathf.Sin(angRad);
				return new Vector2(ca * v.x - sa * v.y, sa * v.x + ca * v.y);
			}

			public static Vector2 RotateAround(Vector2 v, Vector2 pivot, float angRad)
			{
				return Rotate(v - pivot, angRad) + pivot;
			}

			// Vector2/3/4
			public static Vector2 Abs(Vector2 v)
			{
				return new Vector2(Mathf.Abs(v.x), Mathf.Abs(v.y));
			}

			public static Vector3 Abs(Vector3 v)
			{
				return new Vector3(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z));
			}

			public static Vector4 Abs(Vector4 v)
			{
				return new Vector4(Mathf.Abs(v.x), Mathf.Abs(v.y), Mathf.Abs(v.z), Mathf.Abs(v.w));
			}

			public static Vector2 Round(Vector2 v)
			{
				return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
			}

			public static Vector3 Round(Vector3 v)
			{
				return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
			}

			public static Vector4 Round(Vector4 v)
			{
				return new Vector4(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z), Mathf.Round(v.w));
			}

			public static Vector2 Floor(Vector2 v)
			{
				return new Vector2(Mathf.Floor(v.x), Mathf.Floor(v.y));
			}

			public static Vector3 Floor(Vector3 v)
			{
				return new Vector3(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z));
			}

			public static Vector4 Floor(Vector4 v)
			{
				return new Vector4(Mathf.Floor(v.x), Mathf.Floor(v.y), Mathf.Floor(v.z), Mathf.Floor(v.w));
			}

			public static Vector2 Ceil(Vector2 v)
			{
				return new Vector2(Mathf.Ceil(v.x), Mathf.Ceil(v.y));
			}

			public static Vector3 Ceil(Vector3 v)
			{
				return new Vector3(Mathf.Ceil(v.x), Mathf.Ceil(v.y), Mathf.Ceil(v.z));
			}

			public static Vector4 Ceil(Vector4 v)
			{
				return new Vector4(Mathf.Ceil(v.x), Mathf.Ceil(v.y), Mathf.Ceil(v.z), Mathf.Ceil(v.w));
			}
		}

		public static class URandom
		{
			public static int GetRandomMean(int mean, int range)
			{
				return mean + UnityEngine.Random.Range(-range, range);
			}

			public static float GetRandomMean(float mean, float range)
			{
				return mean + UnityEngine.Random.Range(-range, range);
			}

			public static float GetRandomRange(RangeFloat rangeFloat)
			{
				return UnityEngine.Random.Range(rangeFloat.min, rangeFloat.max);
			}

			public static float GetRandomRange(RangeInt rangeInt)
			{
				return UnityEngine.Random.Range(rangeInt.min, rangeInt.max);
			}

			public static string GetRandomArrayElement(string[] collection)
			{
				var random = GetRandomElement(collection);

				return random;
			}

			public static T GetRandomElement<T>(List<T> collection)
			{
				return collection is
				{
					Count: > 0
				}
					? collection[UnityEngine.Random.Range(0, collection.Count)]
					: default;
			}

			public static T GetRandomElement<T>(T[] collection)
			{
				return collection is
				{
					Length: > 0
				}
					? collection[UnityEngine.Random.Range(0, collection.Length)]
					: default;
			}
		}

		public static class Remap
		{
			public static float Range(float iMin,
			                          float iMax,
			                          float oMin,
			                          float oMax,
			                          float value)
			{
				var t = Mathf.InverseLerp(iMin, iMax, value);
				return Mathf.Lerp(oMin, oMax, t);
			}

			public static float RangeUnclamped(float iMin,
			                                   float iMax,
			                                   float oMin,
			                                   float oMax,
			                                   float value)
			{
				var t = InverseLerpUnclamped(iMin, iMax, value);
				return Mathf.LerpUnclamped(oMin, oMax, t);
			}
		}
	}

	public static class UCollections
	{
		public static void RotateArrayLeft<T>(this T[] array, int count)
		{
			if (array == null || array.Length < 2)
			{
				return;
			}

			count %= array.Length;
			if (count == 0)
			{
				return;
			}

			var left = count < 0 ? -count : array.Length + count;
			var right = count > 0 ? count : array.Length - count;
			if (left <= right)
			{
				for (var i = 0; i < left; i++)
				{
					var temp = array[0];
					Array.Copy(array,
					           1,
					           array,
					           0,
					           array.Length - 1);
					array[^1] = temp;
				}
			}
			else
			{
				for (var i = 0; i < right; i++)
				{
					var temp = array[^1];
					Array.Copy(array,
					           0,
					           array,
					           1,
					           array.Length - 1);
					array[0] = temp;
				}
			}


			public static bool IsBitArrayAllTrue(BitArray bitArray)
			{
				for (var i = 0; i < bitArray.Count; i++)
				{
					if (bitArray[i] == false)
					{
						return false;
					}
				}

				return true;
			}

			public static bool Contains(ref int[] source, ref int target)
			{
				for (var elementIndex = 0; elementIndex < source.Length; elementIndex++)
				{
					if (source[elementIndex] == target)
					{
						return true;
					}
				}

				return false;
			}

			public static bool Contains<T>(List<T> collection, T entry)
			{
				if (entry is string entryString && string.IsNullOrEmpty(entryString))
				{
					return false;
				}

				return collection.Contains(entry);
			}

			public static bool ArraysEqualCheck<T>(T[] first, T[] second)
			{
				if (ReferenceEquals(first, second))
				{
					return true;
				}

				if (first == null || second == null || first.Length != second.Length)
				{
					return false;
				}

				var comparer = EqualityComparer<T>.Default;
				for (var i = 0; i < first.Length; i++)
				{
					if (!comparer.Equals(first[i], second[i]))
					{
						return false;
					}
				}

				return true;
			}


			public static TKey GetRandomKey<TKey, TValue>(this IDictionary<TKey, TValue> dict)
			{
				if (dict == null || dict.Count == 0)
				{
					return default;
				}

				return dict.ElementAt(UnityEngine.Random.Range(0, dict.Count)).Key;
			}

			public static TValue GetRandomValue<TKey, TValue>(this IDictionary<TKey, TValue> dict)
			{
				return dict.ElementAt(UnityEngine.Random.Range(0, dict.Count)).Value;
			}

			public static T GetRandomEntry<T>(this HashSet<T> set)
			{
				if (set == null || set.Count == 0)
				{
					return default;
				}

				return set.ElementAt(UnityEngine.Random.Range(0, set.Count));
			}

			public static T GetRandomEntry<T>(this List<T> list)
			{
				if (list == null || list.Count == 0)
				{
					return default;
				}

				return list[UnityEngine.Random.Range(0, list.Count)];
			}

		private static readonly List<(float weight, object element)> weightCache =
			new List<(float weight, object element)>();

		public static T GetRandomWeighted<T>(this Dictionary<T, float> weights)
		{
			if (weights == null || weights.Count == 0)
			{
				return default;
			}

			weightCache.Clear();

			var totalWeight = 0.0f;
			foreach (var element in weights)
			{
				weightCache.Add((element.Value, element.Key));
				totalWeight += element.Value;
			}

			weightCache.Shuffle();

			var selected = UnityEngine.Random.Range(0.0f, totalWeight);
			foreach (var element in weightCache)
			{
				selected -= element.weight;
				if (selected <= 0.0f)
				{
					return (T)element.element;
				}
			}

			return default;
		}

		public static void Shuffle<T>(this IList<T> list)
		{
			var n = list.Count;
			while (n > 1)
			{
				n--;
				var k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
				var value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static void Sort<T>(ref HashSet<T> set)
		{
			if (set == null || set.Count == 0)
			{
				return;
			}

			var list = set.ToList();
			list.Sort();
			set = new HashSet<T>(list);
		}

		public interface IWeightedCollectionEntry
		{
			float GetWeight();
		}

		public static T RandomElementByWeight<T>(this IEnumerable<T> sequence) where T : class, IWeightedCollectionEntry
		{
			if (sequence == null || sequence.Count() == 0)
			{
				return null;
			}

			var weightTotal = 0f;
			foreach (var entry in sequence)
			{
				weightTotal += entry.GetWeight();
			}

			var weightRandom = UnityEngine.Random.Range(0f, 1f) * weightTotal;
			var weightCurrent = 0f;

			foreach (var entry in sequence)
			{
				weightCurrent += entry.GetWeight();
				if (weightCurrent >= weightRandom)
				{
					return entry;
				}
			}

			return null;
		}

		public static T RandomElementByWeight<T>(this List<T> sequence, List<float> weights)
			where T : class, IWeightedCollectionEntry
		{
			if (sequence == null ||
			    sequence.Count == 0 ||
			    weights == null ||
			    weights.Count == 0 ||
			    sequence.Count != weights.Count)
			{
				Debug.LogWarning("Failed to select random element by weight using external weight list");
				return null;
			}

			var weightTotal = 0f;
			foreach (var weight in weights)
			{
				weightTotal += weight;
			}

			var weightRandom = UnityEngine.Random.Range(0f, 1f) * weightTotal;
			var weightCurrent = 0f;

			for (var i = 0; i < sequence.Count; ++i)
			{
				var entry = sequence[i];
				var weight = weights[i];

				weightCurrent += weight;
				if (weightCurrent >= weightRandom)
				{
					return entry;
				}
			}

			return null;
		}
/*
 public static bool ContainsMoreElements(int[] source, int[] target)
		{
			Assert.IsTrue(source.Length == State.Economy.elementsCount);
			Assert.IsTrue(source.Length == target.Length);

			for (var elementIndex = 0; elementIndex < State.Economy.elementsCount; elementIndex++)
			{
				if (source[elementIndex] < target[elementIndex])
				{
					return false;
				}
			}

			return true;
		}
		public static int[] AddElements(int[] source, int[] target)
		{
			Assert.IsTrue(source.Length == target.Length);

			for (var elementIndex = 0; elementIndex < State.Economy.elementsCount; elementIndex++)
			{
				source[elementIndex] += target[elementIndex];
			}

			return source;
		}

		public static int[] SubtractElements(int[] source, int[] target)
		{
			Assert.IsTrue(source.Length == target.Length);

			for (var elementIndex = 0; elementIndex < State.Economy.elementsCount; elementIndex++)
			{
				source[elementIndex] -= target[elementIndex];
			}

			return source;
		}

		public static int[] MergeToHighestElements(int[] source, int[] target)
		{
			Assert.IsTrue(source.Length == target.Length);

			for (var elementIndex = 0; elementIndex < State.Economy.elementsCount; elementIndex++)
			{
				if (source[elementIndex] < target[elementIndex])
				{
					source[elementIndex] = target[elementIndex];
				}
			}

			return source;
		}
		*/
	}

	public static class UGeneral
	{
	}

	public static class UGameplay
	{
		private static Camera mainCamera;
		private static Plane worldPlane = new Plane(Vector3.up, 0);
		public static Vector3 cursorWorldPosition = GetGroundedCursorPosition();

		private static Vector3 GetGroundedCursorPosition()
		{
			if (mainCamera == null)
			{
				mainCamera = Camera.main;
			}

			Assert.IsTrue(mainCamera != null);

			var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
			return !worldPlane.Raycast(ray, out var distance) ? Vector3.zero : ray.GetPoint(distance);
		}

		public static IEnumerator WaitForInput()
		{
			while (!Input.GetKeyDown(KeyCode.Space))
			{
				yield return null;
			}
		}

		public static bool IsApplicationNotPlaying()
		{
			#if UNITY_EDITOR
			return !EditorApplication.isPlayingOrWillChangePlaymode && !Application.isPlaying;
			#else
        return false;
			#endif
		}

		public static void ClearChildren(Transform parent, bool forceImmediate = false)
		{
			var parentWasHidden = parent.gameObject.activeSelf == false;
			if (parentWasHidden)
			{
				parent.gameObject.SetActive(true);
			}

			for (var i = parent.childCount - 1; i >= 0; --i)
			{
				if (!Application.isPlaying || forceImmediate)
				{
					Object.DestroyImmediate(parent.GetChild(i).gameObject);
				}
				else
				{
					Object.Destroy(parent.GetChild(i).gameObject);
				}
			}

			if (parentWasHidden)
			{
				parent.gameObject.SetActive(false);
			}
		}
	}

	public static class ThreadSafeRandom
	{
		[ThreadStatic]
		private static Random Local;

		public static Random ThisThreadsRandom
		{
			get
			{
				return Local ??=
					       new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId));
			}
		}
	}
}