using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

public static class MonoBehaviourExtensions
{


    /// <summary>
    /// This will Call a method wtih a delay (including being able to use delegates unlike Invoke(nameof(method), delay);)
    /// </summary>
    /// <param name="mono"></param>
    /// <param name="method"></param>
    /// <param name="delay"></param>
    public static void CallWithDelay(this MonoBehaviour mono, Action method, float delay)
        => mono.StartCoroutine(CallWithDelayRoutine(method, delay));

    static IEnumerator CallWithDelayRoutine(Action method, float delay)
    {
        yield return new WaitForSeconds(delay);
        method();
    }


    public static GameObject[] ShuffleList(this MonoBehaviour mono, List<GameObject> list)
    {
        GameObject[] array = list.ToArray();

        // Shuffle the array using Fisher-Yates algorithm
        for (int i = 0; i < array.Length - 1; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, array.Length);
            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }

        return array;
    }

    public static float RoundValue(float value, int decimalAmount)
    {
        float multiplier = Mathf.Pow(10.0f, decimalAmount);
        return Mathf.Round(value * multiplier) / multiplier;
    }

}

