﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace SimpleGelf.Extensions.Logging
{
    // http://docs.graylog.org/en/2.4/pages/gelf.html#gelf-payload-specification
    public class GelfMessage
    {
        [JsonProperty("version")]
        public string Version { get; } = "1.1";

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("short_message")]
        public string ShortMessage { get; set; }

        [JsonProperty("timestamp")]
        public double Timestamp { get; set; }

        [JsonProperty("level")]
        public SyslogSeverity Level { get; set; }

        [JsonProperty("_logger")]
        public string Logger { get; set; }

        [JsonProperty("_exception")]
        public string Exception { get; set; }

        [JsonProperty("_event_id")]
        public int EventId { get; set; }

        [JsonProperty("_event_name")]
        public string EventName { get; set; }

        [JsonProperty("_payload")]
        public object Data { get; set; }

        [JsonProperty("_correlationId")]
        public string CorrelationId { get; set; }

        [JsonIgnore]
        public IEnumerable<KeyValuePair<string, object>> AdditionalFields { get; set; }
    }
}
