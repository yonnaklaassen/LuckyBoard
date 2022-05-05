using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    private TileType GetTileType(string currentPos)
    {
        if (currentPos.Equals("DamageTile"))
        {
            return TileType.RedTile;
        }
        else if (currentPos.Equals("HealthTile"))
        {
            return TileType.GreenTile;
        }
        else if (currentPos.Equals("TeleportTile"))
        {
            return TileType.PurpleTile;
        }
        else if (currentPos.Equals("RollAgainTile"))
        {
            return TileType.BlueTile;
        }
        else if (currentPos.Equals("BattleTile"))
        {
            return TileType.BlackTile;
        }

        return TileType.YellowTile;
    }

    private void OnEnable()
    {
        Player.getTileType += GetTileType;
    }

    private void OnDisable()
    {
        Player.getTileType -= GetTileType;
    }

}
