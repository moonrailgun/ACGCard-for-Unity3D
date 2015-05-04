using System;
using System.Collections.Generic;
using UnityEngine;

public class CardDesPanel : MonoBehaviour
{
    private UIPanel cardDesPanel;
    private UILabel cardNameLabel;
    private UILabel cardLevelLabel;
    private UILabel cardDamageLabel;
    private UILabel cardSpeedLabel;
    private UIWidget stateListWidget;
    private UIGrid stateListGrid;

    private Card latestShowCard;

    private void Awake()
    {
        cardDesPanel = GetComponent<UIPanel>();
        cardNameLabel = transform.FindChild("Container/CardInfo/CardName/Value").GetComponent<UILabel>();
        cardLevelLabel = transform.FindChild("Container/CardInfo/CardLevel/Value").GetComponent<UILabel>();
        cardDamageLabel = transform.FindChild("Container/CardInfo/CardDamage/Value").GetComponent<UILabel>();
        cardSpeedLabel = transform.FindChild("Container/CardInfo/CardSpeed/Value").GetComponent<UILabel>();
        stateListWidget = transform.FindChild("Container/CardInfo/StateList").GetComponent<UIWidget>();
        stateListGrid = transform.FindChild("Container/CardInfo/StateList/List").GetComponent<UIGrid>();
    }

    private void Update()
    {
        if (cardDesPanel != null && cardDesPanel.alpha != 0)
        {
            UpdateDesPanelUI(latestShowCard);//降低效率，后期可以考虑优化
        }
    }

    /// <summary>
    /// 更新描述面板UI
    /// </summary>
    public void UpdateDesPanelUI(Card cardInfo)
    {
        this.latestShowCard = cardInfo;//缓存数据
        if (cardInfo is CharacterCard)
        {
            CharacterCard charater = cardInfo as CharacterCard;
            cardNameLabel.text = charater.GetCardName();
            cardLevelLabel.text = charater.GetCardLevel().ToString();
            cardDamageLabel.text = charater.GetBaseCardDamageValue().ToString();
            cardSpeedLabel.text = charater.GetBaseCardSpeedValue().ToString();

            ClearStateList();//清空状态列表

            List<StateSkill> states = charater.GetCardState();
            if (states.Count == 0)
            {
                stateListWidget.alpha = 0;
            }
            else
            {
                //有状态
                stateListWidget.alpha = 1;

                int addedDamage = 0;
                int addedSpeed = 0;

                foreach (StateSkill state in states)
                {
                    //实例化并修正位置
                    GameObject prefab = Resources.Load<GameObject>("State");
                    GameObject stateGo = NGUITools.AddChild(stateListGrid.gameObject, prefab);
                    stateListGrid.Reposition();

                    //修改信息
                    stateGo.GetComponentInChildren<UILabel>().text = state.GetSkillShowName();

                    //状态改变
                    if (state is AttackUp)
                    {
                        AttackUp temp = state as AttackUp;
                        addedDamage += temp.GetAddedDamage();
                    }
                }

                //附加值
                if (addedDamage != 0)
                {
                    cardDamageLabel.text += string.Format("( + {0})", addedDamage);
                }
                if (addedSpeed != 0)
                {
                    cardSpeedLabel.text += string.Format("( + {0})", addedSpeed);
                }
            }
        }


    }

    /// <summary>
    /// 清空状态列表
    /// </summary>
    private void ClearStateList()
    {
        int childcount = stateListGrid.transform.childCount;
        for (int i = childcount; i > 0; i--)
        {
            DestroyImmediate(stateListGrid.transform.GetChild(0).gameObject);
        }
    }
}