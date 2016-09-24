﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GenerateGrid : MonoBehaviour {

    public GameObject FieldAsset;
    public GameObject BoxAsset;
    public GameObject CrossAsset;
    public GameObject PlayerAsset;

    public static int AreaSize = 10 ;
    public int NumberOfBoxes = 3;
    public List<Color> TableOfColor = new List<Color> { Color.blue, Color.red, Color.yellow, Color.green };
    public static Tile [,] TableOfTiles;
    public static List<GameObject> Boxes, Crosses;


    public class Tile : MonoBehaviour
    {
        int x, y;
        public GameObject Asset { get; set; }
        public bool isLocked { get; set; }

        //Contructors
        public Tile(int x, int y, GameObject a)
        {
            this.x = x;
            this.y = y;
            Asset = Instantiate(a, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        }
        public Tile(int x, int y)
        {
            this.x = x;
            this.y = y;
            Asset = new GameObject();
        }
        public Tile()
        {
            Asset = new GameObject();
        }
        //End of constructors

        
        static public Vector3 RandomField()
        {
            int x, y;
            do
            {
                x = Random.Range(1, AreaSize -1);
                y = Random.Range(1, AreaSize -1);
            } while (IsLocked(x, y));
            TableOfTiles[x, y].isLocked = true;
            return new Vector3(x,y,0);
        }

        static bool IsLocked(int x, int y)
        {
            return TableOfTiles[x, y].isLocked;
        }

    };

    void Start () {

        TableOfTiles = new Tile[AreaSize, AreaSize];
        Boxes = new List<GameObject>();
        Crosses = new List<GameObject>();

        Cataloges.ListOfCataloges = Cataloges.GenerateCataloges("Boxes", "Tiles", "Crosses");
        GenerateTiles();
        GenerateBoxesAndCrosses();
        GeneratePlayer();
	}
	

	void Update () {
        if(GameIsOver())
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GenerateTiles()
    {
        for (int i = 0; i < AreaSize; i++)
            for (int j = 0; j < AreaSize; j++)
            {
                Tile field = new Tile(i, j, FieldAsset);
                Cataloges.AddToCatalog("Tiles", field.Asset);
                TableOfTiles[i, j] = field;
            }
    }

    void GenerateBoxesAndCrosses()
    {
        for(int i = 0; i< NumberOfBoxes; i++)
        {

            int IndexOfColor = Random.Range(0, TableOfColor.Count);

            GameObject Box = Instantiate(BoxAsset, Tile.RandomField(), Quaternion.identity) as GameObject;
            Cataloges.AddToCatalog("Boxes", Box);            
            Box.GetComponent<Renderer>().material.color = TableOfColor[IndexOfColor];
            Boxes.Add(Box); // It is not a catalog. It a List to help!

            GameObject Cross = Instantiate(CrossAsset, Tile.RandomField(), Quaternion.Euler(90,0,0)) as GameObject;
            Cataloges.AddToCatalog("Crosses", Cross);
            Cross.GetComponent<Renderer>().material.color = TableOfColor[IndexOfColor];
            Crosses.Add(Cross); // It is not a catalog. It a List to help!

            TableOfColor.RemoveAt(IndexOfColor);
        }
    }

    void GeneratePlayer()
    {
        GameObject Player = Instantiate(PlayerAsset, Tile.RandomField(), Quaternion.Euler(90,0,0)) as GameObject;
    }

    bool GameIsOver()
    {
        for(int i = 0; i <Boxes.Count; i++)
        {
            if (Boxes[i].transform.position != Crosses[i].transform.position) return false;
        }
        return true;
    }

    public class Cataloges
    {
        public static List<GameObject> ListOfCataloges;

        public static List<GameObject> GenerateCataloges(params string[] t)
        {
            if(ListOfCataloges == null)
                ListOfCataloges = new List<GameObject>();

            foreach (string name in t)
            {
                GameObject ob = new GameObject();
                ob.name = name;
                ListOfCataloges.Add(ob);
            }
            return ListOfCataloges;
        }

        public static bool AddToCatalog(string s, GameObject GameObjectToAdd)
        {
            if(ListOfCataloges.Count > 0)
            foreach(GameObject Catalog in ListOfCataloges)
            {
                if (Catalog.name == s)
                {
                    GameObjectToAdd.transform.parent = Catalog.transform;
                    return true;
                }
            }
            //if there is not catalog for that name 
            GenerateCataloges(s);
            GameObjectToAdd.transform.parent = ListOfCataloges[ListOfCataloges.Count-1].transform;
            return false;
        }
    };

    

}
