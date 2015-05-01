using UnityEngine;
using System.Collections;

public class ItemCard : Card
{
    public virtual void OnUse()
    { }
    public virtual void OnUse(GameObject target)
    { }
}