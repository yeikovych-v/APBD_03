namespace APBD_03.service;

public static class ReportService
{
    public static void ReportDangerousSituation(string message) {
        Console.Write("REPORT :: ");
        Console.WriteLine(message);
    }
}