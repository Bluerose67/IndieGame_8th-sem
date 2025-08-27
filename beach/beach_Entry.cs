using UnityEngine;

public class beach_Entry : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -3;
    }
}
