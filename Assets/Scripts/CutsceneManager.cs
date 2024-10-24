using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    // List of all cutscenes
    [HideInInspector] public List<GameObject> frames = new();
    // Indicates all cutscenes have been iterated through
    [HideInInspector] public bool cutsceneEnded = false;
    // The current time in seconds
    public float timer = 0f;
    // After the text is fully revealed, it waits for this time before hiding
    private float pause = 3f;

    // Start is called before the first frame update
    void Start(){
        foreach (Transform child in transform){
            frames.Add(child.gameObject);
        }
        StartCoroutine(FadeFrames());
    }

    void Update(){
        // When space is pressed, it will skip the cutscenes
        if (Input.GetKeyDown(KeyCode.Space)){
            StopAllCoroutines();
            cutsceneEnded = true;
        }
    }

    void FixedUpdate(){
        if (!LevelManager.instance.isPaused){
            timer += Time.deltaTime;
        }
    }

    // Reveals each cutscene at a time.
    // If hide is true, it will fade each image out after being shown.
    public IEnumerator FadeFrames(bool hide = true){
        for (int i = 0; i < frames.Count; i++){
            // The current cutscene being shown
            GameObject frame = frames[i];

            Image image = frame.GetComponent<Image>();
            // Ensures Image property exists
            if (image){
                Color color = image.color;
                float speed = 0.1f;
                float fade = 0.08f;
                // Starts hidden
                color.a = 0f;
                image.color = color;
                // Reveals over time
                // Increases the alpha value of the object's Image.
                for (float a = 0f; a < 1f; a += fade){
                    color.a = a;
                    image.color = color;
                    yield return new WaitForSeconds(speed);
                }
                // Ensures it is finally revealed
                color.a = 1f;
                image.color = color;
                
                // Now that the scene is fully revealed, it starts showing text
                GameObject cutsceneText = frame.transform.GetChild(0).gameObject;
                TMP_Text cutsceneTextbox = cutsceneText.GetComponent<TMP_Text>();
                // Sets alpha value to be visible
                cutsceneTextbox.color = new Color(1, 1, 1, 1);
                // This will wait for the RevealText coroutine to finish before finishing
                yield return StartCoroutine(cutsceneText.GetComponent<TextAnim>().RevealText());
                // Extra time to see the screen
                yield return new WaitForSeconds(pause);
                // Fades out
                if (hide){
                    // Starts revealed
                    color.a = 1f;
                    image.color = color;
                    cutsceneTextbox.color = color;
                    // Hides over time
                    for (float a = 1f; a > 0f; a -= fade){
                        color.a = a;
                        image.color = color;
                        cutsceneTextbox.color = color;
                        yield return new WaitForSeconds(speed);
                    }
                    // Ensures it is finally hidden
                    color.a = 0f;
                    image.color = color;
                    cutsceneTextbox.color = color;
                }
            }
        }
        cutsceneEnded = true;
    }

    // Returns the current time
    public float GetTime(){
        return timer;
    }

    // Returns a bool indicating if the cutscenes have finished
    public bool IsCutsceneEnded(){
        return cutsceneEnded;
    }

}
