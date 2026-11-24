namespace l99.fanuc;

public partial class Platform
{
    public async Task<dynamic> RdMacroR2Async(int number, int numberOfVariables = 1, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(RdMacroR2(number, numberOfVariables, cancellationToken));
    }

    public dynamic RdMacroR2(int number, int numberOfVariables = 1, CancellationToken cancellationToken = default)
    {
        if (numberOfVariables < 1 || numberOfVariables > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(numberOfVariables), "The current native response structure supports one through five macros.");
        }

        var macro = new Focas.IODBMR1();
        int count = numberOfVariables;
        var ndr = _nativeDispatch(() =>
            (Focas.focas_ret)Focas.cnc_rdmacror2(_handle, number, ref count, macro), cancellationToken: cancellationToken);

        var nr = new
        {
            @null = false,
            method = "cnc_rdmacror2",
            invocationMs = ndr.ElapsedMilliseconds,
            doc = $"{_docBasePath}/ncdata/cnc_rdmacror2",
            success = ndr.RC == Focas.EW_OK,
            rc = ndr.RC,
            request = new { number, numberOfVariables },
            response = new { macro, numberOfVariables = count }
        };

        _logger.Trace($"Platform invocation result:\n{JObject.FromObject(nr)}");
        return ToPublicResponse(nr);
    }
}
