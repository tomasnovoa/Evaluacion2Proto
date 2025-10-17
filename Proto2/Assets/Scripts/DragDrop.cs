using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragDrop : MonoBehaviour
{ [Header("Configuración")]
    public bool isPizza = false;
    public Transform dropTarget; // Asignar la novia aquí
    
    private bool isDragging = false;
    private Vector3 startPosition;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.position;
        
        // Asegurar que sea clickeable
        UnityEngine.UI.Image img = GetComponent<UnityEngine.UI.Image>();
        if (img != null)
        {
            img.raycastTarget = true;
        }

        // Asegurar que existe CanvasGroup
        if (GetComponent<CanvasGroup>() == null)
        {
            gameObject.AddComponent<CanvasGroup>();
        }
    }

    void Update()
    {
        if (isDragging)
        {
            // Seguir el mouse
            Vector3 mousePos = Input.mousePosition;
            rectTransform.position = mousePos;
        }
    }

    // Estos métodos son necesarios para el Event Trigger
    public void OnPointerDown(PointerEventData eventData)
    {
        StartDrag();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EndDrag();
    }

    void StartDrag()
    {
        isDragging = true;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    void EndDrag()
    {
        isDragging = false;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

        CheckDrop();
    }

    void CheckDrop()
    {
        if (dropTarget == null) return;

        RectTransform targetRect = dropTarget.GetComponent<RectTransform>();
        
        // Calcular distancia entre el objeto y el target
        float distance = Vector3.Distance(rectTransform.position, targetRect.position);
        
        // Si está lo suficientemente cerca, es un drop válido
        if (distance < 150f) // Ajusta este valor según necesites
        {
            if (isPizza)
            {
                // GANAR - Pizza en la novia
                FindObjectOfType<SceneBreaker>().WinGame();
            }
            else
            {
                // PERDER - Anillo en la novia
                FindObjectOfType<SceneBreaker>().LoseGame();
            }
            
            // Mover objeto al centro de la novia
            rectTransform.position = targetRect.position;
        }
        else
        {
            // Volver a posición inicial
            ResetToStart();
        }
    }

    public void ResetToStart()
    {
        isDragging = false;
        rectTransform.position = startPosition;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
    }
}
