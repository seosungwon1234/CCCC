using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace myListener
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("서버콘솔창. \n\n\n");

            // TcpListener 생성 뒤에붙는 매개변수는 첫번째는 어떤 ip를 받을지,
            // 두번째는 port를 설정해준다.
            TcpListener server = new TcpListener(IPAddress.Any, 9999);

            // 서버를 시작한다.
            server.Start();


            // 클라이언트 객체를 만들어 9999에 연결한 client를 받아온다
            TcpClient client = server.AcceptTcpClient();

            byte[] byteData = new byte[1024];


            // client가 write한 정보를 읽어옵니다.
            client.GetStream().Read(byteData, 0, byteData.Length);

            // Socket은 byte[] 형식으로 데이터를 주고받으므로 출력을 위해 
            // string형으로 바꿔줍니다.
            Console.WriteLine(Encoding.Default.GetString(byteData));

            server.Stop();
        }
    }

}
