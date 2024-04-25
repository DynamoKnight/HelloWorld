using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextAnim : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI _textMeshPro;

    public string[] stringArray;

    [SerializeField] float timeBtwnChars;

    [SerializeField] float timeBtwnWords;

    [SerializeField] private string nextScene;

    private GameObject gm;
    private StateManager stateManager;

    int i = 0;
    void Start(){
        gm = GameObject.Find("GameManager");
        stateManager = gm.GetComponent<StateManager>();
        
        EndCheck();
    }

    public void EndCheck(){
        if (i < stringArray.Length){
            _textMeshPro.text = stringArray[i];
            StartCoroutine(TextVisible());
        }
        else{
            stateManager.ChangeSceneByName(nextScene);
        }
    }

    private IEnumerator TextVisible() {
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1); 
            _textMeshPro.maxVisibleCharacters = visibleCount;

            if(visibleCount >= totalVisibleCharacters)
            {
                i++;
                //Invoke("EndCheck", timeBtwnChars);
                break;
            }

            counter++;
            yield return new WaitForSeconds(timeBtwnChars);

        }
    }
}
