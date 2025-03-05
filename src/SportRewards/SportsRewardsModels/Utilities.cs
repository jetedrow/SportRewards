using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportsRewardsModels
{
    // This class should be in its own project, not living with models,
    // however it is placed here to avoid having another project in the solution.
    // In a production (non-coding exercise) environment, this would be separated.
    public static class Utilities
    {

        public static byte[] SerializeToByteArray<TInput>(TInput source, JsonSerializerOptions? serializerOptions = null)
        {
            var json = JsonSerializer.Serialize<TInput>(source, serializerOptions);
            return System.Text.Encoding.UTF8.GetBytes(json);
        }

    }
}
