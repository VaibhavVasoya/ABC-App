using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Text;

public class LogSaverAndSender : MonoBehaviour
{
    public bool enableSave = true;
    public bool enableMailing = true;

    public string yourEmail = "";
    public string yourEmailPassword = "";
    public string toEmail = "vaibhavvasoya8@gmail.com";


    [Serializable]
    public struct Logs
    {
        public string condition;
        public string stackTrace;
        public LogType type;

        public string dateTime;

        public Logs(string condition, string stackTrace, LogType type, string dateTime)
        {
            this.condition = condition;
            this.stackTrace = stackTrace;
            this.type = type;
            this.dateTime = dateTime;
        }
    }

    [Serializable]
    public class LogInfo
    {
        public List<Logs> logInfoList = new List<Logs>();
    }

    LogInfo logs = new LogInfo();

    void OnEnable()
    {
        //Email last saved log
        if (enableMailing)
        {
            mailLog();
        }

        //Subscribe to Log Event
        Application.logMessageReceived += LogCallback;
    }

    //Called when there is an exception
    void LogCallback(string condition, string stackTrace, LogType type)
    {
        //Create new Log
        Logs logInfo = new Logs(condition, stackTrace, type, DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz"));

        //Add it to the List
        logs.logInfoList.Add(logInfo);
    }

    void mailLog()
    {
        //Read old/last saved log
        LogInfo loadedData = DataSaver.loadData<LogInfo>("savelog");
        string date = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:sszzz");

        //Send only if there is something to actually send
        if (loadedData != null && loadedData.logInfoList != null

            && loadedData.logInfoList.Count > 0)
        {

            Debug.Log("Found log to send!");

            //Convert to json
            string messageToSend = JsonUtility.ToJson(loadedData, true);

            string attachmentPath = Path.Combine(Application.persistentDataPath, "data");
            attachmentPath = Path.Combine(attachmentPath, "savelog.txt");

            //Finally send email
            sendMail(yourEmail, yourEmailPassword, toEmail, "Log: " + date, messageToSend, attachmentPath);

            //Clear old log
            DataSaver.deleteData("savelog");
        }
    }

    void sendMail(string fromEmail, string emaiPassword, string toEmail, string eMailSubject, string eMailBody, string attachmentPath = null)
    {
        try
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(fromEmail);
            mail.To.Add(toEmail);
            mail.Subject = eMailSubject;
            mail.Body = eMailBody;

            if (attachmentPath != null)
            {
                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(attachmentPath);
                mail.Attachments.Add(attachment);
            }

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Credentials = new System.Net.NetworkCredential(fromEmail, emaiPassword) as ICredentialsByHost;
            smtpClient.EnableSsl = true;
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
            smtpClient.Send(mail);
        }
        catch (Exception e) { }
    }

    void OnDisable()
    {
        //Un-Subscribe from Log Event
        Application.logMessageReceived -= LogCallback;
    }

    //Save log  when focous is lost
    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            //Save
            if (enableSave)
                DataSaver.saveData(logs, "savelog");
        }
    }

    //Save log on exit
    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            //Save
            if (enableSave)
                DataSaver.saveData(logs, "savelog");
        }
    }
}

[System.Serializable]
public class DataSaver
{
    //Save Data
    public static void saveData<T>(T dataToSave, string dataFileName)
    {
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Convert To Json then to bytes
        string jsonData = JsonUtility.ToJson(dataToSave, true);
        byte[] jsonByte = Encoding.ASCII.GetBytes(jsonData);

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(tempPath));
        }
        //Debug.Log(path);

        try
        {
            File.WriteAllBytes(tempPath, jsonByte);
            Debug.Log("Saved Data to: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To PlayerInfo Data to: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }
    }

    //Load Data
    public static T loadData<T>(string dataFileName)
    {
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Debug.LogWarning("Directory does not exist");
            return default(T);
        }

        if (!File.Exists(tempPath))
        {
            Debug.Log("File does not exist");
            return default(T);
        }

        //Load saved Json
        byte[] jsonByte = null;
        try
        {
            jsonByte = File.ReadAllBytes(tempPath);
            Debug.Log("Loaded Data from: " + tempPath.Replace("/", "\\"));
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Load Data from: " + tempPath.Replace("/", "\\"));
            Debug.LogWarning("Error: " + e.Message);
        }

        //Convert to json string
        string jsonData = Encoding.ASCII.GetString(jsonByte);

        //Convert to Object
        object resultValue = JsonUtility.FromJson<T>(jsonData);
        return (T)Convert.ChangeType(resultValue, typeof(T));
    }

    public static bool deleteData(string dataFileName)
    {
        bool success = false;

        //Load Data
        string tempPath = Path.Combine(Application.persistentDataPath, "data");
        tempPath = Path.Combine(tempPath, dataFileName + ".txt");

        //Exit if Directory or File does not exist
        if (!Directory.Exists(Path.GetDirectoryName(tempPath)))
        {
            Debug.LogWarning("Directory does not exist");
            return false;
        }

        if (!File.Exists(tempPath))
        {
            Debug.Log("File does not exist");
            return false;
        }

        try
        {
            File.Delete(tempPath);
            Debug.Log("Data deleted from: " + tempPath.Replace("/", "\\"));
            success = true;
        }
        catch (Exception e)
        {
            Debug.LogWarning("Failed To Delete Data: " + e.Message);
        }

        return success;
    }
}