using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using PepijnWillekens.ManagerSystem;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Splines;

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
            FindObjectOfType<SplineAnimate>().Pause();
            int i = 0;
            int r = 0;
            while (r>-2 && i < sequence.Count) {
                showIndex.Value = i;
                r = await sequence[i].Run();
                if (r < 0) i++;
                else {
                    i = r;
                }
            }

            FindObjectOfType<SplineAnimate>().Play();
            Destroy(gameObject);
        }
[Button]
        public void OnValidate() {
            if(parent == null) return;
            
            for (int i = 0; i < parent.childCount; i++) {
                var e = parent.GetChild(i).gameObject;

                var realName = e.gameObject.name.Split("]").Last();

                e.name = $"[{i}]{realName}";
            }
        }
    }
}