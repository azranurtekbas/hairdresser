
using haircaredeneme.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Text.Json;


namespace haircaredeneme.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration; // Sadece IConfiguration'� tutuyoruz, API anahtar�n� GetGPTModelResponse metodunda kullanaca��z.


        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;

        }

            public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult about()
        {
            return View();
        }

        public IActionResult blog()
        {
            return View();
        }

     
        public IActionResult cal�sany�net()
        {
            return View();
        }


        public IActionResult islemy�net()
        {
            return View();
        }


        public IActionResult onay()
        {
            return View();
        }

        public IActionResult contact()
        {
            return View();
        }

        public IActionResult services()
        {
            return View();
        }


        public IActionResult galeri()
        {
            return View();
        }

        public IActionResult Randevularim()
        {
            // Oturumdan kullan�c� ID'sini al
            int? kullaniciId = HttpContext.Session.GetInt32("UserId");

            // Kullan�c� oturum a�mam��sa, giri� sayfas�na y�nlendir
            if (kullaniciId == null)
            {
                TempData["Error"] = "L�tfen giri� yap�n.";
                return RedirectToAction("galeri", "Home");
            }

            // Admin kontrol�
            bool isAdmin = HttpContext.Session.GetString("IsAdmin") == "true";

            // Kullan�c�n�n randevular�n� �ek
            var randevular = _context.Randevular
                .Where(r => r.musteriId == kullaniciId)
                .Include(r => r.Calisan)
                .Include(r => r.Islem)
                .OrderByDescending(r => r.RandevuTarihSaat)
                .ToList();

            // En son randevuyu belirle
            var sonRandevu = randevular.FirstOrDefault();

            // ViewBag'e bilgileri ekle
            ViewBag.SonRandevuId = sonRandevu?.randevuId;
            ViewBag.IsAdmin = isAdmin;

            // Randevular listesini view'e g�nder
            return View(randevular);
        }


        [HttpPost]
        public async Task<IActionResult> Register(string kullaniciAdi, string mail, string telefonNo, string sifre)
        {
            if (string.IsNullOrEmpty(kullaniciAdi) || string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(telefonNo) || string.IsNullOrEmpty(sifre))
            {
                TempData["Error"] = "L�tfen t�m alanlar� doldurun.";
                return View("RegisterForm"); // Hata varsa formu tekrar g�ster
            }

            if (await _context.Musteriler.AnyAsync(m => m.mail == mail))
            {
                TempData["Error"] = "Bu e-posta adresi zaten kay�tl�.";
                return View("RegisterForm"); // Hata varsa formu tekrar g�ster
            }
            // 3. Yeni Musteri nesnesi olu�tur
            var yeniMusteri = new Musteri
            {
                kullan�c�Ad = kullaniciAdi,
                mail = mail,
                telefonNo = telefonNo,
                sifre = sifre // �ifreyi g�venli bir �ekilde hashlemeniz �nerilir!
            };

            // 4. Musteri'yi veritaban�na ekle
            _context.Musteriler.Add(yeniMusteri);
            await _context.SaveChangesAsync();

            // 5. Ba�ar� mesaj�n� TempData'ya ekle
            TempData["Success"] = "Ba�ar�yla kaydoldunuz.";

            // 6. Oturum a�ma i�lemini ger�ekle�tir (iste�e ba�l�)
            // ... (Kullan�c�y� otomatik olarak oturum a�t�rabilirsiniz) ...

            // 7. Musteriler listesine y�nlendir
            return RedirectToAction("MusterilerListesi");
        }

        public IActionResult MusterilerListesi()
        {
            // T�m m��terileri, en son eklenenden ba�layarak s�rala
            var musteriler = _context.Musteriler
                .OrderByDescending(m => m.musteriId)
                .ToList();

            // En son eklenen m��teriyi belirle
            var sonMusteri = musteriler.FirstOrDefault();

            // ViewBag'e son m��terinin ID'sini ekle
            ViewBag.SonMusteriId = sonMusteri?.musteriId;

            // M��teri listesini view'e g�nder
            return View(musteriler);
        }


        public IActionResult Services1()
        {
            // T�m �al��anlar� veritaban�ndan �ek
            var calisanlar = _context.Calisanlar.ToList();
            var islemler = _context.Islemler.ToList();

            // �al��anlar� ViewBag'e ekle
            ViewBag.Calisanlar = calisanlar;
            ViewBag.Islemler = islemler;

            return View();
        }


        [HttpGet]
        public IActionResult yapayzeka()
        {
            return View();
        }
        
        // POST: /YapayZeka
        [HttpPost]
        public async Task<IActionResult> yapayzeka(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                TempData["Error"] = "L�tfen ge�erli bir foto�raf y�kleyin.";
                return RedirectToAction("YapayZeka");
            }

            try
            {
                // Foto�raf� sunucuya y�kleme ve URL alma
                var photoUrl = await UploadPhotoToServer(photo);

                // API'ye daha k�sa ve y�netilebilir bir prompt g�nder
                string prompt = $"Bu ki�i i�in �nerdi�iniz �� sa� stili nedir? Foto�raf URL'si: {photoUrl}";

                // API'ye istek g�nder
                var response = await GetGPTModelResponse(prompt);

                // Modelden gelen cevab� al
                string suggestion = response?.Trim() ?? "Sa� stili �nerisi al�namad�.";

                // Sonucu ViewBag'e atama
                ViewBag.Suggestion = suggestion;

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bir hata olu�tu: " + ex.Message;
                return RedirectToAction("YapayZeka");
            }
        }

        // Foto�raf� sunucuya y�kleyip URL olu�turma
        private async Task<string> UploadPhotoToServer(IFormFile photo)
        {
            var filePath = Path.Combine("wwwroot/uploads", Path.GetFileName(photo.FileName));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            // Foto�raf�n sunucu �zerindeki URL'sini d�nd�r
            return Url.Content($"~/uploads/{Path.GetFileName(photo.FileName)}");
        }

        // OpenAI API'ye istek g�nderme
        private async Task<string> GetGPTModelResponse(string prompt)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer xxxxx");
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
            new { role = "system", content = "You are a helpful assistant." },
            new { role = "user", content = prompt }
        },
                max_tokens = 150
            };

            var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"API iste�i ba�ar�s�z oldu: {errorMessage}");
            }

            // JSON yan�t�n� okuma
            var responseJson = await response.Content.ReadAsStringAsync();
            var result = Newtonsoft.Json.Linq.JObject.Parse(responseJson);
            var suggestion = result["choices"]?[0]?["message"]?["content"]?.ToString();

            return suggestion;
        }



        // Foto�raf� base64 format�na �evirme
        private async Task<string> ConvertPhotoToBase64(IFormFile photo)
        {
            using (var ms = new MemoryStream())
            {
                await photo.CopyToAsync(ms);
                var fileBytes = ms.ToArray();
                return Convert.ToBase64String(fileBytes);
            }
        }


        public IActionResult AylikKazancRaporu()
        {
            // 1. Dinamik tarih aral���n� belirle (�rne�in, son 3 ay)
            var bugun = DateTime.UtcNow;
            var baslangicTarihi = bugun.AddMonths(-3).Date; // Son 3 ay�n ba�lang�c� (g�n, ay, y�l - saat bilgisi at�l�yor)
            baslangicTarihi = new DateTime(baslangicTarihi.Year, baslangicTarihi.Month, baslangicTarihi.Day, 0, 0, 0, DateTimeKind.Utc); // Saat, dakika, saniyeyi s�f�rla (UTC)
            var bitisTarihi = bugun; // Bug�n�n sonuna kadar olan t�m tarihleri dahil etmek i�in (UTC olarak de�i�tiriliyor)
            bitisTarihi = new DateTime(bitisTarihi.Year, bitisTarihi.Month, bitisTarihi.Day, 23, 59, 59, DateTimeKind.Utc);

            // 2. Onaylanm�� randevular�, i�lem ve �al��an bilgilerini al (Dinamik tarih aral���na g�re UTC olarak filtrele)
            var onaylanmisRandevular = _context.Randevular
                .Include(r => r.Islem)
                .Include(r => r.Calisan)
                .Where(r => r.Durum == RandevuDurum.Onayland� &&
                            r.RandevuTarihSaat >= baslangicTarihi &&
                            r.RandevuTarihSaat <= bitisTarihi)
                .ToList();

            // 3. Kazan�lar� hesapla (�al��an baz�nda)
            var aylikKazancListesi = onaylanmisRandevular
                .GroupBy(r => r.CalisanId)
                .Select(g => new
                {
                    CalisanId = g.Key,
                    CalisanAdSoyad = g.First().Calisan.Cal�sanAd + " " + g.First().Calisan.Cal�sanSoyad,
                    ToplamKazanc = g.Sum(r => r.Islem.Fiyat)
                })
                .ToList();

            // 4. Listeyi ViewBag ile View'e g�nder
            ViewBag.AylikKazancRaporu = aylikKazancListesi;

            // 5. View'� d�nd�r
            return View("AylikKazancRaporu");
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // E�er oturum a��lmam��sa, ��k�� yap�lacak bir �ey yoktur
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                // Kullan�c� sisteme giri� yapmam��sa, hata mesaj� g�sterilebilir
                HttpContext.Session.SetString("LogoutError", "Hen�z giri� yapmad�n�z.");
                return RedirectToAction("Index", "Home"); // Ana sayfaya y�nlendir
            }

            // Oturumu temizle
            HttpContext.Session.Clear();

            // Ba�ar�yla ��k�� yap�ld���nda, ana sayfaya y�nlendir
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> CalisanEkleGuncelle(int? calisanID, string calisanAd, string calisanSoyad, string uzmanlikAlani, string uygunlukSaati, string islem)
        {
            try
            {
                if (islem == "ekle")
                {
                    // Yeni �al��an Ekleme
                    var yeniCalisan = new Calisan
                    {
                        Cal�sanAd = calisanAd,
                        Cal�sanSoyad = calisanSoyad,
                        UzmanlikAlani = uzmanlikAlani,
                        UygunlukSaati = TimeSpan.Parse(uygunlukSaati)
                    };
                    _context.Calisanlar.Add(yeniCalisan);
                }
                else if (islem == "guncelle" && calisanID.HasValue)
                {
                    // �al��an G�ncelleme
                    var mevcutCalisan = await _context.Calisanlar.FindAsync(calisanID.Value);
                    if (mevcutCalisan != null)
                    {
                        mevcutCalisan.Cal�sanAd = calisanAd;
                        mevcutCalisan.Cal�sanSoyad = calisanSoyad;
                        mevcutCalisan.UzmanlikAlani = uzmanlikAlani;
                        mevcutCalisan.UygunlukSaati = TimeSpan.Parse(uygunlukSaati);
                    }
                }
                else if (islem == "sil" && calisanID.HasValue)
                {
                    // �al��an Silme
                    var calisanSil = await _context.Calisanlar.FindAsync(calisanID.Value);
                    if (calisanSil != null)
                    {
                        _context.Calisanlar.Remove(calisanSil);
                    }
                }

                // De�i�iklikleri kaydet
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bir hata olu�tu: " + ex.Message;
            }

            // Admin paneline y�nlendir
            return RedirectToAction("admin_panel");
        }
        public async Task<IActionResult> admin_panel()
        {
            // Admin kontrol� (sadece adminlerin eri�ebilmesi i�in)
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return Unauthorized(); // Veya giri� sayfas�na y�nlendir
            }

            // Calisanlar tablosundaki t�m verileri al
            var calisanlar = await _context.Calisanlar.ToListAsync();

            // Islemler tablosundaki t�m verileri al
            var islemler = await _context.Islemler.ToListAsync();

            // Onay bekleyen randevular� �ek
            var randevular = await _context.Randevular
                .Where(r => r.Durum == RandevuDurum.Beklemede)
                .Include(r => r.Calisan)
                .Include(r => r.Islem)
                .Include(r => r.musteri)
                .OrderBy(r => r.RandevuTarihSaat)
                .ToListAsync();

            // Verileri ViewBag'e ekle
            ViewBag.Calisanlar = calisanlar;
            ViewBag.Islemler = islemler;
            ViewBag.Randevular = randevular; // Onay bekleyen randevular� da ekledik




            return View();
        }

        private bool IsAdmin(string username, string password)
        {
            return username == "g221210082@sakarya.edu.tr" && password == "sau";
        }


        [HttpPost]
        public async Task<IActionResult> RandevuReddet(int id)
        {
            // Admin kontrol�
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return Unauthorized(); // Veya giri� sayfas�na y�nlendir
            }

            var randevu = await _context.Randevular.FindAsync(id);

            if (randevu == null)
            {
                return NotFound();
            }

            randevu.Durum = RandevuDurum.IptalEdildi; // Durumu "Reddedildi" olarak ayarla
            _context.Randevular.Update(randevu);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Randevu reddedildi."; // Ge�ici mesaj
            return RedirectToAction("admin_panel"); // Admin paneline y�nlendir
        }

        [HttpPost]
        public async Task<IActionResult> RandevuOnayla(int id)
        {
            // Admin kontrol� (sadece adminlerin eri�ebilmesi i�in)
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return Unauthorized(); // Veya giri� sayfas�na y�nlendir
            }

            var randevu = await _context.Randevular.FindAsync(id);

            if (randevu == null)
            {
                return NotFound(); // Randevu bulunamazsa 404 hatas� d�nd�r
            }

            randevu.Durum = RandevuDurum.Onayland�;
            _context.Randevular.Update(randevu);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Randevu onayland�.";
            return RedirectToAction("admin_panel"); // Admin paneline y�nlendir
        }


        [HttpPost]
        public IActionResult AdminLogin(string admin_username, string admin_password)
        {
            if (IsAdmin(admin_username, admin_password))
            {
                // Admin giri�i ba�ar�l�, session'a admin oldu�unu kaydet
                HttpContext.Session.SetString("IsAdmin", "true");

                return RedirectToAction("admin_panel", "Home"); // Admin paneline y�nlendir
            }
            else
            {
                TempData["Error"] = "Hatal� kullan�c� ad� veya �ifre!";
                return RedirectToAction("galeri", "Home"); // Giri� sayfas�na geri g�nder
            }
        }

        public IActionResult AdminLogout()
        {
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Index", "Home"); // Veya istedi�iniz ba�ka bir sayfaya
        }

        [HttpPost]
        public async Task<IActionResult> IslemEkleGuncelle(int? islemID, string islemAd, string islemTur, string islemSuresi, decimal fiyat, int calisanId, string islem)
        {
            try
            {
                if (islem == "ekle")
                {
                    // Yeni ��lem Ekleme
                    var yeniIslem = new Islem
                    {
                        IslemAd = islemAd,
                        IslemTur = islemTur,
                        IslemSuresi = islemSuresi,
                        Fiyat = fiyat,
                        Cal�sanId = calisanId
                    };
                    _context.Islemler.Add(yeniIslem);
                }
                else if (islem == "guncelle" && islemID.HasValue)
                {
                    // ��lem G�ncelleme
                    var mevcutIslem = await _context.Islemler.FindAsync(islemID.Value);
                    if (mevcutIslem != null)
                    {
                        mevcutIslem.IslemAd = islemAd;
                        mevcutIslem.IslemTur = islemTur;
                        mevcutIslem.IslemSuresi = islemSuresi;
                        mevcutIslem.Fiyat = fiyat;
                        mevcutIslem.Cal�sanId = calisanId;
                    }
                }
                else if (islem == "sil" && islemID.HasValue)
                {
                    // ��lem Silme
                    var islemSil = await _context.Islemler.FindAsync(islemID.Value);
                    if (islemSil != null)
                    {
                        _context.Islemler.Remove(islemSil);
                    }
                }

                // De�i�iklikleri kaydet
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bir hata olu�tu: " + ex.Message;
            }

            // Admin paneline y�nlendir
            return RedirectToAction("admin_panel");
        }

        [HttpGet]
        public async Task<IActionResult> GetCalisanlarByIslem(int islemId) // islemId int olarak de�i�tirildi
        {
            try
            {
                Console.WriteLine($"GetCalisanlarByIslem �a�r�ld�. islemId: {islemId}");

                // 1. Ad�m: islemId'ye g�re Islem nesnesini bul
                var islem = await _context.Islemler
                    .Include(i => i.CalisanNavigation)
                    .FirstOrDefaultAsync(i => i.IslemsId == islemId); // islemId'ye g�re filtrele

                Console.WriteLine($"Sorgu sonucu: {islem != null}");
                if (islem != null)
                {
                    Console.WriteLine($"Bulunan islem: IslemId: {islem.IslemsId}, CalisanId: {islem.Cal�sanId}");
                }

                if (islem == null)
                {
                    // E�le�en i�lem bulunamazsa bo� bir liste d�nd�r
                    Console.WriteLine($"islemId: {islemId} i�in i�lem bulunamad�");
                    return Json(new List<object>());
                }

                // 2. Ad�m: CalisanId'ye g�re Calisan nesnesini bul
                var calisan = await _context.Calisanlar.FindAsync(islem.Cal�sanId);

                if (calisan == null)
                {
                    Console.WriteLine($"CalisanId {islem.Cal�sanId} i�in �al��an bulunamad�.");
                    return Json(new List<object>());
                }

                // 3. Ad�m: �lgili Calisan bilgilerini al
                var calisanBilgisi = new
                {
                    calisan.Id,
                    calisan.Cal�sanAd,
                    calisan.Cal�sanSoyad,
                    calisan.UzmanlikAlani,
                    calisan.UygunlukSaati
                };

                Console.WriteLine($"Gelen �al��an: {calisanBilgisi.Cal�sanAd} {calisanBilgisi.Cal�sanSoyad}");

                // Sadece ilgili �al��an� d�nd�r
                return Json(new List<object> { calisanBilgisi });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                return Json(new { message = $"Hata: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Services1(int islemId, int calisanId, DateTime randevuTarihSaat, string adSoyad, string email, string telefon, string ekstraBilgi)
        {
            try
            {
                var sessionEmail = HttpContext.Session.GetString("UserEmail");
                if (string.IsNullOrEmpty(sessionEmail))
                {
                    TempData["Error"] = "L�tfen giri� yap�n.";
                    return RedirectToAction("Login", "Account");
                }

                if (!string.Equals(sessionEmail, email, StringComparison.OrdinalIgnoreCase))
                {
                    TempData["Error"] = "Formda girdi�iniz e-posta, giri� yapt���n�z e-posta ile e�le�miyor.";
                    return RedirectToAction("Services1");
                }
                // ��lemi ID'ye g�re bul (IslemsId kullan�n)
                var islem = await _context.Islemler.FirstOrDefaultAsync(i => i.IslemsId == islemId);

                if (islem == null)
                {
                    // Hata mesaj�nda da IslemsId kullan�n
                    _logger.LogError($"��lem ID: {islemId} bulunamad�. (IslemsId kontrol edilmeli)");
                    throw new Exception($"��lem ID: {islemId} bulunamad�.");
                }



                var calisan = await _context.Calisanlar.FirstOrDefaultAsync(c => c.Id == calisanId);
                if (calisan == null) throw new Exception("�al��an bulunamad�.");

                string hataMesaji = "";

                // ... di�er kodlar ...

                // Saati UTC olarak i�aretle
                if (randevuTarihSaat.Kind == DateTimeKind.Unspecified)
                {
                    randevuTarihSaat = DateTime.SpecifyKind(randevuTarihSaat, DateTimeKind.Utc);
                }

                // �al��ma saatleri kontrol� (UTC saate g�re)
                var salonCalismaSaatleri = new { BaslangicSaati = 6, BitisSaati = 15 };
                if (randevuTarihSaat.Hour < salonCalismaSaatleri.BaslangicSaati || randevuTarihSaat.Hour >= salonCalismaSaatleri.BitisSaati)
                {
                    TempData["Error"] = "Randevu saatleri UTC'ye g�re 06:00 - 15:00 aras�nda olmal�d�r (T�rkiye saati 09:00-18:00).";
                    return RedirectToAction("Services1");
                }

             
                // Ge�mi� tarih kontrol�
                if (randevuTarihSaat < DateTime.UtcNow)
                {
                    hataMesaji = "Ge�mi� bir tarih i�in randevu al�namaz.";
                }
                // 1 ay sonras� kontrol�
                else if (randevuTarihSaat > DateTime.UtcNow.AddMonths(1))
                {
                    hataMesaji = "Sadece 1 ay sonras�na kadar randevu al�nabilir.";
                }
                // �ak��ma kontrol�
                else if (await _context.Randevular.AnyAsync(r => r.CalisanId == calisanId && r.RandevuTarihSaat == randevuTarihSaat))
                {
                    hataMesaji = "Bu tarih ve saat i�in randevu zaten al�nm��.";
                }

                // Hata varsa, hata mesaj�yla birlikte geri d�n
                if (!string.IsNullOrEmpty(hataMesaji))
                {
                    TempData["Error"] = hataMesaji;
                    return RedirectToAction("Services1");
                }

                // Randevu nesnesini olu�tur
                var randevu = new Randevu
                {
                    musteriId = HttpContext.Session.GetInt32("UserId") ?? 0,
                    islemId = islem.IslemsId,
                    CalisanId = calisanId,
                    RandevuTarihSaat = randevuTarihSaat,
                    Durum = RandevuDurum.Beklemede
                };

                _context.Randevular.Add(randevu);
                await _context.SaveChangesAsync();

                // Yeni randevu olu�turuldu�unda admin paneline bildirim g�nder
                TempData["YeniRandevuBildirimi"] = "Yeni bir randevu olu�turuldu! Onaylamak i�in l�tfen Randevu Onay sekmesine gidin.";

                TempData["Success"] = "Randevunuz ba�ar�yla olu�turuldu.";
                return RedirectToAction("Randevularim");


            }
            catch (Exception ex)
            {
                TempData["Error"] = "Randevu olu�turulurken bir hata olu�tu: " + ex.Message;
                return RedirectToAction("Services1");
            }
        }


    }
}