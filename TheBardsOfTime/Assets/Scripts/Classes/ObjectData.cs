[System.Serializable]
public class ObjectData
{
    public string name;
    public double[] pos;
    public double rot;
    public bool dormant;
    public bool destroyOnLoad;

    public ObjectData(string n, double[] p, double r, bool d, bool dol)
    {
        name = n; pos = p; rot = r; dormant = d; destroyOnLoad = dol;
    }
}