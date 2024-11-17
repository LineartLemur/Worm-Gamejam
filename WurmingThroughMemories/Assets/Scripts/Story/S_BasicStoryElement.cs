using Cysharp.Threading.Tasks;
using PepijnWillekens.ManagerSystem;
using UnityEngine;

namespace DefaultNamespace {
    
    public  class S_BasicStoryElement : S_StoryElement {
        public float duration=3;
        public Vector2 startPos;
        public Vector2 endPos;
        public float startScale=1;
        public float endScale=1;
        public int nextFrame = -1;
        
        public override async UniTask<int> Run() {
            float t = 0;

            while (t<1) {
                t += Time.deltaTime / duration;
                rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                rectTransform.localScale = Vector3.one* Mathf.Lerp(startScale, endScale, t);
                await UniTask.Yield();
            }

            return nextFrame;
        }
    }
}