using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private void Awake() {
        // Implement Singleton pattern to ensure only one instance of SceneLoader exists
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName, Vector3 playerPosition, Quaternion playerRotation) {
        StartCoroutine(LoadSceneWithFade(sceneName, playerPosition, playerRotation));
    }

    private IEnumerator LoadSceneWithFade(string sceneName, Vector3 playerPosition, Quaternion playerRotation) {
        // Start the fade-out effect
        FadeManager.instance.StartFadeOut();

        // Wait for the fade-out to complete before loading the new scene
        yield return new WaitForSeconds(FadeManager.instance.defaultFadeDuration);

        // Start loading the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone) {
            yield return null;
        }

        // Set the player's position and rotation after the scene is loaded
        SetPlayerPositionAndRotation(playerPosition, playerRotation);

        // Start the fade-in effect
        FadeManager.instance.StartFadeIn();
    }

    private void SetPlayerPositionAndRotation(Vector3 position, Quaternion rotation) {
        // Find the player object in the new scene
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null) {
            player.transform.position = position;
            player.transform.rotation = rotation;
        }
        else {
            Debug.LogWarning("Player object not found in the scene.");
        }
    }
}
