using Cysharp.Threading.Tasks;
using PepijnWillekens.ManagerSystem;
using UnityEngine;

namespace DefaultNamespace {
    
    public  class S_ChoiceStoryElement : S_StoryElement {

        public int choice = -1;
        public override async UniTask<int> Run() {
            await UniTask.WaitUntil(() =>  choice >= 0 );

            return choice;
        }

        public void Set(int i) {
            choice = i;
        }
    }
}