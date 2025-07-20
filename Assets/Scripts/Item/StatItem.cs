using UnityEngine;
using UnityEngine.UI;

public class StatItem : MonoBehaviour
{
    public Text statNameText;
    public Text statValueText;

    private Stat stat;

    public void SetStat(Stat stat)
    {
        this.stat = stat;
        statNameText.text = stat.name;
        statValueText.text = stat.value.ToString();
    }
}

[System.Serializable]
public class Stat
{
    public string name;
    public int value;
}
