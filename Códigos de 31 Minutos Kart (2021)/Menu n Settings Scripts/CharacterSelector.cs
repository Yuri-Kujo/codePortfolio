using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public bool isCircuitScene;
    public static int chosenCharID;
    public List<GameObject> playerPrefabs;
    public List<GameObject> botPrefabs;
    public Transform playerSpawn;
    public Transform[] botSpawns;
    
    public void Tulio()
    {
        chosenCharID = 5;
    }
    public void Guaripolo()
    {
        chosenCharID = 1;
    }
    public void Juanin()
    {
        chosenCharID = 2;
    }
    public void Mhugo()
    {
        chosenCharID = 3;
    }
    public void Policarpo()
    {
        chosenCharID = 4;
    }
    public void Bodoque()
    {
        chosenCharID = 0;
    }

    private void Awake()
    {
        if(isCircuitScene)
        {
            Instantiate(playerPrefabs[chosenCharID], playerSpawn.position, playerSpawn.rotation);
            botPrefabs.RemoveAt(chosenCharID);
            for(int i = 0; i < 6; i++)
            {
                int x = Random.Range(0, botPrefabs.Count);
                Instantiate(botPrefabs[x], botSpawns[i].position, botSpawns[i].rotation);
                botPrefabs.RemoveAt(x);
            }
        }
    }
}
