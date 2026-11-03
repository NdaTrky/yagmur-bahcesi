# Yağmur Bahçesi — Android release çalıştırma planı

**Kontrol tarihi: 3 Kasım 2026.** Oyun kodu 0d2a5c9, sürüm 0.2.0 / versionCode 2. Bu belge yapılacak işleri tanımlar; yayın, hesap açılışı, production imzalama veya yeni build yapıldığı anlamına gelmez. Mevcut durum **geliştirme/test adayıdır; Play Store yayınına hazır kabul edilmemiştir**.

[Teknik referans](TEKNIK_VE_FONKSIYONEL_DOKUMANTASYON.md) · [112 test](TEST_SENARYOLARI_VE_KABUL_KRITERLERI.md) · [B01–B26 açık konular](KOD_DENETIMI_VE_ACIK_KONULAR.md)

## 1. Başlangıç durumu ve bağımlılıklar

| Konu | Bugün | Yayın için gereken |
|---|---|---|
| Hedef | Android; iOS ertelendi | Android adayına odaklan; Mac/iOS build gerekmiyor |
| Kod/Editor | Unity 6000.3.17f1, tek Garden sahnesi | Sabitlenen kaynak ve bağımlılık manifesti |
| Paket | com.raingarden.yagmurbahcesi.dev | İlk mağaza kaydından önce nihai kimlik ve yayıncı adı |
| Android çıktı | ARM64 test APK, development kapalı; X86_64 emülatör APK, development açık | Production imzalı AAB ve ondan dağıtılan APK'ların testi |
| Veri | Yerel JSON/PNG, profil hesabı yok | Göç, hata kurtarma, reset ve OS backup politikasının kabulü |
| Sunucu/veritabanı | Yok; API/DB migration yok | Bu sürüm için kurulacak oyun sunucusu veya DB yok |
| Test | Editor/Windows/Android emülatör kısmi kanıtı | Kritik regresyonlar, fiziksel ARM64, 16 KB ve AAB kabulü |
| Play hesabı | Kullanıcı hesabı olmadığını belirtti | Hesap sahibinin kayıt/kimlik ve gerekli cihaz doğrulaması |
| Telefon | Kullanıcının Android cihazı yok | Test için gerçek cihaza erişim; satın almak tek seçenek değil |
| Mağaza/yayın | Oluşturulmadı/yayınlanmadı | Beyanlar, kanallar, koşullu kapalı test, production erişimi |
| İzleme | Yerel log/JSON; uzaktan SDK yok | Play kalite takibi ve destek iş akışı |

Emülatörle geliştirme ve birçok oynanış testi yapılabilir. Gerçek cihaza erişim gerektiren doğrulama ve fiziksel donanım ölçümü ayrı işlerdir; telefon yokluğu bugünkü dokümantasyonu veya hata düzeltmeyi durdurmaz.

## 2. Kaynağı ve kabul kapsamını sabitle

1. Çalışma kopyasının değişikliklerini ve kaynak commit'ini kaydet. Kullanıcının mevcut düzenlemelerini koru; başka bir Editor aynı projeyi kullanırken batch build başlatma.
2. Yayın kapsamı: serbest bahçe, 3 mini oyun × 3 düzen, 8 tür/12 dekor/6 alan, 4 proje/Sumi, TR/EN/ayarlar, 6 anı. Günlük/haftalık, online, gerçek para, reklam veya Roblox özelliği vaat etme.
3. [Bulgu sırasını](KOD_DENETIMI_VE_ACIK_KONULAR.md) uygula. Önce kayıt/ödül/giriş, sonra UI/erişilebilirlik ve build kanıtı. Koddan çıkarılan riskleri ilgili testle tekrar üret; sonucu kaydetmeden “düzeldi” yazma.
4. Her düzeltmenin etkilediği QA testlerini çalıştır. Son adayda hızlı regresyon + tam platform kabulü yap. Yeni kod değişmedikçe yalnız süre doldurmak için aynı testleri tekrar tekrar çalıştırma.
5. Test oturumunu commit + paket hash + cihaz + tarih ile bağla. Eski Artifacts JSON'larını yeni koşunun sonucu sanmamak için koşu başına ayrı arşiv/kimlik gerekir; mevcut otomasyonun bu iyileştirmesi B22'de açıktır.

**Kapı R1:** veri kaybı/çift ödül/kalıcı giriş kilidi açık değil; hata durumları kullanıcıya görünür. Test başarısızlığını beklentiyi gevşeterek yeşile çevirmeme.

## 3. Mevcut geliştirme kontrollerini çalıştırma

Aşağıdaki komutlar **depoda mevcut**. Proje kökünde PowerShell ile çalışır. Bu belgeyi yazarken tekrar çalıştırılmadılar.

~~~powershell
.\tools\Invoke-Garden.ps1 -Action Check
.\tools\Invoke-Garden.ps1 -Action BuildWindows
.\tools\Invoke-Garden.ps1 -Action Smoke
.\tools\Invoke-Garden.ps1 -Action Smoke -Width 1280 -Height 960
.\tools\Invoke-Garden.ps1 -Action BuildAndroid
.\tools\Invoke-Garden.ps1 -Action BuildEmulator
.\tools\Test-AndroidPackage.ps1
~~~

- Check → GardenChecks.Run → GardenBuilder.Setup. **Salt okuma değildir:** proje/asset/PlayerSettings hazırlığı yapar.
- BuildWindows → Builds/Windows/YagmurBahcesi.exe. Smoke geçici profil kullanır, render için görünür test penceresi açar. Mevcut Windows kapanış çökmesi nedeniyle nonzero exit beklenebilen açık hatadır; başarısızlık raporlanmalı.
- BuildAndroid → Builds/Android/YagmurBahcesi-test.apk, ARM64, BuildOptions.None, AAB kapalı.
- BuildEmulator → Builds/Android/YagmurBahcesi-emulator.apk, X86_64, Development. Normal ARM64 ayarına finally ile döner.
- Test-AndroidPackage ARM64 APK içindir; development emülatör APK'sına veya AAB'ye başarı bekleyerek uygulanmaz.
- Android build, ASCII zorunluluğu nedeniyle Assets/Packages/ProjectSettings'i %USERPROFILE%/UnityBuilds/RainGardenAndroid altına kopyalar. Asıl proje taşınmaz. /E senkronizasyonu silinen dosyaları kaldırmadığından R2 için temiz staging/manifest doğrulaması şarttır.
- Logs altında Check/BuildWindows/BuildAndroid/BuildEmulator log'ları, Artifacts altında mevcut test çıktıları bulunur. Sabit klasörler önceki kanıtı örtebilir; koşu arşivini önceden ayır.

Ayrılmış emülatör yoksa/çalışmıyorsa görünür önizleme:

~~~powershell
.\tools\Start-AndroidPreview.ps1
~~~

Hazır RainGarden_API36 AVD'sine emülatör APK'sı kurulmuş olmalı. Kurulum gerektiğinde yalnız ayrılmış seri numarasını doğrulayarak:

~~~powershell
$gardenAdb = Join-Path $env:USERPROFILE 'UnityBuilds\AndroidSdk\platform-tools\adb.exe'
& $gardenAdb devices
& $gardenAdb -s emulator-5554 emu avd name
& $gardenAdb -s emulator-5554 install -r '.\Builds\Android\YagmurBahcesi-emulator.apk'
.\tools\Test-AndroidGarden.ps1 -Serial emulator-5554
~~~

Son komut AVD adını doğrular ve **o test uygulamasının verisini sıfırlar**, Wi-Fi/veriyi kapatır. Normal kullanıcı cihazına genel test komutu olarak taşınmaz. Önceki JSON kopyalanır fakat bu tam albüm/profil yedeği değildir. Mevcut test koordinatlarının ekranla uyumu kontrol edilmelidir.

Çıktıdaki “passed” tek başına yeterli değildir: yeni zaman damgası, doğru APK, bütün assertion'lar ve süreç exit durumu birlikte okunur. Android paket raporunun signing alanı sabit açıklama olduğundan gerçek sertifika ayrıca doğrulanmalıdır.

## 4. Production yapılandırmasını oluştur

Bu bölümdeki release yolu **henüz kodda yoktur**. Var olmayan BuildAAB komutu çalışırmış gibi sunulmamalı.

1. Nihai uygulama adı, şirket/yayıncı kimliği, paket adı ve ilk mağaza sürümünü belirle. Mevcut .dev kimliğini değiştirmek farklı uygulama/veri alanı oluşturabilir; test kayıtlarının otomatik taşınacağı varsayılmaz.
2. GardenBuilder.Settings'in her Setup'ta geliştirme değerlerini geri yazmasını düzelt. Geliştirme/test ve production ayarlarını açık, denetlenebilir bir kaynakta ayır; Check'in final ayarları bozmadığını test et.
3. Android release: IL2CPP, ARM64, development/profiler/script debugging kapalı, nihai versionName/versionCode, doğru ikon ve yatay yönler. VersionCode her yeni mağaza adayı için uygun biçimde artmalı; kullanılan kod tekrar yüklenebilir varsayılmamalı.
4. Minimum API26 mevcut ürün sınırı; fiziksel/uygun ortamda doğrulanmalı. Yeni telefon/tablet uygulamasının Play gönderimi için **target API36 veya üstü** gerekir. 31 Ağustos 2026 başlangıcı ve 1 Kasım 2026 uzatma tarihi bugün geride kaldı. [Google Play hedef API politikası](https://support.google.com/googleplay/android-developer/answer/11926878?hl=en-AU)
5. Android manifestini final build'den çıkar: tüm izinler, exported bileşenler, debuggable, package/sürüm ve backup ayarları. Mevcut izin filtresi bütün olası izni taramaz.
6. JSON/PNG için OS yedekleme ve cihaz taşıma politikasını açıkça seç. Mevcut APK'da açık backup kuralları bulunmaması “yedekleme yok” anlamına gelmez. Android yedek davranışı uygulama/backend ayrımından bağımsızdır; cihazda doğrulanmalı. [Android Auto Backup](https://developer.android.com/identity/data/autobackup)
7. Destek ve gizlilik metnini gerçek yayıncı, veri kapsamı, reset/albüm/yedek davranışıyla tamamla. Kodda bulunmayan hesap silme/online servis akışı eklenmiş sayılmaz.
8. Smoke/test kodunun release erişimini gözden geçir. Şu an Android release'te --garden-smoke koşulu Debug.isDebugBuild false nedeniyle etkin değil; test sınıflarının kaynakta bulunması ayrıca envanterlenmeli.
9. Kaynak/Unity/paket/SDK/NDK sürümleri, final ayarlar ve build log'unu aday manifestine ekle. Yalnız “aynı projeden aldık” izlenebilirlik sağlamaz.

**Kapı R2:** geliştirme ayarlarına geri dönmeyen, temiz staging'den tekrarlanabilir production konfigürasyonu; kimlik ve versionCode doğrulaması.

## 5. İmzalama ve AAB

1. Upload key ve güvenli yedek/kurtarma sorumluluğunu belirle. Keystore ve parolalar depoya, bu dokümana, terminal çıktısına veya sohbete yazılmaz.
2. Unity Android Publishing Settings veya hazırlanacak release build yolu üzerinden upload key'i kullan; development kapalı ve buildAppBundle=true olacak gerçek AAB üretimi ekle.
3. Play App Signing yapılandırmasını tamamla. AAB yükleme anahtarıyla imzalanır; kullanıcılara dağıtılan uygulama imzası Play App Signing tarafından yönetilir. Upload certificate ile app-signing certificate aynı olmak zorunda değildir. [Android uygulama imzalama](https://developer.android.com/studio/publish/app-signing)
4. AAB'nin hash'ini, upload sertifikası özetini ve versionCode'u kaydet. Play'in oluşturduğu/cihaza dağıtılan APK'ların uygulama imzasını ve split içeriğini ayrıca doğrula.
5. Yerel test APK'sı için gerçek sertifika denetimi örneği:

~~~powershell
$gardenBuildTools = 'C:\Program Files\Unity\Hub\Editor\6000.3.17f1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\build-tools\36.0.0'
& (Join-Path $gardenBuildTools 'apksigner.bat') verify --verbose --print-certs '.\Builds\Android\YagmurBahcesi-test.apk'
Get-FileHash -LiteralPath '.\Builds\Android\YagmurBahcesi-test.apk' -Algorithm SHA256
~~~

Bu komut **APK** doğrular; AAB için doğrudan apksigner kullanılmaz. Yerel debug imzalı APK'nin production sertifikasıyla aynı imzaya sahip olduğu varsayılmaz. Farklı imzayla güncelleme kurulamıyorsa “çözüm” diye oyuncu verisi silinmez; T108 için uygun imzalı test zinciri gerekir.

6. Native sembol çıktıları ve kaynak eşlemesini sakla; Play native crash çözümlemesi için uygun sembol dosyalarını adayla ilişkilendir.
7. AAB'den üretilen split kurulumda bütün sanat/ses/font, JSON göçü ve albüm akışını doğrula. Şu an AAB çıktısı/test sonucu yoktur.

**Kapı R3:** doğru imzalı gerçek AAB, değişmeyen kaynak/ayar/hash kaydı ve T106–T108.

## 6. Cihaz, performans ve son regresyon

| Test katmanı | Asgari kapsam | Kabul kanıtı |
|---|---|---|
| Kural | 9 mini oyun düzeni; claim/id; fiyat/sahiplik; bütün 12 keşif; v1/v2/future | Güncel assertion + fixture + commit |
| Gerçek UI | 3 türün bütün varyantları; yanlış girdi/yardım/geri dönüş/çoklu dokunma | Cihaz/ekran kaydı + state önce/sonra |
| Yaşam döngüsü | Arka plan, odak kaybı, kilit, süreç sonu, yükseltme | Yeniden açılış sonucu + log |
| Veri hatası | Yazma/backup/rename hatası, bozuk dosya, tamamlanmış-ödülsüz run | İzole hata enjeksiyonu + kurtarma kanıtı |
| Cihaz | Düşük/orta ARM64, güncel telefon, tablet; API26/API36; çentik/oran | Cihaz kimliği, OS, ABI, sayfa boyutu |
| Performans | İlk açılış, 30 dk karışık oturum, albüm/efekt yükü | Frame time, RAM, ısı ve süre ölçümleri |
| Erişilebilirlik/yaş | TR/EN, ses, azaltılmış hareket, küçük hedefler, yönerge anlaşılabilirliği | Gözlem notları ve sorun çözümü |
| Final dağıtım | Play kanalından kurulum + güncelleme | AAB/split ilişkisi, imza, versionCode |

Mevcut dosya hizalama kontrolü altı ARM64 .so ve APK için başarılıdır. **16 KB native çalışma testi yapılmadı.** API35+ hedefli 64-bit uygulamalar için 16 KB desteği gereklidir; resmî sayfa ayrıca güncellemeler için 1 Şubat 2027 yaptırım tarihini belirtmektedir. Eski tarihleri tek bağlamdan alıp bugünkü yeni uygulama için varsayım yapma; adayın gerçek 16 KB ortamda çalışması kabul şartıdır. [Android 16 KB rehberi](https://developer.android.com/guide/practices/page-sizes)

Performans için depodaki hedef en az 30 FPS, tercih 60. İlk açılış süresi, RAM ve ısı için ölçülmüş bütçe henüz yok; ilk fiziksel cihaz oturumundan sonra açık eşikler belirlenmeli. targetFrameRate=60 bir ölçüm değildir.

**Kapı R4:** kritik testler geçti; açık P1 yok; ölçülmemiş alan “geçti” yazılmadı. Windows sorunu ayrı takipte; Android odaklı yayın Windows raporunu geçmişe çevirmez.

## 7. Mağaza ve hesap hazırlığı

Hesap türü henüz seçilmedi. Gerçek kişi/kuruluş durumuna uygun hesap açılmalı; bir test şartını atlamak için yanlış kuruluş beyanı yapılmaz.

- Yeni kişisel hesaplarda gerçek, root edilmemiş **Android 10+** cihaza erişim Play Console mobil uygulamasıyla doğrulanır. Emülatör bu şartın yerine geçmez; uygun ödünç cihaz kullanılabilir. Kimlik ve oturum adımı hesap sahibi tarafından tamamlanır. [Google cihaz doğrulaması](https://support.google.com/googleplay/android-developer/answer/14316361?hl=en)
- 13 Kasım 2023 sonrası oluşturulmuş kişisel hesaplar için en az **12 testçinin kesintisiz en az 14 gün katıldığı kapalı test** ve ardından production erişim başvurusu gerekir. Sayı/gün tamamlanması otomatik yayın onayı değildir; iç test bu kapalı testin yerine geçmez. [Google kişisel hesap test koşulları](https://support.google.com/googleplay/android-developer/answer/14151465?hl=en-GB)
- Çocukları hedefleyen ürünün yaş grubu, içerik derecelendirmesi ve gizlilik beyanı gerçek hedef/işlevle uyumlu olmalı. Mevcut 4–6 tasarım hedefi mağaza formundaki seçeneklere doğru çevrilmeli; matematik sorusu yasal onay veya yaş doğrulaması olarak sunulmamalı. [Google Play Families politikası](https://support.google.com/googleplay/android-developer/answer/9893335?hl=en)
- Data Safety ve gizlilik politikası final binary'nin veri/SDK davranışıyla doldurulmalı. Reklam/analitik olmaması beyan işini ortadan kaldırmaz; OS backup ve yerel anı kapsamı açıklanmalı. Henüz “mağaza tarafından onaylandı” durumu yoktur. [Google Data Safety rehberi](https://support.google.com/googleplay/android-developer/answer/10787469?hl=en)

Hazırlanacak mağaza paketi:

| Teslim | İçerik / kontrol |
|---|---|
| Kimlik | Nihai ad, paket, yayıncı ve destek adresi; marka uygunluk araştırması tamamlanmalı |
| Açıklama | Uygulanmış çevrimdışı mekanikler; gerçek para/online/günlük görev vaadi yok |
| Görseller | Gerçek final build ekranları, ikon ve Play Console'un istediği güncel görsel alanları |
| Gizlilik | Herkese erişilebilir uygun sayfa; oyun içi bilgiyle aynı veri/backup/reset açıklaması |
| İçerik beyanı | Hedef yaş, içerik derecelendirmesi, reklam/uygulama erişimi ve ilgili form yanıtları |
| Lisans | Nunito OFL, üretilmiş sanat prompt/varlık kayıtları, kodla üretilmiş ses envanteri |
| Sürüm notu | Gerçek değişiklikler, bilinen sınırlar, versionCode ile eşleşme |
| QA paketi | Test matrisi sonuçları, açık sorun kararı, cihaz/log/hash listesi |

Bu oyun için veritabanı, sunucu, DNS API dağıtımı, secret API token veya sunucu ölçekleme adımı yoktur. Gizlilik sayfası/destek adresi ayrı yayıncı hizmetidir; oyun backend'i kurulduğu anlamına gelmez.

**Kapı R5:** hesap/cihaz doğrulaması, gerekli kapalı test, gerçek beyanlar ve production erişimi tamam. Kullanıcının şu an hesabı ve telefonu olmaması bu kapının açık bağımlılığıdır.

## 8. Dağıtım sırası

1. **Yerel aday:** R1–R4 sonuçlarını ve AAB'yi dondur; yalnız bilinen/izlenebilir aday yüklenir.
2. **İç test:** uygulama kaydını ve Play App Signing'i yapılandır; AAB'yi yükle; Play üzerinden temiz kurulum/güncelleme ile T107–T108'i çalıştır.
3. **Kapalı test:** hesap türünün şartlarına göre aktif testçilerle senaryoları paylaş; geri bildirimleri ve gerçek kullanım/test düzeltmelerini kaydet. Sadece katılım listesi QA değildir.
4. **İnceleme:** Console'un ön test/uyumluluk raporlarını ve içerik beyanlarını kontrol et; hata varsa düzelt, daha yüksek uygun versionCode ile yeni aday üret ve etkilenen testleri tekrarla.
5. **Production erişimi ve gönderim:** gerekiyorsa test sürecini açıklayarak başvur. Mağaza onayı, build yüklemekten ayrı dış bağımlılıktır.
6. **Yayın:** ürünün nihai mağaza içeriği, ülke/dağıtım kapsamı, fiyatlandırma ve aday sürümü somut olarak gözden geçirilir; hesap sahibinin yetkilendirdiği kapsamda gönderilir. Bu belge aşamasında gönderim yapılmadı; hazırlanmamış ayrıntılar için erken onay istenmiyor.
7. **Yayın sonrası doğrulama:** mağazadan farklı cihazda indir; versionCode/imza, yeni profil, oyun/ödül/dikim/albüm ve güncelleme kayıtlarını kontrol et. Konsolda “yayında” görünmesi bütün kullanıcı akışlarının geçtiği anlamına gelmez.

Açık test veya kademeli dağıtımın her hesap/ilk yayın için aynı anda kullanılabildiği varsayılmaz; ilgili kanalda Console'un sunduğu seçenekler ve hesap erişimi esas alınır.

## 9. İzleme, destek ve düzeltme dağıtımı

Mevcut kodda uzaktan analiz veya crash servisi bulunmuyor. Bugün elde edilen Player.log, logcat ve yerel test JSON'ları otomatik oyuncu telemetrisi değildir.

Yayın sonrasında ilk hafta günlük, sonra düzenli olarak:

1. Play Console'da sürüm ve cihaz kırılımında kullanıcıya yansıyan crash/ANR ve performans göstergelerini incele. Veriler kullanıcı/cihaz paylaşım koşullarına ve gecikmeye bağlıdır; “hiç olay yok” sıfır hata kanıtı değildir. [Android vitals](https://developer.android.com/google/play/vitals)
2. Destek ve testçi bildirimlerini Txxx/Bxx kaydıyla ilişkilendir. Gerekli bilgiler: sürüm, cihaz/OS, işlem sırası, beklenen/gerçek sonuç; çocuk kimliği veya gereksiz kişisel dosya isteme.
3. Yerel kayıt kaybı/çift ödül/kalıcı kilit gibi kritik bulguda yeni dağıtımı durdurma veya kapsamını sınırlama kararını ver; Console'da mümkün olan işlem ilgili yayın durumuna bağlıdır.
4. Önceki iyi kaynağa dönmek cihazda otomatik veri geri alma değildir. Hatalı kaydın korunması/göçüyle uyumlu düzeltme üret; aynı paket/imza ve daha yüksek versionCode ile yeni sürüm yayımla.
5. Hatanın testini ekle, ilgili alan ve hızlı regresyonu çalıştır, düzeltmeyi önce test kanalında doğrula. Kayıt şemasını geriye uyumsuz biçimde düşürme.
6. Haricî crash/analytics SDK'sı ileride eklenirse bu yeni uygulama kapsamıdır; SDK/izin/veri beyanı ve çocuk kullanıcı etkisi yeniden değerlendirilir. Bu sürümde kurulmuş gibi raporlanmaz.

## 10. Yayın kararı için son kayıt

| Kapı | 3 Kasım durumu | Kapanış için somut teslim |
|---|---|---|
| R1 Oynanış/veri hataları | Açık | B02–B16 ilgili düzeltme/tekrar testi ve sonuçları |
| R2 Production/build kaynağı | Açık | Ayar ayrımı, temiz staging, aday manifesti |
| R3 AAB/imza | Açık | İmzalı AAB + gerçek sertifika/kurulum kontrolü |
| R4 Cihaz/QA | Kısmi | 112 senaryonun uygulanabilir kapsamı + fiziksel/16 KB kanıtı |
| R5 Hesap/mağaza | Açık | Hesap doğrulamaları, beyanlar, gerekli test ve erişim |
| R6 Yayın sonrası | Başlamadı | Mağaza kurulumu, izleme ve düzeltme sorumluluğu |

Release klasörüne kaynak commit, kullanılan araç sürümleri, build ayar özeti, AAB/APK hash'leri, sertifika özetleri, test sonuç CSV'si, ilgili log/semboller, açık sorun kararları ve mağaza sürüm notu birlikte konmalıdır. Özel imzalama anahtarı ve parola bu klasörün paylaşılabilir içeriğine dahil edilmez.
