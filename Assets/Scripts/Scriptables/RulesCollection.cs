using System;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Assets/Scriptable libraries/RulesCollection")]
public class RulesCollection : ScriptableObject
{
     [Serializable]
     public struct RoundRules
     {
          public int selectedCount;
          public int bunchesCount;
          public int bunchCapacity;
     }

     public RoundRules[] Collection;

     public RoundRules GetRandom()
     {
          if (Collection == null || Collection.Length == 0) throw new InvalidOperationException("RulesCollection :: GetRandom : No rules loaded!");
          return Collection[Random.Range(0, Collection.Length)];
     }
}