using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SummaryDocument
{
    /// <summary>
    /// tokenizer wrapper class
    /// </summary>
    public class KoreanTokenizer
    {
        private Kiwi.KiwiCS kiwi;

        public KoreanTokenizer()
        {
            var cwd = ".\\lang-model\\";
            Trace.WriteLine($"Load Model Path : {cwd}");
            kiwi = new Kiwi.KiwiCS(cwd, 0);
            kiwi.prepare();
        }
        public string[] Tokenize(string sentence)
        {
            var tt = kiwi.analyze(sentence, 1);
            var result = tt[0].morphs;
            return (from morph in result
                    where !morph.Item2.StartsWith("J") //조사 거르기.
                    where !morph.Item2.StartsWith("S") //기호 거르기.
                    select morph.Item1).ToArray();
        }
    }
    public class TextRank
    {
        const double MinSimilarity = 0.3;
        public KoreanTokenizer KoreanTokenizer;
        /// <summary>
        /// Tokens of Sentences Array.
        /// </summary>
        private string[][] Sentences;
        public TextRank()
        {
            KoreanTokenizer = new KoreanTokenizer();
        }
        
        static public string[] SentenceSplit(string paragraph)
        {
            return paragraph.Split('.');
        }
        public void MakeTokens(string[] sentences)
        {
            Sentences = new string[sentences.Length][];
            for (int i = 0; i < sentences.Length; i++)
            {
                string sentence = sentences[i];
                var tokens = KoreanTokenizer.Tokenize(sentence);
                Sentences[i] = tokens;
            }
        }
        public double[,] GetSimilarityMatrix()
        {
            var ret = new double[Sentences.Length, Sentences.Length];
            for (int i = 0; i < Sentences.Length; i++)
            {
                for (int j = 0; j < Sentences.Length; j++)
                {
                    if (i < j)
                    {
                        var sim = CalculateSimilarity(i, j);
                        ret[i, j] = sim >= MinSimilarity ? sim : 0;
                    }
                    else if(i == j)
                    {
                        ret[i, j] = 0;
                    }
                    else
                    {
                        ret[i, j] = ret[j, i];
                    }
                }
            }
            return ret;
        }
        public double CalculateSimilarity(int i,int j)
        {
            var s1 = Sentences[i];
            var s2 = Sentences[j];
            if (s1.Length <= 1 || s2.Length <= 1) return 0;
            double dimender = s1.Intersect(s2).Count();
            double divider = Math.Log(s1.Length) + Math.Log(s2.Length);
            return dimender / divider;
        }
    }
}
