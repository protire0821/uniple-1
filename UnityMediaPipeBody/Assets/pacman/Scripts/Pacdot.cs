using UnityEngine;

public class Pacdot : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Pacman")
        {
            
            GameManager.Instance.OnEatPacdot(gameObject);
            Destroy(gameObject);
        }
    }
}
