
public abstract class BaseProxy
{
    public const string NAME = "Proxy";

    protected object Data { get; set; }

    public string ProxyName
    {
        get;
        private set;
    }

    public BaseProxy(string proxyName, object data = null)
    {
        ProxyName = proxyName ?? NAME;
        Data = data;
    }

    public abstract void Register();//注册数据

    public virtual void Remove() { }//移除数据

    protected virtual void Save() { }//存储数据

}
