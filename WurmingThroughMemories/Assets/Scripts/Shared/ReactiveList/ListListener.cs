using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class ListListener<TData> : IDisposable {
        protected readonly List<IDisposable> disposables = new List<IDisposable>();
        private bool inited;

        protected ListListener() { }

        protected ListListener(ReactiveCollection<TData> data) {
            Init(data);
        }

        protected void Init(ReactiveCollection<TData> data) {
            if (inited) return;
            inited = true;
            int i = 0;
            foreach (var d in data) {
                OnAdd(d);
                OnNotifyOrder(d, i);
                i++;
            }

            data.ObserveAdd().Subscribe(e => {
                OnAdd(e.Value);
                for (int index = e.Index; index < data.Count; index++) {
                    OnNotifyOrder(data[index], index);
                }
            }).AddTo(disposables);
            data.ObserveRemove().Subscribe(e => {
                OnRemove(e.Value);
                for (int index = e.Index; index < data.Count; index++) {
                    OnNotifyOrder(data[index], index);
                }
            }).AddTo(disposables);
            data.ObserveReplace().Subscribe(e => {
                OnRemove(e.OldValue);
                OnAdd(e.NewValue);
                OnNotifyOrder(e.NewValue, e.Index);
            }).AddTo(disposables);
            data.ObserveMove().Subscribe(e => {
                var lower = Mathf.Min(e.OldIndex, e.NewIndex);
                var higher = Mathf.Max(e.OldIndex, e.NewIndex);
                for (int index = lower; index <= higher; index++) {
                    OnNotifyOrder(data[index], index);
                }
            }).AddTo(disposables);
            data.ObserveReset().Subscribe(_ => {
                OnClear();
            }).AddTo(disposables);
        }

        protected abstract void OnAdd(TData data);
        protected abstract void OnRemove(TData data);
        protected abstract void OnNotifyOrder(TData data, int order);
        protected abstract void OnClear();

        public void Dispose() {
            disposables.Dispose();
        }
    }