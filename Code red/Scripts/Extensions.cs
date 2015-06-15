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

    //Remove  first instances waypoints in a path that belongs to a room except the first waypoint, starts from path end.
    public static List<Waypoint> CullPath(this List<Waypoint> path, Room targetRoom)
    {

        int cullIndexA = -1;
        int cullLength = 0;
        var newRefList = new List<Waypoint>();
        foreach (var WP in path)
        {
            newRefList.Add(WP);
        }

        for (int i = path.Count - 1; i > 0; i--)
        {
            var currentWP = path[i];

            Waypoint nextWP = null;
            if (path.Count - (i+1) >= 0)
            {
                nextWP = path[i - 1];
            }

            if ((nextWP != null && nextWP.room.GetComponent<Room>() != targetRoom && currentWP.room.GetComponent<Room>() == targetRoom) && cullIndexA == -1)
            {
                cullIndexA = i;
                cullLength++;
                break;
            }
            if (currentWP.room.GetComponent<Room>() == targetRoom)
            {
                cullLength++;
            }
            




            
        }

        newRefList.RemoveRange(cullIndexA, cullLength);

        return newRefList;
    }
}


