using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PshGameApi.Data;
using PshGameApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace PshGameApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatsController : ControllerBase
    {
        private readonly PshContext _context;

        public StatsController(PshContext context)
        {
            _context = context;
        }

        [HttpGet("TopAllTime")]
        public IEnumerable<Stat> GetTopAllTime()
        {
            var result =_context.Stats
                .Include(x => x.User)
                .OrderByDescending(x => x.Score)
                .Take(10);
            return result.ToList();
        }

        [HttpGet("TopLastTime")]
        public IEnumerable<Stat> GetTopLastTime()
        {
            var aux = _context.Stats
                .Include(x => x.User)
                .OrderByDescending(x => x.Created)
                .Take(10);
            var result = aux
                .OrderByDescending(x => x.Score)
                .Take(10);
            return result.ToList();
        }

    }
}
