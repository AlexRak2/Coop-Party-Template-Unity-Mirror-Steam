using UnityEngine;

public static class ArrayExtension
{

    public static T GetRandomFromArray<T>(T[] array)
    {
        if (array.Length > 0)
        {
            return array[Random.Range(0, array.Length)];
        }
        return default;
    }

}
