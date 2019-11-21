using System;
using System.Threading;
using System.IO;

namespace Core.Personal
{
    public partial class Think
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the program.");

            var autoEvent = new AutoResetEvent(false);
            var think = new Think();

            try
            {
                think.ThinkTimer = new Timer(think.Poll, autoEvent, 0, Timeout.Infinite);

                autoEvent.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                think.ThinkTimer.Dispose();
            }
        }        
    }
}