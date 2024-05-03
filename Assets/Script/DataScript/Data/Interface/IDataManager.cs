using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataManager<T>
{
    public List<T> GetDatas(string data);
    public T GetData(string[] datas);
}
