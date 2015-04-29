using System;
using System.Collections.Generic;
using UnityEngine;

interface ISkill
{
    /// <summary>
    /// 当技能被点击时
    /// </summary>
    void OnUse();
    /// <summary>
    /// 当技能被指向后
    /// </summary>
    void OnUse(GameObject target);
    /// <summary>
    /// 当技能被指向后（有技能来源）
    /// </summary>
    void OnUse(GameObject from, GameObject target);
}