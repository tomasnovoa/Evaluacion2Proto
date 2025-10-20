using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragDrop : MonoBehaviour
{   [Header("Configuración")]
    public bool isPizza = false;
    public Transform dropTarget; // Asignar la novia aquí
    
    private bool isDragging = false;
    private Vector3 startPosition;
    private Vector3 offset;
    private SpriteRenderer spriteRenderer;
    private bool isOverTarget = false;
    void Start()
    {
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Asegurar que tenga Collider
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    void OnMouseDown()
    {
        if (!isDragging)
        {
            StartDrag();
        }
    }

    void OnMouseUp()
    {
        if (isDragging)
        {
            EndDrag();
        }
    }

    void Update()
    {
        if (isDragging)
        {
            // Seguir el mouse en el mundo 2D
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition + offset;
        }
    }

    void StartDrag()
    {
        isDragging = true;
        
        // Calcular offset para que el objeto no salte al mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        offset = transform.position - mousePosition;
        
        // Efecto visual
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.7f);
        }
    }

    void EndDrag()
    {
        isDragging = false;
        
        // Restaurar efecto visual
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
        
        CheckDrop();
    }
   void CheckDrop()
    {
        if (isOverTarget)
        {
            // Éxito - objeto soltado sobre la novia
            if (isPizza)
            {
                // GANAR - Pizza en la novia
                FindObjectOfType<SceneBreaker>().PlayerWon();
            }
            else
            {
                // PERDER - Anillo en la novia
                FindObjectOfType<SceneBreaker>().PlayerLost();
            }
            
            // Mover objeto al centro de la novia
            transform.position = dropTarget.position;
        }
        else
        {
            // Volver a posición inicial
            ResetPosition();
        }
    }

    // ESTOS MÉTODOS SE LLAMAN AUTOMÁTICAMENTE CON TRIGGERS
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == dropTarget)
        {
            isOverTarget = true;
            Debug.Log("Entró en la novia");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == dropTarget)
        {
            isOverTarget = false;
            Debug.Log("Salió de la novia");
        }
    }

    public void ResetPosition()
    {
        isDragging = false;
        transform.position = startPosition;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
        }
    }
}
