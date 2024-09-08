using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters; // Authentication -> Kimlik Do�r�lama

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // Uygulamam�za View ve Controllerimizi ekler

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Login"; // Kimlik Do�rulama ba�ar�s�z oldu�undan y�nlendirilcek sayfa(URL)

		/*
        CookieAuthenticationDefaults.AuthenticationScheme parametresi, kimlik do�rulaman�n cookie tabanl� olaca��n� belirtir.
         */
	});

builder.Services.AddMvc(config =>
{     // Build -> olu�turmak.
    var policy = new AuthorizationPolicyBuilder() // AuthorizationPolicyBuilder -> Yetkilendirme Politikas� Olu�turucu
	.RequireAuthenticatedUser() // RequireAuthenticatedUser ->  Kimli�i do�rulanm�� kullan�c�  Gerekli
	.Build();
    config.Filters.Add(new AuthorizeFilter(policy)); // Bu Kimli�i Do�rulam�� filtresini uygumlaman�n t�m�n� uygula!

	/*
    Bu kod blo�u uygulamadaki t�m controller ve i�indeti t�m ActionResultara kimlik do�rulanmad���n� s�rece eri�im k�s�d� getirir.
    Herhangi bir Controller veya Controller i�erisindeki ActionResulta k�s�tlama uygulama istemiyorsak "[AllowAnonymous]" ile
    anaonime izin ver yani bilinmeyen izin ver diyerek bu k�s�tlmadan muaf tutuyorsuz.

    Kodun tamam�nda kimli�i do�rulanm�� kullan�c� olu�tur ve bunu MVC'ye filtresini ekle diyoruz.

     */
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	/*
     Uygulaman�n �al��ma ortam�n� kontrol eder.E�er ortam bir geli�tirme ortam� de�ilse �zel hata sayfalar�n� kullan�c�ya g�stermek yerine
     genel bir hata sayfas� g�sterir.
     */
	app.UseExceptionHandler("/Home/Error"); // E�er bir hata olu�ursa "/Home/Error" url'sine y�nlendir.
											// Use -> Kullanmak , Exception -> istisna , Handler -> �dareci

	app.UseHsts(); // HTTP Strict Transport Security (HSTS) protokol�n� etkinle�tirir.
				   // Bu sayede uyglaman�n HTTPS �zerinde �al��mas�n� sa�lar ve HTTPS olmayan ba�lant�lar� engeller.
}

app.UseHttpsRedirection(); // Gelen Http isteklerini otomatik olarak Https'ye y�nlendirir g�venlili�i artt�rmak i�in.

app.UseStaticFiles(); // wwwroot dosyas�ndaki CSS,JavaScript,resim gibi dosyalar�n sunulmas�n� sa�lar.

app.UseRouting(); // Routing gelen URL'leri uygun Controller ve Actionlara y�nlendirir.

app.UseAuthorization(); // Authorization -> Yetkilendirme. Kullan�c�n�n belirli bir kayna�a veya i�leme eri�im iznini olup olmad���n� kontrol eder.


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Category}/{action=GetAllCategory}/{id?}"); // Var say�lan url'yi belirler.

app.Run(); // Uygulama �al��maya ba�lar ve HTTP isteklerini kabul etmeye ba�lar.



