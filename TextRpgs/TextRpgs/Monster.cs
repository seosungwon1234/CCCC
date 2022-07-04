using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public enum MonsterType  //클래스 상수 몬스터타입 
    {
        None = 0,
        Slime = 1,
        Orc = 2,
        Skeleton = 3
    }

    class Monster : Creature //클래스 몬스터 크리처  타입이란 상황이 있다 몬스터 플레이어 크리처 혼동되지않게 붙여줌 
    {
        protected MonsterType type = MonsterType.None;
        // 같은 class 또는 해당 class 에서 파생된 class 의 코드에서만 형식 또는 멤버에 액세스할 수 
        protected Monster(MonsterType type) : base(CreatureType.Monster)
        {
            this.type = type;
        }

        //CreatureType의 문제는 아니고,
        //base를 이용해 Creature의 생성자를 호출하려 하는데
        //그 부분에 public이나 protected이 누락된 것이 아닐까

       // public Creature(CreatureType type) 혹은 protected Creature(CreatureType type)
        //으로 되어 있는지 확인.
        //둘다 되는 이유는 protected Monster(MonsterType type) : base(CreatureType.Monster)
        //여기서 protected보다 보호수준이 낮거나 같으면
        public MonsterType GetMonsterType() { return type; } //공공의 몬스터타입 읽고쓰는 방식 반환함 타입을
    }

    class Slime : Monster //클래스슬라임몬스터
    {
        public Slime() : base(MonsterType.Slime) //베이스를 통해 공개 슬라임함수를 호출 
        {
            SetInfo(10, 1);
        }
    }

    class Orc : Monster
    {
        public Orc() : base(MonsterType.Orc) //베이스를 통해 공개 오크함수를 호출 
        {
            SetInfo(20, 2);
        }
    }

    class Skeleton : Monster
    {
        public Skeleton() : base(MonsterType.Skeleton)  //베이스를 통해 공개 스켈래톤 함수를 호출 
        {
            SetInfo(15, 5);
        }
    }
}
