using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HSM.Game
{
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //
    // RandomMaker
    // 난수 생성기
    //
    //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    public static class RandomMaker
    {
        #region[RandomMaker]
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        public static int[] MakeRandomNumbers(int minValue, int maxValue, int randomSeed = 0)
        {
            if (randomSeed == 0)
                randomSeed = (int)DateTime.Now.Ticks;

            List<int> values = new List<int>();
            for (int v = minValue; v < maxValue; v++)
            {
                values.Add(v);
            }

            int[] result = new int[maxValue - minValue];
            System.Random random = new System.Random(Seed: randomSeed);
            int i = 0;
            while (values.Count > 0)
            {
                int randomValue = values[random.Next(0, values.Count)];
                result[i++] = randomValue;

                if (!values.Remove(randomValue))
                {
                    // Exception
                    break;
                }
            }

            return result;
        }
        #endregion

    }

}
