using AUA.AiS_FruiT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CargoLampTest.Model;

namespace CargoLampTest.ViewModel
{
    public partial class MainViewModel
    {
        System.Threading.Thread _threadProcess;
        bool _isRunThread = false;
        public TestList _testList = new TestList();

        public void Start()
        {
            if (_isRunThread == false)
            {
                _isRunThread = true;
                _threadProcess = new Thread(Thread_Process);
                _threadProcess.Priority = ThreadPriority.Highest;
                _threadProcess.Start();
            }
        }

        public void Stop()
        {
            if (_isRunThread == true)
            {
                _isRunThread = false;
                _threadProcess.Join();
            }
        }

        private void Thread_Process()
        {
            bool newFlag = false;
            bool oldFlag = false;
            try
            {
                while (_isRunThread == true)
                {
                    //시작버튼이 눌려지면, 각 뷰모델에 있는 버튼도 같이 눌리도록 한다. 
                    newFlag = Program.IsRunSW;
                    if (oldFlag != newFlag)
                    {
                        oldFlag = Program.IsRunSW;
                        if (newFlag)
                        {
                            _USB7230ViewModel.StartCommand.Execute(_USB7230ViewModel);
                            _CurrentMeterViewModel.StartCommand.Execute(_CurrentMeterViewModel);
                        }

                        else
                        {
                            _USB7230ViewModel.StopCommand.Execute(_USB7230ViewModel);
                            _CurrentMeterViewModel.StopCommand.Execute(_CurrentMeterViewModel);
                        }
                    }

                    //자동테스트
                    if (Program.IsRunSW)
                    {
                        //Station시작 신호
                        bool startSwitchOn = _USB7230ViewModel._deviceUSB7230.ReadLine(DI.Start);
                        bool isReadyToStarted = false;
                        if (startSwitchOn)
                        {
                            Task.Delay(1000).Wait();
                            if (startSwitchOn)
                            {
                                isReadyToStarted = _USB7230ViewModel._deviceUSB7230.ReadLine(DI.Detection);
                            }

                        }
                        if (isReadyToStarted)
                        {
                            bool isSuccess = false;
                            CylinderDown(); //실린더 다운
                            TestStart();

                            //PostTestCommon();

                            if (isSuccess)
                            {
                                PostTestOK();
                                CylinderUp();
                                
                            }
                            else
                            {
                                PostTestNG();
                                
                            }
                            CylinderUp();  //실린더 업
                            SaveTestResult();

                            //TODO 제품 제거 하면 화면및 기타 리셋
                            SignalInit();
                        }


                    }
                    else
                    {

                    }
                    //꼭 필요하다. 
                    Task.Delay(1000).Wait();


                    string time = DateTime.Now.ToString();
                    string[] abc = time.Split(':');
                    int abcd = Convert.ToSByte(abc[2]);
                    int rem = abcd % 2;

                    if (rem == 0)
                    {
                        Console.WriteLine("-");
                        //_USB7230ViewModel.Items[0].IsSelected = true;
                    }
                    else
                    {
                        Console.WriteLine("안");
                        //_USB7230ViewModel.Items[0].IsSelected = false;
                    }

                    

                }
            }
            catch (Exception ex)
            {
                eLogger.Error(ex.ToString());
            }

            finally
            {

            }
        }

        private void PostTestNG()
        {
            //NG후 메시지 박스 표현 => NG 스위치 길게 눌렀을때 빠져나감. 
        }

        private void PostTestOK()
        {
            //OK후 바코드 라벨 발행
        }

        private void CylinderDown()
        {
            _USB7230ViewModel._deviceUSB7230.WriteLine(DO.CylinderDown, 1);
            Task.Delay(1000).Wait();
            _USB7230ViewModel._deviceUSB7230.WriteLine(DO.CylinderDown, 0);
        }
        private void CylinderUp()
        {
            _USB7230ViewModel._deviceUSB7230.WriteLine(DO.CylinderUp, 1);
            Task.Delay(1000).Wait();
            _USB7230ViewModel._deviceUSB7230.WriteLine(DO.CylinderUp, 0);
        }
        private void TestStart()
        {
            //TestLogStart();

            //계측기, DIO, PS 초기화
            //InitAll();

            try
            {
                ////테스트 진입점
                //릴레이 연결 안되어 있을때 전류 전압측정 => 무부하

                // 보드 1 테스트

                _USB7230ViewModel.Items[0].IsSelected = true;
                try
                {
                    _CurrentMeterViewModel._deviceMT4N.ModbusSerialRTUMasterReadRegisters();
                }
                catch (Exception ex)
                {
                    eLogger.Error(ex.ToString());
                }

                _USB7230ViewModel.Items[0].IsSelected = false;

                //TODO 테스트관련 모든 Method를 리스트 박스에 보여줄수 있을까?

                //MeasureValue1 = _VoltMeterViewModel._deviceMT4N._measureValue[0];
                //MeasureValue2 = _VoltMeterViewModel._deviceMT4N._measureValue[1];
                //MeasureValue3 = _VoltMeterViewModel._deviceMT4N._measureValue[2];

                string time = DateTime.Now.ToString();
                string[] abc=time.Split(':');
                int abcd = Convert.ToSByte(abc[2]);
                int rem = abcd % 2;

                if (rem==0)
                {
                    Console.WriteLine("-");
                    _HomeViewModel.TestFields[0].DetailItem = "-------";
                    //_HomeViewModel.RaisePropertyChanged("TestFields");
                }
                else
                {
                    Console.WriteLine("안");
                    _HomeViewModel.TestFields[0].DetailItem = "안녕하세요";
                    //_HomeViewModel.RaisePropertyChanged("TestFields");
                }
                


                //isSuccess = TestProcess();
            }
            catch (System.Exception ex)
            {
                //isSuccess = false;
                eLogger.Error(ex.ToString());
            }
            finally
            {

            }
        }
        private void MainProcess()
        {

            //double measure=_VoltMeterViewModel._deviceMT4N._measureValue[1];

            //uint measure1 = _USB7230ViewModel._deviceUSB7230.ReadPort();

            _HomeViewModel.CycleTime = DateTime.Now.ToString();

            //TODO 기존 process참조하여 진행

        }


        /// <summary>
        /// 검사 결과데이터 DB에 저장하기
        /// </summary>
        /// String sql = "INSERT INTO [dbo].[RESULT_SEARCH] ([WORK_DATE],[MODEL],[SCAN_BARCODE],[OUT_BARCODE],[RESULT],[ID],[NAME])"+
        //	"VALUES ('"+dataTIme+"','"+model+"','"+scanBCR+"','"+outBCR+"','"+result+"','"+id+"','"+name+"')";
        private bool SaveResultData(ResultSearch rs)
        {
            try
            {
                String sql = "INSERT INTO [RESULT_SEARCH] " +
                    "([WORK_DATE],[MODEL],[SCAN_BARCODE],[OUT_BARCODE],[RESULT],[ID],[NAME],[MEAS1],[MEAS2],[MEAS3],[MEAS4],[MEAS5])"
                    + "VALUES ('"
                             + rs.WORK_DATE + "','"
                             + rs.MODEL + "','"
                             + rs.SCAN_BARCODE + "','"
                             + rs.OUT_BARCODE + "','"
                             + rs.RESULT + "','"
                             + rs.ID + "','"
                             + rs.NAME + "','"
                             + rs.MEAS1 + "','"
                             + rs.MEAS2 + "','"
                             + rs.MEAS3 + "','"
                             + rs.MEAS4 + "','"
                             + rs.MEAS5 + "')";
                bool isSuccess = _db.ExecuteQuery(sql);
                if (isSuccess != true)
                {
                    eLogger.Error("DB저장 실패!!!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                eLogger.Error(ex.ToString());
                return false;
            }
            return true;
        }

        private void SaveTestResult()
        {

        }
        private void SignalInit()
        {
        }
    }
}
