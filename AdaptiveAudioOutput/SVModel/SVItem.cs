using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AdaptiveAudioOutput.SVModel {
    internal class SVItem {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Direction { get; set; }

        [JsonProperty("Device Name")]
        public string DeviceName { get; set; }
        public string Default { get; set; }

        [JsonProperty("Default Multimedia")]
        public string DefaultMultimedia { get; set; }

        [JsonProperty("Default Communications")]
        public string DefaultCommunications { get; set; }

        [JsonProperty("Device State")]
        public string DeviceState { get; set; }
        public string Muted { get; set; }

        [JsonProperty("Volume dB")]
        public string VolumeDB { get; set; }

        [JsonProperty("Volume Percent")]
        public string VolumePercent { get; set; }

        [JsonProperty("Min Volume dB")]
        public string MinVolumeDB { get; set; }

        [JsonProperty("Max Volume dB")]
        public string MaxVolumeDB { get; set; }

        [JsonProperty("Volume Step")]
        public string VolumeStep { get; set; }

        [JsonProperty("Channels Count")]
        public string ChannelsCount { get; set; }

        [JsonProperty("Channels dB")]
        public string ChannelsDB { get; set; }

        [JsonProperty("Channels  Percent")]
        public string ChannelsPercent { get; set; }

        [JsonProperty("Item ID")]
        public string ItemID { get; set; }

        [JsonProperty("Command-Line Friendly ID")]
        public string CommandLineFriendlyID { get; set; }

        [JsonProperty("Process Path")]
        public string ProcessPath { get; set; }

        [JsonProperty("Process ID")]
        public string ProcessID { get; set; }

        [JsonProperty("Window Title")]
        public string WindowTitle { get; set; }

        [JsonProperty("Registry Key")]
        public string RegistryKey { get; set; }
    }
}
