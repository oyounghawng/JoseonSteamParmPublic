using System;

public class Singleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)Activator.CreateInstance(typeof(T));
            }
            return instance;
        }
    }
    public virtual void Init()
    {

    }
}
