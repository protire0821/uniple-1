using UnityEngine;

[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    [SerializeField]
    private AnimatedSprite deathSequence;
    private SpriteRenderer spriteRenderer;
    private Movement movement;
    private new Collider2D collider;

    private float[] angles;

    private string pacmanMove()
    {
        angles = PipeServer.nodeAngles; // °ÊºAÀò¨ú nodeAngles

        Debug.Log("nodeAngles: " + string.Join(", ", angles));

        if (angles[11] > 30 && angles[11] < 60 && angles[12] > 120 && angles[12] < 160)
        {
            return "left";
        }
        else if (angles[11] > 120 && angles[11] < 160 && angles[12] > 30 && angles[12] < 60)
        {
            return "right";
        }
        else if (angles[11] > 60 && angles[11] < 120 && angles[12] > 60 && angles[12] < 120)
        {
            return "up";
        }
        else if (angles[11] > 60 && angles[11] < 120 && angles[12] > 120 && angles[12] < 160)
        {
            return "down";
        }
        else { return ""; } // Return empty string if no condition is met
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<Movement>();
        collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        string moveDirection = pacmanMove();

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || moveDirection == "up")
        {
            movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || moveDirection == "down")
        {
            movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || moveDirection == "left")
        {
            movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || moveDirection == "right")
        {
            movement.SetDirection(Vector2.right);
        }
        else
        {
            movement.SetDirection(Vector2.zero);
        }

        // Rotate pacman to face the movement direction
        float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
        transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        deathSequence.enabled = false;
        movement.ResetState();
        gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        enabled = false;
        spriteRenderer.enabled = false;
        collider.enabled = false;
        movement.enabled = false;
        deathSequence.enabled = true;
        deathSequence.Restart();
    }
}
