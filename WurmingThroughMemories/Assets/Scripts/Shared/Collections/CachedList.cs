using System;
using System.Collections.Generic;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
public class CachedListItemDisposable<T> : IDisposable {
    private CachedList<T> list;
    private T item;

    public void Set(CachedList<T> list, T item) {
        this.list = list;
        this.item = item;
    }
    public void Dispose() {
        if(list==null) return;
        list.Release(item);
        disposablePool.Release(this);
        list = null;
    }
    public static CachedList<CachedListItemDisposable<T>> disposablePool =
        new CachedList<CachedListItemDisposable<T>>(() => new CachedListItemDisposable<T>());
}
public class CachedList<T>
{
    

    
    public delegate T ItemMaker();

    public delegate void Preparer(T i);
    public delegate bool Validator(T i);

    ItemMaker newItemMaker;
    Preparer enabler;
    Preparer disabler;
    Validator validator;
    List<T> freeObjects = new List<T>();
    LinkedList<T> usedObjects = new LinkedList<T>();

    public CachedList(ItemMaker newitem, Preparer enabler = null, Preparer disabler = null, Validator validator = null)
    {
        newItemMaker = newitem;
        this.enabler = enabler;
        this.disabler = disabler;
        this.validator = validator;
    }

    public void Reset()
    {
        if (disabler != null)
        {
            foreach (var obj in usedObjects) {
                disabler(obj);
            }
        }

        freeObjects.AddRange(usedObjects);
        usedObjects.Clear();
    }

    public void Release(T item)
    {
        if (!usedObjects.Contains(item))
        {
            Debug.LogError("Error on releasing item from CachedList -> item not found");
            return;
        }
        usedObjects.Remove(item);
        
        if (validator != null) {
            if (!validator(item)) return;
        }
        
        freeObjects.Add(item);
        if (disabler != null)
        {
            disabler(item);
        }
    }

    public T GetItem(out IDisposable disposeInstance) {
        var item = GetItem();
        var newDisposable = CachedListItemDisposable<T>.disposablePool.GetItem();
        newDisposable.Set(this, item);
        disposeInstance = newDisposable;
        return item;
    }

    public T GetItem()
    {
        if (freeObjects.Count == 0)
        {
            AddObject();
        }

        T item = freeObjects[freeObjects.Count - 1];
        freeObjects.Remove(item);
        
        if (validator != null) {
            if (!validator(item)) return GetItem();
        }
        
        usedObjects.AddFirst(item);

        if (enabler != null)
        {
            enabler(item);
        }

        return item;
    }

    private T AddObject() {
        var item = newItemMaker();
        freeObjects.Add(item);
        return item;
    }

    public void DestroyAll(Preparer destroyer = null)
    {
        if (destroyer != null)
        {
            foreach (var obj in usedObjects) {
                destroyer(obj);
            }
            
            foreach (var obj in freeObjects) {
                destroyer(obj);
            }
        }

        usedObjects.Clear();
        freeObjects.Clear();
    }
    public void GrowPoolTo(int n) {
        var current = usedObjects.Count + freeObjects.Count;
        for (int i = 0; i < n-current; i++) {
            disabler(AddObject());
        }
    }
}

