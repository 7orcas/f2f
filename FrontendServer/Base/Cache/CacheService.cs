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

        public void PutLabels(Dictionary<string, LangLabelDto> labels)
        {
            _cache.Set(GC.LabelCacheKey, labels);
        }

        public Dictionary<string, LangLabelDto> GetLabels()
        {
            Dictionary<string, LangLabelDto>? labels = null;

            if (_cache.TryGetValue(GC.LabelCacheKey, out Dictionary<string, LangLabelDto> labelsX))
                labels = labelsX;

            return labels != null ? labels : new Dictionary<string, LangLabelDto>();
        }
    }
}
