# Unity üzerinde doğrudan geliştirme — erişim ve çalışma raporu

## Güncelleme — 2 Kasım 2026

0.2 genişlemesi gerçek Unity proje dosyalarına uygulandı; üç mini oyun, koleksiyon ve ilerleme Android emülatöründe sınandı. Windows işlevleri çalışsa da normal kapanışta yerel Unity hatası sürüyor; “bütün testler geçti” denmiyor. Roblox için teknik/yayın planı hazırlandı, Roblox Editor bağlantısı veya Roblox yayını yapılmadı. Güncel ayrıntılar [raporda](docs/GELISTIRME_DURUMU.md).

Kullanıcı Yağmur Bahçesi konseptini ve Android geliştirmesini onayladı; iOS ertelendi. Unity 6000.3.17f1 ile gerçek proje ve sahne oluşturuldu, C# kodları ve görsel/ses assetleri eklendi, Windows ve Android APK derlemeleri alındı. Unity Editor API/CLI otomasyonu ve gerçek Player çalıştırması doğrulandı. Android SDK/NDK/OpenJDK ve Android 16 emülatörü kuruldu. Önceki incelemenin aşağıdaki “henüz yapılmadı” ifadeleri tarihsel kayıttır; güncel sonuçlar [geliştirme durum raporundadır](docs/GELISTIRME_DURUMU.md).

Asıl proje `YagmurBahcesi` klasöründe korunuyor. Android derleyicisinin Unicode yol kısıtlaması nedeniyle kullanıcı klasöründe ayrı bir derleme kopyası kullanılıyor. Git sürüm kaydı var. Unity penceresinde fareyle yapılan işlemler iddia edilmiyor; sahne/asset ayarları gerçek Unity Editor koduyla oluşturulup derleniyor.

İnceleme tarihi: 1 Kasım 2026, Europe/Istanbul.
Durum: Ortam incelemesi tamamlandı. Konsept onayı bekleniyor. Unity projesi oluşturulmadı; oyun kodu yazılmadı.

## Doğrulanan yerel durum

| Kontrol | Gözlenen sonuç |
|---|---|
| Çalışma klasörü | `C:\Users\nturk\OneDrive\Masaüstü\oyun` |
| Mevcut çalışma | İlk incelemede klasör boştu; korunması gereken mevcut proje dosyası bulunmadı. |
| Dosya ve komut erişimi | PowerShell komutları ve dosya okuma çalıştı. Bu raporların yazılmasıyla belge yazma erişimi de kullanıldı. |
| Unity Hub | `C:\Program Files\Unity Hub\Unity Hub.exe` bulundu. |
| Editor 1 | `6000.3.17f1`; `Editor\Unity.exe` mevcut. PlaybackEngines altında Windows desteği görüldü. |
| Editor 2 | `6000.4.7f1`; `Editor\Unity.exe` mevcut. Windows ve WebGL desteği görüldü. |
| Editor 3 | `6000.4.9f1`; `Editor\Unity.exe` mevcut. Windows ve WebGL desteği görüldü. |
| Mobil modüller | İncelenen üç Editor'ün PlaybackEngines klasörlerinde Android ve iOS modülleri bulunmadı. Bu bulgu, bilgisayarın başka yerinde SDK bulunmadığını kanıtlamaz. |
| Çalışan Unity işlemi | İnceleme anında adı Unity ile başlayan işlem bulunmadı. |
| Lisans ve Editor açılışı | Sınanmadı; kurulu dosyanın bulunması geçerli lisans veya başarılı açılış kanıtı değildir. |
| Cihazlar ve mağaza hesapları | Android/iOS cihaz bağlantısı, Apple hesabı, Play Console ve imzalama erişimi sınanmadı. |
| Grafik arayüz kontrolü | Bu oturumda Unity'ye bağlı bir MCP aracı veya genel fare/klavye ile masaüstü kontrol aracı sunulmuyor. |

## Doğrudan çalışma yöntemi

Dosya sistemi ve komut satırı erişimi sayesinde yalnızca kopyalaman için kod vermekle sınırlı değilim. Onaylanan aşamada proje dosyalarını oluşturabilir, kurulu Unity yürütülebilir dosyasını çağırabilir ve Unity'nin kendi Editor API'lerini çalıştıran C# Editor scriptleriyle sahneyi üretebilirim. Bu yol, Unity'nin desteklediği `-createProject`, `-batchmode`, `-projectPath` ve `-executeMethod` mekanizmalarına dayanır. Yerel lisans ve açılış testi henüz yapılmadı. [Unity komut satırı belgesi](https://docs.unity3d.com/6000.3/Documentation/Manual/EditorCommandLineArguments.html)

| İstenen iş | Uygulama yolu | Bugünkü durum |
|---|---|---|
| 1. Proje oluşturma | Unity komut satırı ile ayrı proje klasörü oluşturma | Onay sonrası lisans/açılış doğrulanacak |
| 2. Sahne ve nesne oluşturma | Editor scriptiyle GameObject ve bileşen ekleme; sahne kaydetme | Henüz uygulanmadı |
| 3. Karakter ve ortam yerleştirme | Prefab üretme, konumlandırma, kamera ve sıralama ayarları | Henüz uygulanmadı |
| 4. C# scriptlerini ekleme | Dosyaları doğrudan projeye yazma; Unity derleme sonucunu okuma | Henüz uygulanmadı |
| 5. Animasyon, fizik, etkileşim | AnimationClip/Animator, collider ve giriş bileşenlerini yapılandırma | Henüz uygulanmadı |
| 6. UI oluşturma | Canvas, buton, simge, safe area ve ölçekleme yapılandırması | Henüz uygulanmadı |
| 7. Sahne düzenleme | Editor API üzerinden mevcut nesneleri hedefleyerek düzenleme | Henüz uygulanmadı |
| 8. Görsel, materyal ve ses | Dosya içe aktarma; importer, sprite atlası ve materyal ayarları | Henüz uygulanmadı |
| 9. Çalıştırma ve test | EditMode/PlayMode testleri, günlükler; uygun aşamada çalışır build ve görüntü yakalama | Oyun çalıştırılmadı |
| 10. Hata düzeltme | Derleme/test/çalıştırma günlüklerinden sorunu bulup yeniden doğrulama | Henüz oyun hatası test edilemez |
| 11. Android/iOS ayarları | PlayerSettings, build profilleri ve hedef ayarlarını otomatik hazırlama | Mobil modül kurulumu ve platforma özgü build adımları gerekiyor |

Başlangıç için ilave bir Unity MCP bağlantısı zorunlu değil. Sürekli açık Editor'e daha etkileşimli komut göndermek istenirse sonradan gözden geçirilmiş bir Editor köprüsü eklenebilir; bugün böyle bir bağlantı kurulmadı.

## Motor ve platform kararı için öneri

Kurulu sürümler arasından **Unity 6000.3 LTS hattı** başlangıç adayıdır; küçük projede sürüm sabitleme ve destek süresi avantajı sağlar. Resmî kaynak Unity 6.3 LTS desteğini Aralık 2027'ye kadar listeliyor. `6000.3.17f1` dosyasının bulunması, bunun en güncel veya bütün güvenlik düzeltmelerini içeren yama olduğunu göstermez. Proje kurulumundan önce uygun güncel 6000.3 yaması ve paket uyumu kontrol edilecek; kurulum değişikliği ayrıca kapsamlandırılacak. [Unity sürüm desteği](https://unity.com/releases/unity-6/support)

Android için seçilen Editor'e Android Build Support, SDK/NDK ve OpenJDK bileşenleri gerekir. Modül kurma, indirme alanı ve cihaz testi ayrı somut adımlar olarak ele alınacak. Bir APK'nın derlenmesi dokunmatik deneyimin gerçek cihazda doğrulandığı anlamına gelmez.

iOS proje ayarları bu Windows bilgisayarda hazırlanabilir. iOS modülüyle Xcode projesi üretmek ile imzalı iPhone uygulaması üretmek farklı adımlardır. Son derleme/imzalama için **macOS + Xcode** veya uygun bir macOS bulut derleme hizmeti gerekir. Mağaza dağıtımı ayrıca Apple Developer üyeliğine ve imzalama yetkisine bağlıdır. Şu anda bunlara erişim doğrulanmış değil. [Unity iOS ortam gereksinimleri](https://docs.unity3d.com/6000.3/Documentation/Manual/ios-environment-setup.html)

## Onay ve mevcut çalışmayı koruma

Ana metindeki talimat: “Ben onay vermeden kesin oyun tasarımını ve kodlama sürecini başlatma.” Bu nedenle bu turda araştırma ve erişim belgeleri hazırlandı. Sonraki adımın kapsamı araştırma raporunun sonunda açıklanıyor.

Konsept seçimi, ana sanat yönü, ilk prototip kapsamı, motor/paket yükseltmesi, kapsam büyütme, ücretli satın alma, dosya silme/taşıma ve mağaza yayını önemli kararlar olarak önceden sunulacak. Bir aşama açıkça onaylandığında o aşamanın rutin dosya düzenlemeleri ve hata düzeltmeleri için tekrar tekrar izin istenmeyecek.

Geliştirme başladığında önce mevcut dosyalar ve değişiklikler kontrol edilecek; kaynaklar geri dönülebilir bir sürüm kaydı veya yedekle korunacak. Sahne üreticisi kullanıcı tarafından yapılmış sahneyi habersiz yeniden oluşturmamalı. Kimlik bilgileri raporlara veya kaynak koduna yazılmamalı.

Çalışma konumu OneDrive altında. Unity'nin ürettiği Library/Temp gibi büyük geçici klasörlerin senkronizasyonu ileride değerlendirilmelidir. Bu incelemede klasör taşınmadı ve OneDrive ayarı değiştirilmedi.

## Her aşamanın teslim kanıtı

Her teknik aşama raporu şu dört bilgiyi içerecek: değişen dosyalar ve davranış; gerçekten çalıştırılan kontroller; geçen/kalan testler ve günlük yolu; sıradaki somut işlem. Ekran görüntüsü ancak gerçekten yakalandığında ekran görüntüsü olarak sunulacak. Konsept çizimi, Editor görüntüsü ve cihaz görüntüsü açıkça ayrılacak.

İlk teknik kabul testi, ilgili geliştirme aşaması onaylandığında: Unity açılışı ve lisans kontrolü → boş proje oluşturma → tek sahne üretme → C# derlemesi → PlayMode'da tek etkileşim → kayıtlı test sonucu. Bu zincir geçmeden “Unity üzerinde otomasyon çalışıyor” sonucu verilmeyecek.

Bugünkü doğrulama sonucu: **kurulum ve dosya erişimi mevcut; uçtan uca Unity otomasyonu henüz sınanmadı; Android/iOS build alınmadı.**
