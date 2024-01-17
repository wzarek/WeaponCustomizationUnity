using UnityEngine;

public class ActivateSelf: MonoBehaviour
{
    [SerializeField] public GameObject ObjectToActivate;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ObjectToActivate.SetActive(!ObjectToActivate.activeSelf);
        }
    }
}