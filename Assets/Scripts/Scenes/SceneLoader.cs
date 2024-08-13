using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public FadeManager fadeManager;
    public static SceneLoader Instance;
    public Vector3 originalPos;
    public Quaternion originalRot;

    private GameObject SceneLoaderObj;

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

    public void Start()
    {

        
    }

    public void LoadDungeon(string sceneName, Transform exitTransform) {
        originalPos = exitTransform.position;
        originalRot = exitTransform.rotation;
        StartCoroutine(LoadDungeonWithFade(sceneName));
    }

    public void LoadToWorld(string sceneName) {
        StartCoroutine(LoadToWorldWithFade(sceneName));
    }

    private IEnumerator LoadDungeonWithFade(string sceneName) {
        fadeManager.StartFadeIn();

        yield return new WaitForSeconds(fadeManager.defaultFadeDuration);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone) {
            yield return null;
        }

        DungeonInitialization dungeonInitialization = GameObject.FindGameObjectWithTag("Dungeon Initialization").GetComponent<DungeonInitialization>();
        dungeonInitialization.SetPlayerLocation();

        fadeManager.StartFadeOut();
    }

    private IEnumerator LoadToWorldWithFade(string sceneName) {
        fadeManager.StartFadeIn();

        yield return new WaitForSeconds(fadeManager.defaultFadeDuration);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the scene is fully loaded
        while (!asyncLoad.isDone) {
            yield return null;
        }

        // Ensure only one player instance exists and return it to the original position
        //PlayerManager.Instance.SpawnPlayer();

        SetPlayerPositionAndRotation(originalPos, originalRot);

        fadeManager.StartFadeOut();
    }

    public void SetPlayerPositionAndRotation(Vector3 position, Quaternion rotation) {
        // Find the player object in the new scene
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) {
            CharacterController characterController = player.GetComponent<CharacterController>();
            if (characterController != null) {
                characterController.enabled = false;
                player.transform.position = position;
                player.transform.rotation = rotation;
                characterController.enabled = true;
            }
            else {
                Debug.LogWarning("CharacterController component not found on the Player object.");
            }
        }
        else {
            Debug.LogWarning("Player object not found in the scene.");
        }
    }
}
