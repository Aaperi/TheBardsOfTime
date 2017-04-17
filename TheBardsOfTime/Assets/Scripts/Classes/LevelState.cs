[System.Serializable]
class LevelState
{
    public string levelName;
    public bool completed;
    public ObjectData[] objects;

    public LevelState(string n, bool c, ObjectData[] o)
    {
        levelName = n; completed = c; objects = o;
    }
}
