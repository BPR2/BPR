using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Npgsql;
using System.Net.Http.Headers;
using System.Text.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace FuncBearerToken;

public class Function
{
    private HttpClient _client;
    private string _url;
    private HttpContent _content;
    private string _dbConnectionString = "Host=bpr-db.c7szkct1z4j9.us-east-1.rds.amazonaws.com;Username=bpr_group4;Password=dingdong420 ;Database=postgres";

    public Function()
    {
        _client = new HttpClient();
        _url = "https://api.trusted.dk:443/Token";
        _content = new StringContent("grant_type=password&username=es@fasterholt.dk&password=sI1nkyEw");
    }

    public async Task FunctionHandler(APIGatewayProxyRequest request, ILambdaContext context)
    {
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        var response = await _client.PostAsync(_url, _content);

        string result = response.Content.ReadAsStringAsync().Result;

        Response token = JsonSerializer.Deserialize<Response>(result);

        await InsertTokenToDB(token.access_token);
    }

    public async Task InsertTokenToDB(string token)
    {
        await DeletePreviousTokensFromDB();

        using var con = new NpgsqlConnection(_dbConnectionString);
        con.Open();

        string command = $"INSERT INTO public.\"bearerToken\"(\"token\") VALUES (@token);";

        await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
        {
            cmd.Parameters.AddWithValue("@token", token);

            cmd.ExecuteNonQuery();
        }

        con.Close();
    }

    public async Task DeletePreviousTokensFromDB()
    {
        using var con = new NpgsqlConnection(_dbConnectionString);
        con.Open();

        string command = $"DELETE FROM public.\"bearerToken\";";

        await using (NpgsqlCommand cmd = new NpgsqlCommand(command, con))
        {
            cmd.ExecuteNonQuery();
        }

        con.Close();
    }
}
