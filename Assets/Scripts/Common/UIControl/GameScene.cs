using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameScene : MonoBehaviour
{
    public GameCard cardList = new GameCard();

    private void Awake()
    {

        //获取对战信息
        //获取卡片列表
    }











    public class GameCard
    {
        public List<Card> OwnerCharacterCard = new List<Card>();
        public List<Card> EnemyCharacterCard = new List<Card>();
        public List<Card> OwnerHandCard = new List<Card>();
        public List<Card> EnemyHandCard = new List<Card>();
    }
}


