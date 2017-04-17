class PlayerData
{
    public int health;
    public int notes;
    public double[] pos;
    public double rot;
    public string lastLevel;

    public PlayerData(int h, int n, double[] p, double r, string ll)
    {
        health = h; notes = n; pos = p;
        rot = r; lastLevel = ll;
    }
}
