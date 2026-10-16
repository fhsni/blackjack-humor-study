using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChipClickHandler : MonoBehaviour, IPointerClickHandler
{
    public BetManager betManager;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (betManager != null)
        {
            betManager.SelectChip(GetComponent<Image>());
        }
    }
}
