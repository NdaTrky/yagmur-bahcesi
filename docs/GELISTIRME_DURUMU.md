# Geliştirme sonucu — 2 Kasım 2026

## Güncel durum: 0.2 genişleme test paketi

Üç mini oyun, pul/kalıcı koleksiyon, altı dikim alanı, sekiz çiçek, on iki dekor, Sumi, dört proje ve yerel albüm uygulandı. Kapsamın tam davranışları ve henüz yapılmayan içerik [GENISLEME_02.md](GENISLEME_02.md) belgesinde. Android geliştirmesi sürüyor; Roblox için [ayrı uyarlama planı](ROBLOX_UYARLAMA_PLANI.md) hazırlandı. **Hiçbir mağazada/platformda yayın yapılmadı.**

| Kontrol | 0.2 sonucu | Kanıt |
|---|---|---|
| Editor / kurallar | **150/150 geçti**; dokuz düzen, eski kayıt göçü, özgün yedek, bozulan kayıt, satın alma ve tekil ödül | `Artifacts/editor-tests.json` |
| Windows oynanışı | **29/29 işlev kontrolü geçti**; gerçek düğme callback'leri, pointer akışı, dikim, tüm projeler, Sumi ve albüm | `Artifacts/Smoke-1600x900/runtime-tests.json`, `Artifacts/Smoke-1280x960/runtime-tests.json` |
| Windows süreç kapanışı | **Başarısız**; normal çıkışta `0xc0000005` / `-1073741819`. İşlev kontrolünün geçmesi bütün masaüstü testinin geçtiği anlamına gelmez | `process-exit.json`, Player günlüğü ve Windows Application Error kaydı |
| Android API 36 emülatörü | **22/22 geçti**; gerçek ADB dokunuşları, çevrimdışı oyun, yarım etkinlikten dönüş, üç ödül, yeni çiçek, albüm ve süreç yeniden başlatma | `Artifacts/Android-API36/android-tests.json` |
| ARM64 test APK | 0.2.0 / versionCode 2; derlendi. Kesin boyut ve SHA256 paket raporunda | `Builds/Android/YagmurBahcesi-test.apk`, `Artifacts/Android-Package/package-tests.json` |
| İzin / statik 16 KB kontrolü | Harici depolama, internet, kamera ve mikrofon izni yok; ZIP ve 6 yerel kütüphane hizalama kontrolü geçti | `Artifacts/Android-Package/` |

Emülatörde bu genişleme oturumunun son ölçümü toplam PSS 191.188 KB (~186,7 MiB), RSS 316.240 KB. Bu SwiftShader/x86_64 ölçümüdür; fiziksel telefon FPS, pil veya bellek kabulü sayılmaz. Emülatör 4 KB bellek sayfası kullanır; 16 KB gerçek çalışma testi henüz yok.

### Windows kapanış sorunu

Hata yalnız test sonucundaki exit kodu atlanarak gizlenmedi; test scripti sıfır olmayan kodu başarısız sayar ve ayrıca `process-exit.json` yazar. Windows olay kaydındaki adres yerel Unity PDB sembolleriyle `RuntimeStatic<PlatformAccessibilityManager,0>::StaticDestroy` olarak çözüldü. Çökme oyun kontrolleri bitip normal çıkış başlatıldıktan sonra oluşuyor. İlerleme kapanıştan önce diske yazılıyor, ancak bundan veri kaybının her durumda imkânsız olduğu sonucu çıkarılmaz.

Development modunu kapatma, erişilebilirlik modülünü başlatma ve pencereyi önce pasifleştirme denemeleri çözmedi. Başarısız geçici kodlar kaldırıldı. Kurulu Unity 6000.4.9f1, asıl proje değiştirilmeden ayrı derleme kopyasında denendi; onda da 29 kontrolün ardından aynı çıkış kodu görüldü. Ana proje **6000.3.17f1** olarak kaldı. İşletim sisteminin erişilebilirlik ayarları değiştirilmedi.

Unity'nin UUM-157903 kaydı, UI Automation bağlıyken kapanışta erişilebilirlik temizliğinden giriş sistemine gelen mesajla ilgili benzer bir hatayı ve 6000.3.29f1 düzeltmesini listeliyor. Bu bizim hatamızın kesin aynı hata olduğunu kanıtlamaz. Sonraki teknik adım, düzeltilmiş LTS sürümünü ayrı kopyada sınamak ve ardından sürüm yükseltmesini değerlendirmektir. [Resmî hata kaydı](https://issuetracker.unity.com/issues/26102/player-crashes-on-newinputactivate-during-shutdown-when-narrator-is-enabled)

### Yayına çıkmak için kalanlar

Windows kapanışının çözümü; konuşmalı TR/EN yönergeler; Piknik Sepeti'nde dizilerden öte kural çeşitliliği; gerçek Android cihaz/uzun oturum ve aile pilotu; yayıncı/Play Console hesabı, ürün kimliği, destek/gizlilik metni, yayın imzası, AAB ve mağaza süreçleri. Otomatik testlerden yaşa uygunluk veya uzun vadeli bağlılık garantisi çıkarılmıyor.

## 0.1 tarihsel raporu

Aşağıdaki içerik önceki prototipin kaydıdır. Sayılar ve çıktı boyutları güncel 0.2 raporu yerine kullanılmamalıdır. Özellikle eski Windows işlev testleri kapanışın sorunsuz olduğunu kanıtlamaz.

**Yağmur Bahçesi'nin ilk oynanabilir Unity sürümü hazır. Windows'ta ve yerel Android 16 emülatöründe gerçekten çalıştırıldı. Google Play'de yayın yapılmadı.**

Kullanıcının onayladığı kapsam Android'dir; iOS beklemede. Google Play Console hesabı ve fiziksel Android cihazı henüz yok. Araştırma/konsept aşamasından gerçek proje, asset, sahne, oyun döngüsü ve APK aşamasına geçildi.

## Oynanabilir içerik

- Tek bir bütünlüklü bahçe; yağmur/güneş ile üç bitki yatağı, yaprak tekne/rüzgâr ve meyve paylaşmalı piknik.
- Dört hayvan karakteri, 16 ifade/poz, sürüklenebilir üç dekor, tepki animasyonları ve yağmur/dalga parçacıkları.
- Dokunma ve fare girişi; küçük nesnelere öncelik; etkileşimi kesen menüler, yetişkin sorusu ve sıfırlama onayı.
- Türkçe/İngilizce arayüz, ses ve azaltılmış hareket ayarları; yerel otomatik kayıt ve bozuk ana dosyadan yedek kurtarma.
- Özgün üretilmiş çevre/karakter/nesne/ikon resimleri, Nunito fontu ve sentezlenmiş kısa sesler.

Unity sahnesi ve ayarları Editor API üzerinden üretildi; C# kodu Unity tarafından derlendi. Bu rapordaki ekran görüntüleri çalışan oyundan alınmıştır; konsept çizimi olarak üretilmiş ekran tasarımları değildir.

## Doğrulanan kontroller

| Aşama | Sonuç | Kanıt |
|---|---|---|
| Unity proje/sahne ve C# derlemesi | Başarılı, Unity 6000.3.17f1 | `Logs/Check.log`, `Logs/BuildWindows.log` |
| Editor kontrolleri | **50/50 geçti**; büyüme, sınır değerler, kayıt/kurtarma, 16 nesnenin referansları | `Artifacts/editor-tests.json` |
| Windows çalışma testi | **10/10 geçti**, 1600×900; gerçek Player, programatik giriş/kurallar ve ekran yakalama | `Artifacts/Smoke-1600x900/runtime-tests.json` |
| Tablet oranı görsel kontrol | 1280×960, **10/10**; başlık, kontrol alanları ve bahçe görünür | `Artifacts/Smoke-1280x960/` |
| Android 16 emülatör testi | **11/11 geçti**, 1920×1080, x86_64; ADB ile gerçek Android dokunma/sürükleme olayları | `Artifacts/Android-API36/android-tests.json` |
| Yetişkin ekranları | Doğru cevapla açılma, resetten vazgeçince kayıt korunması ve azaltılmış hareket kaydı geçti; metinler görsel olarak incelendi | `Artifacts/Android-API36/parent-tests.json`, `settings.png`, `privacy.png` |
| ARM64 telefon APK'sı | Derleme başarılı; **24.377.589 bayt (23,25 MiB)**; min API 26, target API 36 | `Builds/Android/YagmurBahcesi-test.apk` |
| Android izinleri | Telefon APK'sında internet/kamera/mikrofon/konum/harici depolama izni yok; yalnız uygulamaya özel receiver izni | `Artifacts/Android-Package/manifest.txt` |
| 16 KB statik paket kontrolü | ZIP hizalaması ve **6/6** ARM64 yerel kütüphanenin ELF LOAD hizalaması geçti | `Artifacts/Android-Package/package-tests.json` |

Android testi Wi-Fi ve mobil veri kapalıyken yapıldı. Çiçek büyümesi, tekne ve meyve sürükleme, dekor konumu, dil/ses, arka plandan dönüş ve süreç yeniden başlatıldığında kayıt kontrol edildi. Test günlüğünde Unity exception veya Android fatal crash görülmedi. Bu sonuç tüm cihazlarda hatasız çalışma garantisi değildir.

Emülatörde bir ölçümde toplam PSS **176.626 KB (~172,5 MiB)**, RSS 302.820 KB idi. `am start` Activity açılışı 688 ms raporladı; bu tam oyunun etkileşime hazır olma süresi değildir. İlk emülatör açılışında sistemin tam ekran tanıtımı ve grafik hazırlığı nedeniyle siyah ekran gözlendi; tanıtım kapatılıp uygulamaya dönülmesinden sonraki soğuk açılış ve yeniden açılış testleri geçti. Bu bilgisayarın SwiftShader emülatör ölçümü fiziksel telefon performansı yerine kullanılamaz.

1280×960 testi son tekne konumu/dokunma önceliği düzeltmesinden önceki aynı arayüz sürümünde yapıldı; son düzeltme 1600×900 ve Android 1920×1080'de yeniden doğrulandı.

## Bulunan ve giderilen sorunlar

1. Ana menüde uzun başlığın ikinci satırı kesiliyordu. Başlık iki ayrı, sığan satıra ayrıldı.
2. Nesne atlasında komşu görsellerin küçük parçaları görünüyordu. Dilim sınırları ve şeffaflık kırpması düzeltildi.
3. Tekne sürüklenirken sincabın seçilebildiği Android üzerinde görüldü. Tekne boş kıyıya taşındı, tekne/meyveler için çizim ve dokunma önceliği güncellendi; gerçek sürükleme testine eklendi.
4. Android derleyicisi `Masaüstü` içeren proje yolunu reddetti. Ana proje korunarak yalnız derleme için ASCII yolda kopya kullanıldı.
5. Gizli Windows penceresinde ekran yakalama tamamlanmıyordu. Otomatik görsel test artık görünür D3D11 önizlemesi ve süre sınırı kullanıyor.
6. Android derlemesi bittikten sonra bekleyen ADB alt süreci PowerShell'i açık tutabiliyordu. Yardımcı script yalnız Unity ana sürecinin çıkışını bekliyor.

## Çalıştırma ve dosyalar

`OYUNU_AC.cmd` dosyasını çift tıklamak Windows önizlemesini açar. Android emülatörü ve APK bu bilgisayara kuruldu; yeniden açmak için `tools/Start-AndroidPreview.ps1` kullanılabilir. Uygulama emülatörün uygulama listesinde **Yagmur Bahcesi** adıyla görünür.

Asıl kaynak: `YagmurBahcesi/`. Windows çıktı: `Builds/Windows/`. Telefon APK'sı: `Builds/Android/YagmurBahcesi-test.apk`. Emülatör APK'sı: `Builds/Android/YagmurBahcesi-emulator.apk`.

Emülatör sürümünde Unity development araçları vardır. Telefon APK'sında development modu kapalıdır; her iki dosya yerel debug imzalı test çıktısıdır. Mağazaya gönderilecek yayıncı imzalı AAB değildir. İmzalama anahtarı/parolası Git'e veya rapora eklenmedi.

Kaynaklar, Unity `.meta` dosyaları, proje/paket ayarları ve belgeler Git ile korunur. Derlemeler, Library, Temp, indirilen araçlar ve anahtar dosyaları Git dışında kalır. OneDrive ayarları ve asıl proje konumu değiştirilmedi.

## Henüz tamamlanmayan yayın aşamaları

- Fiziksel ARM64 telefon/tablette kurulum, gerçek çoklu dokunma, çentikli/uzun ekranlar, uzun oturum, ses dinleme, FPS, pil/ısı ve 16 KB çalışma testi.
- Ebeveyn izinli 4–6 yaş kullanım oturumları; metin okumadan keşif, sürükleme kolaylığı, tekrar oynama ve içerik yeterliliği. Eğitsel etki ölçülmedi.
- Kullanım bulgularına göre içerik, sesli ipuçları ve karakter geçişlerinin son düzenlemesi.
- Kesin uluslararası ad/marka taraması, paket kimliği, yayıncı adı, destek e-postası, gizlilik sayfası ve fiyat.
- Play Console hesabı/kimlik/ödeme kurulumu; yeni kişisel hesap için fiziksel cihaz erişimi doğrulaması ve gerekli 12 kişi/14 gün kapalı test.
- Yayıncı upload key'i, imzalı AAB, mağaza görsellerinin son seçimi, derecelendirme/Families/Data safety beyanları ve Google incelemesi.

Bu adımlar tamamlanmadan “bütün aşamalar bitti” veya “canlıya alındı” denemez. Yerel geliştirme ve emülatör testi telefon satın almadan sürdürülebilir. Yayın koşulları ve kaynakları [yayın hazırlığında](YAYIN_HAZIRLIGI.md) kayıtlıdır.
