namespace l99.fanuc;

public partial class Platform
{
    public async Task<dynamic> RdPdfDriveAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(RdPdfDrive(cancellationToken));
    }

    public dynamic RdPdfDrive(CancellationToken cancellationToken = default)
    {
        // ODBPDFDRV: short max_num; short dummy; char drive[16][12];
        // max_num returns how many drive names are valid (up to 16), drive1..driveN.
        var pdf_drv = new Focas.ODBPDFDRV();

        var ndr = _nativeDispatch(() =>
        {
            return (Focas.focas_ret) Focas.cnc_rdpdf_drive(_handle, pdf_drv);
        }, cancellationToken: cancellationToken);

        var nr = new
        {
            @null = false,
            method = "cnc_rdpdf_drive",
            invocationMs = ndr.ElapsedMilliseconds,
            doc = $"{_docBasePath}/program/cnc_rdpdf_drive",
            success = ndr.RC == Focas.EW_OK,
            rc = ndr.RC,
            request = new { cnc_rdpdf_drive = new { } },
            response = new { cnc_rdpdf_drive = new { pdf_drv } },
            bag = new Dictionary<dynamic, dynamic>()
        };

        _logger.Trace($"Platform invocation result:\n{JObject.FromObject(nr)}");

        return nr;
    }
}
