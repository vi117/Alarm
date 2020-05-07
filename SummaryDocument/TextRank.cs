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
            var cwd = ".\\model\\";
            Trace.WriteLine($"Load Model Path : {cwd}");
            kiwi = new Kiwi.KiwiCS(cwd, 0);
            kiwi.prepare();
        }
        public string[] Tokenize(string sentence)
        {
            var result = kiwi.analyze(sentence, 1)[0].morphs;
            return (from morph in result
                    where !morph.Item2.StartsWith("J") //조사 거르기.
                    where !morph.Item2.StartsWith("S") //기호 거르기.
                    select morph.Item1).ToArray();
        }
    }
    public class TextRank
    {
        public KoreanTokenizer KoreanTokenizer;
        
        private Dictionary<string,int> StringToIndexTable;
        private string[] IndexToStringTable;
        private Dictionary<string, int> CountTable;
        /// <summary>
        /// Tokens of Sentences Array.
        /// </summary>
        private string[][] Sentences;
        public TextRank()
        {
            KoreanTokenizer = new KoreanTokenizer();
        }
        public string GetIndexToWord(int i)
        {
            return IndexToStringTable[i];
        }
        public int GetWordToIndex(string token)
        {
            return StringToIndexTable[token];
        }
        static public string[] SentenceSplit(string paragraph)
        {
            return paragraph.Split('.');
        }/*

        public void MakeIndexTable(string[] sentences)
        {
            MakeCountTable(sentences);
            var order = CountTable.OrderBy((k) => k.Value);
            IndexToStringTable = new string[order.Count()];
            StringToIndexTable = new Dictionary<string, int>();
            int i = 0;
            foreach (var s in order)
            {
                IndexToStringTable[i] = s.Key;
                StringToIndexTable.Add(s.Key, i);
            }
        }
        private void MakeCountTable(string[] sentences)
        {
            CountTable = new Dictionary<string, int>();
            Sentences = new string[sentences.Length][];
            for (int i = 0; i < sentences.Length; i++)
            {
                string sentence = sentences[i];
                var tokens = KoreanTokenizer.Tokenize(sentence);
                Sentences[i] = tokens;
                foreach (var token in tokens)
                {
                    if (!CountTable.ContainsKey(token))
                    {
                        CountTable.Add(token, 0);
                    }
                    else
                    {
                        CountTable[token] += 1;
                    }
                }
            }
        }*/
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
                        ret[i, j] = CalculateSimilarity(i, j);
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
            double dimender = s1.Intersect(s2).Count();
            double divider = Math.Log(s1.Length) + Math.Log(s2.Length);
            return dimender / divider;
        }
    }
}
