using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdaptiveAudioOutput
{
    internal class AdaptiveAudioOutput
    {
        private ProgramWatcher pw;
        private Dictionary<string, string> mappings;
        private string defaultDevice = "";

        SoundVolumeViewInterface svcl = new SoundVolumeViewInterface();


        public AdaptiveAudioOutput() {

            Console.WriteLine(@"    ___        __               __   _               ___               __ _         ____          __                 __ ");
            Console.WriteLine(@"   /   |  ____/ /____ _ ____   / /_ (_)_   __ ___   /   |  __  __ ____/ /(_)____   / __ \ __  __ / /_ ____   __  __ / /_");
            Console.WriteLine(@"  / /| | / __  // __ `// __ \ / __// /| | / // _ \ / /| | / / / // __  // // __ \ / / / // / / // __// __ \ / / / // __/");
            Console.WriteLine(@" / ___ |/ /_/ // /_/ // /_/ // /_ / / | |/ //  __// ___ |/ /_/ // /_/ // // /_/ // /_/ // /_/ // /_ / /_/ // /_/ // /_  ");
            Console.WriteLine(@"/_/  |_|\__,_/ \__,_// .___/ \__//_/  |___/ \___//_/  |_|\__,_/ \__,_//_/ \____/ \____/ \__,_/ \__// .___/ \__,_/ \__/  ");
            Console.WriteLine(@"                    /_/                                                                           /_/                   ");

            JObject config = JObject.Parse(File.ReadAllText(@"process_mapping.json"));

            mappings = config["processes"].ToObject<Dictionary<string, string>>();

            defaultDevice = config.GetValue("defaultDevice").ToString();

            if (defaultDevice == "__default__") {
                defaultDevice = svcl.getDefaultAudioDevice().CommandLineFriendlyID;
            }

            Console.WriteLine($"Using Default Device: {defaultDevice}");
            Console.WriteLine();

            var outputDevices = svcl.getRenderDevices();
            Console.WriteLine("Fetching valid Device IDs");
            Console.WriteLine("Name | ID (Use this in the config file)");
            Console.WriteLine("---------------------------------------");
            foreach (var outputDevice in outputDevices) {
                Console.WriteLine($"{outputDevice.Name} | {outputDevice.CommandLineFriendlyID}");
            }



            var programs = mappings.Keys.ToList();

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = false;

                pw = new ProgramWatcher(programs);

                pw.onProgramStarted += onProgramStarted;
                pw.onProgramStopped += onProgramStopped;

                Console.ReadLine();

            }).Start();
        }

        void onProgramStarted(string processName) {
            var device = mappings[processName];
            Console.WriteLine($"[AdaptiveAudioOutput] {processName} started. Changing output device to: {device}");
            svcl.setAudioDevice(device);
        }

        void onProgramStopped(string processName) {
            Console.WriteLine($"[AdaptiveAudioOutput] {processName} stopped. Changing output device to default device: {defaultDevice}");
            svcl.setAudioDevice(defaultDevice);
        }
    }
}
