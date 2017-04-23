[System.Serializable]
public class LevelState
{
    public int buildID;
    public string levelName;
    public bool completed;
    public ObjectData[] objects;

    public LevelState(int bi, string n, bool c, ObjectData[] o)
    {
        buildID = bi;  levelName = n; completed = c; objects = o;
    }
}
