using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Managers {
    public static class RandomManager {
        public static int Seed { get; private set; }
        private static System.Random m_Random = new System.Random();

        #region Seed Control
        public static int GenerateSeed() {
            Seed = new System.Random().Next(int.MinValue, int.MaxValue);
            m_Random = new System.Random(Seed);
            return Seed;
        }

        public static void SetSeed(int newSeed) {
            Seed = newSeed;
            m_Random = new System.Random(newSeed);
        }

        public static float UseSeed(int seed) {
            System.Random random = new System.Random(seed);
            return Convert.ToSingle(random.NextDouble());
        }
        #endregion

        public static int GetValue() { return m_Random.Next(int.MinValue, int.MaxValue); }

        public static int GetValue(int min, int max) { return m_Random.Next(min, max); }

        public static float GetFloat() { return Convert.ToSingle(m_Random.NextDouble()); }

        public static float GetRangeFloat(float min, float max) {
            double val = (m_Random.NextDouble() * (max - min) + min);
            return (float)val;
        }

        public static bool RollChances(int chance) => m_Random.Next(0, 101) <= chance;

        public static T ItemFromDict<T>(Dictionary<int, T> dict) {
            if (dict.Count <= 0) { Debug.LogError("This list is empty, that means there is nothing to randomly select."); }
            int listCount = dict.Keys.Count;
            int randomIndex = GetValue(0, listCount);
            int key = dict.Keys.ToList()[randomIndex];
            return dict[key];
        }

        public static T ItemFromArray<T>(T[] list) {
            if (list.Length <= 0) { Debug.LogError("This list is empty, that means there is nothing to randomly select."); }
            int listCount = list.Length;
            int randomIndex = GetValue(0, listCount);
            return list[randomIndex];
        }

        public static T ItemFromList<T>(List<T> list) {
            if (list.Count <= 0) { Debug.LogError("This list is empty, that means there is nothing to randomly select."); }
            int listCount = list.Count;
            int randomIndex = GetValue(0, listCount);
            return list[randomIndex];
        }

        public static int IndexFromList<T>(List<T> list) {
            if (list.Count <= 0) { Debug.LogError("This list is empty, that means there is nothing to randomly select."); }
            int listCount = list.Count;
            int randomIndex = GetValue(0, listCount);
            return randomIndex;
        }

        public static void ShuffleList(ref List<int> input) {
            if (input.Count <= 0) { Debug.LogError("Nothing in list to shuffle..."); }

            List<int> randomList = new List<int>();
            int iteration = 0;
            do {
                int count = input.Count;
                int randomIndex = GetValue(0, count);

                if (!randomList.Contains(input[randomIndex])) {
                    randomList.Add(input[randomIndex]);
                }

                iteration++;
            } while (randomList.Count < input.Count && iteration <= 100);

            input = randomList;
        }
    }
}