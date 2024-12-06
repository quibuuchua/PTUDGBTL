using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Cache
{
    private static Dictionary<Collider, Character> characters = new Dictionary<Collider, Character>();

    public static Character GenCharacter(Collider collider)
    {
        if (!characters.ContainsKey(collider))
        {
            characters.Add(collider, collider.GetComponent<Character>());
        }

        return characters[collider];
    }

    private static Dictionary<Collider, Brick> bricks = new Dictionary<Collider, Brick>();

    public static Brick GenBrick(Collider collider)
    {
        if (!bricks.ContainsKey(collider))
        {
            bricks.Add(collider, collider.GetComponent<Brick>());   
        }

        return bricks[collider];
    }

    private static Dictionary<Collider, Door> doors = new Dictionary<Collider, Door>();

    public static Door GetDoor(Collider collider)
    {
        if (!doors.ContainsKey(collider))
        {
            doors.Add(collider, collider.GetOrAddComponent<Door>());
        }

        return doors[collider];
    }
}
