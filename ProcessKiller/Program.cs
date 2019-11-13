using System;
using System.Diagnostics;
using System.Threading;

namespace PK
{
    class InputParams
    {
        public readonly string _processName = "notepad";
        public readonly float _allowableTimeMins = 0.02F;
        public readonly float _checkFreqMins = 0.01F;

        public InputParams(string[] programArgs)
        {
            int Length = programArgs.Length;
            if (Length != 0)
            {
                _processName = programArgs[0];

                if (Length > 1)
                {
                    try
                    { _allowableTimeMins = Convert.ToSingle(programArgs[1]); }
                    catch
                    { Console.WriteLine("Параметр \"Допустимое время(мин)\" имеет недопустимое значение"); }
                }

                if (Length > 2)
                {
                    try
                    { _checkFreqMins = Convert.ToSingle(programArgs[2]); }
                    catch
                    { Console.WriteLine("Параметр \"Частота проверки(мин)\" имеет недопустимое значение"); }
                }
            }

            Console.WriteLine("Параметр \"Название процесса\" равен {0}", _processName);
            Console.WriteLine("Параметр \"Допустимое время(мин)\" равен {0}", _allowableTimeMins);
            Console.WriteLine("Параметр \"Частота проверки(мин)\" равен {0}", _checkFreqMins);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            InputParams Params = new InputParams(args);//Входные аргументы
            float ProcRunTime = 0;

            do
            {
                Process[] listprosecc = Process.GetProcesses();
                foreach (Process oneproc in listprosecc)
                {
                    string ProsessName = oneproc.ProcessName;

                    if (ProsessName.Equals(Params._processName))
                    {
                        if (ProcRunTime > Params._allowableTimeMins)
                        {
                            oneproc.Kill();
                            Console.WriteLine("Процесс с именем {0} убит", Params._processName);
                            ProcRunTime = 0;
                        }
                        else
                        {
                            ProcRunTime += Params._checkFreqMins;
                        }
                    }
                }

                Thread.Sleep( (int)(Params._checkFreqMins * 60 * 1000) );
            }
            while (true);

        }
    }
}
