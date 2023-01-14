using System;
using System.Collections.Generic;
using System.Threading;
using Confluent.Kafka;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using ECommerceExampleProject;


public static class PaymentData{
    public static List<Payment> payments = loadPayments();

    public static List<Payment> loadPayments()
    {
        string filePath = "payment.txt";

        string jsonString = File.ReadAllText(filePath);

        if (jsonString != null && jsonString != "") return JsonConvert.DeserializeObject<List<Payment>>(jsonString);
        else return new List<Payment>();
    }

    public static void writeJson()
    {
        File.WriteAllText("payments.txt", JsonConvert.SerializeObject(payments));
    }
}

class PaymentService
{


    public static void StartService()
    { }
}