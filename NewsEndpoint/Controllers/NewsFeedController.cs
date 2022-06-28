using Microsoft.AspNetCore.Mvc;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsEndpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NewsFeedController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<NewsFeedController> _logger;

        public NewsFeedController(IHttpClientFactory httpClientFactory ,ILogger<NewsFeedController> logger)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("~/rss", Name = "GetRssAsJson")]
        public async Task<IActionResult> GetRssAsJson([FromQuery] string feed)
        {
            if (!Uri.IsWellFormedUriString(feed, UriKind.Absolute))
                return BadRequest($"Rss endpoint: {feed} is malformed!");

            var httpClient = _httpClientFactory.CreateClient(); //Client with retry
            var stream = await (await httpClient.GetAsync(feed)).Content.ReadAsStreamAsync();

            XmlReader reader = XmlReader.Create(stream);
            SyndicationFeed syndicationFeed = SyndicationFeed.Load(reader);
            reader.Close();

            var result = new FeedJson()
            {
                title = syndicationFeed.Title.Text,
                desc = syndicationFeed.Description.Text,
                entries = syndicationFeed.Items.Select(x => new Entry()
                {
                    title = x.Title.Text,
                    link = x.Links.FirstOrDefault().Uri.ToString(),
                    authors = x.Authors.Select(auth => auth.ToString()),
                    pubdate = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(x.PublishDate, "E. Europe Standard Time").ToString("G") //A1
                })
            };
            return Ok(result.ToJson());
        }
    }
}