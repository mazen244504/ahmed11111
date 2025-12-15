using UnityEngine;
public class PlayAttack : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            this.GetComponent<Animator>().SetTrigger("Attack");
        }
    }
}