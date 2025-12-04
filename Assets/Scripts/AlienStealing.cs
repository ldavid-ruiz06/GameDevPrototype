using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlienStealing : MonoBehaviour
{
    public enum StolenPart
    {
        Nothing,
        Head,
        Arm,
        Leg
    };


    public enum AlienState
    {
        Idle, // idle
        Escape, // running away from the player
        ChasePlayer, // sneaking on the player to steal something
        StealPlayer, // steal item from player
        RunAway, // running away with item
        Taunt, // idle with item
        Defeated // killed
    };

    
    public AlienState currentState;
    public StolenPart stolenPart;

    void Idle()
    {
        print("I'm idle.");
    }

    void Escape()
    {
        print("I'm escaping.");
    }

    void ChasePlayer()
    {
        print("I'm chasing the player.");
    }

    void StealPlayer()
    {
        // check if the alien doesn't have anything
        if(stolenPart == StolenPart.Nothing)
        {
            print("I'm stealing the player.");

            // choose a random part to steal
            int rnd = Random.Range(0,3);
            switch (rnd)
            {
                case 0: stolenPart = StolenPart.Head; break;
                case 1: stolenPart = StolenPart.Arm; break;
                case 2: stolenPart = StolenPart.Leg; break;
            }

            // notify which part was stolen
            if(stolenPart == StolenPart.Head)
            {
                print("Stole the head.");
                
            }
            if(stolenPart == StolenPart.Arm)
            {
                print("Stole the arm.");
                
            }
            if (stolenPart == StolenPart.Leg)
            {
                print("Stole the leg.");
            }
        }else print("Already stolen something.");


        // change state to run away
        currentState = AlienState.RunAway;
    }

    void RunAway()
    {
        print("I'm running away from the player.");
    }

    void Taunt()
    {
        print("I'm taunting the player.");
    }

    void Defeat()
    {
        print("I'm defeated.");
    }


    void Update()
    {
        switch (currentState)
        {
            case AlienState.Idle: Idle(); break;
            case AlienState.Escape: Escape(); break;
            case AlienState.ChasePlayer: ChasePlayer(); break;
            case AlienState.StealPlayer: StealPlayer(); break;
            case AlienState.RunAway: RunAway(); break;
            case AlienState.Taunt: Taunt(); break;
            case AlienState.Defeated: Defeat(); break;
        }
    }   
    
}
