public class DiscadorService
{
    private readonly AsteriskService _asterisk;
    private readonly KafkaProducerService _kafka;
    public DiscadorService(AsteriskService a, KafkaProducerService k){_asterisk=a;_kafka=k;}

    public async Task<string?> DiscarNumerosAsync(List<string> nums)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var tasks = nums.Select(async n =>
        {
            await _kafka.ProduceAsync(n, $"{{\"Type\":\"CallStarted\",\"Numero\":\"{n}\",\"Timestamp\":\"{DateTime.UtcNow:O}\"}}");
            var res = await _asterisk.DiscarAsync(n, cts.Token);
            await _kafka.ProduceAsync(n, $"{{\"Type\":\"Call{res.Estado}\",\"Numero\":\"{n}\",\"Timestamp\":\"{DateTime.UtcNow:O}\"}}");
            return res;
        }).ToList();

        var first = await Task.WhenAny(tasks);
        cts.Cancel();
        var result = await first;
        return result.Estado == "Answered" ? result.Numero : null;
    }
}
