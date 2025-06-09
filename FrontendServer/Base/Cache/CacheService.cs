using Microsoft.Extensions.Caching.Memory;
using GC = FrontendServer.GlobalConstants;

namespace FrontendServer.Base.Cache
{
    public class CacheService
    {
        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        private string LabelKey(string key)
        {
            return GC.LabelCacheKey + "-" + key;
        }

        public bool HasLabels(string langCode)
        {
            if (_cache.TryGetValue(LabelKey(langCode), out Dictionary<string, LangLabelDto> labelsX))
                return true;
            return false;
        }

        public void PutLabels(string langCode, Dictionary<string, LangLabelDto> labels)
        {
            _cache.Set(LabelKey(langCode), labels);
        }

        public Dictionary<string, LangLabelDto> GetLabels(string langCode)
        {
            Dictionary<string, LangLabelDto>? labels = null;

            if (_cache.TryGetValue(LabelKey(langCode), out Dictionary<string, LangLabelDto> labelsX))
                labels = labelsX;

            return labels != null ? labels : new Dictionary<string, LangLabelDto>();
        }

        public void PutString(string key, string s)
        {
            _cache.Set(key, s);
        }

        public string? GetString(string key)
        {
            if (_cache.TryGetValue(key, out string s))
                return s;

            return null;
        }

        public void RemoveString(string key)
        {
            _cache.Remove(key);
        }

    }
}
