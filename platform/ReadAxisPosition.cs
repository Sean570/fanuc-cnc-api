namespace l99.fanuc;

public partial class Platform
{
    public async Task<dynamic> ReadAxisPositionAsync(short positionType, short axis = -1, short length = 132, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(ReadAxisPosition(positionType, axis, length, cancellationToken));
    }

    public dynamic ReadAxisPosition(short positionType, short axis = -1, short length = 132, CancellationToken cancellationToken = default)
    {
        var positions = new Focas.ODBAXIS();
        var ndr = _nativeDispatch(() =>
        {
            Focas.focas_ret result = positionType switch
            {
                0 => (Focas.focas_ret)Focas.cnc_machine(_handle, axis, length, positions),
                1 => (Focas.focas_ret)Focas.cnc_absolute(_handle, axis, length, positions),
                2 => (Focas.focas_ret)Focas.cnc_relative2(_handle, axis, length, positions),
                _ => throw new ArgumentOutOfRangeException(nameof(positionType))
            };
            return result;
        }, cancellationToken: cancellationToken);

        var nr = new
        {
            @null = false,
            method = "cnc_axis_position",
            invocationMs = ndr.ElapsedMilliseconds,
            doc = $"{_docBasePath}/position/cnc_machine",
            success = ndr.RC == Focas.EW_OK,
            rc = ndr.RC,
            request = new { positionType, axis, length },
            response = new { positions }
        };

        _logger.Trace($"Platform invocation result:\n{JObject.FromObject(nr)}");
        return ToPublicResponse(nr);
    }
}
