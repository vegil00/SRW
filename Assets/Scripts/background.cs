using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CreativeSpore.SuperTilemapEditor;

public class background : MonoBehaviour {
    public int unpassableId;
	// Use this for initialization
	void Start () {
        Tilemap map = transform.GetComponent<Tilemap>();
        
        for (int y = map.MinGridY; y <= map.MaxGridY; y++)
        {
            for (int x = map.MinGridX; x <= map.MaxGridX; x++)
            {
                Tile tile = map.GetTile(TilemapUtils.GetGridWorldPos(map, x, y));
                TileData data = new TileData(map.GetTileData(x, y));
                if(data.tileId== unpassableId)
                {

                    tile.paramContainer.SetParam("passable", false);
                }
                else
                {
                    tile.paramContainer.SetParam("passable", true);
                }
                tile.paramContainer.SetParam("race", 0);
                //tile.paramContainer.AddParam("moveCost", 1);
                //tile.paramContainer.AddParam("race", 0);
                //tile.paramContainer.AddParam("passable", true);
                //tile.paramContainer.RemoveParam("moveCost");
                //tile.paramContainer.RemoveParam("race");
                //tile.paramContainer.RemoveParam("passable");
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
