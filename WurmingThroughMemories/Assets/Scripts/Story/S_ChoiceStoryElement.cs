using Cysharp.Threading.Tasks;
using PepijnWillekens.ManagerSystem;
using UnityEngine;

namespace DefaultNamespace {
    
    public  class S_ChoiceStoryElement : S_StoryElement {

        private int choice = -3;

        protected override void OnInit() {
            base.OnInit();
            choice = -3;
        }

        public override async UniTask<int> Run() {
            await UniTask.WaitUntil(() =>  choice >= -2 );

            return choice;
        }

        public void Set(int i) {
            choice = i;
        }
    }
}