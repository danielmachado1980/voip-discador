public class KafkaProducerService
{
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;
    public KafkaProducerService(IConfiguration cfg)
    {
        var section = cfg.GetSection("Kafka");
        _topic = section["Topic"]!;
        var config = new ProducerConfig { BootstrapServers = section["BootstrapServers"] };
        _producer = new ProducerBuilder<string, string>(config).Build();
    }

    public Task ProduceAsync(string key, string json)
        => _producer.ProduceAsync(_topic, new Message<string, string> { Key = key, Value = json });
}
