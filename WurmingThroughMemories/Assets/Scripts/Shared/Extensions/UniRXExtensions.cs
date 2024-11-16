using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
#if ENABLE_NETWORK_MULTIPLAYER
using Unity.Netcode;
#endif
using UnityEngine.UI;

namespace Utility {
    public static class UniRXExtensions {
        public static void Report<T>(this ScheduledNotifier<T> me) {
            me.Report(default);
        }
        public static void Report<T>(this Subject<T> me) {
            me.OnNext(default);
        }
        
        #if ENABLE_NETWORK_MULTIPLAYER
        public static IDisposable Subscribe<T>(this NetworkVariable<T> variable, Action<T> listener, bool ignoreInitial = false) where T : unmanaged {
            NetworkVariable<T>.OnValueChangedDelegate f = (T prev, T next) => listener(next);
            variable.OnValueChanged +=f;
            
            if(!ignoreInitial) listener(variable.Value);
            
            return Disposable.Create(() => {
                variable.OnValueChanged -= f;
            });
        }
        #endif
        
        public static void Link<T>(
            this IReactiveProperty<T> a,
            IReactiveProperty<T> b,
            IList<IDisposable> disposableListToAddTo
        ) {
            disposableListToAddTo.Add(b.Subscribe((v) => a.Value = v));
            disposableListToAddTo.Add(a.Subscribe((v) => b.Value = v));
        }
        public static IDisposable Link<T>(
            this IReactiveProperty<T> a,
            IReactiveProperty<T> b
        ) {
            
            var bSub = b.Subscribe((v) => a.Value = v);
            var aSub = a.Subscribe((v) => b.Value = v);
            return Disposable.Create(() => {
                aSub.Dispose(); 
                bSub.Dispose();
            });
        }
        public static IDisposable Attach(this Toggle me, ReactiveProperty<bool> property) {
            void Listener(bool on) {
                property.Value = on;
            }

            me.onValueChanged.AddListener(Listener);
            var sub = property.Subscribe(on => { me.isOn = on; }).AddTo(me);

            return Disposable.Create(() => {
                sub.Dispose();
                if (me) {
                    me.onValueChanged.RemoveListener(Listener);
                }
            });
        }

        public static IDisposable Attach(
            this TMP_InputField me,
            IReactiveProperty<string> property,
            Action<string> sideEffect = null
        ) {
            void Listener(string newText) {
                property.Value = newText;
                sideEffect?.Invoke(newText);
            }

            me.onValueChanged.AddListener(Listener);
            var sub = property.Subscribe(me.SetTextWithoutNotify).AddTo(me);

            return Disposable.Create(() => {
                sub.Dispose();
                if (me) {
                    me.onValueChanged.RemoveListener(Listener);
                }
            });
        }

        public static void Match<T>(this ReactiveCollection<T> collection, IEnumerable<T> list) {
            for (int i = 0; i < collection.Count; i++) {
                if(list.Contains(collection[i])) continue;
                collection.RemoveAt(i);
                i--;
            }
            
            foreach (var element in list) {
                if(collection.Contains(element)) continue;
                collection.Add(element);
            }
        }
        public static IDisposable SetValueFromSubscriptions<T, A>(this ReactiveProperty<T> collection, ReactiveProperty<A> a, Func<A,T> getValue) {
            return a.Subscribe(e => collection.Value = getValue(e));
        }
        public static IDisposable SetValueFromSubscriptions<T, A, B>(this ReactiveProperty<T> collection,
            ReactiveProperty<A> a, ReactiveProperty<B> b, Func<A, B, T> getValue) {
            DisposableList list = new DisposableList();
            list.Add(a.Subscribe(a => collection.Value = getValue(a, b.Value)));
            list.Add(b.Subscribe(b => collection.Value = getValue(a.Value, b)));
            return list;
        }
        
        public static IDisposable SetValueFromSubscriptions<T, A, B, C>(this ReactiveProperty<T> collection,
            ReactiveProperty<A> a, 
            ReactiveProperty<B> b, 
            ReactiveProperty<C> c, 
            Func<A, B, C, T> getValue) {
            DisposableList list = new DisposableList();
            list.Add(a.Subscribe(a => collection.Value = getValue(a, b.Value, c.Value)));
            list.Add(b.Subscribe(b => collection.Value = getValue(a.Value, b, c.Value)));
            list.Add(c.Subscribe(c => collection.Value = getValue(a.Value, b.Value, c)));
            return list;
        }
        public static IDisposable SetValueFromSubscriptions<T, A, B, C, D>(this ReactiveProperty<T> collection,
            ReactiveProperty<A> a, 
            ReactiveProperty<B> b, 
            ReactiveProperty<C> c, 
            ReactiveProperty<D> d, 
            Func<A, B, C, D, T> getValue) {
            DisposableList list = new DisposableList();
            list.Add(a.Subscribe(a => collection.Value = getValue(a, b.Value, c.Value, d.Value)));
            list.Add(b.Subscribe(b => collection.Value = getValue(a.Value, b, c.Value, d.Value)));
            list.Add(c.Subscribe(c => collection.Value = getValue(a.Value, b.Value, c, d.Value)));
            list.Add(d.Subscribe(d => collection.Value = getValue(a.Value, b.Value, c.Value, d)));
            return list;
        }
        public static IDisposable SetValueFromSubscriptions<T, A, B, C, D, E>(this ReactiveProperty<T> collection,
            ReactiveProperty<A> a, 
            ReactiveProperty<B> b, 
            ReactiveProperty<C> c, 
            ReactiveProperty<D> d, 
            ReactiveProperty<E> e, 
            Func<A, B, C, D, E, T> getValue) {
            DisposableList list = new DisposableList();
            list.Add(a.Subscribe(a => collection.Value = getValue(a, b.Value, c.Value, d.Value, e.Value)));
            list.Add(b.Subscribe(b => collection.Value = getValue(a.Value, b, c.Value, d.Value, e.Value)));
            list.Add(c.Subscribe(c => collection.Value = getValue(a.Value, b.Value, c, d.Value, e.Value)));
            list.Add(d.Subscribe(d => collection.Value = getValue(a.Value, b.Value, c.Value, d, e.Value)));
            list.Add(e.Subscribe(e => collection.Value = getValue(a.Value, b.Value, c.Value, d.Value, e)));
            return list;
        }
        
    }
}