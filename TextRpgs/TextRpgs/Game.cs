using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    public enum GameMode  //클래스 타입의 상수 이넘 
    {
        None,
        Lobby,
        Town,
        Field
    }

    class Game
    {
        private GameMode mode = GameMode.Lobby; //비공개 클래스 게임 안의  로비는 비공개 게임모드 안에 저장 
        private Player player = null;  //비공개 플레이어 값을 값이 없다로 저장
        private Monster monster = null;
        private Random rand = new Random(); //랜덤값을 새로 생성하여 비공개 rand로 저장 

        public void Process() //내부외부 접속가능 반환하지않는 프로세스 
        {
            switch (mode)    //반복 모드 
            {
                case GameMode.Lobby:   //선택문 게임모드로비는 로비
                    ProcessLobby(); //프로세스 클래스안의 로비함수 선택하면  38행 비공개 반환하지않는 프로세스 로비로 간다.
                    break; //반복문 나감 
                case GameMode.Town: //선택 게임모드 마을은 마을
                    ProcessTown(); //프로세스 클래스 안의 마을 함수 선택하면 64행 비공개 반환하지않는 마을 로비로 간다. 
                    break;
                case GameMode.Field: //선택 클래스 안의 사냥터 함수 선택하면 83행 비공개 반환하지 않는 사냥터 로 간다 .
                    ProcessField();
                    break;
            }
        }

        private void ProcessLobby()  //로비로 간다를 선택하엿을때~~~
        {
            Console.WriteLine("직업을 선택하세요"); //출력이ㅡ나온다 
            Console.WriteLine("[1] 기사");
            Console.WriteLine("[2] 궁수");
            Console.WriteLine("[3] 법사");

            string input = Console.ReadLine(); //커서가 계속 깜빡인데 

            switch (input)//반복 입력값
            {
                case "1":  //케이스 1 
                    player = new Knight(); //기사값은 플레이어함수에 저장 
                    mode = GameMode.Town; //사용은 타운에 게임모드함수 모드에 저장
                    break; //반복문을 나간다. 
                case "2":
                    player = new Archer();  //요정값은 플레이어 함수에 저장
                    mode = GameMode.Town; //사용은 타운에게임모드 함수 모드에 저장 
                    break; //나간다 
                case "3":
                    player = new Mage();   //마법사 값은 플레이어 함수에 저장 
                    mode = GameMode.Town; //사용은 타운에 게임모드 함수에저장
                    break;  //나간닾
            }
        }

        private void ProcessTown()
        {
            Console.WriteLine("마을에 입장 했습니다.");
            Console.WriteLine("[1] 필드로 간다.");
            Console.WriteLine("[2] 로비로 돌아가기.");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    mode = GameMode.Field;
                    break;
                case "2":
                    mode = GameMode.Lobby;
                    break;
            }
        }

        private void ProcessField()
        {
            Console.WriteLine("필드에 입장 했습니다.");
            Console.WriteLine("[1] 전투 모드 돌입");
            Console.WriteLine("[2] 일정 확률로 마을로 도망");

            CreateRandomMonster();

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    ProcessFight();
                    break;
                case "2":
                    TryEscape();
                    break;
            }
        }

        private void CreateRandomMonster()
        {
            int randValue = rand.Next(0, 3);

            switch (randValue)
            {
                case 0:
                    monster = new Slime();
                    Console.WriteLine("슬라임이 스폰 되었습니다!");
                    break;
                case 1:
                    monster = new Orc();
                    Console.WriteLine("오크가 스폰 되었습니다!");
                    break;
                case 2:
                    monster = new Skeleton();
                    Console.WriteLine("스켈레톤이 스폰 되었습니다!");
                    break;
            }
        }

        private void ProcessFight()
        {
            while (true)
            {
                int damage = player.GetAttack();
                monster.OnDamaged(damage);
                if (monster.IsDead())
                {
                    Console.WriteLine("승리했습니다!");
                    Console.WriteLine($"남은 체력 : {player.GetHP()}");
                    break;
                }
                damage = monster.GetAttack();
                player.OnDamaged(damage);
                if (player.IsDead())
                {
                    Console.WriteLine("패배했습니다!");
                    mode = GameMode.Lobby;
                    break;
                }
            }
        }

        private void TryEscape()
        {
            int randValue = rand.Next(0, 101);
            if (randValue <= 33)
                mode = GameMode.Town;
            else
                ProcessFight();
        }
    }
}