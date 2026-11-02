# Yağmur Bahçesi test planı

2 Kasım 2026 · Bu belge yapılacak kontrolleri tanımlar; yapılmış test kanıtları test JSON'ları ve teslim raporundadır.

## Otomatik kontroller

0.2 için `GardenChecks` v1 göçü ve özgün dosyanın korunması, yeni kayıt sürümüne yazmayı reddetme, kopya işlemle bakiye koruma, yetersiz bakiye, ikinci kez ücret kesmeme, dokuz etkinlik düzeninin çözülebilirliği ve yeniden yükleme sonrası tekil ödülü sınar. `GardenSmoke` üç mini oyunu gerçek UI callback'leriyle tamamlar, dikim/dekor/projeler/Sumi/albümü kontrol eder. `Test-AndroidGarden.ps1` aynı döngünün Android giriş ve süreç yeniden başlatma kısmını doğrular.

Windows işlev raporu ile süreç çıkışı ayrı kabul koşullarıdır. `runtime-tests.json` başarılı olsa bile `process-exit.json` başarısızsa Windows testi bütün olarak geçmez. 0.2'de saptanan yerel Unity kapanış çökmesi açık hatadır; ayrıntılar geliştirme raporundadır.

Unity menüsü: `Rain Garden > Prepare project and scene`; komut satırı kontrolleri: `GardenChecks.Run`. Kural, sınır değerleri, yerel kayıt, yedekten kurtarma ve sahne referansları birlikte denetlenir. `GardenSmoke` yalnız `--garden-smoke` argümanıyla çalışan yerel test senaryosudur ve ayrı geçici kayıt kullanır; oyuncunun gerçek kaydına dokunmaz.

Bir testin başarısızlığı sonraki ticari sürüme taşınmaz. Testin beklentisi yanlışsa neden belgelenir; yalnız yeşil sonuç için davranış gevşetilmez.

## Gerçek Android cihaz matrisi

En az bir düşük/orta sınıf ARM64 Android telefon, bir güncel Android telefon ve bir tablet. Android 8 alt sınırı ancak gerçek cihaz/emülatör desteği kontrol edilince ilan edilir. 16:9, uzun ekran, 16:10 tablet ve 4:3'e yakın yerleşimler karşılaştırılır. Çentik, hareket çubuğu ve ekran yönü değişimi UI öğelerini örtmemeli.

Her cihazda: temiz kurulum; ilk açılış; uçak modunda yeniden açılış; üç etkileşim alanı; ses kapalı; ikinci parmak; sürüklerken bildirim/arka plan; geri tuşu; uygulamayı zorla kapatıp yeniden açma; kaydın korunması; sıfırlamayı iptal etme; doğrulanmış sıfırlama; Türkçe/İngilizce; azaltılmış hareket.

Performans: açılıştan etkileşime süre, APK indirme/kurulum boyutu, ilk ve 15. dakika kare süreleri, bellek tepe değeri, ısı ve ses seviyesi. Minimum hedef 30 FPS, tercih 60 FPS. Ölçülmemiş sayılar “geçti” olarak yazılmaz.

## Çocuk/ebeveyn oturumları

8 çocukla küçük ilk tur; 4, 5 ve 6 yaşlar dengeli temsil edilmeye çalışılır. Ebeveyn izin ve gözetimi gerekir. Çocuk istemezse veya bırakmak isterse oturum durur. Çocuğun ismi, fotoğrafı, sesi veya tanımlayıcı ayrıntıları zorunlu değildir; C01 gibi anonim notlar yeterlidir. Kayıt alınacaksa ayrı açık izin ve silme planı gerekir.

İlk 30 saniye: en az 6/8 çocuğun yetişkin adımı olmadan anlamlı etkileşim bulması ilk iç hedef. İlk beş dakika: en az 6/8'in üç etkileşimden en az ikisini bağımsız bulması; üçüncünün neden bulunmadığı ayrıca not edilir. Bu eşik araştırma belgesindeki üç etkileşim hedefini gevşeterek geçer not vermek için değil, ayrı “kısmi öğrenme” metriği olarak tutulur; üç etkileşim hedefi de ayrıca raporlanır.

Dokunmada yanlış hedefler, sürüklemeyi erken bırakma, metni okumaya ihtiyaç ve ses kapalı kullanım gözlenir. Çocuğa çözümü göstermeden önce ne denediği kaydedilir. Bir başka gün tarafsız davette yeniden oynamak isteyip istemediği ve farklı bir oyun kurup kurmadığı not edilir. Ödülle veya ısrarla geri dönüş ölçülmez.

Ebeveyn: güven, görsel kalite, kapsamı anlama, tek ödeme tercihi, fiyat karşılaştırması, sesi rahatsız edici bulma, oyundan ayrılma kolaylığı. Satın alma sözü gerçek satış sayılmaz. İki kültür/dil pazarının sonucu birbirine otomatik genellenmez.

## Yayına engel sorunlar

- Çökme, açılmama, kayıt kaybı veya başka kullanıcı kaydına test müdahalesi.
- Kullanıcının iptal ettiği resetin uygulanması.
- Ebeveyn kapısının yanlışlıkla kolay açılması veya dünya etkileşiminin modal arkasına geçmesi.
- Görünmeyen/erişilemeyen zorunlu kontrol ve sürekli başarısız sürükleme.
- Beklenmeyen reklam, izleme SDK'sı, kişisel veri veya gereksiz izin.
- Mağaza kimliği, yayıncı bilgisi, gizlilik adresi, fiyat ve imzalama bilgilerinin eksikliği.

## Gözlem kayıt şablonu

Tarih / anonim oturum kodu / yaş / cihaz ve Android sürümü / dil / ilk etkileşime süre / bağımsız keşfedilen alanlar / dokunma hataları / yardım gereksinimi / bırakma davranışı / ebeveyn yorumu / sorun önceliği / düzeltme ve tekrar kontrolü.

Durum alanları: yapılmadı, geçti, kaldı, uygulanamaz. Teknik test ile kullanıcı testi ayrı raporlanır.
