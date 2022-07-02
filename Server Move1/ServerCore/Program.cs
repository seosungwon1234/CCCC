using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        static Listener _listener = new Listener();

        static void OnAcceptHandler(Socket clientSocket)

        {

            try
            {
                //받는다  블로킹 함수를 사용한다면 만명의 유저를 소통하고 해야한다.보내고 받아야 하는데 블로킹함수로 무한대기 자체는 말도 안대는 것이다. 
                //비동기 함수 논블로킹 방식으로 해야 한다. 
                byte[] recvBuff = new byte[1024];
                int recvBytes = clientSocket.Receive(recvBuff); //받을수 있긴한데 buff를 넣어준다 몇바이트를 받았냐 계산을 한다 몇개를 보낼지 모르니 1024로 한다.
                                                                //보내준 데이터를 1024에 저장이 된다 recvbuff에 저장이 된다.
                Encoding.UTF8.GetString(recvBuff, 0, recvBytes);//문자열을 받는다고 해서 간단하게한라고 생각한다. 
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes); //실제 게임은 변수 데이터 
                Console.WriteLine($"[From Client] {recvData}"); //클라이언트에서 온 메세지이고, 데이터를 출력해준다.

                //보내준다

                byte[] sendBuff = Encoding.UTF8.GetBytes("welcom to MMORPG Server!"); //문자를 버퍼로 만들어줌
                clientSocket.Send(sendBuff);

                //쫓아낸다 

                clientSocket.Shutdown(SocketShutdown.Both); //서버를 종료할거니까 경고를 줄게
                clientSocket.Close(); //서버를 닫는다. 

            }

            catch (Exception e)
            {
                Console.WriteLine(e);
            }




        }

        static void Main(string[] args)
        {
            string host = Dns.GetHostName(); //내 로컬 컴퓨터에 시스템 이름 
            IPHostEntry iPHost = Dns.GetHostEntry(host); //방금 찾은 호스트 이름을 넣어준다. 
            IPAddress ipAddr = iPHost.AddressList[0]; //dns서버의 망 하나의 아이피 주소를 하나뽑았다.
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //아이피 엔드 포인트는 우리의 진짜 주소가 된다 포트는 무조건 정해야 하고 7777로 정해 준다 .
                                                                //식당에 비유하면 ipAddr이 정문이고 7777이 후문인데 문의 숫자다.  
                                                                //클라이언트가 포트번호가 다르면 입장이 불가하다. 







            _listener.init(endPoint, OnAcceptHandler);
            Console.WriteLine("Listening....");
            //리스너 cs에서 문지기 교육과 영업시작의 대기열이  호출이 된다 
            //문지기야 우리의 초기화부분은 엔드포인트 실제 아이피 주소 값이고.
            //누가 접속한다면 OnAcceptHandler로 알려줘★★

            while (true) {; }










        }
    }
}

