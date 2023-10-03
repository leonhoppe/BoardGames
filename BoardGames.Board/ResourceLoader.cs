namespace BoardGames.Board; 

public static class ResourceLoader<T> where T : IResource {
    public const string ResourceDirectory = "assets/";
    private static readonly IDictionary<string, IResource> Resources = new Dictionary<string, IResource>();

    public static T Add(string name, T resource) {
        Resources.Add(name, resource);
        return resource;
    }

    public static T Get(string name) {
        return (T)Resources[name];
    }

    public static void Remove(string name) {
        Resources.Remove(name);
    }
}

public interface IResource {
    void Load();
    void Bind();
    void Unbind();
    void Destroy();
}
