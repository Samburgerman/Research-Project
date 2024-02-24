using System.Collections.Generic;

[System.Serializable]
public struct GameState
{
    public int turnNumber;
    public List<PlayerData> playerDatas;

    public GameState(int turnNumber,List<PlayerData> playerDatas)
    {
        this.playerDatas=new();
        this.turnNumber=turnNumber;
        foreach(PlayerData playerData in playerDatas)
            this.playerDatas.Add(playerData);
    }

    public override string ToString()
    {
        string playerDatasMessage = "";
        foreach(PlayerData playerData in playerDatas)
            playerDatasMessage+=playerData.ToString();
        return "Turn: "+turnNumber+" PlayerDatasMessage: "+playerDatasMessage;
    }
}

[System.Serializable]//need a wrapper class for the List<GameState> so it can be .json-ed
public struct GameStates
{
    public List<GameState> gameStatesList;

    public GameStates(List<GameState> gameStateList)
    { gameStatesList=gameStateList; }

    public void Add(GameState gameState) => gameStatesList.Add(gameState);
}