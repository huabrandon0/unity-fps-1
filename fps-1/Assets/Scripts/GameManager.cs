using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public MatchSettings matchSettings;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError(GetType() + "More than one GameManager in scene");
        }
        else
        {
            instance = this;
        }
    }

    #region Player tracking

    private static Dictionary<uint, Player> players = new Dictionary<uint, Player>();
    private const string PLAYER_ID_PREFIX = "Player ";

    public static void RegisterPlayer(uint netId, Player player)
    {
        players.Add(netId, player);
        player.transform.name = PLAYER_ID_PREFIX + netId;
    }

    public static void DeregisterPlayer(uint netId)
    {
        players.Remove(netId);
    }

    public static Player GetPlayer(uint netId)
    {
        return players[netId];
    }

    //void OnGUI()
    //{
    //    GUILayout.BeginArea(new Rect(200, 200, 200, 500));
    //    GUILayout.BeginVertical();

    //    foreach(uint netId in players.Keys)
    //    {
    //        GUILayout.Label(netId + " - " + players[netId].transform.name);
    //    }

    //    GUILayout.EndVertical();
    //    GUILayout.EndArea();
    //}

    #endregion
}
