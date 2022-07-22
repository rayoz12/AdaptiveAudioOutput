using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdaptiveAudioOutput.SVModel;
using Newtonsoft.Json;

namespace AdaptiveAudioOutput {
    internal class SoundVolumeViewInterface {

        string svclBinary = "svcl.exe";
        bool debugLogging = false;

        public SoundVolumeViewInterface(bool debugLogging = false, string svclBinary = "svcl.exe") {
            this.svclBinary = svclBinary;
            this.debugLogging = debugLogging;
        }

        public List<SVItem> launchsvclAndGetOuptut() {
            var proc = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = svclBinary,
                    Arguments = "/sjson",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            proc.Start();
            var output = proc.StandardOutput.ReadToEnd();
            if (debugLogging) {
                Console.WriteLine(output);
            }
            return JsonConvert.DeserializeObject<List<SVItem>>(output);
        }

        public SVItem getDefaultAudioDevice() {
            var items = launchsvclAndGetOuptut();
            return items.Find(it => it.Type == "Device" && it.Default == "Render");
        }

        public List<SVItem> getRenderDevices() {
            return launchsvclAndGetOuptut().Where(it => it.Type == "Device" && it.Direction == "Render").ToList();
        }

        public void setAudioDevice(string name) {
            var proc = new Process {
                StartInfo = new ProcessStartInfo {
                    FileName = svclBinary,
                    Arguments = $"{svclBinary} /SetDefault \"{name}\" 1",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            if (debugLogging) {
                Console.WriteLine(proc.StartInfo);
            }

            proc.Start();
            proc.WaitForExit();
            
            if (debugLogging) {
                Console.WriteLine(proc.StandardOutput.ReadToEnd());
            }
        }
    }
}
