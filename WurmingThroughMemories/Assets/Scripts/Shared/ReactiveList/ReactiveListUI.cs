using System;
using System.Collections.Generic;
using PepijnWillekens;
using PepijnWillekens.ManagerSystem;
using PepijnWillekens.Shared;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

public abstract class ReactiveListUI<T, TE> : AutoInit, IResettable where T :  MonoBehaviour, IInitable<TE> {
    
    [Inject]
    private GameObjectPool pool;

    [Title("Assets")]
    public T elementUiPrefab;
    
    [Title("Components")]
    public Transform elementsParent;

    protected List<T> items = new List<T>();
    protected override void OnInit() {
        base.OnInit();
        //RefreshListOfPrizes();
        GetNotifier().Subscribe(RefreshListOfPrizes).AddTo(this);
    }

    private void RefreshListOfPrizes(Unit u = new Unit()) {
        DestroyCurrent();
        foreach (var element in GetElements()) {
            AddElementUI(element);
        }

        OnRefreshed();
    }

    public virtual void OnRefreshed(){}

    protected abstract IObservable<Unit> GetNotifier();
    protected abstract IEnumerable<TE> GetElements();
    private void AddElementUI(TE element) {
        if (element == null) return;
        var ui = pool.GetInstance(elementUiPrefab);
        ui.transform.SetParent(elementsParent,false);
        ui.transform.SetSiblingIndex(elementsParent.childCount);
        ui.Init(element);
        items.Add(ui);
    }
    
    private void DestroyCurrent() {
        foreach (var item in items) {
            pool.Release(item);
        }
        items.Clear();
    }

    public void Reset() {
        DestroyCurrent();
    }
}