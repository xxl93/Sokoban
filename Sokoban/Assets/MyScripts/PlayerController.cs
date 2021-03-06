﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    int x = 0;
    int y = 0;
    static int AreaSize = 10;

    PlayerController()
    {
        AreaSize = GenerateGrid.AreaSize;
    }

    // Use this for initialization
    void Start () {
        //AreaSize = GenerateGrid.AreaSize;
        //Debug.Log("AreaSize: " + AreaSize);
    }
    
    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Colider pos: " + other.transform.position + "x,y" + x + "," + y);
        other.transform.Translate(new Vector3(x, y, 0));
    }

    // Update is called once per frame
    void FixedUpdate () {
        Movement();
        //Invoke("Movement", 2);
    }

    void Movement()
    {
        x = 0; y = 0;

        if (Input.GetKeyDown(KeyCode.D)) x = 1;
        if (Input.GetKeyDown(KeyCode.A)) x = -1;
        if (Input.GetKeyDown(KeyCode.W)) y = 1;
        if (Input.GetKeyDown(KeyCode.S)) y = -1;

 
        //Debug.Log("current pos: , x, z: "+ transform.position + x + "," + y);
        Vector3 targetPosition = (transform.position+ new Vector3(x, y, 0));
        if(MoveIsPossible(transform.position, targetPosition, AI.CastFromGameObjectListToPositionVector(GameObject.FindGameObjectsWithTag("Box"))))
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, 10);   
    }



    public static bool MoveIsPossible(Vector3 currentPos, Vector3 targetPos, Vector3 [] Boxes)
    {
        //PlayerOutOfBound
        if (ObjectOutOBound(targetPos)) return false;
        
        //Box: outofbound or secondbox on target position.
        Vector3 positionBehindBox = targetPos + (targetPos - currentPos);
        if (BoxAtPos(Boxes, targetPos) 
            && (BoxAtPos(Boxes, positionBehindBox) || ObjectOutOBound(positionBehindBox) || WallOnPos(positionBehindBox)))
                return false;

        if (WallOnPos(targetPos)) return false;

        return true;
    }
    public static bool ObjectOutOBound(Vector3 targetPos)
    {
        if (targetPos.x >= 0 && targetPos.x <AreaSize && targetPos.y >= 0 && targetPos.y < AreaSize)
            return false;

        return true;
    }
    public static bool BoxAtPos(Vector3 [] Boxes, Vector3 posToCheck)
    {
        foreach(Vector3 b in Boxes)
        {
            if (b == posToCheck) return true;
        }
        return false; 
    }
    public static bool WallOnPos(Vector3 targetPos)
    {
        Vector3[] wallList = AI.CastFromGameObjectListToPositionVector(GameObject.FindGameObjectsWithTag("Wall"));

        foreach(Vector3 v in wallList)
        {
            if (v == targetPos) return true;
        }
        return false;
    }

    public static int iBoxAtPos(Vector3[] Boxes, Vector3 posToCheck)
    {
        for(int i = 0; i < Boxes.Length; i++)
        {
            if (Boxes[i] == posToCheck) return i;
        }
        return -1;
    }

}
