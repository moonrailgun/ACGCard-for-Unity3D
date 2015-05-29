using System;
using System.Collections.Generic;
using UnityEngine;

interface ISkill
{
    void OnUse(CharacterCard toCard, string skillAppendData);
}