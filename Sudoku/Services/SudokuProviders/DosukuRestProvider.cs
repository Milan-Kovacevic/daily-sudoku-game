using Newtonsoft.Json;
using Sudoku.Exceptions;
using Sudoku.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Sudoku.Services.SudokuProviders
{
    public class DosukuRestProvider : ISudokuProvider
    {
        private static readonly string DOSUKU_PROVIDER_BASE_ENDPOINT = "https://sudoku-api.vercel.app/api/dosuku";
        private readonly HttpClient client;
        private static readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public DosukuRestProvider()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(DOSUKU_PROVIDER_BASE_ENDPOINT);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private record class DosukuRestModel(SudokuBoard Newboard);

        public async Task<SudokuBoard> GetSudokuBoard(int numberOfGrids)
        {
            client.CancelPendingRequests();
            SudokuBoard? newBoard = null;
            HttpResponseMessage response = await client.GetAsync($"?query={{newboard(limit:{numberOfGrids}){{grids{{value,solution,difficulty}}}}}}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var restModel = JsonConvert.DeserializeObject<DosukuRestModel>(json);

                newBoard = restModel?.Newboard;
            }

            if (newBoard == null)
                throw new SudokuProviderUnavailableException(nameof(DosukuRestProvider));

            return newBoard;
        }
    }
}
