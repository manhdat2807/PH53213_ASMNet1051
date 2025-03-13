using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PH53213_ASMNet1051.Models;
using System.Net.Http;

namespace PH53213_ASMNet1051.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DanhMucController : Controller
    {
        private readonly HttpClient _httpClient;
        public DanhMucController(HttpClient httpclient)
        {
            _httpClient = httpclient;
            _httpClient.BaseAddress = new Uri("https://localhost:7228/");
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetStringAsync("api/danhmucs");
            var dm = JsonConvert.DeserializeObject<List<DanhMuc>>(response);
            return View(dm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetStringAsync($"api/danhmucs/{id}");
            var dm = JsonConvert.DeserializeObject<DanhMuc>(response);
            return View(dm);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(DanhMuc dm)
        {
            await _httpClient.PostAsJsonAsync("api/danhmucs", dm);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetStringAsync($"api/danhmucs/{id}");
            var dm = JsonConvert.DeserializeObject<DanhMuc>(response);
            return View(dm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DanhMuc dm)
        {
            await _httpClient.PutAsJsonAsync($"api/danhmucs/{dm.MaDanhMuc}", dm);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _httpClient.DeleteAsync($"api/danhmucs/{id}");
            return RedirectToAction("Index");
        }
    }
}
