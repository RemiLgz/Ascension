using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class ShowPanels : MonoBehaviour
{
    public Button showButton;
    public GameObject[] panels;
    public float panelDelay = 0.2f;
    public float buttonScaleAmount = 1.05f; // Un léger agrandissement pour le bouton
    public float animationDuration = 0.5f;
    public CanvasGroup buttonCanvasGroup; // Assurez-vous que le bouton a un CanvasGroup

    private EventTrigger trigger;
    private Vector3 originalButtonScale; // Stocker l'échelle d'origine du bouton

    void Start()
    {
        showButton.onClick.AddListener(OnButtonClick);

        // Ajouter l'EventTrigger pour détecter le survol du bouton
        trigger = showButton.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = showButton.gameObject.AddComponent<EventTrigger>();
        }

        // Ajouter les événements de survol
        AddEventTrigger(EventTriggerType.PointerEnter, OnMouseEnterButton);
        AddEventTrigger(EventTriggerType.PointerExit, OnMouseExitButton);

        // Stocker la taille d'origine du bouton
        originalButtonScale = showButton.transform.localScale;
    }

    void AddEventTrigger(EventTriggerType eventType, UnityEngine.Events.UnityAction<BaseEventData> action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = eventType
        };
        entry.callback.AddListener((data) => action.Invoke(data));
        trigger.triggers.Add(entry);
    }

    void OnButtonClick()
    {
        // Désactiver le bouton avec une animation de fondu
        if (buttonCanvasGroup != null)
        {
            buttonCanvasGroup.DOFade(0f, animationDuration).OnComplete(() =>
            {
                showButton.gameObject.SetActive(false); // Désactiver après le fade
            });
        }

        // Afficher les panneaux un par un avec un effet de fondu
        for (int i = 0; i < panels.Length; i++)
        {
            int index = i;
            panels[i].SetActive(true);
            CanvasGroup panelCanvasGroup = panels[i].GetComponent<CanvasGroup>();

            if (panelCanvasGroup == null)
            {
                panelCanvasGroup = panels[i].AddComponent<CanvasGroup>(); // Ajouter un CanvasGroup si nécessaire
            }

            panelCanvasGroup.alpha = 0f; // Assurez-vous que les panneaux sont invisibles au départ
            panelCanvasGroup.DOFade(1f, animationDuration).SetDelay(index * panelDelay);
        }
    }

    void OnMouseEnterButton(BaseEventData data)
    {
        // Agrandir légèrement le bouton en préservant sa taille d'origine
        showButton.transform.DOScale(originalButtonScale * buttonScaleAmount, 0.2f);
    }

    void OnMouseExitButton(BaseEventData data)
    {
        // Rétablir la taille d'origine du bouton
        showButton.transform.DOScale(originalButtonScale, 0.2f);
    }
}
