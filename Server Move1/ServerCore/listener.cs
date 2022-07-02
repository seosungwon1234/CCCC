using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    class Listener
    {
        Socket _listenSocket;
        Action<Socket> _onAcceptHander; //액션으로 만든다 엑셉트가 완료면 처리?

        public void init(IPEndPoint endPoint, Action<Socket> onAcceptHander) //초기화할때 같이 받아준다.
        {
            //문지기가 들고있는 휴대폰 리슨 소켓
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _onAcceptHander += onAcceptHander;// 리셋소캣을 만들어주고 이부분을 OnAcceptCompleted의 if함수에 적용하여 준다. 




            //소켓의 리슨 타입은 새로운 소켓인데(엔드포인트는 아이피 버전 4냐 6냐 자동으로 dns에서 만들어 줬기에 그냥 넣어도 된다
            // TCP를 사용할것인지 UDP를 사용할것인지 넣어주는데 TCP로 작업한다. TCP는 소켓타입이 무조건 스트림이다.

            //문지기 교육 (문지기 핸드폰에 IPHost와 AddressList를 넣어준다 오류가 나지 않게 
            //리슨소켓에 우리가 찾는 아이피 주소를 넣어주는데 Bind로 넣어주면된다 엔드 포인트는 우리의 진짜 주소이다 
            _listenSocket.Bind(endPoint);

            //영업시작


            _listenSocket.Listen(10); //리슨소켓의 리슨은 서버의 대기자수이다. 백로그 최대대기자수 천명이 동시접속하였을때 
                                      //문지기가 안내를 해줄때까지 대기자수 이고 이상이면 불가가 뜨게된다 
                                      //손님이 한번만 받고 끝날게 아니기에 그렇다 

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            //소켓 에이싱크 이벤트 아그스는 한번 해주면 계속 사용할수 있다. 
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            RegisterAccept(args); //★낚시대를 던졌다  물고기가 잡혔다 ★★
        }   // new를 만들어줬는데 필요할때마다 메세지를 전달해주고 소켓도 가르쳐준다 
            //가져온다음에 날린다음에 만드는게 아니라 리지스터 엑셉트에 똑같이 새로운 소켓을 만들어서 넣어주었다.
            //성능은 좋아지지만...


        void RegisterAccept(SocketAsyncEventArgs args) //리지스터 엑셉트 등록형 함수 반환하지않는 
        {
            args.AcceptSocket = null;

            bool pending = _listenSocket.AcceptAsync(args);//비동기 많이 사용한다. 
            //기다리지 않고 바로 내려오고 클라이언트 소캣을 콜백방식으로 호출한다 (접속시) 
            //c++과 똑같다 서버를 만들때도 같기에 연습해놓으면 좋다. 요청이 되었는데 리턴값은 참과 거짓으로 뱉는데
            //팬딩값이다. 비동기 방식으로 예약만 한다는 의미가 된다. 
            if (pending == false) //만약 밴딩이 펄스 라면 에이싱크 비동기 버전을 호출했지만 바로 완료가 되었다고 한다.
                //예약만 한다는 의미 그래서 리지스터 엑셉트 팬딩이 트루면 호출안댐. 자동으로 On엑셉트 를 호출한다 
                OnAcceptCompleted(null, args);//팬딩이 트루라면? 거꾸로 통보가 온다. 유저가 커넥트 요청이 와서 억셉트 요청이
                                              //왔다면 해주고 끝낫다면 리지스터를 다시 한번 던져 주는데?  모든일이 끝낫으니 다음유저를 위해서 등록한다.
                                              //로직을 따라가보면 엑셉트나 리시브나 샌드하는 부분도 다 그렇다. 맨처음 리지스터 한거는 낙시대를 강물에 휙 던졌고
                                              //랜덤성 바로 처리되고 안되고 팬딩이 펄스면 바로 잡혔다 트루면? 조금이따가 자동으로 잡는다. 
                                              //리지스터를 한번 했으면 on리지스터 컴플리트가 한번에 될텐데 물고기를 통에 넣고 그다음에 또 잡으러 간다.




        }

        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args) //등록이 완료 되엇을때 
        {     //물고기를 낚았다 그리고 통에 집어 넣었다. ★★ 
            if (args.SocketError == SocketError.Success) //소캣 에러를 확인한다 소켓에러가 같다면?
                                                         //소캣에러가 석세스 즉, 에러가 없다면? 
            {
                _onAcceptHander.Invoke(args.AcceptSocket); //어떻게 보면 일꾼같은 느낌인데?
                                                           //당장 값을 추출하기 불가능 하니 먼저 socketAsync이벤트아그스 값을 불러온다
                                                           //onAcceptHsnder을 엑셉트 소켓에 적용시켜 불러온다. 


            }
            else
                Console.WriteLine(args.SocketError.ToString());

            RegisterAccept(args);//낚시대를 다시 한번 던져야 한다. ★★
        }

        // void OnAcceptCompleted(object sender, SocketAsyncEventArgs args 67번 
        //RegisterAccept(args); 81번 이렇게 넣어주게 되면 나중에 돌아갈때 크래시가 나게 된다
        //두번째 바퀴를 돌때는, 엉뚱한 값을 가지고 있게 된다는 거니까 
        //그래서 48번처럼 null을 넣어주어 크러시가 나지 않게 한다. 초기화로 해준다 한번 밀어준다 null로



    }
}
