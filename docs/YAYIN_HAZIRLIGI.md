# Android yayın hazırlığı

2 Kasım 2026. Durum: geliştirme; Google Play'e yükleme veya genel yayın yapılmadı. iOS kullanıcının talimatıyla ertelendi.

## Ürün ve mağaza kimliği

Türkçe çalışma adı: Yağmur Bahçesi. İngilizce çalışma adı: Rain Garden. İlk mağaza taramasında **Rain Garden: Water Puzzle** bulundu; tek başına Rain Garden adı ayırt edicilik açısından uygun görünmüyor. [Google Play'deki ürün](https://play.google.com/store/apps/details?id=com.nasr.raingarden)

Konsept korunarak maskot/marka ekli bir uluslararası ad seçilmeli; örneğin Mina's Rain Garden yalnız bir adayıdır, müsaitlik garantisi değildir. TÜRKPATENT/WIPO/EUIPO ve hedef ülkelerde benzer marka taraması tamamlanmadı. Logo, kesin mağaza ikonu ve son paket kimliği bu karardan sonra sabitlenmeli.

Geliştirme paket kimliği `com.raingarden.yagmurbahcesi.dev`. Bu bir yayıncı alan adı veya mağaza uygulaması sahipliği iddiası değildir. Nihai paket kimliğini değiştirmek Play'deki uygulama kaydını etkileyebileceği için ilk yüklemeden önce seçilmelidir.

## Teknik gereklilikler

Yeni Android uygulamalarının Google Play'e gönderiminde güncel hedef **Android 16 / API 36**. Resmî sayfa 31 Ağustos 2026 başlangıcını ve 1 Kasım 2026'ya kadar uzatma seçeneğini listeliyor. Bugün 2 Kasım 2026; yeni ürün için API 35 hedefiyle plan yapılmıyor. [Google hedef API gerekliliği](https://support.google.com/googleplay/android-developer/answer/11926878?hl=en)

ARM64 APK'nın altı yerel kütüphanesinin ELF LOAD segmentleri 16.384 bayt hizalı; `zipalign -c -P 16 -v 4` kontrolü geçti. Sonuçlar `Artifacts/Android-Package` altında. Bu statik paket kontrolüdür; 16 KB sayfa kullanan cihazdaki çalışma testi henüz yapılmadı. Mevcut emülatör 4 KB sayfa kullanıyor. [Android sayfa boyutu belgesi](https://developer.android.com/guide/practices/page-sizes)

Test APK'sı ile mağaza release AAB farklı çıktılardır. Geliştirme işaretleri, test komutları ve geçici yayıncı metinleri release sürümünden çıkarılmalı. Release imzası için upload key güvenli yerelde tutulur; parola veya özel anahtar sohbet, Git ya da günlük dosyasına yazılmaz. Henüz release anahtarı oluşturulmadı.

## Hesap ve dış bağımlılıklar

- Kullanıcı Google Play Console hesabının henüz olmadığını bildirdi; hesap açılması gerekiyor.
- Yayıncı adı, destek e-postası, gizlilik sayfasının adresi, şirket/şahıs bilgileri ve fiyat kararı bekliyor.
- Kullanıcının Android cihazı yok. Yerel Android 16 x86_64 emülatörü kuruldu; fiziksel cihaz testi hâlâ bekliyor.
- Mağaza yüklemesi için erişim yöntemi veya yetkili Play Developer API bağlantısı henüz yok.

13 Kasım 2023 sonrasında açılan kişisel hesaplarda, üretim erişimi başvurusu öncesinde en az **12 test kullanıcısının kesintisiz 14 gün katıldığı kapalı test** gerekiyor. Geliştirici kendi otomatik testlerini çalıştırarak bu süreyi veya insan katılımını yerine getirmiş sayılmaz. Hesap durumuna göre uygulanabilirliği kontrol edilir. [Google test gereklilikleri](https://support.google.com/googleplay/android-developer/answer/14151465?hl=en)

Yeni kişisel hesaplarda Play Console mobil uygulamasıyla **gerçek Android cihaz erişimi doğrulaması** da gerekiyor. Android 10 veya üstü, root yapılmamış bir fiziksel cihaz kullanılmalı; emülatör bu gerekliliği karşılamaz. Geçici olarak ödünç alınan uygun cihazla doğrulama yapılabilir; Google cihazı sürekli elde tutma şartı belirtmiyor. [Google cihaz doğrulama koşulları](https://support.google.com/googleplay/android-developer/answer/14316361?hl=en)

Mevcut ARM64 test APK'sının development modu kapalı; internet, kamera, mikrofon, konum ve harici depolama izni manifestte yok. Yalnız AndroidX'in uygulamaya özel imza seviyeli receiver izni var. Emülatör APK'sında development modu açık olduğu için Unity Player Connection ağ izni/yerel keşif trafiği bulunabilir; bu dosya mağaza adayı değildir. İki APK da otomatik yerel Android debug anahtarıyla imzalıdır; yayıncının upload key'i oluşturulmadı.

## Türkçe mağaza metni taslağı

Kısa açıklama: Hayvan dostlarınla çiçek büyüt, yaprak yüzdür ve kendi pikniğini kur.

Tam açıklama:

Küçük bir bulut, meraklı bir tavşan ve keşfedilecek bir bahçe… Yağmur Bahçesi'nde çocuklar kendi hızlarında oynar. Yağmuru toprağa getirir, filizleri büyütür, yaprak tekneyi gölete bırakır ve hayvan dostlarıyla piknik kurarlar.

Dokunarak ya da sürükleyerek oyna. Çiçeklerin rengini değiştir, küçük süsleri taşı ve bir sonraki ziyaretinde bahçeni kaldığın gibi bul. Kazanma veya kaybetme baskısı yok; keşif ve hayal gücü var.

4–6 yaş için tasarlanan oyun Türkçe ve İngilizce arayüz sunar. Temel oyun ilk indirmeden sonra çevrimdışı çalışır. Reklam, sohbet, günlük giriş ödülü ve oyun içinde satın alma ekranı içermez. Yetişkin alanında ses, dil ve hareket ayarlarını yönetebilirsiniz.

İlerleme cihazda saklanır. Uygulamayı kaldırmak yerel kaydı silebilir. Bu oyun eğitimsel başarı veya terapi sonucu vaat etmez.

Bu metin yayın taslağıdır; gerçek cihaz kontrolleri ve nihai kapsam doğrulandıktan sonra kullanılmalıdır.

## English store copy draft

Short description: Grow flowers, sail a little leaf and share a picnic with animal friends.

A little cloud, a curious rabbit, and a garden waiting to be discovered. In Rain Garden, children play at their own pace. Bring rain to the soil, help a sprout bloom, send a leaf boat across the pond, and share a picnic with four little friends.

Tap or drag to explore. Change flower colors, move small decorations, and return to your saved garden. There are no timers, scores or daily rewards.

Designed for ages 4–6, with Turkish and English interfaces. The core game works offline after installation. No ads, chat or in-game purchase screens. A grown-ups area includes sound, language and reduced-motion settings.

Progress stays on the device and may be removed when the app is uninstalled. No learning or therapeutic outcomes are promised.

The final English brand name is pending. This copy is a draft, not a published listing.

## Gizlilik metninin tamamlanması

Ürünün çekirdeği hesap, reklam/analitik SDK'sı ve kişisel bilgi talebi olmadan tasarlandı. Nihai metin; yayıncı ve iletişim kimliği, uygulamanın gerçek veri akışı, destek mesajlarının işlenmesi, mağaza ödeme süreci, saklama/silme, hedef ülke hükümleri ve yürürlük tarihi içermeli. Placeholder iletişim adresiyle yayın yapılmaz.

Android manifesti ve ağ davranışı denetlenince Data safety beyanı buna göre hazırlanır. Ücretsiz bir privacy-policy üreticisinin varsayımlarını doğrulamadan kopyalamak yeterli değildir. COPPA/GDPR/KVKK ve Families kaynakları araştırma raporunda bulunur; burada kesin hukuki uygunluk beyanı yoktur.

## Yayın kapıları

1. Oyun derleniyor; kurallar ve kayıt testleri geçiyor.
2. Android APK gerçek cihazlarda, uçak modunda ve kesinti/geri dönüş durumlarında çalışıyor.
3. Çocuk/ebeveyn oturumlarından çıkan kritik kullanılabilirlik sorunları düzeltiliyor.
4. Nihai ad, paket kimliği, yayıncı/destek/gizlilik adresi ve fiyat sabitleniyor.
5. İmzalı release AAB, manifest ve 16 KB uyumu kontrol ediliyor.
6. İç test/kapalı test ve hesap gereklilikleri tamamlanıyor.
7. Gerçek oyun görüntüleriyle mağaza görselleri, içerik derecelendirmesi, Families ve Data safety hazırlanıyor.
8. Mağaza incelemesi tamamlanınca üretim yayını yapılıyor; sonrasında hata/geri bildirim izleniyor.

Kullanıcının yayın yetkisi mevcut. Yeniden genel izin istemek yerine eksik hesap/kimlik/işletme bilgileri somut olarak istenir. Bu kapılardan geçilmeden “canlıya alındı” denmez.
