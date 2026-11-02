# Geliştirme sonucu — 2 Kasım 2026

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
