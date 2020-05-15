using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SummaryDocument
{
	static public class Ranker
	{
		const double d = 0.85;
		const double precision = 0.0001;

		static public int[] Rank(double[,] weights)
		{
			var s = Score(weights);
			return Enumerable.Zip(s.AsEnumerable(),
						Enumerable.Range(0, s.Length),
						(a, b) => (a, b))
				.OrderByDescending<(double, int), double>((t) => t.Item1)
				.Select((t) => t.Item2)
				.Take(10)
				.ToArray();
		}

		// Score text vertices until the scores converges to given precision
		static double[] Score(double[,] weights)
		{
			// Initialize score array
			double[] scores = new double[weights.GetLength(0)];
			for (int i = 0; i < weights.GetLength(0); i++)
			{
				scores[i] = 1; // set default score as 1
			}

			// Loop until given precision
			while (true)
			{
				double[] new_scores = Score_once(scores, weights);
				if (Difference(scores, new_scores) < precision)
				{
					return new_scores;
				}
				else
				{
					scores = new_scores;
					continue;
				}
			}
		}

		// Calculate new scores from previous scores and weights
		static double[] Score_once(double[] scores, double[,] weights)
		{
			double[] new_score = new double[scores.Length];
			for (int i = 0; i < scores.GetLength(0); i++)
			{
				new_score[i] = Score_i(scores, weights, i); 
			}

			return new_score;
		}

		// Calculate new score of i from previous scores
		static double Score_i(double[] scores, double[,] weights, int i)
		{
			return (1 - d) + d * Sum_with_weight(scores, weights, i);
		}

		static double Sum_with_weight(double[] scores, double[,] weights, int i)
		{
			double s = 0;
			for (int j = 0; j < weights.GetLength(0); j++)
			{
				double sum_w = 0;
				for (int k = 0; k < weights.GetLength(1); k++)
				{
					sum_w += weights[j, k];
				}
				if (sum_w != 0)
					s += weights[j, i] / sum_w * scores[j];
			}

			return s;
		}

		// Return difference
		static double Difference(double[] ns, double[] ns2)
		{
			double d = 0;
			if (ns.Length == ns2.Length)
			{
				double l = ns.Length;
				for (int i = 0; i < ns.Length; i++)
				{
					var a = ns[i];
					var b = ns2[i];
					d += Math.Pow((a - b), 2) / l;
				}
			}

			return Math.Sqrt(d) / ns.Length;
		}
	}
}
