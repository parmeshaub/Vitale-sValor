using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject playerManager; // Activate to spawn the player.
    [SerializeField] private GameObject playerAnimationObject;
    [SerializeField] private GameObject bossAnimationObject;
    [SerializeField] private GameObject BossObject;
    [SerializeField] private GameObject cutsceneCamera;

    [SerializeField] private string sceneName;

    public void ActivateTutorial() {
        StartCoroutine(ActivateTutorialCoroutine());

    }
    private IEnumerator ActivateTutorialCoroutine() {
        playerAnimationObject.SetActive(false);
        bossAnimationObject.SetActive(false);
        playerManager.SetActive(true);
        cutsceneCamera.SetActive(false);
        BossObject.SetActive(true);
        yield return new WaitForEndOfFrame();
        FadeManager.instance.StartFadeOut(4);

    }

    public void EndTutorial() {
        BossObject.SetActive(false);
        playerManager.SetActive(false);

        playerAnimationObject.SetActive(true);
        bossAnimationObject.SetActive(true);
        
        cutsceneCamera.SetActive(true);
        Animator cutsceneCam = cutsceneCamera.GetComponent<Animator>();
        cutsceneCam.SetTrigger("Cam3");
    }

    public void ExitWorld() {
        // Check if the scene exists
        if (Application.CanStreamedLevelBeLoaded(sceneName)) {
            Destroy(playerManager);

            SceneManager.LoadScene(sceneName);
        }
        else {
            Debug.LogError("Scene '" + sceneName + "' cannot be loaded. Please check the scene name.");
        }
    }
}
