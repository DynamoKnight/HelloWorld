using UnityEngine;
using System.Collections;


namespace TMPro.Examples
{
    
    public class TeleType : MonoBehaviour
    {


        //[Range(0, 100)]
        //public int RevealSpeed = 50;

        public string label01 = "UR mom";

        // When the text will be revealed
        public float startTime;
        private float timer;
        private bool coroutineStarted = false;

        private TMP_Text m_textMeshPro;


        void Awake()
        {
            // Get Reference to TextMeshPro Component
            m_textMeshPro = GetComponent<TMP_Text>();
            m_textMeshPro.text = label01;
            m_textMeshPro.enableWordWrapping = true;
            m_textMeshPro.alignment = TextAlignmentOptions.Top;
        }


        void Start(){
            timer = 0f;
        }

        void Update(){
            timer += Time.deltaTime;
            // Starts the Revealing text only at 2 seconds once
            if(!coroutineStarted){
                if (timer >= startTime){
                    StartCoroutine(RevealText());
                }
            }
            
        }

        IEnumerator RevealText(){
            // Set the flag to indicate that the coroutine is running
            coroutineStarted = true;

            // Force and update of the mesh to get valid information.
            m_textMeshPro.ForceMeshUpdate();


            int totalVisibleCharacters = m_textMeshPro.textInfo.characterCount; // Get # of Visible Character in text object
            int counter = 0;
            int visibleCount = 0;

            while (visibleCount < totalVisibleCharacters){
                visibleCount = counter % (totalVisibleCharacters + 1);

                m_textMeshPro.maxVisibleCharacters = visibleCount; // How many characters should TextMeshPro display?

                // Once the last character has been revealed, wait 1.0 second and start over.
                if (visibleCount >= totalVisibleCharacters)
                {
                    yield return new WaitForSeconds(1.0f);
                }

                counter += 1;

                yield return new WaitForSeconds(0.05f);
            }

            //Debug.Log("Done revealing the text.");
        }

    }
}