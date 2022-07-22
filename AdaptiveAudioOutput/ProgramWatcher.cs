using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;

namespace AdaptiveAudioOutput
{
    internal class ProgramWatcher
    {

        public delegate void OnProgramEvent(string programName);

        public event OnProgramEvent onProgramStarted;
        public event OnProgramEvent onProgramStopped;

        Dictionary<string, List<Process>> processMap = new Dictionary<string, List<Process>>();

        List<string> processesToWatch;

        ManagementEventWatcher startWatcher = null;
        WqlEventQuery startQuery;

        public ProgramWatcher(List<string> processesToWatch) {
            this.processesToWatch = processesToWatch;

            // Initialise the processMap
            foreach (var process in processesToWatch) {
                processMap[process] = new List<Process>();
            }

            try
            {
                startQuery = new WqlEventQuery();
                startQuery.EventClassName = "Win32_ProcessStartTrace";


                startWatcher = new ManagementEventWatcher(startQuery);
                startWatcher.EventArrived += new EventArrivedEventHandler(ProcessStartEventArrived);
                startWatcher.Start();
            }
            catch (Exception e) {
                Console.Error.WriteLine(e);
            }
        }

        public void Stop() {
            startWatcher?.Stop();
        }

        public void ProcessStartEventArrived(object sender, EventArrivedEventArgs e)
        {
            PropertyDataCollection eventProperties = e.NewEvent.Properties;
            string processName = (string) eventProperties["ProcessName"].Value;
            uint pid = (UInt32) eventProperties["ProcessID"].Value;

            // Console.WriteLine($"[Program Watcher] Program Started: {processName}. PID: {pid}");

            if (processesToWatch.Contains(processName)) {
                var p = Process.GetProcessById((int)pid);
                p.EnableRaisingEvents = true;
                p.Exited += (_, __) => {
                    onWatchedProcessExited(processName, p);
                };


                // None of this actually handles the problem of multithreading if this function is called at the same time.
                // i.e. it's not thread safe
                processMap[processName].Add(p);

                if (processMap[processName].Count == 1) {
                    onProgramStarted?.Invoke(processName);
                }
            }
        }

        private void onWatchedProcessExited(string processName, Process process) {
            // We'll just need to check if this is the last instance of the program to exit and raise the event

            // Console.WriteLine($"[Program Watcher] Program Stopped: {processName}. PID: {process.Id}");

            processMap[processName].Remove(process);
            if (processMap[processName].Count == 0) {
                onProgramStopped?.Invoke(processName);
            }
        }
    }
}
