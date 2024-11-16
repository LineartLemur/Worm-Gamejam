using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Curiosmos.Shared.CollectionCaching {
    public static class CollectionPool<C,E> where C:ICollection<E>, new() {
        public class Container : IDisposable {
            public C c { get; }
            public void Dispose() {
                c.Clear();
                myList.Release(this);
            }

            private CachedList<Container> myList; 

            public Container(CachedList<Container> cachedList) {
                myList = cachedList;
                c = new C();
            }
        }

        private static CachedList<Container> cachedList = new(() => new Container(cachedList));

        public static Container Get() {
            return cachedList.GetItem();
        }
    }
}