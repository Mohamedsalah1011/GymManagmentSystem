using GymManagmentDAL.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagmentDAL.DataSeed
{
    public static class GymDbContextDataSeeding
    {
        public static bool SeedData(GymDbContext dbContext)
        {
            try 
            {
                var HasPlans = dbContext.Planes.Any();
                var HasCategories = dbContext.Categories.Any();

                if (HasPlans && HasCategories) return false;
                if (!HasPlans)
                {
                    var plans = LoadDataFromJasonFile<Entites.Plan>("Plans.json");

                    if (plans.Any())
                    {
                        dbContext.Planes.AddRange(plans);
                    }

                }

                if (!HasCategories)
                {
                    var categories = LoadDataFromJasonFile<Entites.Category>("Categories.json");

                    if (categories.Any())
                    {
                        dbContext.Categories.AddRange(categories);
                    }
                }

                return dbContext.SaveChanges() > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Seeding Failed {ex}");
                return false;
            }
        }
        private static List<T> LoadDataFromJasonFile<T> (string fileName)
        {
            //D:\Rout Assignment\MVC project\GymManagmentSystemSolution\GymManagmentPL\wwwroot\Files\plans.json
            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", fileName);
            
            if (!File.Exists(FilePath)) throw new FileNotFoundException();

            string Data = File.ReadAllText(FilePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            return JsonSerializer.Deserialize<List<T>>(Data, options) ?? new List<T>();

        }
    }
}
