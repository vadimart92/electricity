using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace electricity;

public class db_init
{
    private readonly DatabaseContext _databaseContext;

    public db_init(DatabaseContext databaseContext) {
        _databaseContext = databaseContext;
    }

    [Function("db_init")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] 
            HttpRequestData req,
            FunctionContext executionContext) {
        await _databaseContext.Database.EnsureDeletedAsync();
        await _databaseContext.Database.EnsureCreatedAsync();
        
        _databaseContext.Locations.Add(new Location {
            Id = LocationIds.Location1,
            LastAlive = DateTime.MinValue
        });
        await _databaseContext.SaveChangesAsync();
        var responseData = req.CreateResponse(HttpStatusCode.OK);
        await responseData.WriteStringAsync("Db reinitialized");
        return responseData;
    }

}
