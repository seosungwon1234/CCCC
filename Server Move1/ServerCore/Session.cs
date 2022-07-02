using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace ServerCore
{
    class Session
    {
        Socket _socket; //큰 소켓 어디서든 사용 가능 

        public void init(Socket socket)
        {
            _socket = socket; //소캣은 소캣이다 
            SocketAsyncEventArgs recvArgs = new SocketAsyncEventArgs();
            recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);

            //바로 성공하는거면 바로 실행이 되고 조금 이따 성공할꺼면 콜백으로 넣은 함수가 
            //OnRecvCompleted가 실행된다. 

            recvArgs.SetBuffer(new byte[1024], 0, 1024); //버퍼를 받아주세요 1024
            //큰 값을 할당한다음에 세션끼리 나누는 경우도 있다 세션을 만들때마다 버퍼를 할당하도록 한다. 
            RegisterRecv(recvArgs);

        }

        public void Send(byte[] sendBuff) //
        {
            _socket.Send(sendBuff);
        }

        public void Disconnect()
        {
            _socket.Shutdown(SocketShutdown.Both); //서버를 종료할거니까 경고를 줄게
            _socket.Close(); //서버가 종료야 
        }


        #region 네트워크 통신 

        void RegisterRecv(SocketAsyncEventArgs arg)
        {
            bool pending = _socket.ReceiveAsync(arg);
            if (pending == false)
                OnRecvCompleted(null, arg);
        }



        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {  //몇바이트를 내가 받앗느냐 0보다 커야 한다 무조건 
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            //0보다 커야 하고 소캣에러가 성공인지 아닌지도 분간해야 한다. 
            {
                try //문법이 틀렸는지 아닌지 확인함 
                {

                    string recvData = Encoding.UTF8.GetString(args.Buffer, args.Offset, args.BytesTransferred);
                    //문자열의 리시브 데이터= utf8이고 읽기쓰기 가능한 문자( 버퍼 어디서부터 시작하냐 오프셋 몇바이트를 받앗는지?)
                    Console.WriteLine($"[From Client] {recvData}");
                    RegisterRecv(args); //콜백함수 

                }
                catch (Exception e)
                {
                    Console.WriteLine($"OnRecvCompleted Failed {e} "); //실패 했을때 에러를 뱉어줌 
                }




            }
            else
            {

            }
        }
        #endregion
    }
}

