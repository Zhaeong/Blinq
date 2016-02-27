﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/*Monster that starts chasing the player once a partner mandala-eating monster 
  disables teleportation.*/
public class MandalaKillerMonster : MonoBehaviour {

    /* Movement parameters. */
    private float chaseTime;      //Number of seconds monster will chase for.
    private float chaseVelocity;  //How fast monster chases.
    private float chaseProximity; //How close player has to be to trigger chase.
    private float chaseLimitDist;     //Distance at which monster will quit chasing.

    private GameObject player;

    private NavMeshAgent monster;
    private Vector3 startPos;
    private Transform monsterTr;

    public bool attacking; //Public for access by the mandala-eating partner.
    private bool returning;

    // Use this for initialization
    void Start()     {
        chaseTime = 8;
        chaseVelocity = 5.5f;
        chaseProximity = 5;
        //chaseLimitDist = 10;

        player = GameObject.Find("PlayerAttached");

        monster = GetComponent<NavMeshAgent>();
        monsterTr = GetComponent<Transform>();
        startPos = monsterTr.position;

        attacking = false;
        returning = false;
    }


    // Update is called once per frame
    void Update()   {

        //Begins attack if player is close enough.
        if (!returning && !attacking && (Vector3.Distance(player.transform.position, monsterTr.position) < chaseProximity))  
            attacking = true;

        //Attack cycle.
        if (attacking)   {
            //Chase
            monsterTr.LookAt(player.transform);
            monsterTr.Translate(chaseVelocity * Vector3.forward * Time.deltaTime);

            StartCoroutine(setAttackLimit());
        }


        //Monster is back to its origin spot.
        if (Vector3.Distance(monsterTr.position, startPos) <= 1)
            returning = false;
    }


    void OnTriggerEnter(Collider obj)    {
        if (obj.gameObject.name == "PlayerAttached") {
            //Reset level
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    /* Takes monster back to starting point after some time. */
    IEnumerator setAttackLimit()    {
        //Stop chasing after a time limit.
        yield return new WaitForSeconds(chaseTime);
        attacking = false;

        //Send monster back to start point.
        returning = true;
        monster.destination = startPos;
    }

}