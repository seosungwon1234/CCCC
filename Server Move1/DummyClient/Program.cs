using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName(); //내 로컬 컴퓨터에 시스템 이름 
            IPHostEntry iPHost = Dns.GetHostEntry(host); //방금 찾은 호스트 이름을 넣어준다. 
            IPAddress ipAddr = iPHost.AddressList[0]; //dns서버의 망 하나의 아이피 주소를 하나뽑았다.
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777); //아이피 엔드 포인트는 우리의 진짜 주소가 된다 포트는 무조건 정해야 하고 7777로 정해 준다 .
                                                                //식당에 비유하면 ipAddr이 정문이고 7777이 후문인데 문의 숫자다.  
                                                                //클라이언트가 포트번호가 다르면 입장이 불가하다. 

            while (true) //서버코어에 연속적인 패킷을 보내도 이상한 패킷이 나오지 않게 변경했는데
                         //서버를 실행해서 체크를 해볼것이다 
                         //무한 반복문을 설정하고.
                         //와일문으로 무한적으로 패킷을 보내게 해볼텐데 너무 많은 패킷을 보내면 안되니 때문에
                         //스레드 sleep를 이용해서 0.1초에 한번씩으로 제한한다. 
                         //서버의 다중 이용자들이 서버에 들어오고 나가고를 구현해 본것이다. 
            {


                try
                {

                    //휴대폰설정 문지기한테 입장 가능해? 
                    Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                    //문지기한테 입장 문의 
                    socket.Connect(endPoint); //입장문의 커넥트 나의 아이피 주소 
                    Console.WriteLine($"Connected To {socket.RemoteEndPoint.ToString()}");  //누구한테 입장문의 했는지?
                                                                                            //문의한 사람은? 소켓 접속한  내 아이피주소를 문자열로 출력하라 
                                                                                            //보낸다 먼저 서버에서 받았으니까 여기서는 먼저 보내준다. 

                    byte[] sendBuff = Encoding.UTF8.GetBytes("Hello World!");
                    int sendBytes = socket.Send(sendBuff); //반대로 먼저 보내고 읽기쓰기 get 헬로우월드 
                                                           //정수변수 샌드 바이트는 소켓에서 보낸다 



                    //받는다.

                    byte[] recvBuff = new byte[1024];
                    int recvBytes = socket.Receive(recvBuff);
                    string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes); //바이트를 스트링으로 변환하는 그런 작업이다.
                    Console.WriteLine($"[From Server] {recvData}");

                    //나간다

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();



                }

                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }


                Thread.Sleep(100);

            }
        }




    }
}
