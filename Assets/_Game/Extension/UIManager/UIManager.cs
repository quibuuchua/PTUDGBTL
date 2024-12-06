using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private Transform parent;

    Dictionary<System.Type, UICanvas> canvasesActive = new Dictionary<System.Type, UICanvas>();
    Dictionary<System.Type, UICanvas> canvasPrefabs = new Dictionary<System.Type, UICanvas>();

    private void Awake()
    {
        // load ui prefab tu resources
        UICanvas[] prefabs = Resources.LoadAll<UICanvas>("UI/");
        for (int i = 0; i < prefabs.Length; i++)
        {
            canvasPrefabs.Add(prefabs[i].GetType(), prefabs[i]);
        }
    }

    // mo canvas
    public T OpenUI<T>() where T : UICanvas
    {
        T canvas = GetUI<T>();

        canvas.Setup();
        canvas.Open();

        return canvas;
    }

    // dong canvas sau time(s)
    public void CloseUI<T>(float time) where T : UICanvas
    {
        if (IsLoaded<T>())
        {
            canvasesActive[typeof(T)].Close(time);
        }
    }

    // dong canvas truc tiep
    public void CloseUIDirectly<T>() where T : UICanvas
    {
        if (IsLoaded<T>())
        {
            canvasesActive[typeof(T)].CloseDirectly();
        }
    }

    // kiem tra canvas da duoc tao hay chua
    public bool IsLoaded<T>() where T : UICanvas
    {
        return canvasesActive.ContainsKey(typeof(T)) && canvasesActive[typeof(T)] != null;
    }

    // kiem tra canvas dc active hay chua
    public bool IsOpened<T>() where T : UICanvas
    {
        return IsLoaded<T>() && canvasesActive[typeof(T)].gameObject.activeSelf;
    }

    // lay active canvas
    public T GetUI<T>() where T : UICanvas
    {
        if (!IsLoaded<T>())
        {
            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab, parent);
            canvasesActive[typeof(T)] = canvas; 
        }

        return canvasesActive[typeof(T)] as T;
    }

    // get prefab
    private T GetUIPrefab<T>() where T : UICanvas
    {
        return canvasPrefabs[typeof(T)] as T;
    }

    // dong tat ca canvas
    public void CloseAll()
    {
        foreach (var canvas in canvasesActive)
        {
            if (canvas.Value != null && canvas.Value.gameObject.activeSelf)
            {
                canvas.Value.Close(0);
            }
        }
    }
}
