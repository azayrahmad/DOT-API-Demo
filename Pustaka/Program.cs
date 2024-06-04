using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Pustaka.Models;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Tambahkan layanan ke container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=pustaka.db"));
builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rest API Pustaka", Version = "v1" });
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

// Konfigurasi middleware HTTP request
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rest API Pustaka v1"));
}

app.UseHttpsRedirection();

const string penulisCacheKey = "penulisList";

app.MapGet("/", () => "Selamat datang di demo REST API Pustaka! Silakan tambahkan /swagger di akhir alamat halaman ini untuk mengakses UI Swagger.");

// Endpoint API untuk Penulis
app.MapGet("/api/penulis", async (AppDbContext context, IMemoryCache cache) =>
{
    if (!cache.TryGetValue(penulisCacheKey, out List<Penulis> penulisList))
    {
        penulisList = await context.Penulis.Include(p => p.Buku).ToListAsync();
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5), // Cache kadaluwarsa 5 menit setelah disimpan
            SlidingExpiration = TimeSpan.FromMinutes(2) // Cache kadaluwarsa 2 menit setelah terakhir diakses
        };
        cache.Set(penulisCacheKey, penulisList, cacheEntryOptions);
    }
    return penulisList;
    //return await context.Penulis.Include(p => p.Buku).ToListAsync();
});

app.MapGet("/api/penulis/{id}", async (int id, AppDbContext context) =>
{
    var penulis = await context.Penulis.Include(p => p.Buku).FirstOrDefaultAsync(p => p.PenulisId == id);

    return penulis is not null ? Results.Ok(penulis) : Results.NotFound();
});

app.MapPost("/api/penulis", async (Penulis penulis, AppDbContext context, IMemoryCache cache) =>
{
    if (penulis == null || string.IsNullOrEmpty(penulis.Nama))
    {
        return Results.BadRequest("Data penulis tidak valid.");
    }

    // Validasi bahwa buku yang dimasukkan sudah ada di database
    var bukuIds = penulis.Buku.Select(b => b.BukuId).ToList();
    var existingBuku = await context.Buku.Where(b => bukuIds.Contains(b.BukuId)).ToListAsync();

    if (existingBuku.Count != bukuIds.Count)
    {
        return Results.BadRequest("Salah satu atau lebih buku yang dimasukkan belum ada di database.");
    }

    context.Penulis.Add(penulis);
    await context.SaveChangesAsync();

    // Invalidate cache
    cache.Remove(penulisCacheKey);

    return Results.Created($"/api/penulis/{penulis.PenulisId}", penulis);
});

app.MapPut("/api/penulis/{id}", async (int id, Penulis penulis, AppDbContext context, IMemoryCache cache) =>
{
    if (id != penulis.PenulisId || string.IsNullOrEmpty(penulis.Nama))
    {
        return Results.BadRequest("Data penulis tidak valid.");
    }

    var existingPenulis = await context.Penulis.Include(p => p.Buku).FirstOrDefaultAsync(p => p.PenulisId == id);
    if (existingPenulis == null)
    {
        return Results.NotFound();
    }

    // Validasi bahwa buku yang dimasukkan sudah ada di database
    var bukuIds = existingPenulis.Buku.Select(b => b.BukuId).ToList();
    var existingBuku = await context.Buku.Where(b => bukuIds.Contains(b.BukuId)).ToListAsync();

    if (existingBuku.Count != bukuIds.Count)
    {
        return Results.BadRequest("Salah satu atau lebih buku yang dimasukkan belum ada di database.");
    }

    existingPenulis.Nama = penulis.Nama;

    await context.SaveChangesAsync();
    
    // Invalidate cache
    cache.Remove(penulisCacheKey);

    return Results.NoContent();
});

app.MapDelete("/api/penulis/{id}", async (int id, AppDbContext context, IMemoryCache cache) =>
{
    var penulis = await context.Penulis.Include(p => p.Buku).FirstOrDefaultAsync(p => p.PenulisId == id);
    if (penulis == null)
    {
        return Results.NotFound();
    }

    if (penulis.Buku.Count > 0)
    {
        return Results.BadRequest("Penulis tidak dapat dihapus karena masih memiliki buku.");
    }

    context.Penulis.Remove(penulis);
    await context.SaveChangesAsync();

    // Invalidate cache
    cache.Remove(penulisCacheKey);

    return Results.NoContent();
});

// Endpoint API untuk Buku
app.MapGet("/api/buku", async (AppDbContext context) =>
{
    return await context.Buku.Include(b => b.Penulis).ToListAsync();
});

app.MapGet("/api/buku/{id}", async (int id, AppDbContext context) =>
{
    var buku = await context.Buku.Include(b => b.Penulis).FirstOrDefaultAsync(b => b.BukuId == id);

    return buku is not null ? Results.Ok(buku) : Results.NotFound();
});

app.MapPost("/api/buku", async (Buku buku, AppDbContext context) =>
{
    if (buku == null || string.IsNullOrEmpty(buku.Judul))
    {
        return Results.BadRequest("Data buku tidak valid.");
    }

    if (buku.PenulisId == null)
    {
        return Results.BadRequest("Penulis buku tidak boleh kosong.");
    }

     // Validasi bahwa PenulisId yang dimasukkan ada di database
    var penulisExists = await context.Penulis.AnyAsync(p => p.PenulisId == buku.PenulisId);

    if (!penulisExists)
    {
        return Results.BadRequest("PenulisId tidak ditemukan di database.");
    }

    context.Buku.Add(buku);
    await context.SaveChangesAsync();

    return Results.Created($"/api/buku/{buku.BukuId}", buku);
});

app.MapPut("/api/buku/{id}", async (int id, Buku buku, AppDbContext context) =>
{
    if (id != buku.BukuId || string.IsNullOrEmpty(buku.Judul))
    {
        return Results.BadRequest("Data buku tidak valid.");
    }

     // Validasi bahwa PenulisId yang dimasukkan ada di database
    var penulisExists = await context.Penulis.AnyAsync(p => p.PenulisId == buku.PenulisId);

    if (!penulisExists)
    {
        return Results.BadRequest("PenulisId tidak ditemukan di database.");
    }

    var existingBuku = await context.Buku.FindAsync(id);
    if (existingBuku == null)
    {
        return Results.NotFound();
    }

    existingBuku.Judul = buku.Judul;
    existingBuku.PenulisId = buku.PenulisId;

    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.MapDelete("/api/buku/{id}", async (int id, AppDbContext context) =>
{
    var buku = await context.Buku.FindAsync(id);
    if (buku == null)
    {
        return Results.NotFound();
    }

    context.Buku.Remove(buku);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
