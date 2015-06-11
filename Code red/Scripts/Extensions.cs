using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    static class Extensions
    {
        public static T GetSafeComponent<T>(this GameObject obj) where T : MonoBehaviour
        {
            T component = obj.GetComponent<T>();

            if (component == null)
            {
                Debug.LogError("Expected to find component of type "
                   + typeof(T) + " but found none", obj);
            }

            return component;
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }

