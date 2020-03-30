using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Assets/Scriptable libraries/Cards")]
public class CardConfigsCollection : ScriptableObject
{
     public string prefabName;
     public string backgroundSpriteName;
     
     [Serializable]
     public struct CardConfig
     {
          public int id;
          public string foregroundSpriteName;
     }

     public CardConfig[] collection;

     public List<CardConfig> GetRandomConfigs(int count)
     {
          var result = new List<CardConfig>();
          while (result.Count != count)
          {
               var index = Random.Range(0, collection.Length);
               var config = collection[index];
               if (!result.Contains(collection[index]))
               {
                    result.Add(config);
               }
          }
          return result;
     }
     
}