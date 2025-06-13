using Newtonsoft.Json;

/// <summary>
/// Base entity using encodings
/// Contains utility functions
/// Created: March 2025
/// [*Licence*]
/// Author: John Stewart
/// </summary>

namespace Backend.Base.Entity
{
    public abstract class Encode
    {
        public string Encoded { get; set; } = string.Empty;
        public abstract void Decode();

        protected T Decode<T>() where T : new()
        {
            if (Encoded == null) return new T();
            return JsonConvert.DeserializeObject<T>(Encoded) ?? new T();
        }

        protected int Get (int? value, int valueDefault) => value == null? valueDefault : value.Value;
        protected string Get(string? value, string valueDefault) => string.IsNullOrEmpty(value) ? valueDefault : value;
        protected List<string> Get(List<string>? value, List<string> valueDefault) => value == null ? valueDefault : value;
        protected bool Get(bool? value, bool valueDefault) => value == null ? valueDefault : value.Value;

    }
}
