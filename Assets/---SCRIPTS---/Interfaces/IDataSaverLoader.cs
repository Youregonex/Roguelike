
public interface IDataSaverLoader
{
    public void SaveData(string key, object data);
    public T LoadData<T>(string key);
}
