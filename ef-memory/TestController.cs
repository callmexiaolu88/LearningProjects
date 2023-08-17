using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("[controller]/[action]")]
public class TestController : ControllerBase
{
    private MyDbContext dbContext;
    public TestController(MyDbContext dbContext)
    {
        this.dbContext = dbContext;
    }
    public List<AddressBook> GetInformations()
    {
        return dbContext.ABS.ToList();
    }

    [HttpPost]
    public bool AddnewItem(AddressBook item)
    {
        if (dbContext.ABS.FirstOrDefault<AddressBook>(i=>i.Name==item.Name) !=null )
        {
            return false;
        }
        dbContext.ABS.Add(item);
        dbContext.SaveChanges();
        return true;
    }
}