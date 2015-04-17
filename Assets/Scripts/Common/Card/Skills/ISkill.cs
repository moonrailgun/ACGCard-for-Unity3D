using System;
using System.Collections.Generic;
using UnityEngine;

interface ISkill
{
    void OnUse();
    void OnUse(GameObject target);
}