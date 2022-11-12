using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
// ESCOM_2022


public static class DatabaseConstants
{
    public static string ApiKey { get; } = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im13ZGV1Ynl0cXdkZ2d3ZXFtenByIiwicm9sZSI6ImFub24iLCJpYXQiOjE2NjA3NzAzNTcsImV4cCI6MTk3NjM0NjM1N30.u553dDLve5mOwSJA7wAgJ4PtfT9WpJwfmgTsm01f0I8";
    
    public static string DatabaseUrl { get; } = "https://mwdeubytqwdggweqmzpr.supabase.co/rest/v1/";

    public static string Authenticator { get; } = "Bearer " + ApiKey;

    public static string AllQuestionsUrl { get; } = "https://mwdeubytqwdggweqmzpr.supabase.co/rest/v1/rpc/getallquestionstest1";

    public static string AllQuestionsLocalFile { get; } = "questions-json.json";

    public const string RelationQuestionString = "relation_question";
    public const string OrderedQuestionString = "ordered_answer";
    public const string MultipleOptionString = "multiple_option";
}

public class DataManager
{
    private HttpClient m_httpClient;
    private static HttpClient GlobalHttpClient;
    private List<JsonQuestion> m_allQuestions;
    private bool m_leerDeArchivo;

    public DataManager()
    {
        if (GlobalHttpClient == null)
        {
            GlobalHttpClient = new HttpClient();
            GlobalHttpClient.DefaultRequestHeaders.Add("apikey", DatabaseConstants.ApiKey);
            GlobalHttpClient.DefaultRequestHeaders.Add("Authorization", DatabaseConstants.Authenticator);
        }
        m_httpClient = GlobalHttpClient;
    }

    /// <summary>
    /// Gets a list of all the questions in the database.
    /// </summary>
    /// <returns></returns>
    public async Task<List<JsonQuestion>> GetAllQuestionsAsync()
    {
        var allQuestionsData = await GetAllQuestionsRawDataAsync();
        FileManager.WriteRawDataToFileAsync(DatabaseConstants.AllQuestionsLocalFile, allQuestionsData);
        var questionList = JsonConvert.DeserializeObject<List<JsonQuestion>>(allQuestionsData);
        return questionList;
    }

    /// <summary>
    /// Iterates over all the jsonQuestions, selectes the ones of type Relation and with subject and unit
    /// and returns the questions.
    /// </summary>
    /// <param name="subject">Filter for subject</param>
    /// <param name="unit">Filter for unit</param>
    /// <returns></returns>
    public async Task<List<RelationQuestion>> GetRelationQuestionsAsync(string subject, string unit = null)
    {
        if (m_allQuestions == null)
        {
            m_allQuestions = await GetAllQuestionsAsync();
        }

        List<RelationQuestion> questions = new List<RelationQuestion>();
        foreach (JsonQuestion question in m_allQuestions)
        {
            if (question.type == DatabaseConstants.RelationQuestionString &&
                question.subject == subject &&
                (unit == null || question.unit == unit))
            {
                var relationQuestion = JsonConvert.DeserializeObject<RelationQuestion>(question.json_question);
                questions.Add(relationQuestion);
            }
        }

        return questions;
    }

    /// <summary>
    /// Iterates over all the jsonQuestions, selectes the ones of type Ordered and with subject and unit
    /// and returns the questions.
    /// </summary>
    /// <param name="subject">Filter for subject</param>
    /// <param name="unit">Filter for unit</param>
    /// <returns></returns>
    public async Task<List<OrderedQuestion>> GetOrderedQuestions(string subject, string unit = null)
    {
        if (m_allQuestions == null)
        {
            m_allQuestions = await GetAllQuestionsAsync();
        }

        List<OrderedQuestion> questions = new List<OrderedQuestion>();
        foreach (JsonQuestion question in m_allQuestions)
        {
            if (question.type == DatabaseConstants.OrderedQuestionString &&
                question.subject == subject &&
                (unit == null || question.unit == unit))
            {
                var orderedQuestion = JsonConvert.DeserializeObject<OrderedQuestion>(question.json_question);
                questions.Add(orderedQuestion);
            }
        }

        return questions;
    }

    /// <summary>
    /// Iterates over all the jsonQuestions, selectes the ones of type Ordered and with subject and unit
    /// and returns the questions.
    /// </summary>
    /// <param name="subject">Filter for subject</param>
    /// <param name="unit">Filter for unit</param>
    /// <returns></returns>
    public async Task<List<MultipleOptionQuestion>> GetMultipleOptionQuestions(string subject, string unit = null)
    {
        if (m_allQuestions == null)
        {
            m_allQuestions = await GetAllQuestionsAsync();
        }

        List<MultipleOptionQuestion> questions = new List<MultipleOptionQuestion>();
        foreach (JsonQuestion question in m_allQuestions)
        {
            if (question.type == DatabaseConstants.MultipleOptionString &&
                question.subject == subject &&
                (unit == null || question.unit == unit))
            {
                var multipleOptionQuestion = JsonConvert.DeserializeObject<MultipleOptionQuestion>(question.json_question);
                questions.Add(multipleOptionQuestion);
            }
        }

        return questions;
    }

    private async Task<string> GetAllQuestionsRawDataAsync()
    {
        string result = "";
        var pudoLeerDelServidor = false;

        if (!m_leerDeArchivo)
        {
            try
            {
                var content = "";
                var requestContent = new StringContent(content, Encoding.UTF8, "application/json");
                var response = await m_httpClient.PostAsync(DatabaseConstants.AllQuestionsUrl, requestContent);
                response.EnsureSuccessStatusCode();
            
                result = await response.Content.ReadAsStringAsync();
                m_leerDeArchivo = true;
                pudoLeerDelServidor = true;
                Debug.Log("Succesfully read data from server");
            }
            catch
            {
                Debug.Log("Could not connect to the server.");
                pudoLeerDelServidor = false;
            }
        }
        
        if (!pudoLeerDelServidor)
        {
            try
            {
                result = await FileManager.ReadRawDataFromFileAsync(DatabaseConstants.AllQuestionsLocalFile);
                Debug.Log("Succesfully read data from file");
            }
            catch
            {
                Debug.LogWarning("WARNING: Could not read from local file nor the server.");
            }
        }

        Debug.Log(result);

        return result;
    }
}

public class JsonQuestion
{
    public string type;
    public string unit;
    public string subject;
    public string json_question;
}

public class RelationQuestion
{
    public string Concept;
    public string Definition;
}

public class OrderedQuestion
{
    public string question;
    public List<string> answers;
}

public class MultipleOptionQuestion
{
    public class AnswerOptions {
        public string text;
        public bool isCorrect;
    }

    public string question;
    public List<AnswerOptions> answers;
}


