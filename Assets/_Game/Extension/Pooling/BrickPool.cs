using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BrickPool
{
    private static Dictionary<CommonEnum.ColorType, Pool> poolInstance = new Dictionary<CommonEnum.ColorType, Pool>();

    // khoi tao pool moi
    public static void PreLoad(Brick prefab, int amount, Transform parent)
    {
        if (!prefab)
        {
            Debug.LogError("PREFAB IS EMPTY !");
            return;
        }

        if (!poolInstance.ContainsKey(prefab.GetColorType()) || poolInstance[prefab.GetColorType()] == null)
        {
            Pool p = new Pool();
            p.PreLoad(prefab, amount, parent);
            poolInstance[prefab.GetColorType()] = p;
        }
    }

    // lay phan tu ra tu pool
    public static T Spawn<T>(CommonEnum.ColorType colorType, Vector3 pos, Quaternion rot) where T : Brick
    {
        if (!poolInstance.ContainsKey(colorType))
        {
            Debug.LogError(colorType + " IS NOT PRELOAD!");
            return null;
        }

        return poolInstance[colorType].Spawn(pos, rot) as T;
    }

    // tra pha tu vao trong pool
    public static void Despawn(Brick brick)
    {
        if (!poolInstance.ContainsKey(brick.GetColorType()))
        {
            Debug.LogError(brick.GetColorType() + " IS NOT PRELOAD!");
        }

        poolInstance[brick.GetColorType()].Despawn(brick);
    }

    // thu thap phan tu
    public static void Collect(CommonEnum.ColorType colorType)
    {
        if (!poolInstance.ContainsKey(colorType))
        {
            Debug.LogError(colorType + " IS NOT PRELOAD!");
        }
        poolInstance[colorType].Collect();
    }

    // thu thap tat ca phan tu
    public static void CollectAll()
    {
        foreach(var item in poolInstance.Values)
        {
            item.Collect();
        }
    }

    // destroy 1 pool
    public static void Release(CommonEnum.ColorType colorType)
    {
        if (!poolInstance.ContainsKey(colorType))
        {
            Debug.LogError(colorType + " IS NOT PRELOAD!");
        }
        poolInstance[colorType].Release();
    }

    // destroy tat ca pools
    public static void ReleaseAll()
    {
        foreach(var item in poolInstance.Values)
        {
            item.Release();
        }
    }

    public static Pool GetPool(CommonEnum.ColorType color) => poolInstance[color];
}

public class Pool
{
    Transform parent;
    Brick prefab;

    Queue<Brick> inactives = new Queue<Brick>();    

    List<Brick> actives = new List<Brick>();

    // khoi tao pool
    public void PreLoad(Brick prefab, int amount, Transform parent)
    {
        this.parent = parent;
        this.prefab = prefab;

        for (int i = 0; i < amount; i++)
        {
            Despawn(GameObject.Instantiate(prefab, parent));
        }
    }

    // lay phan tu ra tu pool
    public Brick Spawn(Vector3 pos, Quaternion rot)
    {
        Brick brick;

        if (inactives.Count <= 0)
        {
            brick = GameObject.Instantiate(prefab, parent);
        }
        else
        {
            brick = inactives.Dequeue();
        }

        brick.TF.SetPositionAndRotation(pos, rot);
        actives.Add(brick); 
        brick.gameObject.SetActive(true);

        return brick;
    }

    // tra phan tu vao trong pool
    public void Despawn(Brick brick)
    {
        if (brick != null && brick.gameObject.activeSelf)
        {
            actives.Remove(brick);
            inactives.Enqueue(brick);
            brick.gameObject.SetActive(false);
        }
    }

    // thu thap tat ca cac phan tu dang dung ve pool
    public void Collect()
    {
        while (actives.Count > 0)
        {
            Despawn(actives[0]);
        }
    }

    // destroy tat ca phan tu
    public void Release()
    {
        Collect();

        while (inactives.Count > 0)
        {
            GameObject.Destroy(inactives.Dequeue().gameObject);
        }
        inactives.Clear();
    }

    public Queue<Brick> InactiveBricks() => inactives;
}
