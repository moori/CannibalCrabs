using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Globalization;
using System;

public static class Extensions
{
    private static System.Random rng = new System.Random();

    public static List<T> Shuffle<T>(this List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }

    public static void DelayedAction(this MonoBehaviour mb, float delay, Action onComplete)
    {
        mb.StartCoroutine(DelayedCoroutine(delay, onComplete));
    }
    public static IEnumerator DelayedCoroutine(float delay, Action onComplete)
    {
        yield return new WaitForSeconds(delay);
        onComplete();
    }

    public static T GetRandom<T>(this List<T> list)
    {
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    public static T GetNext<T>(this List<T> list, T value)
    {
        var index = list.IndexOf(value);
        return (index + 1) >= list.Count ? list[0] : list[index + 1];
    }

    public static string RemoveDiacritics(this string text)
    {
        string formD = text.Normalize(NormalizationForm.FormD);
        StringBuilder sb = new StringBuilder();

        foreach (char ch in formD)
        {
            UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
            if (uc != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(ch);
            }
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}
