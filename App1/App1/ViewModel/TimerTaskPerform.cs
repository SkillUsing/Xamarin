using System.Threading;
using Android.Widget;

namespace App1
{
    public class TimerTaskPerform
    {
        private Timer _tmr;


        public void StartTask(TimerTaskEventHandler task)
        {
            var timerDelegate = new TimerCallback(task);
            var timer = new Timer(timerDelegate, this, 0, 1000);
            _tmr = timer;
        }

        public void EndTask(TimerTaskEndEventhandler task)
        {
            task();
            _tmr.Dispose();
        }

        //任务委托
        public delegate void TimerTaskEventHandler(object state);

        public delegate void TimerTaskEndEventhandler();
    }
}