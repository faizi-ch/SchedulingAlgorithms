using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulingAlgorithms
{
    class Program
    {
        
        static void Main(string[] args)
        {
            SchedulingAlgorithms schedulingAlgorithms = new SchedulingAlgorithms();
            schedulingAlgorithms.InputProcessesData();
            
            Console.ReadKey();
        }

        class SchedulingAlgorithms
        {
            int numOfProcesses = 0;
            int[] arrivalTimes, burstTimes, waitingTimes, turnaroundTimes, startTimes;
            private string header;
            public void InputProcessesData()
            {
                Console.Write("How many processes you wanna enter? ");
                numOfProcesses = Convert.ToInt32(Console.ReadLine());

                arrivalTimes = new int[numOfProcesses];
                burstTimes = new int[numOfProcesses];

                Console.WriteLine("\nEnter Arrival Time and Burst Time of processes:\n\n");
                Console.WriteLine("Process\t\tArrival Time\tBurst Time");
                Console.WriteLine("------------------------------------------");
                for (int i = 0; i < numOfProcesses; i++)
                {
                    Console.Write("P" + i);
                    Console.SetCursorPosition(16, 7 + i);
                    arrivalTimes[i] = Convert.ToInt32(Console.ReadLine());
                    Console.SetCursorPosition(32, 7 + i);
                    burstTimes[i] = Convert.ToInt32(Console.ReadLine());

                }
                Console.WriteLine("------------------------------------------");
                Console.ReadKey();
                AlgorithmSelectionMenu();
            }
            
            void AlgorithmSelectionMenu()
            {
                Console.Clear();
                Console.WriteLine(
                    "1. First-Come, First-Served (FCFS) Scheduling\n2. Shortest-Job-First (SJF) Non-Preemptive\n3. Shortest-Job-Next (SJN) Scheduling");
                Console.Write("\nChoose an algorithm: ");
                int algo = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine();

                switch (algo)
                {
                    case 1:
                        {
                            FIFO();
                            break;
                        }
                    case 2:
                        {
                            SJF();
                            break;
                        }

                    default:
                        {
                            Console.WriteLine("\aWrong Entry!\nPress any key to try again...");
                            Console.ReadKey();

                            AlgorithmSelectionMenu();
                            break;
                        }

                }
            }

            void Display()
            {
                header = "Process\t\tArrival Time\tBurst Time\tWaiting Time\tTurnaround Time";
                Console.WriteLine(header);
                Console.WriteLine("-------------------------------------------------------------------------------");
                for (int i = 0; i < numOfProcesses; i++)
                {
                    
                    Console.Write("P" + i);
                    Console.SetCursorPosition(16, 11 + i);
                    Console.Write(arrivalTimes[i]);

                    Console.SetCursorPosition(32, 11 + i);
                    Console.Write(burstTimes[i]);

                    Console.SetCursorPosition(48, 11 + i);
                    Console.WriteLine(waitingTimes[i]);

                    Console.SetCursorPosition(64, 11 + i);
                    Console.WriteLine(turnaroundTimes[i]);

                }
                Console.WriteLine("-------------------------------------------------------------------------------");
            }

            void FIFO()
            {
                double sumWaitingTimes = 0;
                double sumTurnaroundTimes = 0;
                startTimes = new int[numOfProcesses];
                waitingTimes = new int[numOfProcesses];
                turnaroundTimes=new int[numOfProcesses];
                
                for (int i = 0; i < numOfProcesses; i++)
                {
                    if (i==0)
                    {
                        startTimes[i] = arrivalTimes[i];
                    }
                    else
                    {
                        startTimes[i] = startTimes[i - 1] + burstTimes[i - 1];
                    }
                    waitingTimes[i] = startTimes[i] - arrivalTimes[i];
                    sumWaitingTimes += waitingTimes[i];

                    turnaroundTimes[i] = burstTimes[i] + waitingTimes[i];
                    sumTurnaroundTimes += turnaroundTimes[i];
                }
                
                double averageWaitingTime = sumWaitingTimes/numOfProcesses;
                double averageTurnaroundTime = sumTurnaroundTimes/numOfProcesses;
                Display();
                Console.WriteLine("Average Waiting Time: "+averageWaitingTime);
                Console.WriteLine("Average Turnaround Time: " + averageTurnaroundTime);
            }

            void SJF()
            {
                double sumWaitingTimes = 0;
                double sumTurnaroundTimes = 0;
                startTimes = new int[numOfProcesses];
                waitingTimes = new int[numOfProcesses];
                turnaroundTimes = new int[numOfProcesses];


                int temp, temp2;
                int[] t = new int[numOfProcesses];
                for (int i = 0; i < numOfProcesses; i++)
                {
                    t[i] = burstTimes[i];
                }
                
                for (int i = 1; i < numOfProcesses; i++)
                {
                    for (int j = 0; j < numOfProcesses - 1; j++)
                    {
                        if (t[j]>t[j+1])
                        {
                            temp = t[j];
                            t[j] = t[j + 1];
                            t[j + 1] = temp;
                        }
                    }
                }

                for (int i = 0; i < numOfProcesses; i++)
                {
                    if (i == 0)
                    {
                        waitingTimes[i] = 0;
                    }
                    else
                    {
                        waitingTimes[i] = waitingTimes[i - 1] + t[i - 1];
                    }
                   
                    sumWaitingTimes += waitingTimes[i];

                    turnaroundTimes[i] = t[i] + waitingTimes[i];
                    sumTurnaroundTimes += turnaroundTimes[i];
                }
                int a,n;
                for (n = 0; n < numOfProcesses; n++)
                {
                    for (int j = 0; j < numOfProcesses; j++)
                    {
                        if (burstTimes[n]==t[j])
                        {
                            a = waitingTimes[n];
                            waitingTimes[n] = waitingTimes[j];
                            waitingTimes[j] = a;

                            a = turnaroundTimes[n];
                            turnaroundTimes[n] = turnaroundTimes[j];
                            turnaroundTimes[j] = a;
                        }
                    }
                }
                a = waitingTimes[0];
                waitingTimes[0] = waitingTimes[n-1];
                waitingTimes[n-1] = a;
                a = turnaroundTimes[0];
                turnaroundTimes[0] = turnaroundTimes[n-1];
                turnaroundTimes[n-1] = a;

                double averageWaitingTime = sumWaitingTimes / numOfProcesses;
                double averageTurnaroundTime = sumTurnaroundTimes / numOfProcesses;
                Display();
                Console.WriteLine("Average Waiting Time: " + averageWaitingTime);
                Console.WriteLine("Average Turnaround Time: " + averageTurnaroundTime);
                
            }

        }

       
        
    }
}
