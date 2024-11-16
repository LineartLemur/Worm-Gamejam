using System;
using System.Collections;
using System.Collections.Generic;
using PepijnWillekens.ManagerSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace PepijnWillekens.Shared {
    public class GameObjectPoolFiller :AutoInit {
        [Serializable]
        public class Element {
            public GameObject gameObject;
            public int amount;
        }
        [Inject]
        private GameObjectPool gameObjectPool;

        public bool instant;
        [HideIf(nameof(instant))]
        public int elementsPerFrame= 5;

        public Element[] objects;

        private static List<GameObject> cachedGameObjects = new List<GameObject>();


        protected override void OnInit() {
            base.OnInit();

            // StartCoroutine(GrowPool());
            StartCoroutine(GrowPoolOverTime());
            // GrowPoolInstant();
        }
        

        public void GrowPoolInstant() {

            for (int i = 0; i < objects.Length; i++) {
                gameObjectPool.GrowPoolTo(objects[i].amount,objects[i].gameObject);
            }
        }
        public IEnumerator GrowPoolOverTime() {

            for (int i = 0; i < objects.Length; i++) {
                for (int n = 0; n < objects[i].amount; n+=elementsPerFrame) {
                    gameObjectPool.GrowPoolTo(n,objects[i].gameObject);
                    yield return null;
                }
            }
        }

        public IEnumerator GrowPool() {
            cachedGameObjects.Clear();
            int n = 0;
            for (int i = 0; i < objects.Length; i++) {
                for (int j = 0; j < objects[i].amount; j++) {
                    cachedGameObjects.Add(gameObjectPool.GetInstance(objects[i].gameObject, true));
                    if (!instant) {
                        n++;
                        if (n > elementsPerFrame) {
                            n = 0;
                            yield return null;

                            for (int k = 0; k < cachedGameObjects.Count; k++) {
                                gameObjectPool.Release(cachedGameObjects[k]);
                            }
                        }
                    }
                }
            }
            
            yield return null;

            for (int i = 0; i < cachedGameObjects.Count; i++) {
                gameObjectPool.Release(cachedGameObjects[i]);
            }
        }

    }
}
