using System;
using System.Collections.Generic;
using PepijnWillekens;
using PepijnWillekens.ManagerSystem;
using PepijnWillekens.Shared;
using Shared.ReactiveList;
using UniRx;
using UnityEngine;

// public class GameObjectListRepresentation<TData, TPrefab> : ListListener<TData>
//     where TPrefab : MonoBehaviour {
//     [Inject]
//     private GameObjectPool gameObjectPool;
//
//     private TPrefab prefab;
//     private Transform parent;
//
//     private Dictionary<TData, TPrefab> elements = new Dictionary<TData, TPrefab>();
//
//     public GameObjectListRepresentation(ReactiveCollection<TData> data, TPrefab prefab, Transform parent) : base() {
//         this.parent = parent;
//         DependancyCache.InjectDependencies(this);
//         this.prefab = prefab;
//         Init(data);
//     }
//
//     protected override void OnAdd(TData data) {
//         var i = gameObjectPool.GetInstance(prefab);
//         i.transform.SetParent(parent, false);
//         InitializeElement(i, data);
//         elements[data] = i;
//     }
//
//     protected virtual void InitializeElement(TPrefab element, TData data) {
//         if (element is IInitable<TData> initable) {
//             initable.Init(data);
//         }
//     }
//
//     protected override void OnRemove(TData data) {
//         gameObjectPool.Release(elements[data]);
//         elements.Remove(data);
//     }
// }
//
// public class GameObjectListRepresentation<TData, TPrefab, TContext> : GameObjectListRepresentation<TData, TPrefab>
//     where TPrefab : MonoBehaviour, IInitable<TData, TContext> {
//     private TContext context;
//
//     public GameObjectListRepresentation(
//         ReactiveCollection<TData> data,
//         TPrefab prefab,
//         Transform parent,
//         TContext context
//     ) : base(data, prefab, parent) {
//         this.context = context;
//     }
//
//     protected override void InitializeElement(TPrefab element, TData data) {
//         element.Init(data, context);
//     }
// }


public class GameObjectListRepresentation<TData, TPrefab> : ObjectListRepresentation<TData, GameObjectListRepresentation<TData,TPrefab>.Container> 
    where TPrefab : MonoBehaviour {
    public class Container : IDisposable, ISetOrder {
        private GameObjectPool gameObjectPool;
        public TPrefab element;
        
        public Container(TData data, TPrefab prefab, Transform parent) {
            gameObjectPool = M.Get<GameObjectPool>();
            element = gameObjectPool.GetInstance(prefab);
            element.transform.SetParent(parent, false);
            
            if (element is IInitable<TData> initable) {
                initable.Init(data);
            }
        }
        public Container(TData data, Func<TData,TPrefab> prefab, Transform parent) {
            gameObjectPool = M.Get<GameObjectPool>();
            element = gameObjectPool.GetInstance(prefab(data));
            element.transform.SetParent(parent, false);
            
            if (element is IInitable<TData> initable) {
                initable.Init(data);
            }
        }
        public void Dispose() {
            gameObjectPool.Release(element);
        }

        public void SetOrder(int order) {
            element.transform.SetSiblingIndex(order);
            (element as ISetOrder)?.SetOrder(order);
        }
    }

    private Dictionary<TData, TPrefab> elements = new Dictionary<TData, TPrefab>();

    public GameObjectListRepresentation(ReactiveCollection<TData> data, TPrefab prefab, Transform parent) 
        : base( data, d => new Container(d, prefab, parent)) {
    }
    public GameObjectListRepresentation(ReactiveCollection<TData> data, TPrefab prefab, Transform parent, Func<TData,bool> filter) 
        : base( data, d => new Container(d, prefab, parent), filter) {
    }
    public GameObjectListRepresentation(ReactiveCollection<TData> data, Func<TData,TPrefab> prefab, Transform parent) 
        : base( data, d => new Container(d, prefab, parent)) {
    }
    public GameObjectListRepresentation(ReactiveCollection<TData> data, Func<TData,TPrefab> prefab, Transform parent, Func<TData,bool> filter) 
        : base( data, d => new Container(d, prefab, parent), filter) {
    }
}

public class GameObjectListRepresentation<TData, TPrefab, TContext> : ObjectListRepresentation<TData,
    GameObjectListRepresentation<TData, TPrefab>.Container>
    where TPrefab : MonoBehaviour, IInitable<TData, TContext> {
    public GameObjectListRepresentation(
        ReactiveCollection<TData> data,
        TPrefab prefab,
        Transform parent,
        TContext context
    ) : base( data, d => {
        var c = new GameObjectListRepresentation<TData, TPrefab>.Container(d, prefab, parent);
        c.element.Init(d, context);
        return c;
    }) {
        
    }
    public GameObjectListRepresentation(
        ReactiveCollection<TData> data,
        TPrefab prefab,
        Transform parent,
        TContext context,
        Func<TData,bool> filter
    ) : base( data, d => {
        var c = new GameObjectListRepresentation<TData, TPrefab>.Container(d, prefab, parent);
        c.element.Init(d, context);
        return c;
    }, filter) {
        
    }
    public GameObjectListRepresentation(
        ReactiveCollection<TData> data,
        Func<TData,TPrefab> prefab,
        Transform parent,
        TContext context
    ) : base( data, d => {
        var c = new GameObjectListRepresentation<TData, TPrefab>.Container(d, prefab, parent);
        c.element.Init(d, context);
        return c;
    }) {
        
    }
    public GameObjectListRepresentation(
        ReactiveCollection<TData> data,
        Func<TData,TPrefab> prefab,
        Transform parent,
        TContext context,
        Func<TData,bool> filter
    ) : base( data, d => {
        var c = new GameObjectListRepresentation<TData, TPrefab>.Container(d, prefab, parent);
        c.element.Init(d, context);
        return c;
    }, filter) {
        
    }
}