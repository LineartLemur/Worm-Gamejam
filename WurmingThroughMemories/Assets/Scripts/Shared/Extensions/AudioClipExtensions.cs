using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

namespace Utility {
	public static class AudioClipExtensions {
		public static AudioClip MakeSubclip(this AudioClip clip, float start, float stop) {
			/* Create a new audio clip */
			int frequency = clip.frequency;
			float timeLength = stop - start;
			int samplesLength = (int)(frequency * timeLength);

			AudioClip newClip = AudioClip.Create(clip.name + "-sub", samplesLength, clip.channels, clip.frequency, false);
			//Debug.Log(newClip.loadType);

			//Profiler.BeginSample("Data array");
			float[] data = new float[samplesLength * 2];
			//Profiler.EndSample();

			clip.GetData(data, (int)(frequency * start));

			newClip.SetData(data, 0);
			return newClip;
		}
	}
}
