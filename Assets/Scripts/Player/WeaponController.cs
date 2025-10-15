using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform weaponPivot;
    public SpriteRenderer playerSpriteRenderer;

    void Update()
    {
        // Aim towards the mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - weaponPivot.position).normalized;

        // Rotate the weapon
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        weaponPivot.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Flip the player sprite
        if (mousePos.x > transform.position.x)
        {
            playerSpriteRenderer.flipX = false;
        }
        else if (mousePos.x < transform.position.x)
        {
            playerSpriteRenderer.flipX = true;
        }
    }
}