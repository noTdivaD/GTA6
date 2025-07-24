using UnityEngine;
using UnityEngine.UI;

public class InstructionBoxScript : MonoBehaviour
{
    [Header("Instruction Box Settings")]
    [SerializeField] private GameObject instructionBox;

    void Start()
    {
        // Fade in instruction box on start
        if (instructionBox != null)
        {
            instructionBox.SetActive(true);
            StartCoroutine(ShowInstructionBox());
        }
    }

    void Update()
    {
        // Check for Space key press to hide the instruction box
        if(Input.GetKeyDown(KeyCode.Space) && instructionBox.activeSelf)
        {
            StartCoroutine(HideInstructionBox());
        }
    }

    private System.Collections.IEnumerator ShowInstructionBox()
    {
        CanvasGroup instructionGroup = instructionBox.GetComponent<CanvasGroup>();
        if (instructionGroup != null)
        {
            instructionGroup.alpha = 0f;
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime;
                instructionGroup.alpha = Mathf.SmoothStep(0f, 1f, t);
                yield return null;
            }
        }
    }

    private System.Collections.IEnumerator HideInstructionBox()
    {
        CanvasGroup instructionGroup = instructionBox.GetComponent<CanvasGroup>();
        if (instructionGroup != null)
        {
            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime;
                instructionGroup.alpha = Mathf.SmoothStep(1f, 0f, t);
                yield return null;
            }
            instructionBox.SetActive(false);
        }
    }
}
