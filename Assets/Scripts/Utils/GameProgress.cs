using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public enum Checkpoint
{
    Lesson1Complete,
    Lesson2Complete,
    Lesson3Complete,
    Lesson4Complete,
    Lesson5Complete,
    Lesson6Complete,
    NumCheckpoints,
}

public static class GameProgress
{
    [DynamoDBTable("UserData")]
    public class DynamoUserData
    {
        [DynamoDBHashKey]
        public string Email { get; set; }

        [DynamoDBProperty]
        public bool Paid { get; set; }

        [DynamoDBProperty]
        public Dictionary<string, double> Checkpoints;

        [DynamoDBProperty]
        public List<double> LoginTimes;
    }

    static DynamoDBContext dynamoDbContext;
    public static DynamoUserData UserData;

    static double Timestamp
    {
        get
        {
            double result = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            return result;
        }
    }

    // ------------------------------------------------------------------------------------- //

    static GameProgress()
    {
        InitializeAws();
    }

    // ------------------------------------------------------------------------------------- //

    private static void InitializeAws()
    {
        var credentials = new CognitoAWSCredentials("us-east-1:817675d6-cdf2-4149-bd5f-98d96020378a", RegionEndpoint.USEast1);
        AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, RegionEndpoint.USEast1);
        GameProgress.dynamoDbContext = new DynamoDBContext(client);
    }

    // ------------------------------------------------------------------------------------- //

    public static void LoadUserData(string emailAddress, Action<bool> handler = null)
    {
        if (string.IsNullOrEmpty(emailAddress))
        {
            Debug.LogWarningFormat("Not loading user data because no email address was specified");
            return;
        }
        Debug.LogFormat("Loading user [{0}]", emailAddress);
        GameProgress.dynamoDbContext.LoadAsync<DynamoUserData>(
           emailAddress,
           result =>
           {
               if (result.Result == null)
               {
                   if (handler != null)
                   {
                       handler(false);
                   }
                   return;
               }

               GameProgress.UserData = result.Result;
               Debug.LogFormat("Loaded user {0}, paid? {1}", result.Result.Email, result.Result.Paid);
               if (GameProgress.UserData.Checkpoints == null)
               {
                   GameProgress.UserData.Checkpoints = new Dictionary<string, double>();
               }
               if (GameProgress.UserData.LoginTimes == null)
               {
                   GameProgress.UserData.LoginTimes = new List<double>();
               }
               GameProgress.UserData.LoginTimes.Add(GameProgress.Timestamp);
               SyncToAws();
               if (handler != null)
               {
                   handler(true);
               }
           });
    }

    // ------------------------------------------------------------------------------------- //

    private static void SyncToAws()
    {
        if (GameProgress.UserData == null)
        {
            Debug.LogWarningFormat("Not syncing to aws because UserData has not been initialized");
            return;
        }
        GameProgress.dynamoDbContext.SaveAsync(
            GameProgress.UserData,
            result =>
            {
                if (result.Exception != null)
                {
                    Debug.LogErrorFormat("Couldn't save game progress to dynamodb: {0}", result.Exception);
                }
                else
                {
                    Debug.LogFormat("Successfully saved game progress to dynamodb: {0}", result.State);
                }
            });
    }

    // ------------------------------------------------------------------------------------- //

    public static void MarkCheckpointComplete(Checkpoint checkpoint)
    {
        if (GameProgress.UserData == null)
        {
            Debug.LogWarningFormat("Not syncing checkpoint {0} to aws because UserData has not been initialized");
            return;
        }
        GameProgress.UserData.Checkpoints[checkpoint.ToString()] = GameProgress.Timestamp;
        SyncToAws();
    }

    // ------------------------------------------------------------------------------------- //

    public static bool IsCheckpointComplete(Checkpoint checkpoint)
    {
        if (GameProgress.UserData == null)
        {
            throw new NullReferenceException(
                string.Format("Cannot check if checkpoint [{0}] is complete because user data has not been initialized", checkpoint));
        }
        double timestamp;
        bool result = GameProgress.UserData.Checkpoints.TryGetValue(checkpoint.ToString(), out timestamp);
        return result;
    }

    // ------------------------------------------------------------------------------------- //
}

