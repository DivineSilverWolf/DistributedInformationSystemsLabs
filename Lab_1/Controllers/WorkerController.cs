using Md5_Selection;
using Md5_Selection.HashWorkers.InterfaceHashWorker;
using MessagesBetweenManagerAndWorker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text;

namespace Lab_1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WorkerController : ControllerBase
    {
        private readonly ILogger<WorkerController> _logger;

        private readonly string _myValue;
        private readonly WorkerSearchForHash workerSearchForHash;

        public WorkerController(ILogger<WorkerController> logger, IOptions<AlphabetSetting> appSettings, IHashWorker hashWorker)
        {
            _logger = logger;
            _myValue = appSettings.Value.Alphabet;
            workerSearchForHash = new WorkerSearchForHash(appSettings.Value.Alphabet, hashWorker);
            hashWorker.Test();
        }
        

        [HttpPost("/search/word/for/hash")]
        public IActionResult Post([FromBody] HashCodeMessage hash)
        {
            Task.Run(() =>
            {
                string word = workerSearchForHash.StartFindWordForHash(hash.MaxLengthValue, hash.HashCodeValue);
                try
                {
                    using var httpClient = new HttpClient();
                    var json = JsonSerializer.Serialize(new MessageForDecryptedWord(word, hash.IdValue));
                    // var content = JsonContent.Create(json);
                    HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    var url = "http://host.docker.internal:32769/search/word/for/hash";
                    using HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                    response.EnsureSuccessStatusCode();
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                // Здесь можно выполнить вашу задачу без ожидания ее завершения
                // Например:
            });

            return Ok();
        }
    }
}
