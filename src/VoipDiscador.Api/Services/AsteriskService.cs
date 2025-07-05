public class AsteriskService
{
    private readonly AriClient _client;
    public AsteriskService(IConfiguration cfg)
    {
        var section = cfg.GetSection("Asterisk");
        _client = new AriClient();
        _client.Connect(section["Url"]!, section["User"]!, section["Secret"]!);
        _client.OnStasisStartEvent += (s,e) => {};
        _client.OnChannelHangupRequestEvent += (s,e)=>{};
    }

    public async Task<CallResult> DiscarAsync(string numero, CancellationToken token)
    {
        var channel = await _client.Channels.OriginateAsync($"SIP/{numero}", "voipdialer", numero);
        var tcs = new TaskCompletionSource<CallResult>();
        void onAnswered(object? s, StasisStartEvent e) { if(e.Args.Contains(numero)) tcs.TrySetResult(new CallResult { Numero=numero, Estado="Answered" }); }
        void onHangup(object? s, ChannelHangupRequestEvent e) { if(e.Channel.Name.Contains(numero)) tcs.TrySetResult(new CallResult { Numero=numero, Estado="NoAnswer" }); }

        _client.OnStasisStartEvent += onAnswered;
        _client.OnChannelHangupRequestEvent += onHangup;
        using var reg = token.Register(() => tcs.TrySetCanceled());

        var result = await tcs.Task;
        _client.OnStasisStartEvent -= onAnswered;
        _client.OnChannelHangupRequestEvent -= onHangup;

        return result;
    }
}
