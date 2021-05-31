using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PshGameApi.Data;
using PshGameApi.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PshGameApi.Services
{
    public class RandomUserService : BackgroundService
    {
        private IServiceProvider _sp;
        protected HttpClient client;

        public RandomUserService(IServiceProvider sp)
        {
            _sp = sp;
            client = new HttpClient();
            var url = "https://randomuser.me/api/";
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (stoppingToken.IsCancellationRequested)
            {
                return Task.CompletedTask;
            }

            var aTimer = new System.Timers.Timer();
            aTimer.Interval = 300000; // 5 minutos
            aTimer.Elapsed += (source, e) => 
            {
                Task.Run(async () =>
                {
                    using (var scope = _sp.CreateScope())
                    {
                        try
                        {
                            var db = scope.ServiceProvider.GetRequiredService<PshContext>();
                            var rand = new Random();
                            var user = new User();
                            var stat = new Stat();
                            var randomUsers = await GetRandomUsers(rand.Next(1, 10));
                            foreach (var randomUser in randomUsers)
                            {
                                user.UserName = randomUser.Login.Username;
                                user.Picture = randomUser.Picture.Thumbnail;
                                db.Users.Add(user);
                                db.SaveChanges();
                                stat.User = user;
                                stat.Created = DateTime.Now;
                                stat.Score = rand.Next(1, 100);
                                db.Stats.Add(stat);
                                db.SaveChanges();
                            }
                        }
                        catch (Exception e)
                        {
                            throw new NullReferenceException(e.Message);
                        }
                    }
                });
            };
            aTimer.Enabled = true;

            return Task.CompletedTask;
        }

        public async Task<List<RandomUser>> GetRandomUsers(int cant)
        {
            var responseRandomUser = new ResponseRandomUser();
            var response = await client.GetAsync("?inc=login,picture&results=" + cant.ToString());
            if (response.IsSuccessStatusCode)
            {
                string test = await response.Content.ReadAsStringAsync();
                responseRandomUser = JsonConvert.DeserializeObject<ResponseRandomUser>(test);
            }

            return responseRandomUser.Results;
        }

    }
}
