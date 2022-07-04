using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public enum CreatureType //클래스처럼 보이게 하는 상수 
    {
        None,
        Player = 1,
        Monster = 2
    }

    class Creature
    {
        CreatureType type;  // private

        //사실 protected의 문제는 아니고,
        //동일한 이름을 두 번 사용한게 근본적인 문제입니다.

        //CreatureType type에 protected를 안붙이면 기본적으로 private으로 인식되기 때문에
        //Player 클래스 내부에서는 그 존재를 모르지만,
        //CreatureType type의 접근 한정자를 public이나 protected로 설정할 경우
        // 자식 클래스인 Player에서도 CreatureType type을 사용할 수 있는 상태가 됩니다.
        //그런데 그런 상황에서 동일한 이름으로 PlayerType type을 또 만들어줬으니,
        // type이라는 것이 2가지의 의미를 지닌 셈이 되는거죠.

        //물론 이럴 경우에 type이 진짜 2가지 의미를 다 갖진 않고
       // Player한테 더 가까운 PlayerType type으로 사용이 되는데,
        //반대로 얘기하면 CreatureType type은 Player 내부에서 더 이상 접근할 수 없게 됩니다. (이름이 겹쳤기 때문)
        //그렇기 때문에 "CreatureType type의 멤버를 숨깁니다"라는 경고 메시지가 뜨는거죠.
        //의도적으로 그렇게 한 것이라면 앞에 'new'를 붙여서 new PlayerType type로 선언하는 방법이 있습니다.
        //그런데 굳이 그렇게 할 필요까진 있을까 싶고
        //혼동의 여지를 주지 않으려면 동일한 이름을 피해서
        //creatureType, playerType 등으로 구분해주는 것이 좋습니다.

        protected int hp = 0;
        protected int attack = 0;

        protected Creature(CreatureType type)
        {
            this.type = type;
        }

        public void SetInfo(int hp, int attack)
        {
            this.hp = hp;
            this.attack = attack;
        }

        public int GetHP() { return hp; }
        public int GetAttack() { return attack; }

        public bool IsDead() { return hp <= 0; }

        public void OnDamaged(int damage)
        {
            hp -= damage;
            if (hp < 0)
                hp = 0;
        }
    }
}