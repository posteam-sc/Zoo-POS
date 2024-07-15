using System;
using System.Globalization;

public class ApplicationLog
    {
    public void WriteErrorLog(string eMessage, string ErrorModule, string InnerMessaage)
    {
        try
        {
            if (!string.IsNullOrEmpty(eMessage))
            {
                string AppPath = System.Configuration.ConfigurationManager.AppSettings["LogFilePath"];
                string TempYear = DateTime.Now.Year.ToString();
                string Tempmonth = DateTime.Now.Month.ToString();
                string TempDay = DateTime.Now.Day.ToString();

                if (Tempmonth.Length == 1) Tempmonth = "0" + Tempmonth;
                if (TempDay.Length == 1) TempDay = "0" + TempDay;

                System.IO.StreamWriter y; string revdate = DateTime.Now.ToLongDateString();
                y = System.IO.File.AppendText(AppPath + "\\E" + TempYear + Tempmonth + TempDay + ".log");
                y.WriteLine("[" + DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToString("HH",
                    CultureInfo.InvariantCulture) + ":" + DateTime.Now.ToString("mm", CultureInfo.InvariantCulture) +
                    ":" + DateTime.Now.ToString("ss", CultureInfo.InvariantCulture) + "] " + "[" + ErrorModule + "]" + " Message: " + eMessage + Environment.NewLine + InnerMessaage);
                y.Close();
            }
        }
        catch (Exception ex)
        {
            
        }
    }
}
