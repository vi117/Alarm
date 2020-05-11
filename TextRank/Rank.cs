using System;
using System.Linq;
using System.Collections.Generic;

namespace TextRank
{

    static public class Ranker
    {
	const double d = 0.85; // random walker constant
	const double precision = 0.0001;

		// Get top ten rank
		static public int[] Rank(double[,] weights)
		{
			var s = score(weights);
			return Enumerable.Zip(s.AsEnumerable(),
						Enumerable.Range(0, s.Length),
						(a, b) => (a, b))
				.OrderByDescending<(double, int), double>((t) => t.Item1)
				.Select((t) => t.Item2)
				.Take(10)
				.ToArray();
		}

		// Score text vertices until the scores converges to given precision
        static double[] score(double[,] weights)
		{
			// Initialize score array
			double[] scores = new double[weights.GetLength(0)];
			for(int i = 0; i < weights.GetLength(0); i++) {
				scores[i] = 1; // set default score as 1
			}

			// Loop until given precision
			while(true) {
				double[] new_scores = score_once(scores, weights);
				if(difference(scores, new_scores) < precision) {
					return new_scores;
				} else {
					scores = new_scores;
					continue;
				}
			}
		}

        // Calculate new scores from previous scores and weights
        static double[] score_once(double[] scores, double[,] weights)
		{
			double[] new_score = new double[scores.Length];
			for(int i = 0; i < scores.GetLength(0); i++) {
				new_score[i] = score_i(scores, weights, i);
			}

			return new_score;
		}

        // Calculate new score of i from previous scores
        static double score_i(double[] scores, double[,] weights, int i)
        {
			return (1 - d) + d * sum_with_weight(scores, weights, i);
		}

        static double sum_with_weight(double[] scores, double[,] weights, int i)
		{
			double s = 0;
			for(int j = 0; j < weights.GetLength(0); j++) {
				double sum_w = 0;
				for(int k = 0; k < weights.GetLength(1); k++) {
					sum_w += weights[j,k];
				}

				s += weights[j,i] / sum_w * scores[j];
			}

			return s;
		}

		// Return difference
		static double difference(double[] ns, double[] ns2)
		{
			double d = 0;
			double l = ns.Length;
			for(int i = 0; i < l; i++) {
				var a = ns[i];
				var b = ns2[i];
				d += Math.Pow((a - b), 2)/ l;
			}

			return Math.Sqrt(d)/(double)ns.Length;
		}
    }
}
