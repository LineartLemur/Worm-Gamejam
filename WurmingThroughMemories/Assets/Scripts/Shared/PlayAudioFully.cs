using UnityEngine;

public class PlayAudioFully :MonoBehaviour {
        public AudioSource audioSource;

        private void OnEnable() {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        private void Update() {
            if (!audioSource.isPlaying) {
                Destroy(gameObject);
            }
        }
    }