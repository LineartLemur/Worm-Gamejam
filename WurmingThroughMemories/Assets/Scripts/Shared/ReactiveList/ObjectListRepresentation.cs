using System;
using System.Collections.Generic;
using System.Linq;
using Shared.ReactiveList;
using UniRx;

public class ObjectListRepresentation<TData, TRepresentation> : ListListener<TData> where TRepresentation : IDisposable {
    private Dictionary<TData, TRepresentation> elements = new Dictionary<TData, TRepresentation>();
    private Func<TData,TRepresentation> creator;

    private ReactiveCollection<TData> dataCollection;

    private Func<TData,bool> filter;
    public ObjectListRepresentation(ReactiveCollection<TData> data, Func<TData,TRepresentation> creator) : base() {
        this.creator = creator;
        this.dataCollection = data;
        this.filter = (e) => true;
        Init(data);
    }
    public ObjectListRepresentation(ReactiveCollection<TData> data, Func<TData,TRepresentation> creator,Func<TData,bool> filter) : base() {
        this.creator = creator;
        this.dataCollection = data;
        this.filter = filter;
        Init(data);
    }

    protected override void OnAdd(TData data) {
        if(!filter(data)) return;
        var e = creator(data);
        elements[data] = e;
        disposables.Add(e);
    }

    protected override void OnRemove(TData data) {
        if(!elements.TryGetValue(data, out var element)) return;
        element.Dispose();
        disposables.Remove(element);
        elements.Remove(data);
    }

    protected override void OnNotifyOrder(TData data, int order) {
        if(!elements.TryGetValue(data, out var element)) return;
        
        (element as ISetOrder)?.SetOrder(GetOrder(data));
    }

    private int GetOrder(TData data) {
        int i = 0;
        var h = data.GetHashCode();
        foreach (var d in dataCollection.Where(filter)) {
            if (d.GetHashCode() == h) return i;
            i++;
        }
        return -1;
    }
    
    protected override void OnClear() {
        foreach (var kv in elements) {
            kv.Value.Dispose();
            disposables.Remove(kv.Value);
        }
        elements.Clear();
    }
}