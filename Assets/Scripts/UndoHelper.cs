using UnityEngine;
using System.Collections;

public class UndoHelper : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Escape))
        {
            Camera.main.GetComponent<GameHelper>().ExitToMainMenu();
        }
    }
}
