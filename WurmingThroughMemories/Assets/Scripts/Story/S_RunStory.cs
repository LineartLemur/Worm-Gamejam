using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PepijnWillekens.ManagerSystem;
using UniRx;
using UnityEngine;

namespace DefaultNamespace {
    public class S_RunStory : AutoInit {
        public Transform parent;

        private List<S_StoryElement> sequence = new ();
        private IntReactiveProperty showIndex = new ();

        protected override void OnInit() {
            base.OnInit();
            for (int i = 0; i < parent.childCount; i++) {
                var e = parent.GetChild(i);
                sequence.Add(e.GetComponent<S_StoryElement>());
            }

            showIndex.Subscribe(i => {
                for (int k = 0; k < sequence.Count; k++) {
                    sequence[k].gameObject.SetActive(k == i);
                }
            });
            Run().Forget();
        }

        public async UniTask Run() {
            int i = 0;
            int r = 0;
            while (r!= -2 && i < sequence.Count) {
                showIndex.Value = i;
                r = await sequence[i].Run();
                if (r < 0) i++;
                else {
                    i = r;
                }
            }
           

        }

    }
}