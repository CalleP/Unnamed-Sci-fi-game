using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Helper
{

    public static IEnumerable<MonoBehaviour> GetClosestObjectsInOrder(IEnumerable<MonoBehaviour> a, IEnumerable<MonoBehaviour> b)
    {


        List<DistanceGameObj> ClosestGameObjs = new List<DistanceGameObj>();

        foreach (var objA in a)
        {
            var HelperObj = new DistanceGameObj(objA);
            foreach (var objB in b)
            {
                var distance = Vector3.Distance(objA.gameObject.transform.position, objB.transform.position);
                if (distance < HelperObj.Distance)
                {
                    HelperObj.Distance = distance;
                }
            }
            ClosestGameObjs.Add(HelperObj);

        }

        ClosestGameObjs.Sort();

        foreach (var item in ClosestGameObjs)
        {
            yield return item.GObj;
        }
       
    }

    private class DistanceGameObj : IComparable<DistanceGameObj>
    {
        public MonoBehaviour GObj;
        public float Distance;

        public DistanceGameObj(MonoBehaviour gObj)
        {
            this.GObj = gObj;
            Distance = Mathf.Infinity;
        }

        public int CompareTo(DistanceGameObj other)
        {
            if (other.Distance == Distance)
            {
                return 0;
            }
            if (Distance < other.Distance)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }
}

