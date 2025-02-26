using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public static NavigationManager instance;
    public Room startingRoom;
    public Room currentRoom;

    public Exit toKeyNorth; //needed to turn exit to visible from hidden

    private Dictionary<string, Room> exitRooms = new Dictionary<string, Room>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(startingRoom.description);
        toKeyNorth.isHidden = true;
        currentRoom = startingRoom;
        Unpack();
    }

    void Unpack()
    {
        string description = currentRoom.description;
        exitRooms.Clear(); //reset for next room
        foreach(Exit e in currentRoom.exits)
        {
            if (!e.isHidden)
            {
                description += " " + e.description;
                exitRooms.Add(e.direction.ToString(), e.room);
            }
        }

        InputManager.instance.UpdateStory(description);

    }

    public bool SwitchRooms(string direction)
    {
        if (exitRooms.ContainsKey(direction)) //if that exit exists
        {
            currentRoom = exitRooms[direction];
            InputManager.instance.UpdateStory("You go " + direction);
            Unpack();
            return true;
        }

        return false;
    }

    public bool TakeItem(string item)
    {
        if (item == "key" && currentRoom.hasKey)
            return true;
        else if (item == "orb" && currentRoom.hasOrb)
        {
            toKeyNorth.isHidden = false;
            return true;
        }
        else
            return false;
    }
}
