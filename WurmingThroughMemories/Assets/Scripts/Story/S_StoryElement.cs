using Cysharp.Threading.Tasks;
using PepijnWillekens.ManagerSystem;

namespace DefaultNamespace {
    public abstract class S_StoryElement : AutoInit {
        public  abstract UniTask<int> Run();
    }
}