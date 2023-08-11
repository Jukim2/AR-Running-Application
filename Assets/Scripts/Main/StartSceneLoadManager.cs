using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Qualcomm.Snapdragon.Spaces.Samples;

public class StartSceneLoadManager : MonoBehaviour
{
    public GameObject man;
    public float movementStep = 0.001f;
    public int totalSteps = 5000;
    public float delayTime = 1.0f; // Delay time in seconds


    private InteractionManager _interactionManager;

    void Start()
    {
        _interactionManager ??= FindObjectOfType<InteractionManager>(true);
    }

    public void LoadSceneByName(string sceneName)
    {
        StartCoroutine(MoveManGraduallyAndLoad(sceneName));
    }

    private IEnumerator MoveManGraduallyAndLoad(string sceneName)
    {
        // Start both coroutines simultaneously
        StartCoroutine(MoveManGradually());
        yield return new WaitForSeconds(delayTime);

        _interactionManager?.SendHapticImpulse();
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator MoveManGradually()
    {
        Vector3 targetPosition = man.transform.position + new Vector3(0, 0, -movementStep * totalSteps);
        Vector3 targetScale = man.transform.localScale + new Vector3(movementStep * totalSteps, movementStep * totalSteps, movementStep * totalSteps);

        for (int i = 0; i < totalSteps; i++)
        {
            man.transform.position += new Vector3(0, 0, -movementStep);
            man.transform.localScale += new Vector3(movementStep, movementStep, movementStep);
            yield return null; // Wait for one frame
        }

        // Ensure that the final position is set correctly
        man.transform.position = targetPosition;
    }
}