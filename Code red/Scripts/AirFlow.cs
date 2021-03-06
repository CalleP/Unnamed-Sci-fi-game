﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;
public class AirFlow : MonoBehaviour {

	// Use this for initialization
	void Start () {
       allWP = GetComponentsInChildren<Waypoint>();
	}
	
	// Update is called once per frame
	void Update () {
        CalculateAirFlow();
        RedistributeAir();
	}


    public Waypoint[] allWP;
    private float drainRate = -0.3f;
    public List<List<Room>> RoomsWithAirPassage = new List<List<Room>>();
    public void CalculateAirFlow()
    {

        
        /*
                 stack.Push(start);

                while (stack.Count != 0)
                {
                    var tile = stack.Pop();

                    tile.Visited = true;

                    var west = MapGen.GetTileData(map, tile.X - 1, tile.Y);
                    var east = MapGen.GetTileData(map, tile.X + 1, tile.Y);
                    var north = MapGen.GetTileData(map, tile.X, tile.Y + 1);
                    var south = MapGen.GetTileData(map, tile.X, tile.Y - 1);

                    if (west.Visited == false && west.Type == TileType.Dirt) stack.Push(west);
                    if (east.Visited == false && east.Type == TileType.Dirt) stack.Push(east);
                    if (north.Visited == false && north.Type == TileType.Dirt) stack.Push(north);
                    if (south.Visited == false && south.Type == TileType.Dirt) stack.Push(south);

                }

                for (int y = 0; y < map.Length; y++)
                {
                    for (int x = 0; x < map[0].Length; x++)
                    {
                        var tile = map[y][x];
                        if (tile.Type == TileType.Dirt && tile.Visited == false)
                        {
                            tile.Type = TileType.Rock;
                        }
                    }
                }
            }
         */

        Dictionary<Waypoint, bool> visited = new Dictionary<Waypoint, bool>();
        Stack<Waypoint> stack = new Stack<Waypoint>();
        RoomsWithAirPassage.Clear();
        

        foreach (var room in GetComponentsInChildren<Room>())
        {
            room.connectedViaAir.Clear();
        }

        
        if (allWP.Length != 0)
        {
            stack.Push(allWP[0]);
        }


        foreach (var WP in allWP)
        {
            var roomsWithAirpassage = new List<Room>();

            stack.Push(WP);
            while (stack.Count != 0)
            {
                var curr = stack.Pop();


                if (visited.ContainsKey(curr) || curr.PowerWaypoint)
                {
                    continue;
                }
                var currRoomScript = curr.room.GetComponent<Room>();

                visited.Add(curr, true);

                
                foreach (var adj in curr.Adjecent)
                {
                    if (curr.AirTight)
                    {
                        continue;
                    }


                    var adjRoomScript = adj.room.GetComponent<Room>();
                    if (!visited.ContainsKey(adj) && !adj.AirTight)
                    {
                        if (adj.room != curr.room)
                        {
                            if (!currRoomScript.connectedViaAir.Contains(adjRoomScript)) 
                            {
                                currRoomScript.connectedViaAir.Add(adjRoomScript);
                                if (!roomsWithAirpassage.Contains(adjRoomScript)) 
                                {
                                    roomsWithAirpassage.Add(adjRoomScript);
                                }
                            }



                            if (!adjRoomScript.connectedViaAir.Contains(currRoomScript))
                            {
                                adjRoomScript.connectedViaAir.Add(currRoomScript);
                                if (!roomsWithAirpassage.Contains(currRoomScript))
                                {
                                    roomsWithAirpassage.Add(currRoomScript);
                                }
                            }
                           
                        }

                        stack.Push(adj);
                    }
                }

                if (!RoomsWithAirPassage.Contains(roomsWithAirpassage) && roomsWithAirpassage.Count != 0)
                {
                    RoomsWithAirPassage.Add(roomsWithAirpassage);
                }
                
            }
            
        }


    }

    public void RedistributeAir()
    {

        Dictionary<List<Room>, bool> Leaking = new Dictionary<List<Room>, bool>();
        foreach (var list in RoomsWithAirPassage)
        {
            foreach (var room in list)
            {
                var airLock = room.GetComponent<AirLock>();
                if (airLock != null && airLock.PhysicalAirLock.open)
                {
                    Leaking.Add(list, true);
                    foreach (var LeakingRoom in list)
                    {
                        int AirFilledAmt = 0;


                        foreach (var item in list)
                        {
                            if (item.Oxygen > 0)
                            {
                                AirFilledAmt++;
                            }
                        }
                        if (list.Count - AirFilledAmt > 0)
                        {
                            LeakingRoom.ChangeOxygen(-5f / AirFilledAmt);
                        }
                        else
                        {
                            LeakingRoom.ChangeOxygen(-5f);
                        }
                    }
                }
            }
            foreach (var PotentiallySealedRoom in list)
            {
                //TODO: IF connected to ventilation var airLock = room.GetComponent<Ventilation>();
                if (!Leaking.ContainsKey(list))
                {
                    float totalOxygen = 0;
                    foreach (var SealedRoom in list)
                    {
                        totalOxygen += SealedRoom.Oxygen;
                    }

                    foreach (var SealedRoom in list)
                    {
                        if (!InStabilizationCoroutines.Contains(list))
                        {
                            StartCoroutine(StabilizeAirLevel(list, SealedRoom.Oxygen = totalOxygen / list.Count));
                        }
                    }
                }
            }
        }
    }
        

    private List<List<Room>> InStabilizationCoroutines = new List<List<Room>>();
    
    IEnumerator StabilizeAirLevel(List<Room> rooms, float goal)
    {
        List<Tweener> tweeners = new List<Tweener>();
        InStabilizationCoroutines.Add(rooms);
        foreach (var room in rooms)
	    {
            tweeners.Add(HOTween.To(room, 1f, "Oxygen", goal, false));
	    }
        for (int i = 0; i < 11; i++)
        {
            if (!RoomsWithAirPassage.Contains(rooms))
            {
                InStabilizationCoroutines.Remove(rooms);
                foreach (var tween in tweeners)
                {
                    tween.Kill();
                }
                yield return null;
            }
            yield return new WaitForSeconds(.1f);
        }
        
       
    }

}
