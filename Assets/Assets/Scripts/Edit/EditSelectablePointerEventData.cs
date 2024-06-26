using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditSelectablePointerEventData
{
    public PointerEventData PointerEventData { get; set; }

    public EditSelectable SelectedPrevious { get; set; }

    public EditSelectable SelectedNext { get; set; }
}
