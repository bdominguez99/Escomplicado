using System.Collections;
using UnityEngine;

public class Pair<F,S>
{
    public Pair(F first, S second)
    {
        First = first;
        Second = second;
    }

    public F First;
    public S Second;
}