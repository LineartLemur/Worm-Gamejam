using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

namespace PepijnWillekens.ManagerSystem {
    public class AutoInit : MonoBehaviour, IInitable {
	    protected const string COMPONENTS = "Components";
	    protected const string ASSETS = "Assets";
	    protected const string SETTINGS = "Settings";
	    protected const string STATE = "State";
	    
	    private RectTransform _rectTransform;
	    public RectTransform rectTransform {
		    get {
			    if (!_rectTransform) _rectTransform = GetComponent<RectTransform>();
			    return _rectTransform;
		    }
	    }

        protected bool inited = false;
        public void Init() {
			if (!Application.isPlaying) return;

			if (inited) return;

			inited = true;

            var parentalManager= gameObject.GetComponentInParent<Managers>();
            if (parentalManager && parentalManager.IsDoubleDDOLManager()) {
                return; //skip init if we're gonna get destroyed this frame anyway.
            }

            Profiler.BeginSample("FixDependancies");
            FixDependecies();
            Profiler.EndSample();
            Profiler.BeginSample("onInit");
            OnInit();
            Profiler.EndSample();
            //SceneManager.sceneLoaded += SceneLoaded;
        }

        protected void FixDependecies() {
		        if (gameObject) DependancyCache.InjectDependencies(this, gameObject.scene);
	        }


        protected virtual void OnInit() {

        }

        private void Awake() {
            Init();
        }

        [ContextMenu("Name GameObject to this")]
        private void NameGameObject() {
	        string typeName = this.GetType().Name;
	        string readableName = Regex.Replace(typeName, "(?<!^)([A-Z])", " $1");
	        readableName = char.ToUpper(readableName[0]) + readableName.Substring(1);

	        gameObject.name = readableName;
        }

    }
}
