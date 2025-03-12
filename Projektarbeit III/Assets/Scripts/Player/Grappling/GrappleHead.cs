using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleHead : MonoBehaviour
{
    Controller controller;
    GrapplingHook grapplingHook;
    GrappleInputHandler grappleInputHandler;
    GameObject parent;

    public GrappleHead(GrapplingHook grapplingHook, Controller controller, GrappleInputHandler grappleInputHandler)
    {
        this.controller = controller;
        this.grapplingHook = grapplingHook;
        this.grappleInputHandler = grappleInputHandler;
    }

    public void LoadHead(GrapplingHook grapplingHook, Controller controller)
    {
        this.controller = controller;
        this.grapplingHook = grapplingHook;

        parent = GameObject.Find("Player");
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = new Vector3(parent.transform.position.x, (float)(parent.transform.position.y + 0.1), 0);

        if (controller == null)
        {
            Debug.LogError("Controller not found in GrappleHead");
        }
        if (grapplingHook == null)
        {
            Debug.LogError("GrapplingHook not found in GrappleHead");
        }
    }

    void Update()
    {
        RotatePlayerHead();
    }

    public void RotatePlayerHead()
    {
        float maxTiltAngle = controller.movementEditor.headTiltLimit;
        float angle;

        //Vector2 indicatorDirectionController = grappleInputHandler.aimAction.ReadValue<Vector2>().normalized;
        Vector2 indicatorDirection = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        GameObject playerSprite = parent.gameObject.transform.GetChild(0).gameObject;
        RaycastHit2D raycastHit = GetComponentInParent<GrapplingHook>().hit;
        

        if (playerSprite.GetComponent<SpriteRenderer>().flipX)
        {
            SpriteRenderer spriteRenderer = playerSprite.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.red;
            angle = Vector2.SignedAngle(Vector2.left, raycastHit.point + (Vector2)transform.position);
            Debug.Log("angle: " + angle);
        }
        else
        {
            angle = Vector2.SignedAngle(Vector2.right, raycastHit.point - (Vector2)transform.position);
            Debug.Log("angle: " + angle);
        }

        if (angle > maxTiltAngle)
        {
            transform.right = Quaternion.Euler(0, 0, maxTiltAngle) * Vector2.right;
            //Debug.Log("Transforming by Angle: " + Quaternion.Euler(0, 0, maxTiltAngle) * Vector2.right);
        }
        else if (angle < -maxTiltAngle)
        {
            transform.right = Quaternion.Euler(0, 0, -maxTiltAngle) * Vector2.right;
            //Debug.Log("Transforming by Angle: " + Quaternion.Euler(0, 0, -maxTiltAngle) * Vector2.right);
        }
        else
        {
            if (!playerSprite.GetComponentInChildren<SpriteRenderer>().flipX)
            {
                transform.right = indicatorDirection;
                //Debug.Log("Transforming: " + indicatorDirection);
            }
            else
            {
                transform.right = -indicatorDirection;
                //Debug.Log("Transforming: " + -indicatorDirection);
            }
        }
    }
}
