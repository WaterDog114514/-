using System.Collections.Generic;


[System.Serializable]
public class TowerInfoContainer : DataBaseContainer
{
	public Dictionary<int, TowerInfo> dataDic = new Dictionary<int, TowerInfo>();
}