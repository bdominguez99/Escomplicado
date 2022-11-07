using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TripasDeGato
{
    public static class ExtensionMethods
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T GetObject<T>(string className)
        {
            T objectInstance = GameObject.Find(className).GetComponent<T>();
            return objectInstance;
        }

        public static string ToParenthesisListString<T>(this List<T> list)
        {
            var listString = "(";
            foreach (T element in list)
            {
                listString += element.ToString();
            }
            listString += ")";
            return listString;
        }
    }
}