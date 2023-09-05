using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace HTTPChat_Application;

public class ExternClass
{
    public WebApplication webApplication;

    public string Messages;

    public List<string> SendingQueue;
    public List<string> ReceivingQueue;
    public Mutex sendMutex;
    public Mutex receiveMutex;
    public Mutex outputMutex;
    public Mutex inputMutex;
    public SafeWaitHandle sendHandle;
    public SafeWaitHandle receiveHandle;
    public SafeWaitHandle outputWaiting;
    public SafeWaitHandle inputWaiting;

    public ExternClass() {
        SendingQueue = new List<string>();
        ReceivingQueue = new List<string>();
        sendMutex = new();
        receiveMutex = new();
        outputMutex = new();
        inputMutex = new();
        sendHandle = sendMutex.GetSafeWaitHandle();
        receiveHandle = receiveMutex.GetSafeWaitHandle();
        outputWaiting = outputMutex.GetSafeWaitHandle();
        inputWaiting = inputMutex.GetSafeWaitHandle();
        //Task.Run(delegate { sendingQueueTask(); });
        //Task.Run(delegate { receivingQueueTask(); });
    }


    public async void sendingQueueTask() {
        while (true) {
            try{
                sendMutex.WaitOne();
                foreach (string item in SendingQueue) {
                    outputMutex.ReleaseMutex();
                    SendingQueue.Remove(item);
                }
                sendMutex.ReleaseMutex();
            }
            finally {
                sendMutex.Dispose();
            }
        }
    }

    public async void receivingQueueTask() {
        //while (true) {
        //    try{
        //        receiveMutex.WaitOne();
        //        //////////////////////    do something
        //        foreach(var item in ReceivingQueue) {
        //            PrivacyModel.modifyAttribute("msg", $"The message --{item}-- has been sent!");
        //            KeyValuePair<string, string> valuePair = new("msg", item);
        //            PrivacyModel.RedirectToPage("Privacy", "SingleOrder", valuePair);
        //            ReceivingQueue.Remove(item);
        //        }

        //        return PageModel.RedirectToPage("jola");
        //    }
        //    catch {
        //        receiveMutex.Dispose();
        //    }
        //}
    }
}

