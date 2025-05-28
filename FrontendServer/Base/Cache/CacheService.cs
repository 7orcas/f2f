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

        public void PutLabels(Dictionary<string, LangLabelDto> labels, string langCode)
        {
            _cache.Set(GC.LabelCacheKey + "-" + langCode, labels);
        }

        public Dictionary<string, LangLabelDto> GetLabels(string langCode)
        {
            Dictionary<string, LangLabelDto>? labels = null;

            if (_cache.TryGetValue(GC.LabelCacheKey + "-" + langCode, out Dictionary<string, LangLabelDto> labelsX))
                labels = labelsX;

            return labels != null ? labels : new Dictionary<string, LangLabelDto>();
        }
    }
}
