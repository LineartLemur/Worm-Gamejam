using System;
using System.Collections.Generic;
using System.Linq;


public class ListUtil
{
	public static T FindLowestScore<T>(IEnumerable<T> list, Func<T, float> getScore)
	{
		return FindHighestScore(list, (val) =>
		{
			return -getScore(val);
		});
	}

	public static T FindHighestScore<T>(IEnumerable<T> list, Func<T, float> getScore)
	{
		if (list.Count() == 0) throw new Exception("Candidate array must be larger than 0.");
		var bestCandidate = list.First();
		var bestScore = getScore(bestCandidate);
		foreach (T val in list)
		{
			var score = getScore(val);
			if (score > bestScore)
			{
				bestScore = score;
				bestCandidate = val;
			}
		}
		return bestCandidate;
	}
}