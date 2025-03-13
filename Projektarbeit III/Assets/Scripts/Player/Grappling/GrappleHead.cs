
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrappleHead : MonoBehaviour
{
    Controller controller;
    GrapplingHook grapplingHook;
    GrappleInputHandler grappleInputHandler;
    GameObject parent;
    SpriteRenderer playerRenderer;

    GameObject toungeGroup;
    GameObject[] toungePieces;


    public void LoadHead(GrapplingHook grapplingHook, Controller controller)
    {
        this.controller = controller;
        this.grapplingHook = grapplingHook;

        parent = GameObject.FindWithTag("Player");
        gameObject.transform.SetParent(parent.transform);
        playerRenderer = parent.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();

        toungeGroup = transform.GetChild(0).gameObject;

        positionPlayerHead();

        toungePieces = new GameObject[4];
        for (int i = 0; i < 4; i++)
        {
            toungePieces[i] = toungeGroup.transform.GetChild(i).gameObject;
        }

        rotatePlayerHead();
    }

    void positionPlayerHead()
    {
        if (!playerRenderer.flipX)
        {
            gameObject.transform.position = new Vector3(parent.transform.localPosition.x + 0.2f, parent.transform.localPosition.y + 0.2f, 0);
            toungeGroup.transform.localPosition = new Vector3(0, 0.15f, 0);
        }
        else
        {
            gameObject.transform.position = new Vector3(parent.transform.localPosition.x - 0.2f, parent.transform.localPosition.y + 0.2f, 0);
            toungeGroup.transform.localPosition = new Vector3(0, -0.15f, 0);
        }
    }

    void Update()
    {
        manageTounge();
    }

    private void rotatePlayerHead()
    {
        float angle;
        Vector2 indicaterPosition = grapplingHook.hit.point;
        Vector2 indicatorDistance = indicaterPosition - (Vector2)transform.position;

        if (playerRenderer.flipX)
        {
            angle = Vector2.SignedAngle(Vector2.left, indicatorDistance);
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            angle = Vector2.SignedAngle(Vector2.right, indicatorDistance);
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
        }

        if (angle < -90 || angle > 90)
        {
            playerRenderer.flipX = !playerRenderer.flipX;
            gameObject.GetComponent<SpriteRenderer>().flipY = !gameObject.GetComponent<SpriteRenderer>().flipY;
            positionPlayerHead();
        }

        gameObject.transform.right = indicatorDistance;
    }

    private void manageTounge()
    {
        float distance = (grapplingHook.hit.point - (Vector2)transform.position).magnitude;
        Debug.DrawLine(transform.position, grapplingHook.hit.point, Color.red, 0.1f);

        float toungeLength = toungePieces[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y;

        toungePieces[0].transform.localPosition = new Vector3(toungeLength / 2, 0, 0);
        toungePieces[1].transform.localPosition = new Vector3(toungeLength + (toungeLength / 2), 0, 0);
        toungePieces[3].transform.localPosition = new Vector3(distance, 0, 0);

        float middleDistance = distance - ((2 * toungeLength) + (toungeLength / 2));
        float scaleFactor = 1 / toungeLength;

        toungePieces[2].transform.localPosition = new Vector3(2 * toungeLength + middleDistance / 2, 0, 0);
        toungePieces[2].transform.localScale = new Vector3(1, middleDistance * scaleFactor, 1);
    }
}
