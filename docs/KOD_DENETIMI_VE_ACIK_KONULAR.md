# Yağmur Bahçesi — kod denetimi ve açık konular

**3 Kasım 2026 · incelenen kaynak 0d2a5c9 · 0.2.0.** Bu kayıt hata düzeltme raporu değildir. İnceleme sırasında oyun kodu değiştirilmedi, oyun testleri yeniden çalıştırılmadı. Kaynak davranışı, geçmiş çalıştırma kanıtı ve yapılması gereken doğrulama ayrıdır.

[Teknik referans](TEKNIK_VE_FONKSIYONEL_DOKUMANTASYON.md) · [Test matrisi](TEST_SENARYOLARI_VE_KABUL_KRITERLERI.md) · [Release planı](RELEASE_CALISTIRMA_PLANI.md)

## 1. İstenen dört durum sınıfı

### Mevcut ve çalıştığı belirli kapsamda doğrulanmış

| Özellik | Kanıt / sınır |
|---|---|
| Serbest bahçe: sulama, büyüme, tekne, piknik, dekor sürükleme | Editor + Windows işlev smoke + Android temel akış; her araç/nesne birleşimi değil |
| Türkçe/İngilizce, ses, ana ekran, modalın dünyayı engellemesi | Windows/Android kısmi UI kontrolü; bütün metin ve dinamik ekran boyutu değil |
| Üç mini oyun ve birer pul ödül | Android ilk düzenler; Windows callback'ler; bütün 9 düzen Editor kural/yardım seviyesinde |
| Aynı run için ödül tekrarını reddetme | Editor, Windows ve Android temel örnek |
| Kayıtlı yarım mini oyun, tür değiştirince ilerleme koruma | Windows/Android örnekleri; bütün yaşam döngüsü kesinti noktaları değil |
| Bluebell satın alma, dikme/büyütme, ücretsiz sahiplik kullanımı | Windows/Android örneği; bütün katalog kombinasyonları değil |
| Dekor satın alma/toggle, dört proje, Sumi | Windows smoke; Android'de bütün Sumi davranışları doğrulanmış değil |
| Albüme PNG kaydetme | Windows/Android bir görüntü; 6 dosya sınırı, bozuk PNG ve erişim hatası değil |
| v1 göçü, yedek, yeni sürüm dosyasını koruma, Validate sınırları | Editor belirli fixture'lar; dosya sistemi kesintisi/kapsamlı hata enjeksiyonu değil |
| ARM64 APK üretimi, manifest ve 16 KB dosya hizalaması | Statik paket raporu; fiziksel ARM64/16 KB runtime/AAB/mağaza kabulü değil |

“Çalışan” ifadesi yalnız belirtilen kanıt kapsamındadır. [Windows işlev raporu](../Artifacts/Smoke-1600x900/runtime-tests.json) ile [başarısız süreç çıkışı](../Artifacts/Smoke-1600x900/process-exit.json) birlikte okunmalıdır.

### Geliştirilmiş ancak tamamen test edilmemiş

- Bütün 8 türün davranışları, 12 dekorun özel tepkileri, 6 alan × 8 tür × 3 tint birleşimleri.
- Üç mini oyunun bütün varyantlarının gerçek parmakla tamamlanması; her konumdan yardım; çoklu dokunma ve modal/sürükleme kesintileri.
- Bütün keşiflerin sıra dışı tamamlanması, tamamlanmış kart yönlendirmesi, Sumi'yle bütün etkileşimler.
- Disk doluluğu/erişim engeli/kesinti, bozuk ama yapısal olarak geçerli kayıt, resetin tüm kapsamı.
- Altı fotoğraflı albüm budaması, hatalı resim, saat değişimi, uzun oturum belleği.
- API26 cihaz, fiziksel ARM64, 16 KB runtime, çentik ve dinamik ekran boyutu, çocuk/aile kullanılabilirliği.
- Azaltılmış hareketin her efektteki sonucu ve küçük ekran dokunma hedefleri.

### Eksik veya tamamlanmamış

Aşağıdaki B01–B26 kaydı kaynak kusurlarını, doğrulama açıklarını ve release bağımlılıklarını ayrı tanımlar. En önemli tamamlanmamış işler: kayıt hata UX'i ve veri güvenliği sınırları; giriş/modal regresyonları; üretim AAB/imza hattı; cihaz kabulü; gerçek yayıncı/mağaza/gizlilik bilgisi ve Play hesabı süreci.

### Henüz geliştirilmemiş öneriler

Günlük/haftalık görev, seri giriş bonusu, takvimli etkinlik, XP/seviye, çevrimiçi arkadaş/ziyaret, chat, liderlik tablosu, kullanıcı hesabı, bulut kayıt, sunucudan içerik, uzaktan yapılandırma, push bildirimi, reklam, gerçek para satın alması, abonelik, oyuncu analitiği ve haricî crash SDK'sı yoktur. Bunların eksikliği mevcut çevrimdışı oyunun bir kural hatası değildir; yeni ürün kapsamıdır.

Roblox için yalnız [uyarlama planı](ROBLOX_UYARLAMA_PLANI.md) bulunur; Luau deneyimi/Roblox sunucusu yayınlanmış değildir. iOS kullanıcı kararıyla ertelenmiştir. Öneri belgelerindeki özellikler bu sürümün varlık listesine eklenmemelidir.

## 2. Öncelik ve kanıt tanımları

- **P1:** veri kaybı/ödül tutarsızlığı/kalıcı etkileşim engeli ihtimali veya ilgili platformda yayın kapısı. Kaynak çıkarımı olan P1 önce tekrar testiyle doğrulanır; sessizce kapatılmaz.
- **P2:** işlev, erişilebilirlik veya anlaşılabilirlik sorunu; release öncesi düzeltme ya da açık gerekçeli kapsam kararı gerekir.
- **P3:** uç sınır, bakım veya ileride genişletme riski; kayıtlı takip gerekir.
- **Çalıştırma:** önceki gerçek çıktı destekli. **Kod:** mevcut dal/koşul kaynakta görülebilir. **Çıkarım:** kaynak olası hatayı gösterir, cihazda tekrar edilmedi. **Eksik:** özellik/kanıt/operasyon adımı henüz yok.

Sahip ataması yapılmadı. Aşağıdaki her bulgu bu inceleme sonunda **açık** durumdadır.

## 3. Bulgular

### B01 — Windows normal kapanışında native çökme

**P1 / Windows · Çalıştırma.** İki oranda 29 işlev kontrolü geçmesine rağmen süreç -1073741819 ile kapanıyor. [1600×900 çıkış](../Artifacts/Smoke-1600x900/process-exit.json), [1280×960 çıkış](../Artifacts/Smoke-1280x960/process-exit.json), [geliştirme raporu](GELISTIRME_DURUMU.md). Önceki araştırmadaki erişilebilirlik/native cleanup benzerliği kesin kök neden olarak kabul edilmez. Ayrı Unity sürümü denemesi çözüm kanıtı üretmemiştir.

**Etki:** masaüstü önizleme temiz kapanmıyor; Windows testinin tamamı başarısız. Android'deki mevcut akışta aynı hata gözlenmedi; Android'e otomatik olarak taşındığı da yok sayıldığı da söylenemez. **Kapanış:** T105 hem işlev hem exit code ile geçmeli; Android T097–T104 ayrı kalır.

### B02 — Kayıt hatasının görünmemesi ve iki farklı bellek davranışı

**P1 · Kod/çıkarım.** [GardenGame.Save, Water, Sunshine, StartGarden, ResetGarden](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenGame.cs) doğrudan state'i değiştirip Write çağırır. Save hata hint'i üretse bile çağıran başarı hint'iyle örtebilir. ResetGarden yazma başarısızlığında kullanıcıya mesaj vermeden döner. [Transaction](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenProgression.cs) ise klonu önce yazar, sonra State atar; aynı hatada satın alma/mini oyun ilerlemesi geri kalır. Modalın arkasındaki hint görünmeyebilir.

**Etki:** sulama/ayar çalışmış görünür fakat yeniden açılışta kaybolabilir; oyuncu gerçek kayıt durumunu anlayamaz. **Düzeltme hedefi:** bütün yazmaların başarı sonucu UI'ya taşınmalı; hatanın üstü örtülmemeli, doğrudan state değişiminde geri alma veya açık geçici durum politikası belirlenmeli. **Kapanış:** T040, T054–T055, T071, T083.

### B03 — Gelecek sürüm ve kurtarma durumunun oyuncuya anlatılmaması

**P1 · Kod.** [GardenSave.Load](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenState.cs) version>2 görünce dosyayı korur ve ReadOnly açar, fakat yeni boş GardenState döndürür. Oyuncu “verilerim silindi” sanabilir; normal oynanış ve başarı hint'leri devam edebilir. Yedekten kurtarmada Recovered bilgisi için özel UI yok; ana/yedek ikisi bozuksa yeni profil açılır.

**Etki:** dosya korunsa da ilerleme kaybı algısı ve kaydedilemeyen oyun. **Kapanış:** koruma/kurtarma için açık ekran, normal kayıt vaadini durdurma, ham dosyaları koruyarak uygun dönüş; T074–T075, T078. ReadOnly'nin aynı GardenSave örneğinde kaldığı da dikkate alınmalı.

### B04 — Modal açılırken tutulan dünya parmağı kilitlenebilir

**P1 · Çıkarım; henüz cihazda tekrar edilmedi.** [GardenGame.Update/PointerUp](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenGame.cs) BlocksWorld durumunda erken döner. [GardenUI.OpenModal](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenUI.cs) mevcut pointer'ı iptal etmez. İkinci parmak modal açarken ilk parmak modal altında bırakılırsa Ended olayı işlenmeyebilir.

**Etki:** finger/held/dragging kalabilir ve sonraki dokunuşlar reddedilebilir. **Kapanış:** modal açılışı veya bloklu girişte güvenli iptal, T010–T011 ve T097. Bu bulgu gerçekleşmiş Android çökmesi olarak sunulmaz.

### B05 — Dosya yazma fallback'i tam atomik değil

**P1 / dayanıklılık · Kod; kesinti testi yok.** [GardenSave.Write](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenState.cs), geçici dosyadan Replace dener; IOException/PlatformNotSupported durumunda ana dosyaya Copy(overwrite) ve temp silme kullanır. Bu fallback'in kesintiye dayanıklılığı atomik Replace ile eşit değildir. Load .tmp'yi kurtarma adayı saymaz. Eski kaydı yedekleme hatası da yeni yazmayı durdurabilir.

**Etki:** kesintide yarım ana kayıt/yedeğe dönüş; en son işlemin kaybı. **Kapanış:** desteklenen platformda güvenli değiştirme/kurtarma politikası ve hata enjeksiyonu; T073–T075, T084–T085. Güç kesintisine dayanıklılık kanıtlanmış değildir.

### B06 — Tamamlanmış proje düğmesi sonraki projenin adımına taşar

**P2 · Kod.** [GardenExpansionUI.GuideProject](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenExpansionUI.cs), üç flag true iken step değerini proje sonunun bir ilerisine çıkarır. “Tamamlandı” düğmesi hâlâ aktiftir: Mina tamamlandı → tekne ipucu; Luli → meyve paylaşımı; Pufi → Bulut oyunu; Sumi → Sumi ipucu.

**Etki:** tamamlanmış kart yanlış yönlendirme yapar. **Kapanış:** Finished durumunda tamamlanma davranışı ya da pasif düğme; eksik görev için doğru rota korunmalı. T063.

### B07 — Reset kapsamı albüm/yedekler/oturum durumunu kapsamıyor

**P2 · Kod.** [ResetGarden](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenGame.cs) yeni State yazar; dil/ses/azaltılmış hareket korunur. Albüm, .bak ve .v1-preserved silinmez; yazma sırasında .bak eski ilerlemeyi tutabilir. Seçili araç, WindUntil, taşınmış dost merkezleri ve devam eden Sumi coroutine'i bütünüyle başlangıca döndürülmez.

**Etki:** kullanıcı “sıfırla”dan farklı kapsam bekleyebilir; sonradan yedek kurtarma eski ilerlemeyi geri getirebilir. Bu tek başına veri ihlali gerçekleştiğini göstermez. **Kapanış:** “ilerlemeyi sıfırla” ile “yerel veriyi sil” kapsamı açıkça belirlenmeli; gerekli oturum temizliği ve yedek politikası uygulanmalı; T070–T071, T085, T090. Kullanıcı albümü sessizce silinmemeli.

### B08 — v1 koruma kopyası oluşturulamazsa geçerli kayıt yüklenmeyebilir

**P1 · Koddan hata yolu.** [GardenSave.Load](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenState.cs), geçerli v1'i döndürmeden önce .v1-preserved kopyalar. Bu kopya erişim/disk nedeniyle hata verirse dış catch aynı adayı başarısız sayar; yedek/yeni profile düşebilir.

**Etki:** okunabilir eski ilerleme yüklenemeyebilir. **Kapanış:** okuma başarısı, göç ve koruma kopyası hatası ayrı ele alınmalı; kaynak dosya korunmalı, kullanıcıya net hata; T076–T077.

### B09 — Sumi meyve bırakma hedeflerine dahil değil

**P2 · Kod.** [GardenGame.Drop](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenGame.cs) meyve tesliminde dostları i<4 ile dolaşır. Yeni dost indeks 4'tür.

**Etki:** diğer dostlara verilen meyve Sumi'ye verilmez; görsel yakınlığa rağmen yanlış bırakma mesajı. Bunun bilinçli ürün kuralı olduğuna dair kod/doküman gerekçesi yok. **Kapanış:** dost koleksiyonu üzerinden geçerli hedefler; T024, T062.

### B10 — Azaltılmış hareket kısmi

**P2 · Kod.** [GardenEntity.Update](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenEntity.cs), [GardenGame.RainAt/WindTrail/Burst](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenGame.cs), [SumiWalk/LateUpdate](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenProgression.cs): dost/nesne salınımı ve ölçek tepkisi motion çarpanıyla durur, Burst sayısı üçe iner; yağmur/rüzgâr parçacıkları ve GardenParticle dalga büyümesi sürer. Sumi süresi 3 saniyeden 0,1'e iner; göz kırpma devam eder.

**Etki:** ayarın kapsadığı hareket belirsiz; ani hareket erişilebilirlik hedefiyle uyuşmayabilir. **Kapanış:** efekt bazında tanımlı azaltma politikası ve çocuk/erişilebilirlik gözlemi; T068.

### B11 — Albüm hata, sıralama ve kaynak yönetimi açıkları

**P2 · Kod/çıkarım.** [ShowAlbum](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenExpansionUI.cs) GetFiles'ı try dışında çağırır, Texture2D.LoadImage'ın false sonucunu denetlemez; texture yalnız başka albüm açılışında bırakılır. [Snapshot](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenProgression.cs) ekran texture'ını try dışında alır; null olursa sessiz döner; timestamp adına göre budar.

**Etki:** erişim engelinde kontrolsüz hata, bozuk resimde boş görünüm, modal kapalıyken RAM tutulması, saat geri alınırsa yeni anının silinmesi, aynı ad çakışması olasılığı. PNG encode/disk yazması ana iş parçacığında duraklama yaratabilir; ölçüm yok. **Kapanış:** T088–T095; hata UI'sı, dosya sırası/benzersizliği ve texture yaşam döngüsü düzeltilmeli.

### B12 — Yönergeler bazı gerçek kurallarla uyuşmuyor

**P2 · Kod.** [GardenProjects.StepsTR/EN](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenActivities.cs) “Üç çiçek türü dik” der; [RememberFlower](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenProgression.cs) yalnız büyümüş türleri sayar. [Yetişkin metni](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenUI.cs) “satın alma ekranı yok” derken pul kataloğu vardır; gerçek para satın alması olmadığı açık söylenmeli. Sunshine büyümüş çiçekte de yağmur önerir.

**Etki:** hedefi tamamladığını düşünen oyuncu neden ilerlemediğini anlayamaz. **Kapanış:** mekanik/metin uyumu, TR/EN birlikte; T015, T060, T065, T069.

### B13 — Ödül ve satın alma iki yazma arasında yarım kalabilir; tekrar yolu zayıf

**P2; veri/ödül regresyonu varsa P1 · Kod.** [ActivityInput](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenProgression.cs) önce hamleyi, sonra Claim'i ayrı transaction ile yazar. [ShowActivityReward](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenExpansionUI.cs) ödül alınmadıysa tekrar dener; hata metnine rağmen ayrı “yeniden dene” düğmesi yok. Dekor satın alma ve yerleştirme de ayrı yazmalardır.

**Etki:** disk hatasında tamamlanmış-ödülsüz run veya sahip olunan-yerleştirilmemiş dekor. Bu durumlar tek başına çift ödül/para kaybı değildir; yeniden açma/toggle kurtarabilir. **Kapanış:** açık durum/yeniden deneme ve kesinti doğrulaması; T040, T054, T056, T098.

### B14 — Yetersiz puldan mini oyuna yönlendirme bağlamı kaybediyor

**P2 / UX · Kod.** [ShowCollection](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenExpansionUI.cs) yetersiz bakiyede doğrudan ShowActivities çağırır. Eksik pul miktarı ve dönülecek ürün seçimi saklanmaz.

**Etki:** kullanıcı seçiminin neden değiştiğini anlamayabilir. **Kapanış:** açık açıklama/geri dönüş bağlamı; T030, T110. Yeni para birimi veya ödeme sistemi eklemek bu düzeltmenin gereği değildir.

### B15 — Yapısal veri kontrolü semantik tutarlılığı tam sağlamıyor

**P2 / bozuk kayıt; sayaç sınırı P3 · Kod.** [GardenActivityState.Valid ve Claim](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenActivities.cs) step'i 0..6 ile sınırlar fakat türe göre hedef, claimed/complete, kaya üzerinde tekne ve sayaç ilişkisini doğrulamaz. [GardenState.Validate](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenState.cs) sahiplik/species/keşif geçmişini tamamen yeniden kurmaz. Sayaçlar unchecked artar; pul 1.000.000'a clamp olurken UI +1 diyebilir.

**Etki:** yerel dosyası bozuk/değiştirilmiş profilde tutarsız görev/ödül/varyant. JSON imzasız/şifresizdir; mevcut çevrimdışı oyunda bu bir sunucu güvenlik açığı değildir. Gelecek çevrimiçi ekonomi bu kayda güvenemez. **Kapanış:** semantik normalleştirme/onarım politikası ve güvenli sayaç sınırı; T079–T082, T086.

### B16 — Açık modal sırasında Rebuild katman/ölçek sorunu yaratabilir

**P1/P2, etkileşim kaybına göre · Çıkarım.** [GardenUI.Rebuild/Update](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenUI.cs) ana UI'ı yeniden son çocuk olarak kurar; yalnız parentSettings için modalı yeniden açar. Diğer mevcut modal üst sırasını/ölçeğini koruyamayabilir. Dünya etkinlik düğmeleri, kamera LateUpdate boyut değişimiyle aynı anda yeniden oluşturulabilir.

**Etki:** pencere/safe area değişiminde modalın arkasına/önüne yanlış UI, erişilemeyen düğme, nesneden kaymış etkinlik kısayolu. **Kapanış:** aktif modal yaşam döngüsü ve kamera/UI yeniden yerleşimi; T099. Sabit iki oran smoke'u bu dinamik durumu test etmez.

### B17 — Ölçülmemiş performans maliyetleri

**P2 · Koddan risk; ölçülmüş FPS hatası değil.** Her başarılı [Transaction](../YagmurBahcesi/Assets/RainGarden/Scripts/GardenProgression.cs) JSON clone/Validate, senkron dosya I/O ve ApplyState çağırır; ApplyState piknik görsellerini yeniden kurar. Mini oyun hamlesi modalı da yeniden oluşturur. Parçacıklar yeni GameObject oluşturur/yok eder. [Builder](../YagmurBahcesi/Assets/RainGarden/Editor/GardenBuilder.cs) okunabilir, sıkıştırılmamış texture kullanır. UI pul metni her frame yeniden hesaplanır.

**Etki:** düşük cihazda GC, frame spike, RAM, ısı ve pil maliyeti olabilir. Mevcut emülatör bellek çıktısı fiziksel cihaz bütçesi değildir. **Kapanış:** T095, T102; profiler ölçümüne göre UI/state güncellemesini daraltma, önbellek/pooling/texture optimizasyonu. Ölçmeden backend veya karmaşık yeni altyapı eklenmemeli.

### B18 — Production yapılandırması ve AAB yolu uygulanmamış

**P1 / release · Eksik + Kod.** [GardenBuilder.Settings/BuildAndroid](../YagmurBahcesi/Assets/RainGarden/Editor/GardenBuilder.cs), Setup sırasında company/product/package/version değerlerini geliştirme sabitlerine yazar; BuildAndroid buildAppBundle=false ile APK üretir. Upload key/AAB/release kanalı komutu yok.

**Etki:** Editor'da elle değiştirilen nihai ayar Check/Setup/build ile geri alınabilir. **Kapanış:** geliştirme ve production ayarlarının açık ayrımı, final kimlik/sürüm, imzalı AAB ve cihaz doğrulaması; T106–T108. Bu denetimde kod eklenmedi.

### B19 — Paket denetimi sertifikayı gerçekten doğrulamıyor

**P1 / release kanıtı · Kod.** [Test-AndroidPackage.ps1](../tools/Test-AndroidPackage.ps1) raporundaki signing alanı sabit açıklamadır; apksigner sertifika doğrulaması yapmaz. İzin kontrolü belirli adları filtreler; bütün manifest güvenlik denetimi değildir. AAB'ye uygulanamaz.

**Etki:** rapor “passed” olması production imzası/Play uyumluluğu demek değildir. **Kapanış:** final adayda gerçek sertifika özeti, bütün manifest, AAB ve Play-generated APK kontrolü; T106–T107.

### B20 — Android build kopyasında silinen kaynak dosyaları kalabilir

**P1 / build tekrarlanabilirliği · Kod.** [Invoke-Garden.ps1](../tools/Invoke-Garden.ps1) Unicode yol sorununu ASCII staging'e robocopy /E ile çözer; kaynakta silinen dosyayı staging'den kaldırmaz.

**Etki:** artık kaynakta olmayan script/asset adayda kalabilir. **Kapanış:** temiz, doğrulanmış staging veya dosya manifestiyle güvenli eşleme; T109. Asıl çalışma alanında toplu silme yapılmamalı; staging hedefi mutlak yoluyla doğrulanmalı.

### B21 — İşletim sistemi yedeklemesi açıkça kararlaştırılmamış

**P2 / veri ve gizlilik · İncelenen APK manifesti.** APK manifestinde allowBackup/fullBackupContent/dataExtractionRules açık uygulama ayarı görülmedi. Bu, OS yedeklemesinin kapalı olduğunun kanıtı değildir. Kodun backend kullanmaması OS'nin yedek/taşıma davranışını belirlemez.

**Etki:** “yalnız bu cihazda”, kaldırmada silinme ve reset vaatleri gerçek cihaz davranışıyla uyuşmayabilir. **Kapanış:** Android yedek kapsamı açık yapılandırılmalı, T087 ile doğrulanmalı ve gizlilik metnine yansıtılmalı. Bu inceleme veri gönderildiğini tespit ettiği iddiasını taşımaz.

### B22 — Test raporlarının koşu/commit bağı zayıf; eski çıktı riski

**P1 / kabul kanıtı · Kod.** [Invoke-Garden.ps1](../tools/Invoke-Garden.ps1) sabit Artifacts yollarını kullanır; önceki runtime JSON'unu test öncesi ayırmaz. Timeout/erken hata yeni rapor üretmeyebilir. Süreç 0 çıkarsa okunan JSON'un status/commit/hash'i ayrıca doğrulanmaz. Editor/runtime JSON'ları kaynak commit'i taşımaz.

[Test-AndroidGarden.ps1](../tools/Test-AndroidGarden.ps1) belirli ekran koordinatlarına ve log regex'lerine dayanır; bütün ADB adımlarının başarısı ayrı assert değildir. Assertion sayıları bağımsız uçtan uca senaryo sayısı olarak kullanılmamalı.

**Kapanış:** her koşuya benzersiz kimlik/klasör, hash/commit/ortam/zaman ve zorunlu rapor/exit kontrolü; T109. Eski kanıtlar silinmeden arşivlenmeli.

### B23 — Fiziksel Android ve 16 KB runtime kanıtı yok

**P1 / Android kabulü · Eksik.** Mevcut Android çalıştırma X86_64/API36/4 KB emülatör; dağıtım APK'sı ARM64. API26 alt sınır, fiziksel dokunma/ısı/pil ve 16 KB native yükleme doğrulanmadı.

**Kapanış:** T097–T104, T107–T108; ödünç/test laboratuvarı cihazı da kullanılabilir. Emülatör işlev geliştirmesini sürdürmeye uygundur fakat bu kanıtların tamamının yerine geçmez. Kullanıcının şu anda Android cihazı olmadığı kayıtlıdır.

### B24 — Hedef yaş ve erişilebilirlik kullanılabilirliği doğrulanmadı

**P2, temel akış kullanılamıyorsa P1 · Eksik/Kod.** Düğmeler/metinler 1920×1080 referansında üretilir; fiziksel dp ölçümü yok. Sesli yönerge yok; yetişkin matematik kapısı dışında koleksiyon/keşif açıklamaları okumaya dayanır. Klavye navigasyonu Button Navigation.None kullanır; ekran okuyucu akışı uygulanmamıştır.

**Kapanış:** T068, T100, T110; metin/ikon/geri bildirim ve dokunma alanları gerçek hedef cihaz ve yaşta gözlenmeli. Rastgele “çocuklara uygundur” sertifikası veya uygunluk iddiası çıkarılamaz.

### B25 — Yayıncı, gizlilik, marka ve içerik beyanları tamamlanmamış

**P1 / mağaza · Eksik.** Geliştirme şirket/paket adı placeholder; dış gizlilik adresi ve gerçek destek kimliği eksik. Nihai ad/marka kontrolü ve sanat/font/ses lisans envanterinin yayın kontrolü tamamlanmadı. Uygulama içi bilgi metni mağaza sayfasının yerine geçmez.

**Kapanış:** T069, T096, T111 ve release planındaki resmî gereklilikler. Mevcut kodda reklam/analytics/ödeme SDK'sı yok; bu durum final binary ve SDK envanterinden tekrar teyit edilmeli. Matematik kapısı yasal ebeveyn rızası değildir.

### B26 — Play hesabı, dağıtım ve yayın sonrası operasyon yok

**P1 / dış bağımlılık · Eksik.** Kullanıcı Play Console hesabı olmadığını belirtti. Henüz uygulama mağaza kaydı, iç/kapalı test, production erişimi, genel yayın, crash/ANR gözlemi veya düzeltme dağıtım kanıtı yok. Yeni kişisel hesap türü seçilirse hesap doğrulama ve test koşulları uygulanır; güncel koşullar [release planında](RELEASE_CALISTIRMA_PLANI.md).

**Kapanış:** gerekli hesap/kimlik adımları hesap sahibi tarafından, aday build ve beyanlar geliştirici tarafından tamamlanır; T107–T112. Şifre/anahtar paylaşılması gerekmez; henüz gönderim/yayın yapılmış değildir.

## 4. Düzeltme ve doğrulama sırası

1. Veri/ödül ve erişilebilir giriş: B02–B05, B08, B13, B16; testle yeniden üret, dar düzeltme, ilgili regresyon.
2. Açık davranış/UX: B06–B07, B09–B12, B14–B15; metinle kuralı uyumlu yap, reset kapsamını somutlaştır.
3. Kabul kanıtı ve paket: B18–B22; build kaynağı, imza ve test çıktısını izlenebilir yap.
4. Cihaz/performans/yaş: B17, B23–B24; ölçmeden iyileşti kabul etme.
5. Mağaza ve işletim: B25–B26; ad/kimlik, hesap, gizlilik, test kanalları, son aday ve yayın izleme.
6. B01 Windows hattında ayrıca kapanmalı. Android odaklı yayın, Windows hatasının “çözüldü” sayılmasına neden olmaz.

Yeni içerik eklemek bu açıkları kapatmaz. Günlük görev/sosyal sistem gibi önerilere geçmeden önce mevcut kayıt, dokunma, ödül ve yayın hattının kabul ölçütleri tamamlanmalıdır.
