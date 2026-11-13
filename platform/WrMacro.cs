namespace l99.fanuc;

public partial class Platform
{
    public async Task<dynamic> WrMacroAsync(short number, double value, short decimalPlaces = 0, short length = 10)
    {
        return await Task.FromResult(WrMacro(number, value, decimalPlaces, length));
    }

    public dynamic WrMacro(short number, double value, short decimalPlaces = 0, short length = 10)
    {
        double scaledValue = Math.Round(value * Math.Pow(10, decimalPlaces));
        int macroValue = (int)Math.Clamp(scaledValue, int.MinValue, int.MaxValue);

        var ndr = _nativeDispatch(() =>
        {
            return (Focas.focas_ret)Focas.cnc_wrmacro(_handle, number, length, macroValue, decimalPlaces);
        });

        var nr = new
        {
            @null = false,
            method = "cnc_wrmacro",
            invocationMs = ndr.ElapsedMilliseconds,
            doc = $"{_docBasePath}/ncdata/cnc_wrmacro",
            success = ndr.RC == Focas.EW_OK,
            rc = ndr.RC,
            request = new { cnc_wrmacro = new { number, length, macroValue, decimalPlaces } },
            response = new { cnc_wrmacro = new { } }
        };

        _logger.Trace($"Platform invocation result:\n{JObject.FromObject(nr)}");

        return nr;
    }
}
