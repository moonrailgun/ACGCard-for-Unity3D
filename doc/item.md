# 道具 #
----------

## 道具分类 ##

- 装备卡(Equipment)
- 功能卡(Feature)

## 道具功能 ##

- Heal(回复生命)-int
- HealPer(百分比回复生命)-float(0~1)
- Replenish(回复魔力)-int
- ReplenishPer(百分比回复魔力)-float(0~1)
- AddState(添加状态)-object(状态对象)

## 道具DAL对象格式 ##
- 名字
- 类型
- 简介
- 贴图名

### example ###
    {
		"Name":"红宝石(堆)",
		"Type":"Feature",
		"Des":"可以恢复角色最大生命值一半的血量",
		"Pic":"Item-RubyPile",
		"Action":{
			"HealPer":0.5
		}
	}