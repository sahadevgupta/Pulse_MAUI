using System;

namespace Pulse_MAUI.Helpers;

public static class TimeZoneAbbreviationHelper
{
    private static readonly Dictionary<string, (string Standard, string Daylight)> KnownTimeZones = new(StringComparer.OrdinalIgnoreCase)
    {
        ["UTC"] = ("UTC", "UTC"),
        ["GMT Standard Time"] = ("GMT", "BST"),
        ["Europe/London"] = ("GMT", "BST"),
        ["W. Europe Standard Time"] = ("CET", "CEST"),
        ["Central European Standard Time"] = ("CET", "CEST"),
        ["Romance Standard Time"] = ("CET", "CEST"),
        ["Europe/Paris"] = ("CET", "CEST"),
        ["Europe/Berlin"] = ("CET", "CEST"),
        ["Europe/Madrid"] = ("CET", "CEST"),
        ["Europe/Rome"] = ("CET", "CEST"),
        ["E. Europe Standard Time"] = ("EET", "EEST"),
        ["GTB Standard Time"] = ("EET", "EEST"),
        ["Egypt Standard Time"] = ("EET", "EEST"),
        ["FLE Standard Time"] = ("EET", "EEST"),
        ["South Africa Standard Time"] = ("SAST", "SAST"),
        ["Turkey Standard Time"] = ("TRT", "TRT"),
        ["Arabian Standard Time"] = ("GST", "GST"),
        ["India Standard Time"] = ("IST", "IST"),
        ["China Standard Time"] = ("CST", "CST"),
        ["Tokyo Standard Time"] = ("JST", "JST"),
        ["Korea Standard Time"] = ("KST", "KST"),
        ["AUS Eastern Standard Time"] = ("AEST", "AEDT"),
        ["Australia/Sydney"] = ("AEST", "AEDT"),
        ["New Zealand Standard Time"] = ("NZST", "NZDT"),
        ["Eastern Standard Time"] = ("EST", "EDT"),
        ["America/New_York"] = ("EST", "EDT"),
        ["Central Standard Time"] = ("CST", "CDT"),
        ["America/Chicago"] = ("CST", "CDT"),
        ["Mountain Standard Time"] = ("MST", "MDT"),
        ["America/Denver"] = ("MST", "MDT"),
        ["US Mountain Standard Time"] = ("MST", "MST"),
        ["Pacific Standard Time"] = ("PST", "PDT"),
        ["America/Los_Angeles"] = ("PST", "PDT")
    };

    public static string GetLocalTimeZoneCode(DateTimeOffset localTimestamp)
    {
        var localZone = TimeZoneInfo.Local;
        var isDaylight = localZone.IsDaylightSavingTime(localTimestamp.DateTime);

        if (KnownTimeZones.TryGetValue(localZone.Id, out var known))
            return isDaylight ? known.Daylight : known.Standard;

        var offset = localZone.GetUtcOffset(localTimestamp.DateTime);
        var sign = offset < TimeSpan.Zero ? '-' : '+';
        offset = offset.Duration();
        return $"UTC{sign}{offset:hh\\:mm}";
    }
}
