using Oculus.Interaction;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class forceclickonraycast : MonoBehaviour
{
    [SerializeField] List<GameObject> button = new List<GameObject>();
    int Button;
    Ray rayLeft;
    [SerializeField] int number;
    List<Button> button_trigger = new List<Button>();
    List<ToggleDeselect> Scrow_trigger = new List<ToggleDeselect>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < button.Count - 1; i++)
        {
            button_trigger.Add(button[i].GetComponent<Button>());
        }

        Scrow_trigger.Add(button[button.Count - 1].GetComponent<ToggleDeselect>());
    }

    // Update is called once per frame
    void Update()

    {
        rayLeft = Tiltchecknotimed.GetLeftTar_XRray;
        Button = number;
        if (Physics.Raycast(rayLeft, out RaycastHit hitDataLeft))

        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);


            button_trigger[number].onClick.Invoke();
            Scrow_trigger[0].OnBeginDrag(pointer);



        }
    }
}