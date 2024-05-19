using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveGrass : MonoBehaviour
{
    public Material GrassMaterial;
    public Transform player;
    private GameObject character;

    
    void Start()
    {
        
        character = GameObject.FindWithTag("Player");
    }
    void MoveGrass()
    {
        //m.SetVector("_PlayerPos", player.position);
        if(character != null)
        {
            player = character.transform;
            GrassMaterial.SetVector("_PlayerPos", player.position);
            
        }
        else{
            character = GameObject.FindWithTag("Player");
        }
        
    }

    private void OnDrawGizmos()
    {
        MoveGrass();
    }

    private void Update() 
    {
        //character = GameObject.FindWithTag("Player");
        MoveGrass();
    }
}
