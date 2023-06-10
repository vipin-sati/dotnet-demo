using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace test_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        IMemoryCache _memoryCache;
        public TestController(IMemoryCache memoryCache)
        {
            this._memoryCache = memoryCache;
        }

        // [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet(Name = "GetName")]
        public DateTime Get()
        {
            DateTime currentTime;
            bool isExist = _memoryCache.TryGetValue("CacheTime", out currentTime);
            if (!isExist)
            {
                currentTime = DateTime.Now;
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(20));
                _memoryCache.Set("CacheTime", currentTime, cacheEntryOptions);
                // throw new Exception("Test global error handling");
            }
            return currentTime;
        }
    }
}
