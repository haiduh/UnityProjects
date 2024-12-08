using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip interactSound;
    private RectTransform rect;
    private int currentPos;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        //-1 goes up in an array whilst 1 goes down in an array

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            changePosition(-1);
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            changePosition(1);

        //Interacting with options
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
            Interact();
    }

    private void changePosition(int change)
    {
        currentPos += change;

        if(change != 0) 
            SoundManager.instance.playSound(changeSound);

        if (currentPos < 0)
            currentPos = options.Length - 1;
        else if (currentPos > options.Length - 1)
            currentPos = 0;

        //Change the options by changing the Y value
        rect.position = new Vector3(rect.position.x, options[currentPos].position.y, 0);
    }

    private void Interact()
    {
        SoundManager.instance.playSound(interactSound);

        //Access the button component and access onClick method
        options[currentPos].GetComponent<Button>().onClick.Invoke();
    }
}
